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
        typingTower.OnFirstWordPushed += TypingTower_OnFirstWordPushed; ;
    }

    private void TypingTower_OnFirstWordPushed()
    {
        
    }

    private void TypingTower_OnWordCompleted()
    {
        tower.ActivateTower();
    }
}
