using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float waveSpawnRate = 10f;
    public int wavesPerNight = 30;
    public float maxEnemyTimeToChurch = 65f;
    public bool hasFinishedSpawning = false;

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
    }


}
