using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int maxStackedItems = 10;
    public InventorySlot [] inventorySlots; //gets all available inventory slots in heirarchy
    public GameObject InventoryItemPrefab; // gets the inventory item prefab


    //Is ran when the player gets an item in demoscript for now
    public bool AddItem(Item item){

        for (int i = 0; i < inventorySlots.Length; i++)
        {

            InventorySlot slot = inventorySlots[i]; 
           
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); 
            
            if(itemInSlot != null && itemInSlot.item == item && itemInSlot.count < maxStackedItems && itemInSlot.item.stackable == true ){
              
                itemInSlot.count++;
                itemInSlot.InitialiseCount();
                
                return true;
            }

        }

        //loops through inventorySlots array
        for (int i = 0; i < inventorySlots.Length; i++)
        {

            //store current inventory slot in loop inside slot var
            InventorySlot slot = inventorySlots[i]; 
            //Grabs the InventoryItem component from the child of slot
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>(); 
            
            //if the grabbed slot has no inventory item attached then the slot is empty
            if(itemInSlot == null){
                //if empty it will spawn new item
                SpawnNewItem(item, slot); 
                //item (parameter), slot (this current slot)

                return true;// stops the loop
            }

        }

        return false;
    }

  



    void SpawnNewItem(Item item, InventorySlot slot){
        //instantiates the itemprefab inside the slot's position stores it in newItem
        GameObject newItem = Instantiate(InventoryItemPrefab, slot.transform);
        //grabs the InventoryItem component from newItem and store it in another variable with the Inventory Item script
        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        //use the InitialiseItem function from the script which grabs the image
        inventoryItem.InitialiseItem(item);
    }
}
