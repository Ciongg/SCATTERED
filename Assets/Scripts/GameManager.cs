using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI leafText;
    public int score = 0;
    public int life = 5;
    public int leaf = 0; //for ingame text
    int currentleaf; //for saved data also shown in menu
    public GameObject smogPrefab;
    public int leafMultiplier;

    public GameObject deathScreenPanel; 
    public GameObject pauseScreenPanel; 
    public TextMeshProUGUI finalScoreText; 
    public TextMeshProUGUI finalLeafText; 


    public int totalTrashThrown= 0;

    public PlayerDataManager playerDataManager;
    

    //currentleaf = menu
    //leaf = game

    //get the currentleaf and put it in the game

    // Start is called before the first frame update
    void Start()
    {
        
        Time.timeScale = 1f; 
        Application.targetFrameRate = 60;
        currentleaf = playerDataManager.GetLeafCount();
        leafMultiplier = playerDataManager.GetLeafMultiplier();

        
        UpdateScoreText();
        UpdateLifeText();
        UpdateLeafText();

        deathScreenPanel.SetActive(false);
        pauseScreenPanel.SetActive(false);

        totalTrashThrown = playerDataManager.GetTrashThrown();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainMenu(){
        SceneManager.LoadSceneAsync(0);
    }



    public void SaveLeafCount(){
        PlayerPrefs.SetInt("LeafCount", currentleaf);
        PlayerPrefs.Save();
    }

    public void AddScore(int points){
        score += points;
        
        UpdateScoreText();
        playerDataManager.UpdateTrashThrown(1);
        // UpdateTrashThrownInDatabase(totalTrashThrown);
        
    }

    // public void UpdateTrashThrownInDatabase(int trashThrown)
    // {
    //     Debug.Log("UPDATING TRASHTHROWN");
    //     string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId; // Get the current user ID

    //     // Set the ecoCoins value in the database
    //     databaseReference.Child("users").Child(userId).Child("trashThrown").SetValueAsync(trashThrown).ContinueWith(task =>
    //     {
    //         if (task.IsCompleted)
    //         {
    //             Debug.Log("trashThrown updated successfully in database.");
    //         }
    //         else
    //         {
    //             Debug.LogError("Failed to update trashThrown: " + task.Exception);
    //         }
    //     });
    // }

    public void LoseLife(int lifeDeduct){
        life -= lifeDeduct;
        if (life <= 0){
            life = 0;
            SaveLeafCount();
            
            ShowDeathScreen();
        }
        UpdateLifeText();
        
    }

    public void ShowDeathScreen()
    {
        
        deathScreenPanel.SetActive(true);

        
        finalScoreText.text = "Final Score: " + score.ToString();
        finalLeafText.text = "Total Leaves: " + leaf.ToString();
 
    }

    public void Pause(){
        pauseScreenPanel.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void Unpause(){
        pauseScreenPanel.SetActive(false);
        Time.timeScale = 1f; 
    }


    
    

    public void AddLeaf(int leafGained, int leafMultiplier){
        leaf += leafGained * leafMultiplier; //adds to game
        int tempLeafCount = leafGained * leafMultiplier; //adds to playerprefs
        playerDataManager.UpdateLeafCount(tempLeafCount, true);
        UpdateLeafText();

    }


    void UpdateScoreText(){
        scoreText.text = score.ToString();
       
    }

   public void UpdateLifeText(){
        lifeText.text = life.ToString();
    }

    void UpdateLeafText(){
        leafText.text = leaf.ToString();
    }

    public int GetScore(){
        return score;
    }

    public void StartSlowTime(float slowTimeDuration)
    {
        StartCoroutine(SlowTimeEffect(slowTimeDuration));
    }

     private IEnumerator SlowTimeEffect(float slowTimeDuration)
    {
       
        Time.timeScale = 0.5f;
        
        yield return new WaitForSecondsRealtime(slowTimeDuration);


        Time.timeScale = 1f;
    }


    public void ActivateSmog(float smogActiveLength)
    {
        GameObject smogInstance = Instantiate(smogPrefab, Vector3.zero, Quaternion.identity); // Instantiate the smog at the center (or set position as needed)
        StartCoroutine(FadeInSmog(smogInstance, smogActiveLength));
    }

    IEnumerator FadeInSmog(GameObject smog, float smogActiveLength)
    {
        // Get the Renderer component to control fading
        Renderer renderer = smog.GetComponent<Renderer>();
        Color color = renderer.material.color; // Get the material color
        color.a = 0f; // Start with smog completely transparent
        renderer.material.color = color; // Apply the transparency

        float fadeDuration = 2f; // Duration for fading in
        float timePassed = 0f;

        while (timePassed < fadeDuration)
        {
            color.a = Mathf.Lerp(0f, 1f, timePassed / fadeDuration); // Fade in effect
            renderer.material.color = color; // Apply the new color
            timePassed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        color.a = 1f; // Ensure the smog is fully opaque
        renderer.material.color = color; // Apply the final color

        yield return new WaitForSeconds(smogActiveLength); 
        StartCoroutine(FadeOutSmog(smog));
    }

    IEnumerator FadeOutSmog(GameObject smog)
    {
        // Get the Renderer component to control fading
        Renderer renderer = smog.GetComponent<Renderer>();
        Color color = renderer.material.color; // Get the material color
        float fadeDuration = 5f; 
        float timePassed = 0f;

        while (timePassed < fadeDuration)
        {
            color.a = Mathf.Lerp(1f, 0f, timePassed / fadeDuration); // Fade out effect
            renderer.material.color = color; // Apply the new color
            timePassed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        color.a = 0f; // Ensure the smog is completely transparent
        renderer.material.color = color; // Apply the final color
        Destroy(smog); // Destroy the smog instance
    }



}
