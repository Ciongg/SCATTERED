using UnityEditor;
using UnityEngine;

public class PlantPot : MonoBehaviour
{
    public GameObject sunflowerPrefab;
    public GameObject gaolliumPrefab;
    public GameObject gerbarasPrefab;
    public GameObject habacusPrefab;
    public GameObject palmPrefab;
    public GameObject polendronPrefab;
    public GameObject snakeplantPrefab;
    public GameObject[] RareSeedPrefabs;      
    public GameObject[] LegendarySeedPrefabs; 
    public Transform spawnPoint;
    
    public void PlantSeed(ItemType itemType, PlantItemName itemName)
    {
        if (itemType == ItemType.Seed)
        {
             GameObject selectedPrefab = null;  // Variable to hold the selected prefab
            GameObject[] selectedSeedPrefabs = null; // Array for random selection (if applicable)


            switch(itemName){
                case PlantItemName.Sunflower:
                    selectedPrefab = sunflowerPrefab;
                    break;

                case PlantItemName.Gaollium:
                     selectedPrefab = gaolliumPrefab;
                    break;

                case PlantItemName.Gerbaras:
                    selectedPrefab = gerbarasPrefab;
                    break;

                case PlantItemName.RareSeedBag:
                     selectedSeedPrefabs = RareSeedPrefabs;
                    break;

                case PlantItemName.LegendarySeedBag:
                     selectedSeedPrefabs = LegendarySeedPrefabs;
                    break;
                case PlantItemName.Habacus:
                     selectedPrefab = habacusPrefab;
                    break;
                case PlantItemName.Palm:
                     selectedPrefab = palmPrefab;
                    break;
                case PlantItemName.Polendron:
                     selectedPrefab = polendronPrefab;
                    break;
                case PlantItemName.Snakeplant:
                     selectedPrefab = snakeplantPrefab;
                    break;

                default:
                    
                    Debug.LogWarning("Unknown rarity type.");
                    break;
            }
            
            if (selectedPrefab != null)
            {
                GameObject plant = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
                plant.transform.SetParent(spawnPoint.transform);
            }
            // If it's a seed bag based on rarity, select a random prefab
            else if (selectedSeedPrefabs != null && selectedSeedPrefabs.Length > 0)
            {
                int seedIndex = Random.Range(0, selectedSeedPrefabs.Length);
                GameObject plant = Instantiate(selectedSeedPrefabs[seedIndex], spawnPoint.position, Quaternion.identity);
                plant.transform.SetParent(spawnPoint.transform);
            }
        }
    }
}
