using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GardenGameManager : MonoBehaviour
{

    public class PlantData
    {
        public string seedType;
        public int currentTaps;
        public int growthStage;
    }

    //json stuff
    private const string FileName = "plantData.json";
    public string filePath;


    public int tapPower = 1;
    private PlantTap plantTap;
    public TextMeshProUGUI countText;
    private GameObject pot;
    private Transform spawn;
    private Transform plantPrefab;

    public bool isInitialized;
    public bool isAlreadyPlanted;

    public GameObject [] plantPrefabList; // list of prefabsPlants

    //on load save data
        void OnDestroy() 
    {
        SavePlantData();
    }

    void OnApplicationQuit() 
    {
        SavePlantData();
    }


    void Start()
    {
        //initialize pot and spawn 
        pot = GameObject.Find("Pot");
        spawn = pot.transform.Find("Spawn");

        
        isInitialized = false;
        isAlreadyPlanted = false;

        filePath = Path.Combine(Application.persistentDataPath, FileName);

        LoadPlantData();

        if(!isInitialized){
            countText.enabled = false;
        }else{
            countText.enabled = true;
            UpdateCount();
        }
    }


    private int FindSeedPrefabByName(string seedName)
    {
        // Loop through all prefabs in the list
        for (int i = 0; i < plantPrefabList.Length; i++)
        {
            // If the prefab  name is same as seedName of plant, return the prefab using index
            if (plantPrefabList[i].name == seedName)
            {
                return i;
            }
        }

        Debug.Log("No matching prefab found for seed name: " + seedName);
        return -1; 
    }



     public void SavePlantData()
    {
        if (plantTap != null)
        {
            PlantData data = new PlantData
            {
                seedType = plantTap.seedName,
                currentTaps = plantTap.currentTaps,
                growthStage = plantTap.GetCurrentGrowthStage()
            };

            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, jsonData);
            // Debug.Log("Plant data saved with currentTaps: " + plantTap.currentTaps);
        }
    }

   public void LoadPlantData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            PlantData data = JsonUtility.FromJson<PlantData>(jsonData);

            int seedPrefab = FindSeedPrefabByName(data.seedType);

            GameObject plant = Instantiate(plantPrefabList[seedPrefab], spawn.position, Quaternion.identity);
            plant.transform.SetParent(spawn.transform);

            plantTap = plant.GetComponent<PlantTap>();

            if (plantTap != null)
            {
                plantTap.currentTaps = data.currentTaps;
                
                plantTap.UpdatePlantStage(data.growthStage);

                isAlreadyPlanted = true;
                isInitialized = true;
                Debug.Log("Plant data loaded!");
            }
            else
            {
                Debug.LogError("PlantTap component not found on the instantiated plant prefab");
            }
        }
        else
        {
            Debug.LogError("No saved plant data found");
        }
    }


    void Update()
    {
        // Only run the initialization logic if it hasn't been done yet
        if (!isInitialized)
        {
            // Find the Pot object in the scene
            
                if (spawn != null && spawn.childCount > 0) // Check if spawn exists and has children
                {
                    plantPrefab = spawn.GetChild(0); // Get the first child under spawn
                    
                    
                    plantTap = plantPrefab.GetComponent<PlantTap>();

                        if (plantTap != null)
                        {
                            
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

        // Debug.Log(isInitialized);
    }


    public void UpdateCount(){
        countText.text = plantTap.currentTaps.ToString();
        
    }

    public void InitializeCount(){
        countText.enabled = true;
        countText.text = "0";
    }
    
}
