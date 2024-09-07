using System;
using System.Collections;
using TMPro;
using UnityEngine;


public class ShopManager : MonoBehaviour
{   
    public ShopItemSlot [] inventorySlots;
    public GameObject InventoryItemPrefab;
    public TextMeshProUGUI leafCountText;
    public int currentLeaf;
    public int currentBasicSeed;
    public int currentRareSeed;
    public int currentLegendarySeed;

    public ShopItem[] itemsToAdd;

    void Start(){
        currentLeaf = PlayerPrefs.GetInt("LeafCount", 0);
        UpdateLeafText();
        RefreshShop();

        //Get the number of seeds based on their rarity as int
        currentBasicSeed = PlayerPrefs.GetInt("BasicSeedCount", 0);
        currentRareSeed = PlayerPrefs.GetInt("RareSeedCount", 0);
        currentLegendarySeed = PlayerPrefs.GetInt("LegendarySeedCount", 0);
        //reload saved shop slots
        LoadShopSlots();
    }

    private void SaveShopSlots(){
        //loops through all inventory slots
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            // grabs the item in the slot with the script ItemInShop
             ItemInShop itemInSlot = inventorySlots[i].GetComponentInChildren<ItemInShop>();

              if (itemInSlot != null)
            {
                PlayerPrefs.SetString("ShopSlot_" + i, itemInSlot.item.name); 
            }
            else
            {
                PlayerPrefs.DeleteKey("ShopSlot_" + i);
            }
        }
        PlayerPrefs.Save();
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

    public void UpdateLeafText(){
        leafCountText.text = PlayerPrefs.GetInt("LeafCount", 0).ToString();
    }


    public void AddLeaf(int amount){
        currentLeaf += amount;
        PlayerPrefs.SetInt("LeafCount", currentLeaf);
        PlayerPrefs.Save();
        UpdateLeafText();
    }

    public void AddSeed(int amount, Rarity rarity){

        switch(rarity){
            case Rarity.Basic:
                currentBasicSeed += amount;
                PlayerPrefs.SetInt("BasicSeedCount", currentBasicSeed);
                PlayerPrefs.Save();
            break;

            case Rarity.Rare:
                currentRareSeed += amount;
                PlayerPrefs.SetInt("RareSeedCount", currentRareSeed);
                PlayerPrefs.Save();
            break;

            case Rarity.Legendary:
                currentLegendarySeed += amount;
                PlayerPrefs.SetInt("LegendarySeedCount", currentLegendarySeed);
                PlayerPrefs.Save();
            break;

            default:
                Debug.LogWarning("Unknown rarity type.");
            break;


        }

        
    }

    public void MinusLeaf(int amount){
        currentLeaf -= amount;
        PlayerPrefs.SetInt("LeafCount", currentLeaf);
        PlayerPrefs.Save();
        UpdateLeafText();
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
                Debug.Log("Purchased Seed");
                AddSeed(1, item.rarity);
                Debug.Log(currentBasicSeed + " " + currentRareSeed + " " + currentLegendarySeed);
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

            

            

        }else{
            Debug.Log("Purchase Failed!");
        }
        
    }


    public void RefreshShop(){
         if(currentLeaf >= 50){
            
            Debug.Log("Refresh Successful");
            MinusLeaf(50);
            StartCoroutine(RefreshShopCoroutine());
         }else{
            Debug.Log("Purchase Failed!");
         }
    }

      private IEnumerator RefreshShopCoroutine()
    {
       
        ClearAllSlots();

        yield return new WaitForSeconds(0.5f);

        
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

}
