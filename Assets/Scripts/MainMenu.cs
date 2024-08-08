using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void Main()
    {
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
    }
}
