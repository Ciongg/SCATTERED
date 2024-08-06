using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{

    public Transform [] spawnPoints;
    public GameObject [] prefabs;

    public float timeBetweenSpawn;
    float nextSpawnTime;

    private GameObject trash;


    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextSpawnTime){
            spawnTrash();
            nextSpawnTime = Time.time + timeBetweenSpawn;

        }
        
    }


    void spawnTrash(){
        trash = Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
    }
}
