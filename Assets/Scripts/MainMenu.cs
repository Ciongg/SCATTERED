using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI leafText;
    
    public int currentLeaf = 0;
    void Start(){
        
        currentLeaf = PlayerPrefs.GetInt("LeafCount", 0);
        UpdateLeafText();

    }

    void UpdateLeafText(){
        leafText.text = currentLeaf.ToString();
    }


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void PlayGame2()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayGarden()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void PlayShop()
    {
        SceneManager.LoadSceneAsync(4);
    }

    public void Main()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadSceneAsync(0);
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }

     public void ResetPlayerPrefs()
    {
        Debug.Log("Resetted Prefs");
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        currentLeaf = PlayerPrefs.GetInt("LeafCount", 0);
        UpdateLeafText();
    }

    
}
