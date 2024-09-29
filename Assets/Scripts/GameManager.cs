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
    
    public int leafMultiplier;

    public GameObject deathScreenPanel; 
    public GameObject pauseScreenPanel; 
    public TextMeshProUGUI finalScoreText; 
    public TextMeshProUGUI finalLeafText; 

    //currentleaf = menu
    //leaf = game

    //get the currentleaf and put it in the game

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f; 
        Application.targetFrameRate = 60;
        currentleaf = PlayerPrefs.GetInt("LeafCount", 0);
        leafMultiplier = PlayerPrefs.GetInt("LeafMultiplier", 1);

        
        UpdateScoreText();
        UpdateLifeText();
        UpdateLeafText();

        deathScreenPanel.SetActive(false);
        pauseScreenPanel.SetActive(false);

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
    }

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
        currentleaf += leafGained * leafMultiplier; //adds to playerprefs
        UpdateLeafText();

    }


    void UpdateScoreText(){
        scoreText.text = score.ToString();
    }

    void UpdateLifeText(){
        lifeText.text = life.ToString();
    }

    void UpdateLeafText(){
        leafText.text = leaf.ToString();
    }

    public int GetScore(){
        return score;
    }

}
