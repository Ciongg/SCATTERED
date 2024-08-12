using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotManager : MonoBehaviour
{
    public GameObject plantPrefab;
    private GameObject placedPlant;
    SpriteRenderer sp;
    // Start is called before the first frame update

    void OnMouseDown(){
          if (placedPlant == null) // Check if no pot is currently placed
        {
            placedPlant = Instantiate(plantPrefab, transform.position, Quaternion.identity);
            // sp.enabled = false;
            // You can also set the parent of the pot to this spot if needed
            // placedPot.transform.SetParent(transform);
        }
    }

    void Start()
    {
        sp = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
