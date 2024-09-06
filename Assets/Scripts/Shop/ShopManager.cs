using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{   
    public ShopItemSlot [] inventorySlots;
    public GameObject InventoryItemPrefab;

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
    Debug.Log("Spawning " + item + " at " + slot);
    GameObject newItem = Instantiate(InventoryItemPrefab, slot.transform);
    

    ItemInShop inventoryItem = newItem.GetComponent<ItemInShop>();
    
        inventoryItem.InitialiseItem(item);
    
    
}

}
