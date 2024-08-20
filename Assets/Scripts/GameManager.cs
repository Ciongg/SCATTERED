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
    int currentleaf; //for saved data also shown i nmenu

    //currentleaf = menu
    //leaf = game

    //get the currentleaf and put it in the game

    // Start is called before the first frame update
    void Start()
    {
        currentleaf = PlayerPrefs.GetInt("LeafCount", 0);
        UpdateScoreText();
        UpdateLifeText();
        UpdateLeafText();
        

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
            SceneManager.LoadSceneAsync(0);
        }
        UpdateLifeText();
        
    }

    public void AddLeaf(int leafGained){
        leaf += leafGained; //adds to game
        currentleaf += leafGained; //adds to playerprefs
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

     public int GetLife()
    {
        return life;
    }

    public int GetLeaf(){
        return leaf;
    }
}
