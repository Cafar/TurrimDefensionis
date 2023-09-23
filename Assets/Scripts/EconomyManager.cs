using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void OnEnable()
    {
        GameManager.onEndNight += GameManager_OnEndNight;
    }

    private void GameManager_OnEndNight()
    {
        GetEndOfNightReward();
    }

    void Start()
    {
        currentCoin = initialCoin;
        gmObj = GameObject.Find("GameManager");
        gm = gmObj.GetComponent<GameManager>();
        sm = gmObj.GetComponent<SpawnManager>();
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
        float nightTime = gm.nightTimes[gm.nightLevel];
        float maxNightTime = sm.waveSpawnRate * sm.wavesPerNight + sm.maxEnemyTimeToChurch;
        if (sm.hasFinishedSpawning)
            GainCoin(minReward);
        else
        {
            int rewardRange = maxReward - minReward;
            float nightTimeRange = maxNightTime - minNightTime;
            float coinCalc = (rewardRange / nightTime) * nightTimeRange;
            GainCoin(Mathf.RoundToInt(coinCalc * rewardMultiplier));
        }
    }

}
