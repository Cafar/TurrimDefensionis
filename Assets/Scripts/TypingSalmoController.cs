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
using Unity.Burst.Intrinsics;
using UnityEditor;

public class TypingSalmoController : MonoBehaviour
{
    [System.Serializable]
    public class GameModeColors
    {
        public Color NormalColorText;
        public Color CurrentCharColorText;
        public Color CorrectCharText;
        public Color IncorrectCharText;
    }

    public event Action OnSalmoCompleted;
    public event Action OnFirstWordPushed;

    [Header("Settings")]
    [SerializeField]
    private GameModeColors wordsColors;
    [SerializeField]
    private string salmoText;
    [SerializeField]
    private float penaltyErrorSeconds;
    [SerializeField]
    private float secondsToBackLetter;
    [Header("UI")]
    [SerializeField]
    private TMP_Text mainText;
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
    private float penaltyErrorSecondsElapsed;
    private float secondsToBackLetterElapsed;


    private Sequence errorSeq;

    private void Start()
    {
        InitSalmo();
    }

    public void InitSalmo()
    {
        indexCharPos = -1;
        isReady = true;
        currentWord = salmoText;
        mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CurrentCharColorText) + "><voffset=0.2em>"
            + currentWord.Substring(0, 1) + "</voffset></color>" + currentWord[1..];
        penaltyErrorSecondsElapsed = penaltyErrorSeconds;
        secondsToBackLetterElapsed = 0;
        mainText.color = wordsColors.NormalColorText;
        SetNextKeyToPush();
    }

    // Update is called once per frame
    void Update()
    {
        penaltyErrorSecondsElapsed += Time.deltaTime;
        secondsToBackLetterElapsed += Time.deltaTime;

        if(secondsToBackLetterElapsed >= secondsToBackLetter)
        {
            SetLastKeyToPush();
            secondsToBackLetterElapsed = 0;
        }

        if (isReady && penaltyErrorSecondsElapsed > penaltyErrorSeconds)
        {
            var allKeys = System.Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>();
            foreach (var key in allKeys)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Backspace)) return;

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
            secondsToBackLetterElapsed = 0;
        }
        else
        {
            //Si es la primera letra que pulso de la palabra, no comprobamos errores
            if (indexCharPos == 0)
            {
                return;
            }
            FeedbackError();
            string aux = currentWord.Substring(indexCharPos, 1);
            if (aux == " ") aux = "█";
            mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CorrectCharText) + ">" + currentWord.Substring(0, indexCharPos) + "</color>" +
                            "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.IncorrectCharText) + "><" + offsetChar + ">" + aux + "</voffset></color>" + "<color=#"
                           + ColorUtility.ToHtmlStringRGB(wordsColors.NormalColorText) + ">" + currentWord[auxIndexCharPos..] + "</color>";

            //failComplex.Play();

            //isPerfect = false;
        }
    }

    private void FeedbackError()
    {
        Vector2 pos = mainText.rectTransform.anchoredPosition;
        float rng1 = UnityEngine.Random.Range(-10, 10);
        float rng2 = UnityEngine.Random.Range(-10, 10);
        errorSeq = DOTween.Sequence();
        errorSeq.Append(mainText.rectTransform.DOPunchAnchorPos(new Vector2(rng1, rng2), penaltyErrorSeconds, 10, 50)).OnComplete(() =>
        {
            mainText.rectTransform.anchoredPosition = Vector3.zero;
        });
        penaltyErrorSecondsElapsed = 0;
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
            SalmoCompleted();
        }
    }

    private void SetLastKeyToPush()
    {
        if (indexCharPos == 0) return;
        indexCharPos--;
        int auxIndexCharPos = indexCharPos + 1;
        string offsetChar = "voffset=0.2em";
        keyToPush = currentWord.ToLower().ToCharArray()[indexCharPos];
        string aux = currentWord.Substring(indexCharPos, 1);
        if(aux == " ") aux = "█";
        mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CorrectCharText) + ">" + currentWord.Substring(0, indexCharPos) + "</color>" +
                "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CurrentCharColorText) + "><" + offsetChar + ">" + aux + "</voffset></color>" + "<color=#"
               + ColorUtility.ToHtmlStringRGB(wordsColors.NormalColorText) + ">" + currentWord[auxIndexCharPos..] + "</color>";

    }

    private void SalmoCompleted()
    {


        OnSalmoCompleted?.Invoke();
        //perfectWord.Play();
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
        if (keyPushedString.Contains("comma"))
        {
            keyPushedString = keyPushedString.Replace("comma", ",");
        }
        char.TryParse(keyPushedString, out keyPushedChar);
    }
}
