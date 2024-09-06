 
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using JetBrains.Annotations;
using System;
using TMPro;
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    
    public Item item; //is set later in Inventory Manager
    public TextMeshProUGUI countText;
    [HideInInspector]public int count = 1;
    [HideInInspector]public Image image;
    [HideInInspector]public Transform parentAfterDrag; //the transform position of the object before drag

    private PlantPot plantPot;
    GardenGameManager gameManager;
    public void Start()
    {
        plantPot = GameObject.Find("Pot").GetComponent<PlantPot>();
        gameManager = GameObject.Find("GardenGameManager").GetComponent<GardenGameManager>();
        
    }

    // Function to grab scriptable object as parameter and grab its image
    public void InitialiseCount(){
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    public void InitialiseItem(Item newItem){
        item = newItem;
        image.sprite = newItem.image;
        InitialiseCount();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    image.raycastTarget = false; //prevents the dragged object blockin raycasts to other obj
    
    if (countText != null)
    {
        countText.raycastTarget = false; // Disable raycast on the countText during drag
    }
    
    parentAfterDrag = transform.parent; // Store the current parent of the obj before drag

    // set parent of dragged object to the root of the heirarchy (canvas)
    // so the drag object does not get clipped by any UI elements.
    transform.SetParent(transform.root); 
    

    //makes the dragged object appear ontop fo all oth  er obj
    //by setting it as the last sibling in its new parent.
    transform.SetAsLastSibling(); 

    }

    public void OnDrag(PointerEventData eventData)
    {
        
        transform.position = Input.mousePosition; //set position of dragged object to cursor
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        image.raycastTarget = true; // re-enable raycast
        if (countText != null)
        {
                countText.raycastTarget = true; // Re-enable raycast on the countText
        }

        
        if(IsOverPlantPot() && gameManager.isAlreadyPlanted == false){
             Debug.Log("PLANTED SEED!");
            switch (item.type)
        {
            case ItemType.Seed:
                
                 plantPot.PlantSeed(item.type);
                break;

            case ItemType.Tool:
                
                break;

            default:
                Debug.LogWarning("Unhandled item type!");
                break;
        }
            
            if(count > 1){
                transform.SetParent(parentAfterDrag); 
                count--;
                InitialiseCount();
                
            }else{
                
                Destroy(gameObject);
            }
            
            
            
        }else{
        Debug.Log("COULDNT PLANT");
        transform.SetParent(parentAfterDrag); 
        //return dragged object to original parent stored earlier
        //unless InventorySlot Script is ran
        }

        
    }

  

    private bool IsOverPlantPot(){
            // Convert mouse position to world point
    Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    
    // Perform the raycast
    RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
    
    if (hit.collider != null)
    {
        
        
        
        if (hit.collider.CompareTag("PlantPot")) // Ensure your plant pot has the "PlantPot" tag
        {
            
            return true;
        }
    }

    return false;
    
    }

    
}
