using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public GameObject seedlingState;
    public GameObject grownPlantState1;
    public GameObject grownPlantStateFinal;
    
    private int growthStage = 0;

    void Start()
    {
        // Initially disable all states except the first one
        seedlingState.SetActive(false);
        grownPlantState1.SetActive(false);
        grownPlantStateFinal.SetActive(false);
        
    }

    void OnMouseDown(){
        WaterPot(growthStage);
        growthStage++;
    }

    public void WaterPot(int growthStage)
    {
        
        
       
            seedlingState.SetActive(growthStage == 0);
            
            grownPlantState1.SetActive(growthStage == 1);
            
       
            grownPlantStateFinal.SetActive(growthStage == 2);
            
        
    }
}
