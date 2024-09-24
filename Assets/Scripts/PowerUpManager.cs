using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PowerUpManager : MonoBehaviour
{
    
    public TextMeshProUGUI tapCostText;
    public TextMeshProUGUI leafCostText;
    public TextMeshProUGUI minusSpawnCostText;

    public TextMeshProUGUI tapLvlText;
    public TextMeshProUGUI leafLvlText;
    public TextMeshProUGUI minusSpawnLvlText;


    public TextMeshProUGUI leafCountText;
    public TextMeshProUGUI shopLeafCountText;

    public float tapCount;
    public int leafMultiplier;
    public float minusSpawnTime;

    public int IncreaseTapCost;
    public int IncreaseLeafMultiplierCost;
    public int IncreaseMinusSpawnTimeCost;

    public int IncreaseTapLvl;
    public int IncreaseLeafMultiplierLvl;
    public int IncreaseMinusSpawnTimeLvl;

    public ShopManager shopManager;

    int leafCount;
    // Start is called before the first frame update
    void OnEnable()
    {
        tapCount = PlayerPrefs.GetFloat("TapPower", 1);
        leafMultiplier = PlayerPrefs.GetInt("LeafMultiplier", leafMultiplier);
        minusSpawnTime = PlayerPrefs.GetFloat("MinusSpawnTime", 0);
       
        leafCount = PlayerPrefs.GetInt("LeafCount", 0);

        IncreaseTapLvl = PlayerPrefs.GetInt("IncreaseTapLvl", 1);
        IncreaseLeafMultiplierLvl = PlayerPrefs.GetInt("IncreaseLeafMultiplierLvl", 1);
        IncreaseMinusSpawnTimeLvl = PlayerPrefs.GetInt("IncreaseMinusSpawnTimeLvl", 1);

        IncreaseTapCost = PlayerPrefs.GetInt("IncreaseTapCost", 10); // Initial cost
        IncreaseLeafMultiplierCost = PlayerPrefs.GetInt("IncreaseLeafMultiplierCost", 10); // Initial cost
        IncreaseMinusSpawnTimeCost = PlayerPrefs.GetInt("IncreaseMinusSpawnTimeCost", 10); // Initial cost

        tapCostText.text = "Cost: " + IncreaseTapCost;
        tapLvlText.text = "Lvl:" + IncreaseTapLvl;

        leafCostText.text = "Cost: " + IncreaseLeafMultiplierCost;
        leafLvlText.text = "Lvl: " + IncreaseLeafMultiplierLvl;

        minusSpawnCostText.text = "Cost: " + IncreaseMinusSpawnTimeCost;
        minusSpawnLvlText.text = "Lvl: " + IncreaseMinusSpawnTimeLvl;

        leafCountText.text = leafCount.ToString();

        
        
    }

    void OnDisable(){
        PlayerPrefs.SetInt("leafCount", leafCount);
        
    }

    public void IncreaseTap(float amount){
        if(leafCount >= IncreaseTapCost){
        tapCount = Mathf.Round((tapCount + amount) * 100f) / 100f;
        leafCount -= IncreaseTapCost;
        shopManager.MinusLeaf(IncreaseTapCost);
        leafCountText.text = leafCount.ToString();
        IncreaseTapLvl++;

        PlayerPrefs.SetFloat("TapPower", tapCount);
        PlayerPrefs.SetInt("LeafCount", leafCount);
        PlayerPrefs.SetInt("IncreaseTapLvl", IncreaseTapLvl);
        tapCostText.text = "Cost: " + IncreaseTapCost;
        tapLvlText.text = "Lvl:" + IncreaseTapLvl;

        CheckForCostIncrease(ref IncreaseTapLvl, ref IncreaseTapCost, "IncreaseTapCost", 10f);
        PlayerPrefs.Save();
        }
        
    }

    public void IncreaseLeafMultiplier(int amount){
        if(leafCount >= IncreaseLeafMultiplierCost){
        leafMultiplier += amount;
        leafCount -= IncreaseLeafMultiplierCost;
        shopManager.MinusLeaf(IncreaseLeafMultiplierCost);
        leafCountText.text = leafCount.ToString();
        IncreaseLeafMultiplierLvl++;

        PlayerPrefs.SetInt("LeafMultiplier", leafMultiplier);
        PlayerPrefs.SetInt("LeafCount", leafCount);
        PlayerPrefs.SetInt("IncreaseLeafMultiplierLvl", IncreaseLeafMultiplierLvl);
        leafCostText.text = "Cost: " + IncreaseLeafMultiplierCost;
        leafLvlText.text = "Lvl: " + IncreaseLeafMultiplierLvl;
        CheckForCostIncrease(ref IncreaseLeafMultiplierLvl, ref IncreaseLeafMultiplierCost, "IncreaseLeafMultiplierCost", 20f);
        
        PlayerPrefs.Save();
        }
    }

    public void IncreaseMinusSpawnTime(float amount){
        if(leafCount >= IncreaseMinusSpawnTimeCost){
        minusSpawnTime = Mathf.Round((minusSpawnTime + amount) * 100f) / 100f;
        leafCount -= IncreaseMinusSpawnTimeCost;
        shopManager.MinusLeaf(IncreaseMinusSpawnTimeCost);
        leafCountText.text = leafCount.ToString();
        IncreaseMinusSpawnTimeLvl++;
        PlayerPrefs.SetFloat("MinusSpawnTime", minusSpawnTime);
        PlayerPrefs.SetInt("LeafCount", leafCount);
        PlayerPrefs.SetInt("IncreaseMinusSpawnTimeLvl", IncreaseMinusSpawnTimeLvl);
        minusSpawnCostText.text = "Cost: " + IncreaseMinusSpawnTimeCost;
        minusSpawnLvlText.text = "Lvl: " + IncreaseMinusSpawnTimeLvl;
        CheckForCostIncrease(ref IncreaseMinusSpawnTimeLvl, ref IncreaseMinusSpawnTimeCost, "IncreaseMinusSpawnTimeCost", 10f);
        
        PlayerPrefs.Save();
        }

    }

     void CheckForCostIncrease(ref int level, ref int cost, string costKey, float percentageIncrease)
    {
        
        if (level % 1 == 0)
        {
            cost += Mathf.RoundToInt(cost * percentageIncrease / 100f);
            PlayerPrefs.SetInt(costKey, cost);
        }
    }

    // public void UpdateTapText(){
    //     PlayerPrefs.SetFloat("TapPower", tapCount);
    //     PlayerPrefs.Save();
    //     tapCountText.text = "lvl: " + tapCount.ToString();
    //     Debug.Log("tapCountClicked");
    // }
}
