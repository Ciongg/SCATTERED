using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] prefabs;
    public float timeBetweenSpawn;
    public float minForce = 300f; // Minimum force
    public float maxForce = 700f; // Maximum force
    float nextSpawnTime;

    private GameObject trash;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnTrash();
            nextSpawnTime = Time.time + timeBetweenSpawn;
        }
    }

    void SpawnTrash()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        trash = Instantiate(prefabs[Random.Range(0, prefabs.Length)], spawnPoint.position, randomRotation);
        Rigidbody2D rb = trash.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float randomForce = Random.Range(minForce, maxForce);
            Vector2 forceDirection = new Vector2(Random.Range(-0.5f, 0.5f), 1).normalized;
            rb.AddForce(forceDirection * randomForce);
        }
    }
}
