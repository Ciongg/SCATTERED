using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI lifeText;
    public int score = 0;
    public int life = 3;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        UpdateLifeText();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddScore(int points){
        score += points;
        UpdateScoreText();
    }

    public void LoseLife(int lifeDeduct){
        life -= lifeDeduct;
        if (life < 0){
            life = 0;
        }
        UpdateLifeText();
    }

    void UpdateScoreText(){
        scoreText.text = score.ToString();
    }

    void UpdateLifeText(){
        lifeText.text = life.ToString();
    }

    public int GetScore(){
        return score;
    }

     public int GetLife()
    {
        return life;
    }
}
