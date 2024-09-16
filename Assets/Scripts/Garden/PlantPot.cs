using UnityEditor;
using UnityEngine;

public class PlantPot : MonoBehaviour
{
     public GameObject[] BasicSeedPrefabs;    
    public GameObject[] UncommonSeedPrefabs; 
    public GameObject[] RareSeedPrefabs;      
    public GameObject[] LegendarySeedPrefabs; 
    public Transform spawnPoint;
    
    public void PlantSeed(ItemType itemType, RarityType rarityType)
    {
        if (itemType == ItemType.Seed)
        {
              GameObject[] selectedSeedPrefabs;

            switch(rarityType){
                case RarityType.Basic:
                    selectedSeedPrefabs = BasicSeedPrefabs;
                    break;

                case RarityType.Uncommon:
                     selectedSeedPrefabs = UncommonSeedPrefabs;
                    break;

                case RarityType.Rare:
                    selectedSeedPrefabs = RareSeedPrefabs;
                    break;

                case RarityType.Legendary:
                     selectedSeedPrefabs = LegendarySeedPrefabs;
                    break;

                default:
                    selectedSeedPrefabs = BasicSeedPrefabs;
                    Debug.LogWarning("Unknown rarity type.");
                    break;
            }
            
            // Randomly select a seed prefab to instantiate
            int seedIndex = Random.Range(0, selectedSeedPrefabs.Length);
            GameObject plant = Instantiate(selectedSeedPrefabs[seedIndex], spawnPoint.position, Quaternion.identity);
            plant.transform.SetParent(spawnPoint.transform);
        }
    }
}
