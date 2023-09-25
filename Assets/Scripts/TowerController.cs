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
        GameManager.onStartNight += GameManager_OnStartNight;
    }

    private void GameManager_OnStartDay()
    {
        tower.SetNightUIVisibility(false);
        tower.isActive = false;
    }

    private void GameManager_OnStartNight()
    {
        tower.SetNightUIVisibility(true);
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
