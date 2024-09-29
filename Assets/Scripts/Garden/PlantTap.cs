using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlantTap : MonoBehaviour
{

    public int tapRequired;
    private float collectibleSpawnChance = 0.25f;
    public int collectibleSpawnAmount;
    public int collectibleSpawnLimit;
    public int donateCollectibleSpawnAmount;

    private GameObject pot;
    
    


    public GameObject collectiblePrefab;
    private BoxCollider2D plantCollider;
    private PolygonCollider2D plantGrabCollider;
    private GardenGameManager gameManager;
    private Rigidbody2D rb;
    private Button donateButton;
    private Basket basket;
    private ParticleSystem tapParticleEffect;
    GameObject donateButtonObject;
    public GameObject[] plantStages; // Array to hold plant stages

    [HideInInspector]
    public int currentStage = 0;
    [HideInInspector]
    public string seedName;
    [HideInInspector]
    public float currentTaps = 0;

    public int remainingCollectiblesSpawned = 0;
    

    public int GetCurrentGrowthStage()
    {
        return currentStage;
    }

    public void SetGrowthStage(int growthStage)
    {
        currentStage = growthStage;
        UpdatePlantStage(growthStage);
    }



    public void Start()
    {
        Debug.Log(remainingCollectiblesSpawned);
        //set seedname for checking of prefab in gardengamemanager
        //is trimmed of (Clone) tag for proper hcecking
        seedName = gameObject.name.Replace("(Clone)", "").Trim();
        
        pot = GameObject.Find("Pot");
        //Disble rigidbody movements but still have it enabled
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.gravityScale = 0;
        
        tapParticleEffect = GetComponentInChildren<ParticleSystem>();
        plantCollider = GetComponent<BoxCollider2D>();
        plantGrabCollider = GetComponent<PolygonCollider2D>();

        gameManager = GameObject.Find("GardenGameManager").GetComponent<GardenGameManager>();
        basket = GameObject.Find("Basket").GetComponent<Basket>();
        donateButtonObject = GameObject.Find("DonateButton");
        donateButton = donateButtonObject.GetComponent<Button>();

        
        //Initializations
        plantGrabCollider.enabled = false; 
        gameManager.isInitialized = false;
        gameManager.isAlreadyPlanted = true;
        
        if(currentTaps >= tapRequired){
        donateButton.interactable = true;
        }else{
        donateButton.interactable = false;
        }

        donateButton.onClick.AddListener(OnDonateButtonClicked);
        
        gameManager.countText.enabled = true;
        gameManager.UpdateCount();
        gameManager.SavePlantData();

        
        
         

    }

    

    void Update()
    {
        bool isTouchInput = Input.touchCount > 0;


        // Handle mouse and touch input
        if (Input.GetMouseButtonDown(0) && !isTouchInput)
        {
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (plantCollider == Physics2D.OverlapPoint(touchPosition))
            {
                IncrementTap(touchPosition);
            }
        }

         if (isTouchInput)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                if (plantCollider == Physics2D.OverlapPoint(touchPosition))
                {
                    IncrementTap(touchPosition);
                }
            }
        }

        // Update the plant stage based on tap progress
        float tapPercentage = (float)currentTaps / tapRequired;

        if (tapPercentage >= 0.85f && currentStage < 2)
        {
            currentStage = 2;
            UpdatePlantStage(currentStage);
        }
        else if (tapPercentage >= 0.5f && currentStage < 1)
        {
            currentStage = 1;
            UpdatePlantStage(currentStage);
        }
    }

   

    private void IncrementTap(Vector2 tapPosition)
    {
        
        //initial check
        if (currentTaps >= tapRequired)
        {
            

            
            donateButton.interactable = true;
            
        }else {
            
            currentTaps += gameManager.tapPower;;
            gameManager.UpdateCount();
            PlayTapParticleEffect(tapPosition);

            if (Random.value < collectibleSpawnChance && collectibleSpawnAmount < collectibleSpawnLimit) // Random.value gives a value between 0.0 and 1.0
            {
                collectibleSpawnAmount++;
                SpawnCollectible(tapPosition);
            }

            
        }

        //second check
        if (currentTaps >= tapRequired)
        {
            Debug.Log("Tap requirement met!");

            // Enable the donate button since requirement is met

            if(remainingCollectiblesSpawned == 0){

            int remainingCollectibles = collectibleSpawnLimit - collectibleSpawnAmount;
                for (int i = 0; i < remainingCollectibles; i++)
                {
                    SpawnCollectible(pot.transform.position);
                }
                
                    

                    
            }

            remainingCollectiblesSpawned = 1;
            
            donateButton.interactable = true;
            
            PlayerPrefs.SetInt("remainingCollectiblesSpawned", 1);
            PlayerPrefs.Save();
               
                
                
            
           
        }

        gameManager.SavePlantData();
    }


    private void PlayTapParticleEffect(Vector2 position){
        
        ParticleSystem particleInstance = Instantiate(tapParticleEffect, position, Quaternion.identity);
        particleInstance.Play();

        Destroy(particleInstance.gameObject, 1.5f);
    }

   private void SpawnCollectible(Vector2 position)
{
    
    float spawnOffsetRange = Random.Range(0.1f, 0.4f);

    // Generate a random offset
    Vector2 randomOffset = new Vector2(
        Random.Range(-spawnOffsetRange, spawnOffsetRange),
        Random.Range(-spawnOffsetRange, spawnOffsetRange)
    );

    
    Vector2 spawnPosition = position + randomOffset;

    
    GameObject collectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);

    
    Rigidbody2D collectibleRb = collectible.GetComponent<Rigidbody2D>();
    if (collectibleRb != null)
    {
      
        float randomAngle = Random.Range(0, 360);
        
        // Convert angle to direction vector
        Vector2 direction = new Vector2(Mathf.Cos(randomAngle * Mathf.Deg2Rad), Mathf.Sin(randomAngle * Mathf.Deg2Rad)).normalized;

        // Apply force to the collectible
        float forceMagnitude = Random.Range(1f, 5f); 
        collectibleRb.AddForce(direction * forceMagnitude, ForceMode2D.Impulse);
    }
}


  public IEnumerator SpawnBulkPlantCollectible(int amount, Vector2 spawnPosition)
{
    for (int i = 0; i < amount; i++)
    {
        
        GameObject collectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);

        
        Rigidbody2D collectibleRb = collectible.GetComponent<Rigidbody2D>();
        if (collectibleRb != null)
        {
            // Generate random X direction between -1 (left) and 1 (right)
            float randomDirectionX = Random.Range(-2f, 2f);

            // Ensure Y is always upwards (between 0.5 and 1 for upward direction)
            float randomDirectionY = Random.Range(0.5f, 4f);

            // Create the direction vector with both X and Y components
            Vector2 forceDirection = new Vector2(randomDirectionX, randomDirectionY).normalized;

            // Apply a random force magnitude
            float forceMagnitude = Random.Range(3f, 6f); // Adjust force magnitude as needed
            collectibleRb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Impulse);
        }

        
        yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
    }
}



    




    private void OnDonateButtonClicked(){

        // Disable this script to stop counting taps PlantTap.cs
        enabled = false;

        // Enable the drag-and-drop script PlantDragAndDrop.cs
        PlantDragAndDrop dragAndDrop = GetComponent<PlantDragAndDrop>();
        
        dragAndDrop.enabled = true;
        

        //enable rigidbody gravity and constraints
        rb.gravityScale = 1;
        rb.constraints = RigidbodyConstraints2D.None;

        //swap colliders from the tap collider (box)
        //to the collider for the drag and drop (polygon)
        plantGrabCollider.enabled = true;
        plantCollider.enabled = false;

        // disable donatebutton and slide in basket
        donateButton.interactable = false;
        
        basket.SlideIn();
        
    }

    public void UpdatePlantStage(int growthStage)
    {
        // Disable all stages first
        for (int i = 0; i < plantStages.Length; i++)
        {
            plantStages[i].SetActive(false);
        }

        // enable the correct stage
        if (growthStage < plantStages.Length)
        {
            plantStages[growthStage].SetActive(true);
        }
    }
}
