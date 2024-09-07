using UnityEditor;
using UnityEngine;

public class PlantPot : MonoBehaviour
{
    public GameObject[] SeedPrefabs; // Array of seed prefabs
    public Transform spawnPoint;
    
    public void PlantSeed(ItemType itemType)
    {
        if (itemType == ItemType.Seed)
        {
            
            // Randomly select a seed prefab to instantiate
            int seedIndex = Random.Range(0, SeedPrefabs.Length);
            GameObject plant = Instantiate(SeedPrefabs[seedIndex], spawnPoint.position, Quaternion.identity);
            plant.transform.SetParent(spawnPoint.transform);
        }
    }
}
