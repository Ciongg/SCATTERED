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


    void OnEnable(){
        tapCount = PlayerPrefs.GetFloat("TapPower", 1);
        leafMultiplier = PlayerPrefs.GetInt("LeafMultiplier", leafMultiplier);
        minusSpawnTime = PlayerPrefs.GetFloat("MinusSpawnTime", 0);

        tapText.text = "Tap Power: " + tapCount.ToString();
        leafMultiplierText.text = "Bonus Leaf: " + leafMultiplier.ToString();
        minusSpawnTimeText.text = "Increased Leaf Spawnrate " + minusSpawnTime.ToString();
    }
}
