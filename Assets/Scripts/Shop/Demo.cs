using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    public ShopManager inventoryManager;
    public ShopItemSlot [] slots;
    public ShopItem[] itemsToAdd;

    public void RefreshShop(){

        StartCoroutine(RefreshShopCoroutine());
    }

      private IEnumerator RefreshShopCoroutine()
    {
       
        ClearAllSlots();

        
        yield return new WaitForSeconds(0.5f);

        
        for (int i = 0; i < slots.Length; i++)
        {
            int randomNum = Random.Range(0, itemsToAdd.Length);
            bool result = inventoryManager.AddItem(itemsToAdd[randomNum]);

            if (!result)
            {
                Debug.Log("Failed to add item to slot " + i);
            }
        }
    }

    private void ClearAllSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // Clear all children from each slot to reset them
            foreach (Transform child in slots[i].transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
