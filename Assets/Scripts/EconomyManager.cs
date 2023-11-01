using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : MonoBehaviour
{
    [Header("Configuration")]
    public int initialCoin = 0;
    public int maxCoin = 1000;
    public int minReward = 200;
    public int maxReward = 1200;

    public float timeMultiplier = 1f;
    public float churchDamageMultiplier = 1f;

    [Space(10)]

    [Header("Testing")]
    public int currentCoin;

    private GameObject gmObj;
    private GameManager gm;
    private SpawnManager sm;


    void Start()
    {
        currentCoin = initialCoin;
        gmObj = GameObject.Find("GameManager");
        gm = gmObj.GetComponent<GameManager>();
    }

    public void ResetEconomy()
    {
        currentCoin = initialCoin;
    }

    public void SpendCoin(TowerData data)
    {
        currentCoin -= data.cost;
    }

    private void GainCoin(int coinToGain)
    {
        if (currentCoin + coinToGain > maxCoin)
            currentCoin = maxCoin;
        else
            currentCoin += coinToGain;
    }

    public void GetEndOfNightReward()
    {
        sm = GameObject.Find("NightManager").GetComponent<SpawnManager>();
        if (sm.hasFinishedSpawning)
            GainCoin(minReward);
        else
        {
            float timePenalization = gm.nightTimes[gm.nightLevel] * timeMultiplier;
            float healthPenalization = gm.churchDamage * churchDamageMultiplier;
            float coinCalc = maxReward - (timePenalization + healthPenalization);
            GainCoin(Mathf.RoundToInt(coinCalc));
        }
    }

}
