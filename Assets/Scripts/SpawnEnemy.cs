using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public GameObject enemyToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject spawnedEnemy = Instantiate(enemyToSpawn, gameObject.transform);
            spawnedEnemy.transform.position = transform.position;
            spawnedEnemy.transform.rotation = transform.rotation;
        }
    }
}
