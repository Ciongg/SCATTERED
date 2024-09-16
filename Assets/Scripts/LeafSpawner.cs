using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] prefabs;
    public float minTimeBetweenSpawn = 5f;
    public float maxTimeBetweenSpawn = 20f;

    public float minusSpawnTime;
    private float nextSpawnTime;

    void Start(){
        minusSpawnTime = PlayerPrefs.GetFloat("MinusSpawnTime", 0);
        nextSpawnTime = Time.time + Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn - minusSpawnTime);
    }

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnObject();
            nextSpawnTime = Time.time + Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn - minusSpawnTime);
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
