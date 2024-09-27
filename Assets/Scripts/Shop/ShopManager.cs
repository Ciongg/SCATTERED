using System;
using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SeedData
{
    public int sunflower;
    public int gaollium;
    public int gerbaras;
    public int rareSeed;
    public int legendarySeed;

    // Constructor to initialize seed counts
    public SeedData(int sunflower, int gaollium, int gerbaras, int rare, int legendary)
    {
        this.sunflower = sunflower;
        this.gaollium = gaollium;
        this.gerbaras = gerbaras;
        this.rareSeed = rare;
        this.legendarySeed = legendary;
    }
}



public class ShopManager : MonoBehaviour
{   


    public void SaveSeedData()
{
    // Load existing data first
    string existingJson = PlayerPrefs.GetString("SeedData", "");

    SeedData existingData;
    if (!string.IsNullOrEmpty(existingJson))
    {
        // If data exists, load it
        existingData = JsonUtility.FromJson<SeedData>(existingJson);
    }
    else
    {
        // If no existing data, create a new SeedData object
        existingData = new SeedData(0, 0, 0, 0, 0);
    }

    // Update the existing data with the new counts
    existingData.sunflower += currentSunflower;
    existingData.gaollium += currentGaollium;
    existingData.gerbaras += currentGerbaras;
    existingData.rareSeed += currentRareSeed;
    existingData.legendarySeed += currentLegendarySeed;

    // Save the updated data back to PlayerPrefs
    string updatedJson = JsonUtility.ToJson(existingData);
    PlayerPrefs.SetString("SeedData", updatedJson);
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
            
            currentSunflower = 0;
            currentGaollium = 0;
            currentGerbaras = 0;
            currentRareSeed = 0;
            currentLegendarySeed = 0;
        }
    }



    public ShopItemSlot [] inventorySlots;
    public GameObject InventoryItemPrefab;
    public TextMeshProUGUI leafCountText;
    public int currentLeaf;

    public int currentSunflower;
    public int currentGaollium;
    public int currentGerbaras;
    public int currentRareSeed;
    public int currentLegendarySeed;

    public ShopItem[] itemsToAdd;

    void Start(){
        currentLeaf = PlayerPrefs.GetInt("LeafCount", 0);
        UpdateLeafText();


        if (PlayerPrefs.GetInt("IsShopRefreshed", 0) == 0)
        {
        // This is the first time; refresh the shop
        StartCoroutine(RefreshShopCoroutine());
        
        // Set the flag so that the shop won't auto-refresh again
        PlayerPrefs.SetInt("IsShopRefreshed", 1);
        PlayerPrefs.Save();
        }
        else
        {
            // Load saved shop slots if it's not the first time
            LoadShopSlots();
            LoadShopTexts();
        }

        
        
    }

    private void SaveShopSlots(){
        //loops through all inventory slots
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // grabs the item in the slot with the script ItemInShop
             ItemInShop itemInSlot = inventorySlots[i].GetComponentInChildren<ItemInShop>();

              if (itemInSlot == null)
                {
                    // Debug.Log("Deleting Slot: " + i + " (No item in slot)");
                    PlayerPrefs.DeleteKey("ShopSlot_" + i);  // Delete the saved item for this slot
                }
                else if (itemInSlot.item == null)
                {
                    // Debug.Log("Deleting Slot: " + i + " (ItemInShop exists, but item is null)");
                    PlayerPrefs.DeleteKey("ShopSlot_" + i);  // Delete the saved item for this slot
                }
                else
                {
                    // Save the name of the item in the slot
                    // Debug.Log("Saving Slot: " + i + " (Item: " + itemInSlot.item.name + ")");
                    PlayerPrefs.SetString("ShopSlot_" + i, itemInSlot.item.name);
                }
                }
        PlayerPrefs.Save();
    }

    private void LoadShopTexts(){
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            ShopItemSlot slot = inventorySlots[i].GetComponent<ShopItemSlot>();
            ItemInShop itemInShop = slot.GetComponentInChildren<ItemInShop>();
            ShopItem item = itemInShop.item;

            if(item != null){
                    
                    TextMeshProUGUI[] texts = itemInShop.GetComponentsInChildren<TextMeshProUGUI>();

                if (texts.Length >= 2)
                {
                        // Assuming the first text is for rarity and the second is for price
                    TextMeshProUGUI priceText = texts[0];
                    TextMeshProUGUI rarityText = texts[1];

                        // Update the rarity and price text based on the item's details
                    rarityText.text = item.rarity.ToString() + " seed"; // Display the rarity
                    priceText.text = item.cost + " leaves";   // Display the price
                }
                else
                {
                    Debug.LogWarning("Slot does not have enough TextMeshPro components for rarity and price.");
                }
            }
        }
    }

    private void LoadShopSlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            string itemName = PlayerPrefs.GetString("ShopSlot_" + i, "");

            
                ShopItem item = Array.Find(itemsToAdd, shopItem => shopItem.name == itemName);
                //finds first ShopItem in itemstoadd array where name property matches itemName

                if (item != null)
                {
                    AddItem(item);

                  
                }
            
        }
    }

    

    public void AddSeed(int amount, ShopItemName name){

        switch(name){
            case ShopItemName.Sunflower:
                currentSunflower += amount;
                
            break;

            case ShopItemName.Gaollium:
                currentGaollium += amount;
                
            break;
            case ShopItemName.Gerbaras:
                currentGerbaras += amount;
                
            break;

            case ShopItemName.RareSeedBag:
                currentRareSeed += amount;
               
            break;

            case ShopItemName.LegendarySeedBag:
                currentLegendarySeed += amount;
                
            break;

            default:
                Debug.LogWarning("Unknown rarity type.");
            break;


        }

         SaveSeedData();

        
    }

    

    public bool AddItem(ShopItem item){

        for (int i = 0; i < inventorySlots.Length; i++){
            ShopItemSlot slot = inventorySlots[i];
            ItemInShop itemInSlot = slot.GetComponentInChildren<ItemInShop>();

            if(itemInSlot == null){
                SpawnNewItem(item, slot);

                return true;
            }

        }

        return false;
    }

    void SpawnNewItem(ShopItem item, ShopItemSlot slot)
    {
    
    
    GameObject newItem = Instantiate(InventoryItemPrefab, slot.transform);
    
    ItemInShop inventoryItem = newItem.GetComponent<ItemInShop>();
    
    inventoryItem.InitialiseItem(item);

    
    
    
    }

    public void BoughtItem(int index){
        ShopItemSlot slot = inventorySlots[index];
        ItemInShop itemInShop = slot.GetComponentInChildren<ItemInShop>();
        ShopItem item = itemInShop.item;

        if(currentLeaf >= item.cost){
            MinusLeaf(item.cost);

            switch(item.type){
                case ShopItemType.Seed:
                


                
                AddSeed(1, item.itemName);
               
                
                break;

                case ShopItemType.Background:
                Debug.Log("Purchased BG");
                
                break;
                case ShopItemType.Character:
                Debug.Log("Purchased Character");
                
                break;

                default:
                Debug.Log("error purchase");
                break;

            }

                SaveSeedData();
                ClearSpecificSlot(index);

                
        
                

               

            

            

        }else{
            Debug.Log("Purchase Failed!");
        }
        
    }


    public void RefreshShop(){


         for (int i = 0; i < inventorySlots.Length; i++)
        {
           

              

                 if(currentLeaf >= 50){

                    Debug.Log("Refresh Successful");
                    MinusLeaf(50);
                    StartCoroutine(RefreshShopCoroutine());
                    

                }else{
                    
                    Debug.Log("Purchase Failed!");

                }
              
        }


        
    }

      private IEnumerator RefreshShopCoroutine()
    {
       Debug.Log("Refresh Shop Coroutine");
        ClearAllSlots();

        yield return new WaitForSeconds(0.1f);

        
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            int randomNum = UnityEngine.Random.Range(0, itemsToAdd.Length);
            bool result = AddItem(itemsToAdd[randomNum]);

            if (!result)
            {
                Debug.Log("Failed to add item to slot " + i);
            }
        }


        SaveShopSlots();
        LoadShopTexts();

        

        
    }

    

    private void ClearAllSlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // Clear all children from each slot to reset them
            foreach (Transform child in inventorySlots[i].transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

   private void ClearSpecificSlot(int index)
    {
        // Access the specific slot directly by index
        ShopItemSlot slot = inventorySlots[index];

        // Loop through all children in that slot and destroy them
        foreach (Transform child in slot.transform)
        {
            Destroy(child.gameObject);
        }

        // Start the coroutine to save the shop after a short delay
        StartCoroutine(SaveAfterClear());
    }

    private IEnumerator SaveAfterClear()
    {
        // Small delay to ensure the objects are fully destroyed
        yield return new WaitForEndOfFrame();  // Wait until the next frame

        // Now save the shop slots after clearing
        SaveShopSlots();
        
        Debug.Log("Shop slots saved after clearing.");
    }

    public void UpdateLeafText(){
        leafCountText.text = PlayerPrefs.GetInt("LeafCount", 0).ToString();
    }


    public void AddLeaf(int amount){
        currentLeaf += amount;
        PlayerPrefs.SetInt("LeafCount", currentLeaf);
        PlayerPrefs.Save();
        UpdateLeafText();
    }

    public void MinusLeaf(int amount){
        currentLeaf -= amount;
        PlayerPrefs.SetInt("LeafCount", currentLeaf);
        PlayerPrefs.Save();
        UpdateLeafText();
    }

}
