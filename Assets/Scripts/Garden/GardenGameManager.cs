using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GardenGameManager : MonoBehaviour
{
    public int tapPower = 1;
    private PlantTap plantTap;
    public TextMeshProUGUI countText;
    private GameObject pot;
    private Transform spawn;
    private Transform plantPrefab;

    public bool isInitialized;
    public bool isAlreadyPlanted;
    void Start()
    {
        countText.enabled = false;
        isInitialized = false;
        isAlreadyPlanted = false;
    }

    void Update()
    {
        // Only run the initialization logic if it hasn't been done yet
        if (!isInitialized)
        {
            // Find the Pot object in the scene
            pot = GameObject.Find("Pot");

        
            spawn = pot.transform.Find("Spawn");

                if (spawn != null && spawn.childCount > 0) // Check if spawn exists and has children
                {
                    plantPrefab = spawn.GetChild(0); // Get the first child under spawn
                    
                    
                    plantTap = plantPrefab.GetComponent<PlantTap>();

                        if (plantTap != null)
                        {
                            InitializeCount();
                            isAlreadyPlanted = true;
                            isInitialized = true;
                             // Set the flag to true to prevent further runs
                        }
                        else
                        {
                            Debug.LogError("PlantTap component not found on the prefab!");
                        }
                    
                }
            
            
        }else{
            
            
            
        }

        Debug.Log(isInitialized);
    }


    public void UpdateCount(){
        countText.text = plantTap.currentTaps.ToString();
    }

    public void InitializeCount(){
        countText.enabled = true;
        countText.text = "0";
    }
    
}
