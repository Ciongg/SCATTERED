using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;

public class SignOut : MonoBehaviour
{


    // Reference to FirebaseAuth
    private FirebaseAuth auth;

    void Start()
    {
        // Initialize Firebase Auth
        auth = FirebaseAuth.DefaultInstance;
    }

    // Call this function to sign out the user
    public void SignOutUser()
    {
        auth.SignOut();
        Debug.Log("User signed out successfully.");
        SceneManager.LoadSceneAsync(0);



    }

    
}
