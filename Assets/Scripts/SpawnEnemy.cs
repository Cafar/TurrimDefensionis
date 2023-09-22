using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Header("Basic configuration")]
    public GameObject squadFormation;
    public LaneWaveData[] laneWave;
    public float squadSpawnRate = 4f;
    [Space(10)]

    [Header("Testing")]
    public int waveLevel = 0;
    public int squadIndex = 0;

    //private bool isWaveRunning = false;

    public IEnumerator StartWave()
    {
        //isWaveRunning = true;
        while (squadIndex < laneWave[waveLevel].squadSpawnOrder.Length)
        {
            if (laneWave[waveLevel].squadSpawnOrder[squadIndex] != null)
                SpawnSquad(laneWave[waveLevel].squadSpawnOrder[squadIndex]);
            squadIndex++;
            yield return new WaitForSeconds(squadSpawnRate);
        }
        //isWaveRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (laneWave[waveLevel].squadSpawnOrder[squadIndex] != null)
                SpawnSquad(laneWave[waveLevel].squadSpawnOrder[squadIndex]);
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
