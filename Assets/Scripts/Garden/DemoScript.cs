using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public Item[] itemsToPickup;
    public int currentSunflower;
    public int currentGaollium;
    public int currentGerbaras;
    public int currentRareSeed;
    public int currentLegendarySeed;

    private Dictionary<ShopItemName, int> seedCounts;


    public void SaveSeedData()
    {
        SeedData seedData = new SeedData(currentSunflower, currentGaollium, currentGerbaras, currentRareSeed, currentLegendarySeed);
        string json = JsonUtility.ToJson(seedData);
        PlayerPrefs.SetString("SeedData", json);
        PlayerPrefs.Save();
    }

    public void LoadSeedData()
    {
        string json = PlayerPrefs.GetString("SeedData", "");
        if (!string.IsNullOrEmpty(json))
        {
            SeedData seedData = JsonUtility.FromJson<SeedData>(json);
            currentSunflower = seedData.sunflower;
            currentGaollium = seedData.gaollium;
            currentGerbaras = seedData.gerbaras;
            currentRareSeed = seedData.rareSeed;
            currentLegendarySeed = seedData.legendarySeed;
        }
        else
        {
            // Set defaults if no saved data is found
            currentSunflower = 0;
            currentGaollium = 0;
            currentGerbaras = 0;
            currentRareSeed = 0;
            currentLegendarySeed = 0;
        }
    }
    
    public void PickupItem(int id){
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if(result){
            Debug.Log("item Added");
        }else{
            Debug.Log("Inventory Full");
        }
    }


    void Start(){
        
        seedCounts = new Dictionary<ShopItemName, int>
        {
            { ShopItemName.Sunflower, 0 },
            { ShopItemName.Gaollium, 0 },
            { ShopItemName.Gerbaras, 0 },
            { ShopItemName.RareSeedBag, 0 },
            { ShopItemName.LegendarySeedBag, 0 }
        };

        
        LoadSeedData();
        seedCounts[ShopItemName.Sunflower] = currentSunflower;
        seedCounts[ShopItemName.Gaollium] = currentGaollium;
        seedCounts[ShopItemName.Gerbaras] = currentGerbaras;
        seedCounts[ShopItemName.RareSeedBag] = currentRareSeed;
        seedCounts[ShopItemName.LegendarySeedBag] = currentLegendarySeed;

        // Load saved data

        // Spawn the corresponding items
        SpawnSeedItems();

        

    }

      private void SpawnSeedItems()
    {
        // Iterate over the seed counts and spawn items
        foreach (KeyValuePair<ShopItemName, int> entry in seedCounts)
        {
            int itemID = GetItemIDByName(entry.Key);  // Get the ID for the seed
            for (int i = 0; i < entry.Value; i++)
            {
                PickupItem(itemID);  // Pickup the specified number of items
            }
        }
    }

    private int GetItemIDByName(ShopItemName seedName)
    {
        switch (seedName)
        {
            case ShopItemName.Sunflower:
                return 0;
            case ShopItemName.Gaollium:
                return 1;
            case ShopItemName.Gerbaras:
                return 2;
            case ShopItemName.RareSeedBag:
                return 3;
            case ShopItemName.LegendarySeedBag:
                return 4;
            default:
                Debug.LogError("Unknown seed name.");
                return -1; // Return invalid ID if not found
        }
    }

}
