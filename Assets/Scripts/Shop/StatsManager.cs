using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    private float tapCount;
    private int leafMultiplier;
    private float minusSpawnTime;

    public TextMeshProUGUI tapText;
    public TextMeshProUGUI leafMultiplierText;
    public TextMeshProUGUI minusSpawnTimeText;

    public PlayerDataManager playerDataManager;
      
    void OnEnable(){
        tapCount = playerDataManager.GetTapPower();
        leafMultiplier = playerDataManager.GetLeafMultiplier();
        minusSpawnTime = playerDataManager.GetMinusSpawnTime();

        tapText.text = "Tap Power: " + tapCount.ToString();
        leafMultiplierText.text = "Bonus Leaf: " + leafMultiplier.ToString();
        minusSpawnTimeText.text = "Increased Leaf Spawnrate " + minusSpawnTime.ToString();
    }
}
