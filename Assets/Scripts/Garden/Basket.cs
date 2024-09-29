using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Basket : MonoBehaviour
{
    public float slideDuration = 1.0f; // Duration of the slide animation in seconds
    public Vector2 startOffScreenPosition; // Position off-screen to the right
    public Vector2 endOnScreenPosition; // Target position on-screen

    private RectTransform rectTransform;
    public BoxCollider2D boxcollider;
    public GardenGameManager gameManager;
    public PlantTap plantTap;
    private GameObject pot;
    private Transform spawn;

    
    void Start()
    {
        
        rectTransform = GetComponent<RectTransform>();
        
        // Start the basket off-screen
        rectTransform.anchoredPosition = startOffScreenPosition;
    }

    public void OnTriggerEnter2D(Collider2D collider){
        gameManager.isInitialized = false;

         FindPlantTap();

        switch(collider.tag){
            case "gaollium":
                Debug.Log("I got an" + collider.tag);
                Destroy(collider.gameObject);
                 if (plantTap != null)
                {
                    StartCoroutine(plantTap.SpawnBulkPlantCollectible(plantTap.donateCollectibleSpawnAmount, pot.transform.position));
                    
                    
                }
                
            break;

            case "sunflower":
                Debug.Log("I got an" + collider.tag);
                Destroy(collider.gameObject);
                 if (plantTap != null)
                {
                    StartCoroutine(plantTap.SpawnBulkPlantCollectible(plantTap.donateCollectibleSpawnAmount, pot.transform.position));
                }
            break;

            case "gerbaras":
                Debug.Log("I got an" + collider.tag);
                Destroy(collider.gameObject);
                 if (plantTap != null)
                {
                    StartCoroutine(plantTap.SpawnBulkPlantCollectible(plantTap.donateCollectibleSpawnAmount, pot.transform.position));
                }
            break;

            case "ecocoin":
            Debug.Log("I got an" + collider.tag);
            return;
            
          

        }
                // PlayerPrefs.SetInt("remainingCollectiblesSpawned", 0);
                // PlayerPrefs.Save();
                SlideOut();
                ClearPlantDataFile();

                
    }

     private void FindPlantTap()
    {
        // Attempt to find PlantTap in the hierarchy when needed
        pot = GameObject.Find("Pot");
        spawn = pot.transform.Find("Spawn");
        
        plantTap = spawn.GetComponentInChildren<PlantTap>();
    }

    private void ClearPlantDataFile()
{
    if (File.Exists(gameManager.filePath))
    {
        File.Delete(gameManager.filePath);
        Debug.Log("Plant data file cleared.");
    }
}
    public void SlideIn()
    {
        
        gameManager.countText.enabled = false;
        // Start sliding animation
        StartCoroutine(SlideToPosition(endOnScreenPosition, slideDuration));
    }

    public void SlideOut(){
        gameManager.isAlreadyPlanted = false;
        StartCoroutine(SlideToPosition(startOffScreenPosition, slideDuration));
    }

    private IEnumerator SlideToPosition(Vector2 targetPosition, float duration)
    {
        Vector2 initialPosition = rectTransform.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Interpolate position
            rectTransform.anchoredPosition = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Make sure the basket reaches the target position
        rectTransform.anchoredPosition = targetPosition;
    }
}
