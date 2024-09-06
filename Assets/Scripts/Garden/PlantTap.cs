using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlantTap : MonoBehaviour
{

    public int tapRequired;
    public GameObject[] plantStages; // Array to hold plant stages

    public int currentTaps = 0;
    private int currentStage = 0;

    private BoxCollider2D plantCollider;
    private PolygonCollider2D plantGrabCollider;
    private GardenGameManager gameManager;
    private Rigidbody2D rb;
    private Button donateButton;
    private Basket basket;
    private ParticleSystem tapParticleEffect;
    GameObject donateButtonObject;

    public void Start()
    {
        tapParticleEffect = GetComponentInChildren<ParticleSystem>();

        plantCollider = GetComponent<BoxCollider2D>();

        plantGrabCollider = GetComponent<PolygonCollider2D>();
                    
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.gravityScale = 0;

        gameManager = GameObject.Find("GardenGameManager").GetComponent<GardenGameManager>();
         
        basket = GameObject.Find("Basket").GetComponent<Basket>();
         
        donateButtonObject = GameObject.Find("DonateButton");

        UpdatePlantStage(0);

        plantGrabCollider.enabled = false; 
        
        gameManager.isInitialized = false;
        gameManager.isAlreadyPlanted = true;

        donateButton = donateButtonObject.GetComponent<Button>();
        donateButton.interactable = false;
        donateButton.onClick.AddListener(OnDonateButtonClicked);

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
        }

        //second check
        if (currentTaps >= tapRequired)
        {
            Debug.Log("Tap requirement met!");

            // Enable the donate button since requirement is met
            donateButton.interactable = true;
        }
    }


    private void PlayTapParticleEffect(Vector2 position){
        
        ParticleSystem particleInstance = Instantiate(tapParticleEffect, position, Quaternion.identity);
        particleInstance.Play();

        Destroy(particleInstance.gameObject, 1.5f);
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

    private void UpdatePlantStage(int growthStage)
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
