using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float waveSpawnRate = 10f;
    public int wavesPerNight = 30;
    public float maxEnemyTimeToChurch = 65f;
    public bool hasFinishedSpawning = false;

    private float startTime;
    private float maxTime;

    // private void OnEnable()
    // {
    //     GameManager.onStartNight += GameManager_OnStartNight;
    // }

    // private void GameManager_OnStartNight()
    // {
    //     hasFinishedSpawning = false;
    // }

    void Start()
    {
        hasFinishedSpawning = false;
        startTime = Time.time;
        maxTime = waveSpawnRate * wavesPerNight + maxEnemyTimeToChurch;
    }

    void Update()
    {
        if (Time.time - startTime >= maxTime)
        {
            hasFinishedSpawning = true;
            GameManager.Instance.EndNight();
        }
    }




}
