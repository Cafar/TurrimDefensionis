using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class TypingTowerController : MonoBehaviour
{
    [System.Serializable]
    public class GameModeColors
    {
        public Color NormalColorText;
        public Color CurrentCharColorText;
        public Color CorrectCharText;
        public Color IncorrectCharText;
    }

    public event Action OnWordCompleted;
    public event Action OnFirstWordPushed;

    [Header("Settings")]
    [SerializeField]
    private GameModeColors wordsColors;
    [SerializeField]
    private string[] words = new string[1];

    [Header("UI")]
    [SerializeField]
    private GameObject backgroundText;
    [SerializeField]
    private TMP_Text mainText;
    [SerializeField]
    private TMP_Text perfectText;
    [SerializeField]
    private TMP_Text failText;
    //[Header("SOUNDS")]
    //[SerializeField]
    //private AudioSource perfectWord;
    //[SerializeField]
    //private AudioSource typeCorrect;
    //[SerializeField]
    //private AudioSource type;
    //[SerializeField]
    //private AudioSource completeWord;
    //[SerializeField]
    //private AudioSource failSimple;
    //[SerializeField]
    //private AudioSource failComplex;
    //[SerializeField]
    //private AudioSource switchSound;

    private string currentWord;
    private string keyPushedString;
    private char keyPushedChar;
    private char keyToPush;
    private int indexCharPos;
    //private bool isPerfect;
    private bool isReady;
    public bool isInFocus;

    private Sequence errorSeq;
    private Tower tower;

    public void ToggleReady()
    {
        if (isReady)
            isReady = false;
        else
            isReady = true;
    }

    public void SetIsReady(bool ready)
    {
        isReady = ready;
    }
        


    private void OnEnable()
    {
        GameManager.onStartDay += GameManager_OnStartDay;
        GameManager.onStartNight += GameManager_OnStartNight;
    }


    private void GameManager_OnStartDay()
    {
        SetTowerPaused();
    }


    private void GameManager_OnStartNight()
    {
        ResumeTower();
        isInFocus = true;
    }


    private void Start()
    {
        SetTowerPaused();
        tower = transform.parent.GetComponent<Tower>();
    }

    public void InitTower(string word)
    {
        indexCharPos = -1;
        isReady = true;
        currentWord = word;
        mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CurrentCharColorText) + "><voffset=0.2em>"
            + currentWord.Substring(0, 1) + "</voffset></color>" + currentWord[1..];

        mainText.color = wordsColors.NormalColorText;
        backgroundText.SetActive(true);
        SetNextKeyToPush();
    }

    // Update is called once per frame
    void Update()
    {
        if (isReady && isInFocus)
        {
            var allKeys = System.Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>();
            foreach (var key in allKeys)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Tab)) return;

                if (Input.GetKeyDown(key))
                {
                    //type.Play();
                    keyPushedString = key.ToString();
                    CleanKeyStringAndConvertToChar();
                    CheckIfGood();
                }
            }
        }
    }

    private void CheckIfGood()
    {
        int auxIndexCharPos = indexCharPos + 1;
        int auxIndexCharFinalPos = auxIndexCharPos + 1;
        int auxIndexCharPosForBig = indexCharPos + 1;
        string offsetChar = "voffset=0.2em";

        

        if (keyToPush == keyPushedChar)
        {
            if (auxIndexCharPos == currentWord.Length) auxIndexCharFinalPos--;
            string auxChar = currentWord.Substring(auxIndexCharPos, auxIndexCharFinalPos - auxIndexCharPos);
            if (auxChar == " ") auxChar = "█";
            mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CorrectCharText) + ">" + currentWord.Substring(0, auxIndexCharPos) + "</color>" + "<color=#"
                + ColorUtility.ToHtmlStringRGB(wordsColors.CurrentCharColorText) + "><" + offsetChar + ">"
                + auxChar + "</voffset></color>"
                + "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.NormalColorText) + ">" + currentWord[auxIndexCharFinalPos..] + "</color>";
            // typeCorrect.Play();
            SetNextKeyToPush();

            //Si es la primera letra que activo, desactivo las otras torretas para que puedan activarse
            if(indexCharPos == 1)
            {
                OnFirstWordPushed?.Invoke();
            }
        }
        else
        {
            //Si es la primera letra que pulso de la palabra, no comprobamos errores
            if (indexCharPos == 0)
            {
                return;
            }
            FeedbackError();
            tower.TakeDamage();
            if (currentWord.Length > 1)
            {
                string aux = currentWord.Substring(indexCharPos, 1);
                if (aux == " ") aux = "█";
                mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CorrectCharText) + ">" + currentWord.Substring(0, indexCharPos) + "</color>" +
                                "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.IncorrectCharText) + "><" + offsetChar + ">" + aux + "</voffset></color>" + "<color=#"
                               + ColorUtility.ToHtmlStringRGB(wordsColors.NormalColorText) + ">" + currentWord[auxIndexCharPos..] + "</color>";
            }
            else
            {
                mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.IncorrectCharText) + "><" + offsetChar + ">" + currentWord.Substring(indexCharPos, 1) + "</voffset></color>" + "<color=#"
                   + ColorUtility.ToHtmlStringRGB(wordsColors.NormalColorText) + ">" + currentWord[auxIndexCharPos..] + "</color>";
            }


            //failComplex.Play();
            failText.DOFade(0, 0);
            failText.rectTransform.DOAnchorPosY(0, 0);
            failText.rectTransform.DOAnchorPosY(1, 0.5f);
            failText.DOFade(1, 0.5f).OnComplete(() =>
            {
                failText.DOFade(0, 0.5f);
            });


            //isPerfect = false;
        }
    }

    private void FeedbackError()
    {
        Vector2 pos = mainText.rectTransform.anchoredPosition;
        errorSeq = DOTween.Sequence();
        errorSeq.Append(mainText.rectTransform.DOPunchAnchorPos(new Vector2(0.15f, -0.15f), 0.2f, 10, 50)).OnComplete(() =>
        {
            mainText.rectTransform.anchoredPosition = Vector3.zero;
        });
    }

    //Decide qué tecla vamos a tener que pulsar para que lo de como correcto
    private void SetNextKeyToPush()
    {
        indexCharPos++;
        if (indexCharPos < currentWord.Length)
        {
            keyToPush = currentWord.ToLower().ToCharArray()[indexCharPos];
        }
        else
        {
            //   completeWord.Play();
            mainText.rectTransform.DOScale(Vector3.one, 0);
            mainText.rectTransform.DOScale(Vector3.one * 1.2f, 0.1f).OnComplete(() =>
            {
                mainText.rectTransform.DOScale(Vector3.one, 0.1f);
            });
            WordCompleted();
        }
    }

    private void WordCompleted()
    {

        perfectText.DOFade(0, 0);
        perfectText.rectTransform.DOAnchorPosY(0, 0);
        perfectText.rectTransform.DOAnchorPosY(200, 0.5f);
        perfectText.DOFade(1, 0.5f).OnComplete(() =>
        {
            perfectText.DOFade(0, 0.5f);
        });
        OnWordCompleted?.Invoke();
        //perfectWord.Play();
    }

    public void SetTowerPaused()
    {
        isReady = false;
        backgroundText.SetActive(false);
    }

    public void ResumeTower()
    {
        InitTower(GetRandomWord());
    }


    private string GetRandomWord()
    {
        string randomWord = words[new System.Random().Next(0, words.Length)];
        return randomWord;
    }

    private void CleanKeyStringAndConvertToChar()
    {
        keyPushedString = keyPushedString.ToLower();
        if (keyPushedString.Contains("alpha"))
        {
            keyPushedString = keyPushedString.Replace("alpha", "");
        }
        if (keyPushedString.Contains("key pad"))
        {
            keyPushedString = keyPushedString.Replace("key pad", "");
        }
        if (keyPushedString.Contains("minus"))
        {
            keyPushedString = keyPushedString.Replace("minus", "-");
        }
        if (keyPushedString.Contains("space"))
        {
            keyPushedString = keyPushedString.Replace("space", " ");
        }
        char.TryParse(keyPushedString, out keyPushedChar);
    }
}
