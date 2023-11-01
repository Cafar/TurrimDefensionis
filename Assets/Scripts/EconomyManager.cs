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
    public float minNightTime = 60f;
    public float rewardMultiplier = 1.5f;

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
        float nightTime = gm.nightTimes[gm.nightLevel];
        float maxNightTime = sm.waveSpawnRate * sm.wavesPerNight + sm.maxEnemyTimeToChurch;
        if (sm.hasFinishedSpawning)
            GainCoin(minReward);
        else
        {
            int rewardRange = maxReward - minReward;
            float nightTimeRange = maxNightTime - minNightTime;
            float coinCalc = (rewardRange / nightTimeRange) * nightTime;
            GainCoin(Mathf.RoundToInt(coinCalc * rewardMultiplier));
        }
    }

}
