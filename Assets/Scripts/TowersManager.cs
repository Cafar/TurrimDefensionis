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

    private List<string> posibleWords = new List<string>{"agnus dei", "angelus crux", "angelus custos", "aqua sancta", 
        "argentum omnibus", "baptismus", "beatus deus", "benedictio", "brachium dei", "caelos aperire", 
        "castigatio", "cinis pagana", "cor impurum", "crucifixio", "culpa divina", "decapitatio", "Dei Genitrix", 
        "descendere angeli", "dimittis", "divina potentia", "dominus deus", "episcopus", "essentia solis", "evangelizatio", 
        "excommunicatio", "exularet", "fides catholica", "fides cruciata", "flagellum dei", "foras hinc", "furor argenti", 
        "gladius dei", "gloria patri", "gothicus ", "gratia divina", "ictus lucis", "in nomine patris", "in pace mori", 
        "iter vetitum", "iudicium finale", "lapidatio", "laudate dominum", "lex orandi", "lux aeterna", "mater dolorosa", 
        "miraculum", "monasterium", "monstra timete", "moriuntur pagani", "mors vampiro", "obsecrare", "omnpotenita", 
        "oratio ecclesiae", "paenitentia", "passio haeretici", "peccatores mori", "pietas popularis", "potestas solaris", 
        "purgatorium", "redemptor", "Requiesce in pace", "revertere ad inferos", "rosarium", "sacramentum", "sacratum cor", 
        "sacrificium", "Sancta Maria", "sepultura", "sitiens infixit", "sol divinus", "spiritus Sanctus", "taestamentum", 
        "tormentum amen", "trinitas", "tutela trinitatis", "ubiquitas", "ultio divina", "universitas", "vade retro", 
        "veneratio domino", "via ad infernum", "Virgo Maria", "vos trahatis"};

//     private void Awake()
//     {
//         Instance = this;
//     }

//     private void Update()
//     {
//         if(Input.GetKeyDown(KeyCode.Backspace))
//         {
//             ResumeAllTowers();
//         }
//     }

//     private void GameManager_OnGameStart()
//     {
//         //ResumeAllTowers();
//     }

//     private void GameManager_OnStartDay()
//     {
//         PauseAllTowers();
//     }

//     private void GameManager_OnStartNight()
//     {
//         StartAllTowers();
//     }

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
                    item.Tower.SetTowerData(item.Tower.data);
                    item.Tower.isDestroyed = false;
                    item.TypingTower.ResumeTower();
                }
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
