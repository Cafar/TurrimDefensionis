using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TypeController : MonoBehaviour
{
    [System.Serializable]
    public class GameModeColors
    {
        public Color NormalColorText;
        public Color CurrentCharColorText;
        public Color CorrectCharText;
        public Color IncorrectCharText;
    }
    public enum GameMode
    {
        Numbers,
        Words
    }
    private List<string> posibleWords = new List<string>{"Mare","Nostrum","Pija","Dura","Four","Five","Six","Seven","Eight","Nine","Ten","Eleven","Twelve","Thirteen","Fourteen","Fifteen","Sixteen",
        "Seventeen", "Eighteen", "Nineteen","Twenty", "Twentyone", "Twentytwo", "Twentythree", "Twentyfour", "Twentyfive", "Twentysix", "Twentyseven", "Twentyeight", "Twentynine",
        "Thirty", "Thirtyone", "Thirtytwo", "Thirtythree", "Thirtyfour", "Thirtyfive", "Thirtysix", "Thirtyseven", "Thirtyeight", "Thirtynine", "Forty", "Fortyone", "Fortytwo",
        "Fortythree", "Fortyfour", "Fortyfive", "Fortysix", "Fortyseven", "Fortyeight", "Fortynine", "Fifty", "Fiftyone", "Fiftytwo", "Fiftythree", "Fiftyfour", "Fiftyfive",
        "Fiftysix", "Fiftyseven", "Fiftyeight", "Fiftynine", "Sixty", "Sixtyone", "Sixtytwo", "Sixtythree", "Sixtyfour", "Sixtyfive", "Sixtysix", "Sixtyseven", "Sixtyeight",
        "Sixtynine", "Seventy", "Seventyone", "Seventytwo", "Seventythree", "Seventyfour", "Seventyfive", "Seventysix", "Seventyseven", "Seventyeight", "Seventynine", "Eighty",
        "Eightyone", "Eightytwo", "Eightythree", "Eightyfour", "Eightyfive", "Eightysix", "Eightyseven", "Eightyeight", "Eightynine", "Ninety", "Ninetyone", "Ninetytwo", "Ninetythree",
        "Ninetyfour", "Ninetyfive", "Ninetysix", "Ninetyseven", "Ninetyeight", "Ninetynine", "One hundred"};

    [Header("Settings")]
    [SerializeField]
    private GameModeColors wordsColors;
    [Header("UI")]
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

    private GameModeColors currentGameModeColors;
    private string keyPushedString;
    private string currentWord;
    private char keyPushedChar;
    private char keyToPush;
    private int indexNum;
    private int indexCharPos;
    //private bool isPerfect;



    private Sequence errorSeq;

    void Start()
    {
        StartGame();
    }


    private void StartGame()
    {
        indexNum = 0;
        indexCharPos = -1;
        currentGameModeColors = wordsColors;
        ResetLevel();
        //GetNextLevel();
    }

    // Update is called once per frame
    void Update()
    {
        var allKeys = System.Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>();
        foreach (var key in allKeys)
        {
            if (Input.GetKeyDown(key))
            {
                //type.Play();
                keyPushedString = key.ToString();
                CleanKeyStringAndConvertToChar();
                CheckIfGood();
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
            mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(currentGameModeColors.CorrectCharText) + ">" + currentWord.Substring(0, auxIndexCharPos) + "</color>" + "<color=#"
                + ColorUtility.ToHtmlStringRGB(currentGameModeColors.CurrentCharColorText) + "><" + offsetChar + ">"
                + currentWord.Substring(auxIndexCharPos, auxIndexCharFinalPos - auxIndexCharPos) + "</voffset></color>"
                + "<color=#" + ColorUtility.ToHtmlStringRGB(currentGameModeColors.NormalColorText) + ">" + currentWord[auxIndexCharFinalPos..] + "</color>";
           // typeCorrect.Play();
            SetNextKeyToPush();
        }
        else
        {
            FeedbackError();
            if (currentWord.Length > 1)
            {
                mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(currentGameModeColors.CorrectCharText) + ">" + currentWord.Substring(0, indexCharPos) + "</color>" +
                                "<color=#" + ColorUtility.ToHtmlStringRGB(currentGameModeColors.IncorrectCharText) + "><" + offsetChar + ">" + currentWord.Substring(indexCharPos, 1) + "</voffset></color>" + "<color=#"
                               + ColorUtility.ToHtmlStringRGB(currentGameModeColors.NormalColorText) + ">" + currentWord[auxIndexCharPos..] + "</color>";
            }
            else
            {
                mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(currentGameModeColors.IncorrectCharText) + "><" + offsetChar + ">" + currentWord.Substring(indexCharPos, 1) + "</voffset></color>" + "<color=#"
                   + ColorUtility.ToHtmlStringRGB(currentGameModeColors.NormalColorText) + ">" + currentWord[auxIndexCharPos..] + "</color>";
            }


            //failComplex.Play();
            failText.DOFade(0, 0);
            failText.rectTransform.DOAnchorPosY(0, 0);
            failText.rectTransform.DOAnchorPosY(200, 0.5f);
            failText.DOFade(1, 0.5f).OnComplete(() =>
            {
                failText.DOFade(0, 0.5f);
            });


            //isPerfect = false;
        }
    }

    private void FeedbackError()
    {
        Vector2 pos = mainText.rectTransform.position;
        errorSeq = DOTween.Sequence();
        errorSeq.Append(mainText.rectTransform.DOPunchAnchorPos(new Vector2(10, 1), 0.2f, 50, 10)).OnComplete(() =>
         {
             mainText.rectTransform.position = pos;
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
            GetNextLevel();
        }
    }

    private void ResetLevel()
    {
        indexCharPos = -1;


        currentWord = posibleWords[indexNum];
        mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(currentGameModeColors.CurrentCharColorText) + "><voffset=0.2em>"
            + currentWord.Substring(0, 1) + "</voffset></color>" + currentWord[1..];

        mainText.color = currentGameModeColors.NormalColorText;
        //isPerfect = true;
        SetNextKeyToPush();
    }

    private void GetNextLevel()
    {

        perfectText.DOFade(0, 0);
        perfectText.rectTransform.DOAnchorPosY(0, 0);
        perfectText.rectTransform.DOAnchorPosY(200, 0.5f);
        perfectText.DOFade(1, 0.5f).OnComplete(() =>
        {
            perfectText.DOFade(0, 0.5f);
        });
        //perfectWord.Play();
        indexNum++;
        ResetLevel();
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
