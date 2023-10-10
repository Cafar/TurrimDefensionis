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
    //public event Action OnFirstWordPushed;

    [Header("Settings")]
    [SerializeField]
    private GameModeColors wordsColors;
    [SerializeField]
    [TextArea(3, 10)] private List<string> salmoText = new List<string>{"Dominus reget me et nihil mihi deerit in loco pascuae ibi me collocabit uper aquam refectionis educavit me animam meam convertit deduxit me super semitas justitiae propter nomen suum etiam si ambulavero in valle mortis non timebo malum quoniam tu mecum es",
        "Virga tua et baculus tuus ipsa me consolata sunt parasti in conspectu meo mensam adversus eos qui tribulant me Impinguasti in oleo caput meum et calix meus inebrians quam paeclarus est et misericordia tua subsequetur me omnibus diebus vitae et ut inhabitem in domo domini in longitudinem dierum",
        "Beatus vir qui non abiit in consilio impiorum et in via peccatorum non stetit et in cathedra pestilentiae non sedit sed in lege domini voluntas ejus et in lege ejus meditabitur die ac nocte et erit tamquam lignum quod plantatum est secus decursus aquarum quod fructum suum dabit in tempore suo et folium ejus non defluet et omnia quaecumque faciet prosperabuntur",
        "Bonum est confiteri domino et psallere nomini tuo ad annuntiandum mane misericordiam tuam et veritatem tuam per noctem in decachordo psallere cum cantico quia delectasti me domine in factura tua et in operibus manuum tuarum exsultabo quam magnificata sunt opera tua domine nimis profundae factae sunt cogitationes tuae vir insipiens non",
        "Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Ut enim ad minim veniam quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur Excepteur sint occaecat cupidatat non proident sunt in culpa qui officia deserunt mollit anim id est laborum"};

    [SerializeField]
    private float penaltyErrorSeconds;
    [SerializeField]
    private float secondsToBackLetter;
    [SerializeField]
    private float secondsToStartBackLetter;
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

    public bool isInFocus;

    private GameManager gm;
    private NightManager nm;
    private string currentWord;
    private string keyPushedString;
    private char keyPushedChar;
    private char keyToPush;
    private int indexCharPos;
    private float penaltyErrorSecondsElapsed;
    private float secondsToBackLetterElapsed;
    private float secondsToStartBackLetterElapsed;
    private bool isFinished;


    private Sequence errorSeq;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        nm = GameObject.Find("NightManager").GetComponent<NightManager>();
        InitSalmo();
    }

    public void InitSalmo()
    {
        isFinished = false;
        indexCharPos = -1;
        currentWord = salmoText[gm.nightLevel];
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
        secondsToStartBackLetterElapsed += Time.deltaTime;

        if(secondsToStartBackLetterElapsed >= secondsToStartBackLetter && secondsToBackLetterElapsed >= secondsToBackLetter)
        {
            SetLastKeyToPush();
            secondsToBackLetterElapsed = 0;
        }

        if (penaltyErrorSecondsElapsed > penaltyErrorSeconds)
        {
            var allKeys = System.Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>();
            foreach (var key in allKeys)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape) || !isInFocus || isFinished) return;

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
            if (auxChar == " ") auxChar = "\u25A1";
            mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CorrectCharText) + ">" + currentWord.Substring(0, auxIndexCharPos) + "</color>" + "<color=#"
                + ColorUtility.ToHtmlStringRGB(wordsColors.CurrentCharColorText) + "><" + offsetChar + ">"
                + auxChar + "</voffset></color>"
                + "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.NormalColorText) + ">" + currentWord[auxIndexCharFinalPos..] + "</color>";
            // typeCorrect.Play();
            SetNextKeyToPush();
            secondsToStartBackLetterElapsed = 0;
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
            if (aux == " ") aux = "\u25A1";
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
        if(aux == " ") aux = "\u25A1";
        mainText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CorrectCharText) + ">" + currentWord.Substring(0, indexCharPos) + "</color>" +
                "<color=#" + ColorUtility.ToHtmlStringRGB(wordsColors.CurrentCharColorText) + "><" + offsetChar + ">" + aux + "</voffset></color>" + "<color=#"
               + ColorUtility.ToHtmlStringRGB(wordsColors.NormalColorText) + ">" + currentWord[auxIndexCharPos..] + "</color>";

    }

    private void SalmoCompleted()
    {

        OnSalmoCompleted?.Invoke();
        isFinished = true;
        nm.EndNight();
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
