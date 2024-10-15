using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text usernameText;
    public TMP_Text ecoCoinsText; //kills
    public TMP_Text trashThrownText; //deaths
    

    public void NewScoreElement (string _username, int _ecoCoins, int _trashThrown, int _rankText){
        usernameText.text = _username;
        ecoCoinsText.text = _ecoCoins.ToString();
        trashThrownText.text = _trashThrown.ToString();
        rankText.text = _rankText.ToString();
        
    }

}