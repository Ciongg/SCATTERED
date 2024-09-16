using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;

    public Item[] itemsToPickup;
    public int currentBasicSeed;
    public int currentUncommonSeed;
    public int currentRareSeed;
    public int currentLegendarySeed;

    public void PickupItem(int id){
        bool result = inventoryManager.AddItem(itemsToPickup[id]);
        if(result){
            Debug.Log("item Added");
        }else{
            Debug.Log("Inventory Full");
        }
    }


    void Start(){
        

        currentBasicSeed = PlayerPrefs.GetInt("BasicSeedCount", 0);
        currentUncommonSeed = PlayerPrefs.GetInt("UncommonSeedCount", 0);
        currentRareSeed = PlayerPrefs.GetInt("RareSeedCount", 0);
        currentLegendarySeed = PlayerPrefs.GetInt("LegendarySeedCount", 0);

       
        for (int i = 0; i < currentBasicSeed; i++)
        {
            PickupItem(0);
        }
        for (int i = 0; i < currentUncommonSeed; i++)
        {
            PickupItem(1);
        }
        for (int i = 0; i < currentRareSeed; i++)
        {
            PickupItem(2);
        }
        for (int i = 0; i < currentLegendarySeed; i++)
        {
            PickupItem(3);
        }

        

    }

}
