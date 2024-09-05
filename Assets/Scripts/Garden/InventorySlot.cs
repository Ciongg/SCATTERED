using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData){

        //If inventory slot does not have a child
        if (transform.childCount == 0){
            //Grab the InventoryItem component from the object dragged
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            //Set the parentAfterDrag of that object to this slot
            inventoryItem.parentAfterDrag = transform;
        }
    }
}
