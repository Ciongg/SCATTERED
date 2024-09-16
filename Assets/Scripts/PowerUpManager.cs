using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PowerUpManager : MonoBehaviour
{
    public TextMeshProUGUI tapCountText;
    public TextMeshProUGUI leafMultiplierText;
    public TextMeshProUGUI minusSpawnText;

    public float tapCount;
    public int leafMultiplier;
    public float minusSpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        tapCount = PlayerPrefs.GetFloat("TapPower", tapCount);
        tapCountText.text = "Tap Power: " + tapCount.ToString("F2");

        leafMultiplier = PlayerPrefs.GetInt("LeafMultiplier", leafMultiplier);
        leafMultiplierText.text = "Leaf Gain: " + leafMultiplier.ToString();

        minusSpawnTime = PlayerPrefs.GetFloat("MinusSpawnTime", 0);
        minusSpawnText.text = "Lessened Spawnrate: " + minusSpawnTime.ToString();
        
    }

    public void IncreaseTap(float amount){
        tapCount += amount;
        PlayerPrefs.SetFloat("TapPower", tapCount);
        PlayerPrefs.Save();
        tapCountText.text = "Tap Power: " + tapCount.ToString("F2");
        
    }

    public void IncreaseLeafMultiplier(int amount){
        leafMultiplier += amount;
        PlayerPrefs.SetInt("LeafMultiplier", leafMultiplier);
        PlayerPrefs.Save();
        leafMultiplierText.text = "Leaf Gain: " + leafMultiplier.ToString();

    }

    public void IncreaseMinusSpawnTime(float amount){
        minusSpawnTime += amount;
        PlayerPrefs.SetFloat("MinusSpawnTime", minusSpawnTime);
        PlayerPrefs.Save();
        minusSpawnText.text = "Lessened Spawnrate: " + minusSpawnTime.ToString();
        

    }



    // public void UpdateTapText(){
    //     PlayerPrefs.SetFloat("TapPower", tapCount);
    //     PlayerPrefs.Save();
    //     tapCountText.text = "lvl: " + tapCount.ToString();
    //     Debug.Log("tapCountClicked");
    // }
}
