using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField]
    private Tower tower;
    public Tower Tower => tower;

    [SerializeField]
    private TypingTowerController typingTower;
    public TypingTowerController TypingTower => typingTower;

    private void Start()
    {
        typingTower.OnWordCompleted += TypingTower_OnWordCompleted;
        typingTower.OnFirstWordPushed += TypingTower_OnFirstWordPushed;
        GameManager.onStartDay += GameManager_OnStartDay;
    }

    private void GameManager_OnStartDay()
    {
        typingTower.gameObject.SetActive(true);
    }

    private void TypingTower_OnFirstWordPushed()
    {
        TowersManager.Instance.SetAllTowersUnreadyExcept(typingTower);
    }

    private void TypingTower_OnWordCompleted()
    {
        tower.ActivateTower();
        typingTower.SetTowerPaused();
        TowersManager.Instance.ResumeAllTowers();
    }
}
