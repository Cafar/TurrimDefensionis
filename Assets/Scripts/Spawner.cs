using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Basic configuration")]
    public GameObject squadFormation;
    public LaneWaveData[] laneWave;
    public float squadSpawnRate = 4f;
    [Space(10)]

    [Header("Testing")]
    public int squadIndex = 0;

    private GameManager gm;

    private void OnEnable()
    {
        GameManager.onStartDay += GameManager_OnStartDay;
        GameManager.onStartNight += GameManager_OnStartNight;
    }

    private void GameManager_OnStartDay()
    {
        StopAllCoroutines();
    }

    private void GameManager_OnStartNight()
    {
        StartCoroutine(StartWave());
    }

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public IEnumerator StartWave()
    {
        while (squadIndex < laneWave[gm.waveLevel].squadSpawnOrder.Length)
        {
            if (laneWave[gm.waveLevel].squadSpawnOrder[squadIndex] != null)
                SpawnSquad(laneWave[gm.waveLevel].squadSpawnOrder[squadIndex]);
            squadIndex++;
            yield return new WaitForSeconds(squadSpawnRate);
        }
    }

    void Update()
    {
        // FOR TESTING
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (laneWave[gm.waveLevel].squadSpawnOrder[squadIndex] != null)
                SpawnSquad(laneWave[gm.waveLevel].squadSpawnOrder[squadIndex]);
            squadIndex++;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Wave started");
            StartCoroutine(StartWave());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Wave stopped");
            StopAllCoroutines();
        }
    }

    void SpawnSquad(SquadData squadToSpawn)
    {
        GameObject spawnedSquad = Instantiate(squadFormation, gameObject.transform);
        int i = 0;
        foreach(Enemy childEnemy in spawnedSquad.GetComponentsInChildren<Enemy>())
        {
            if (0 <= i && i <= 2)
            {
                if (squadToSpawn.topRow[i % 3] != null)
                    childEnemy.data = squadToSpawn.topRow[i % 3];
                else
                    Destroy(childEnemy.gameObject);
            }
            else if (3 <= i && i <= 5)
            {
                if (squadToSpawn.midRow[i % 3] != null)
                    childEnemy.data = squadToSpawn.midRow[i % 3];
                else
                    Destroy(childEnemy.gameObject);
            }
            else
            {
                if (squadToSpawn.botRow[i % 3] != null)
                    childEnemy.data = squadToSpawn.botRow[i % 3];
                else
                    Destroy(childEnemy.gameObject);
            }
            i++;
        }
        spawnedSquad.transform.position = transform.position;
        spawnedSquad.transform.rotation = transform.rotation;
        Debug.Log("Squad number " + squadIndex + " spawned");
    }

}
