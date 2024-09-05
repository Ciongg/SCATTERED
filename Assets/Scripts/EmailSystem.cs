using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmailSystem : MonoBehaviour
{
    public int emailCount = 0;
    public TextMeshProUGUI emailText;   
    public TextMeshProUGUI timerText;  // Add a new TextMeshProUGUI for the timer
    public Button deleteButton;
    float nextSpawnTime;
    public float baseTimeBetweenSpawn = 10f; 
    public float emailDeletionTime = 5f;
    public GameManager gameManager;
    float emailTimeToDelete;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        UpdateEmailText();
        
        deleteButton.onClick.AddListener(DeleteEmail);
    }

    void Update()
    {


    if (Time.time > nextSpawnTime){
        int email = GenerateEmail();
        emailCount += email;
        nextSpawnTime = Time.time + baseTimeBetweenSpawn;
        UpdateEmailText();
    }

    if(emailCount > 0){
        float timeRemaining = Mathf.Max(emailTimeToDelete - Time.time, 0);
        UpdateTimerText(timeRemaining);

        if(timeRemaining <= 0){
            emailTimeToDelete = Time.time + emailDeletionTime;
            gameManager.LoseLife(1);
        }

        
    }


    if(emailCount == 0 ){
        emailTimeToDelete = Time.time + emailDeletionTime;
    }


    //     // Spawning emails
    //     if (Time.time > nextSpawnTime && emailCount >= 0)
    //     {
    //         int email = GenerateEmail();
    //         emailCount += email;
            
    //         emailTimeToDelete = Time.time + emailDeletionTime;
    //         nextSpawnTime = Time.time + baseTimeBetweenSpawn;
    //         UpdateEmailText();
    //         Debug.Log("Generated Emails: " + emailCount);
    //     }

    //     // Update the countdown timer and handle email deletion logic
    //     if (emailCount > 0)
    //     {
    //         float timeRemaining = Mathf.Max(emailTimeToDelete - Time.time, 0);
    //         UpdateTimerText(timeRemaining);

    //         // Handle the email deletion timer
    //         if (timeRemaining <= 0)
    //         {
    //             if (emailCount > 0)
    //             {
    //                 gameManager.LoseLife(1);  // Lose a life if emails remain
    //                 Debug.Log("Email minus life");
    //             }
                
    //             // Reset for the next batch of emails
    //             emailTimeToDelete = Time.time + emailDeletionTime;  // Reset timer for the next round
    //             Debug.Log("Timer reset for next batch");
    //         }
    //     }

    //     // Allow new emails to spawn if all emails are cleared
    //     if (emailCount == 0)
    //     {
    //         Debug.Log("All Emails Cleared!");
    //         emailTimeToDelete = Time.time + emailDeletionTime;  // Prepare for the next batch of emails
    //     }
    }


    int GenerateEmail()
    {
        int emailCount = Random.Range(1, 5); 
        UpdateEmailText();
        return emailCount;
    }

    void DeleteEmail()
    {
        if (emailCount > 0)
        {
            emailCount--;
        }
        UpdateEmailText();
    }

    void UpdateEmailText()
    {
        emailText.text = emailCount + " unwanted emails";
    }

    void UpdateTimerText(float timeRemaining)
    {
        timerText.text = "Time left: " + timeRemaining.ToString("F2") + " seconds";
    }
}
