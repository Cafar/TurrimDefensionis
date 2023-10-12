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

    public bool towerSelected = false;

    private List<string> posibleWords = new List<string>{"agnus dei", "angelus crux", "aqua sancta", "argentum",
        "baptismus", "beatus deus", "benedictio", "brachium dei",
        "castigatio", "cinis pagana", "cor impurum", "crucifixio",
        "decapitatio", "dei genitrix", "dimittis", "dominus deus",
        "episcopus", "essentia dei", "evangelium", "exularet",
        "fides divina", "flagellum", "foras hinc", "furor argenti", 
        "gladius dei", "gloria patri", "gothicus", 
        "ictus lucis", "iter vetitum", "iudicium", 
        "lapidatio", "laudate", "lex orandi", "lux aeterna", 
        "mater dolorosa", "miraculum", "monasterium", "mors vampiro",
        "obsecrare", "omnipotentia", "oratio",
        "paenitentia", "popularis", "potestas", "purgatorium",
        "redemptor", "rosarium",
        "sacramentum", "sacrificium", "sepultura", "sol divinus",
        "taestamentum", "tormentum", "trinitas", 
        "ubiquitas", "ultio divina", "universitas",
        "vade retro", "veneratio", "virgo maria", "vos trahatis"};

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            ResumeAllTowers();
        }
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

    private string GetRandomWord()
    {
        return posibleWords[Random.Range(0, posibleWords.Count - 1)];
    }

    public string GetNewWord()
    {
        int i = -1;
        string new_word = GetRandomWord();
        while (++i < towers.Count)
        {
            if (towers[i].Tower.data.resistance == 0)
                continue;
            if (towers[i].Tower.isDestroyed)
                continue;
            if (towers[i].TypingTower.currentWord == "")
            {
                towers[i].TypingTower.currentWord = new_word;
                i = -1;
                continue;
            }
            if (towers[i].TypingTower.currentWord[0] == new_word[0])
            {
                new_word = GetRandomWord();
                i = -1;
            }
        }
        return new_word;
    }

}
