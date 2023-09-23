using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Configuration")]
    public int initialCoin = 0;
    public int maxCoin = 1000;

    [Space(10)]

    [Header("Testing")]
    public int currentCoin;

    private GameManager gm;

    private void OnEnable()
    {
        GameManager.onEndNight += GameManager_OnEndNight;
    }

    private void GameManager_OnEndNight()
    {
        
    }

    void Start()
    {
        currentCoin = initialCoin;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public bool SpendCoin(int coinToSpend)
    {
        if (coinToSpend > currentCoin)
            return false;
        currentCoin -= coinToSpend;
        return true;
    }

    private void GainCoin(int coinToGain)
    {
        if (currentCoin + coinToGain > maxCoin)
            currentCoin = maxCoin;
        else
            currentCoin += coinToGain;
    }

    private void GetEndOfNightReward()
    {

    }

}
