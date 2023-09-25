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
    public TextMeshProUGUI coinText;

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

    private void Update()
    {
        coinText.text = currentCoin.ToString();
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
