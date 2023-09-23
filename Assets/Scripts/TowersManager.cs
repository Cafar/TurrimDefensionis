using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Xml.Schema;

public class TowersManager : MonoBehaviour
{
    public static TowersManager Instance;
    [SerializeField]
    private List<TowerController> towers;

    private List<string> posibleWords = new List<string>{"Mare","Nostrum","Pija","Dura","Four","Five","Six","Seven","Eight","Nine","Ten","Eleven","Twelve","Thirteen","Fourteen","Fifteen","Sixteen",
        "Seventeen", "Eighteen", "Nineteen","Twenty", "Twentyone", "Twentytwo", "Twentythree", "Twentyfour", "Twentyfive", "Twentysix", "Twentyseven", "Twentyeight", "Twentynine",
        "Thirty", "Thirtyone", "Thirtytwo", "Thirtythree", "Thirtyfour", "Thirtyfive", "Thirtysix", "Thirtyseven", "Thirtyeight", "Thirtynine", "Forty", "Fortyone", "Fortytwo",
        "Fortythree", "Fortyfour", "Fortyfive", "Fortysix", "Fortyseven", "Fortyeight", "Fortynine", "Fifty", "Fiftyone", "Fiftytwo", "Fiftythree", "Fiftyfour", "Fiftyfive",
        "Fiftysix", "Fiftyseven", "Fiftyeight", "Fiftynine", "Sixty", "Sixtyone", "Sixtytwo", "Sixtythree", "Sixtyfour", "Sixtyfive", "Sixtysix", "Sixtyseven", "Sixtyeight",
        "Sixtynine", "Seventy", "Seventyone", "Seventytwo", "Seventythree", "Seventyfour", "Seventyfive", "Seventysix", "Seventyseven", "Seventyeight", "Seventynine", "Eighty",
        "Eightyone", "Eightytwo", "Eightythree", "Eightyfour", "Eightyfive", "Eightysix", "Eightyseven", "Eightyeight", "Eightynine", "Ninety", "Ninetyone", "Ninetytwo", "Ninetythree",
        "Ninetyfour", "Ninetyfive", "Ninetysix", "Ninetyseven", "Ninetyeight", "Ninetynine", "One hundred"};

    private void Awake()
    {
        Instance = this;
        GameManager.onStartNight += GameManager_OnGameStart;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            ResumeAllTowers();
        }
    }

    private void GameManager_OnGameStart()
    {
        SetAllTowersReady();
    }

    public void SetAllTowersUnreadyExcept(TypingTowerController tower)
    {
        foreach (var item in towers)
        {
            if(item.TypingTower.GetInstanceID() != tower.GetInstanceID())
            {
                item.TypingTower.SetTowerPaused();
            }
        }
    }

    public void ResumeAllTowers()
    {
        foreach (var item in towers)
        {
            item.TypingTower.ResumeTower();
        }
    }

    public void SetAllTowersReady()
    {
        int a = 0;
        foreach (var item in towers)
        {
            //TODO: guardar en una lista las palabras que ya est�n utilizadas por una torre
            //para comprobar que a otra torre no se le asigna una palabra que empiece por la misma letra
            item.TypingTower.InitTower(posibleWords[a++]);
        }
    }
}