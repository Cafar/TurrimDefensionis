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

    private List<string> posibleWords = new List<string>{"Mare","Nos trum","Pija","Dura","Four","Five","Six","Seven","Eight","Nine","Ten","Eleven","Twelve","Thirteen","Fourteen","Fifteen","Sixteen",
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
        GameManager.onStartDay += GameManager_OnStartDay;
        GameManager.onStartNight += GameManager_OnStartNight;
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
        //ResumeAllTowers();
    }

    private void GameManager_OnStartDay()
    {
        PauseAllTowers();
    }

    private void GameManager_OnStartNight()
    {
        StartAllTowers();
    }

    public void SetAllTowersUnreadyExcept(TypingTowerController tower)
    {
        foreach (var item in towers)
        {
            if (item.gameObject.activeSelf)
            {
                if(item.TypingTower.GetInstanceID() != tower.GetInstanceID())
                {
                    item.TypingTower.SetTowerPaused();
                }

            }
        }
    }

    public void PauseAllTowers()
    {
        foreach (var item in towers)
        {
            if (item.gameObject.activeSelf)
            {
                item.Tower.SetNightUIVisibility(false);
                item.Tower.SetDayUIVisibility(true);
                item.Tower.isActive = false;
                item.Tower.backgroundFocus.SetActive(false);
                item.TypingTower.SetTowerPaused();
            }
        }
    }

    public void StartAllTowers()
    {
        foreach (var item in towers)
        {
            if (item.gameObject.activeSelf)
            {
                if (item.Tower.data.resistance > 0)
                {
                    item.Tower.SetNightUIVisibility(true);
                    item.Tower.SetTowerData(item.Tower.data);
                    item.Tower.isDestroyed = false;
                    item.TypingTower.ResumeTower();
                }
                else
                {
                    item.Tower.SetNightUIVisibility(false);
                }
                item.Tower.SetDayUIVisibility(false);
            }
        }
        SetAllTowersIsInFocus(true);
    }

    public void ResumeAllTowers()
    {
        foreach (var item in towers)
        {
            if (item.gameObject.activeSelf)
            {
                if (!item.Tower.isActive && !item.Tower.isDestroyed && item.TypingTower.gameObject.activeSelf && item.Tower.data.resistance > 0)
                {
                    item.Tower.SetNightUIVisibility(true);
                    item.Tower.SetDayUIVisibility(false);
                    item.Tower.SetTowerData(item.Tower.data);
                    item.TypingTower.ResumeTower();
                }
            }
        }
    }

    public void SetAllTowersIsInFocus(bool inFocus)
    {
        foreach (var item in towers)
        {
            if (item.gameObject.activeSelf && item.Tower.data.resistance > 0 && !item.Tower.isDestroyed)
            {
                item.TypingTower.isInFocus = inFocus;
                item.Tower.backgroundFocus.SetActive(inFocus);
            }
        }
    }

}
