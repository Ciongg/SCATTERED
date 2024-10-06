using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSpawner : MonoBehaviour
{
     public Transform[] spawnPoints;
    public GameObject[] prefabs;
    
    public float minTimeBetweenSpawn = 5f;
    public float maxTimeBetweenSpawn = 10f;

    public float minusSpawnTime;
    private float nextSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
       
        nextSpawnTime = Time.time + Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn);
    }

    // Update is called once per frame
    void Update()
    {
         if (Time.time > nextSpawnTime)
        {
            SpawnSpecial();
            nextSpawnTime = Time.time + Random.Range(minTimeBetweenSpawn, maxTimeBetweenSpawn - minusSpawnTime);
        }
    }

    void SpawnSpecial()
    {
        
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];

        
        
        GameObject prefabInstance = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        //add 7 second delay so then 3 seconds left and then 3 secodns = the blink duration then destroy at 10
        StartCoroutine(BlinkBeforeDestruction(prefabInstance, 7f));
        Destroy(prefabInstance, 10f);
    }

    IEnumerator BlinkBeforeDestruction(GameObject prefab, float delay){
        yield return new WaitForSeconds(delay);

        Renderer renderer = prefab.GetComponent<Renderer>();

        if (renderer == null) 
        {
        yield break; 
        }

        float blinkDuration = 3f;
            float blinkInterval = 0.2f; 
            float timePassed = 0f;

            while (timePassed < blinkDuration)
            {
                
                renderer.enabled = !renderer.enabled;
                yield return new WaitForSeconds(blinkInterval);
                timePassed += blinkInterval;
            }

            
            renderer.enabled = true;
    }
}

