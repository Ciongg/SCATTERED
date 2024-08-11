using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] prefabs;
    public float minTimeBetweenSpawn = 6f;
    public float maxTimeBetweenSpawn = 12f;

    private float nextSpawnTime;

    void Start(){
        nextSpawnTime = Time.time + Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
    }

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
        }
    }   

    void SpawnObject()
    {
        // Randomly select a spawn point and a prefab
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        // Instantiate the selected prefab at the selected spawn point
        GameObject prefabInstance = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(prefabInstance, 5f);
    }
}
