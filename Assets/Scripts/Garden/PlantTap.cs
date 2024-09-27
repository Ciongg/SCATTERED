using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlantTap : MonoBehaviour
{

    public int tapRequired;
    private float collectibleSpawnChance = 0.2f;
    public int collectibleSpawnAmount;
    public int collectibleSpawnLimit;
    



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
        //set seedname for checking of prefab in gardengamemanager
        //is trimmed of (Clone) tag for proper hcecking
        seedName = gameObject.name.Replace("(Clone)", "").Trim();
        

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
        if (currentTaps >= (int)tapRequired)
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
            donateButton.interactable = true;
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
    // Define a range for the random offset
    float spawnOffsetRange = Random.Range(1f, 1.5f); // Adjust the range as needed

    // Generate a random offset
    Vector2 randomOffset = new Vector2(
        Random.Range(-spawnOffsetRange, spawnOffsetRange),
        Random.Range(-spawnOffsetRange, spawnOffsetRange)
    );

    // Calculate the spawn position by adding the offset to the original position
    Vector2 spawnPosition = position + randomOffset;

    // Instantiate the collectible prefab at the calculated spawn position
    GameObject collectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);

    // Get the Rigidbody2D component of the collectible
    Rigidbody2D collectibleRb = collectible.GetComponent<Rigidbody2D>();
    if (collectibleRb != null)
    {
        // Set either left or right as the direction (X axis only)
        float randomDirectionX = Random.value < 0.5f ? -1f : 1f; // Randomly pick left (-1) or right (1)
        Vector2 horizontalDirection = new Vector2(randomDirectionX, 0f); // No vertical movement (Y = 0)

        // Apply force to the collectible
        float forceMagnitude = Random.Range(3f, 6f); // Adjust force magnitude as needed
        collectibleRb.AddForce(horizontalDirection * forceMagnitude, ForceMode2D.Impulse);
    }
}

   public IEnumerator SpawnBulkPlantCollectible(int amount, Vector2 spawnPosition)
{
    for (int i = 0; i < amount; i++)
    {
        GameObject collectible = Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);

        // Get the Rigidbody2D component of the collectible
        Rigidbody2D collectibleRb = collectible.GetComponent<Rigidbody2D>();
        if (collectibleRb != null)
        {
            // Set a random direction for X (left or right)
            float randomDirectionX = Random.value < 0.5f ? -1f : 1f; // Randomly pick left (-1) or right (1)

            // Set a random upward speed for Y
            float randomDirectionY = Random.Range(1f, 3f); // Random speed for upward movement

            // Create the force vector with both X and Y directions
            Vector2 force = new Vector2(randomDirectionX, randomDirectionY).normalized; // Normalize to keep direction consistent

            // Apply force to the collectible
            float forceMagnitude = Random.Range(3f, 6f); // Adjust force magnitude as needed
            collectibleRb.AddForce(force * forceMagnitude, ForceMode2D.Impulse);
        }

        // Wait for a random interval between 0.1f and 0.2f seconds before spawning the next collectible
        yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
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
