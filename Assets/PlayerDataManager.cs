using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System;

public class PlayerDataManager : MonoBehaviour
{

    private DatabaseReference databaseReference;
    public FirebaseAuth auth;

    // Removed duplicate string userId
    public static PlayerDataManager Instance { get; private set; }
    public string userId { get; private set; } // Store User ID here
    public bool playerAuthenticated { get; private set; } // Store Authentication status

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        // Ensure this is the only instance of PlayerDataManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicate instances
        }

        // Perform the authentication check and set the userId and authentication status
        
    }

    public void CheckAuthentication()
    {
        if (auth.CurrentUser != null)
        {
            userId = auth.CurrentUser.UserId; // Get user ID from Firebase
            playerAuthenticated = true;
            Debug.Log("User authenticated. User ID: " + userId);
        }
        else
        {
            playerAuthenticated = false;
            userId = null;
            Debug.Log("User not authenticated.");
        }
    }

     public void SetUserId(string _userId)
    {
        userId = _userId; // Store the User ID
    }

    
  

    // Call this to initialize PlayerPrefs and sync with Firebase for a specific user
    public void InitializePlayerPrefs()
    {
        var userRef = databaseReference.Child("users").Child(Instance.userId);

        // Add Leaderboard data
        userRef.Child("Leaderboards").Child("LeafCount").SetValueAsync(PlayerPrefs.GetInt("LeafCount", 0));
        userRef.Child("Leaderboards").Child("totalTrashThrown").SetValueAsync(PlayerPrefs.GetInt("totalTrashThrown", 0));
        userRef.Child("Leaderboards").Child("ecoCoinCount").SetValueAsync(PlayerPrefs.GetInt("ecoCoinCount", 0));

        // Add Powerups data
        userRef.Child("Powerups").Child("TapPower").SetValueAsync(PlayerPrefs.GetFloat("TapPower", 1.0f));
        userRef.Child("Powerups").Child("LeafMultiplier").SetValueAsync(PlayerPrefs.GetInt("LeafMultiplier", 1));
        userRef.Child("Powerups").Child("MinusSpawnTime").SetValueAsync(PlayerPrefs.GetFloat("MinusSpawnTime", 0.0f));

        // Add Powerups Level data
        userRef.Child("PowerupsLvl").Child("IncreaseTapLvl").SetValueAsync(PlayerPrefs.GetInt("IncreaseTapLvl", 1));
        userRef.Child("PowerupsLvl").Child("IncreaseLeafMultiplierLvl").SetValueAsync(PlayerPrefs.GetInt("IncreaseLeafMultiplierLvl", 1));
        userRef.Child("PowerupsLvl").Child("IncreaseMinusSpawnTimeLvl").SetValueAsync(PlayerPrefs.GetInt("IncreaseMinusSpawnTimeLvl", 1));

        // Add Cost data
        userRef.Child("Cost").Child("IncreaseTapCost").SetValueAsync(PlayerPrefs.GetInt("IncreaseTapCost", 0));
        userRef.Child("Cost").Child("IncreaseLeafMultiplierCost").SetValueAsync(PlayerPrefs.GetInt("IncreaseLeafMultiplierCost", 0));
        userRef.Child("Cost").Child("IncreaseMinusSpawnTimeCost").SetValueAsync(PlayerPrefs.GetInt("IncreaseMinusSpawnTimeCost", 0));
    }

    // Get ecoCoins
    public int GetEcoCoins()
    {
        return PlayerPrefs.GetInt("ecoCoinCount", 0); // Default value is 0 if not found
    }

    // Update ecoCoins and sync with Firebase
    public void UpdateEcoCoins(int value)
    {
        int currentCoins = GetEcoCoins();
        currentCoins += value; // Add or subtract based on the passed value
        PlayerPrefs.SetInt("ecoCoinCount", currentCoins);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("Leaderboards").Child("ecoCoinCount").SetValueAsync(currentCoins);
    }

    // Get leafCount
    public int GetLeafCount()
    {
        return PlayerPrefs.GetInt("LeafCount", 0); // Default value is 0 if not found

        
    }

    // Update leafCount and sync with Firebase
    public void UpdateLeafCount(int value, bool isAdding)
    {
       
        int currentLeaf = GetLeafCount(); // Corrected to GetLeafCount()
        currentLeaf = isAdding ? currentLeaf += value : currentLeaf -= value; // Add or subtract based on the passed value
        PlayerPrefs.SetInt("LeafCount", currentLeaf);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("Leaderboards").Child("LeafCount").SetValueAsync(currentLeaf);
        
    }

    // Get trashThrown count
    public int GetTrashThrown()
    {
        return PlayerPrefs.GetInt("totalTrashThrown", 0); // Default value is 0 if not found
    }

    // Update trashThrown count and sync with Firebase
    public void UpdateTrashThrown(int value)
    {
        int currentTrashThrown = GetTrashThrown();
        currentTrashThrown += value; // Add or subtract based on the passed value
        PlayerPrefs.SetInt("totalTrashThrown", currentTrashThrown);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("Leaderboards").Child("totalTrashThrown").SetValueAsync(currentTrashThrown);
    }

    // Power-up management functions
    public float GetTapPower()
    {
        return PlayerPrefs.GetFloat("TapPower", 1.0f); // Default to 1.0 if not found
    }

    public void UpdateTapPower(float value)
    {
        PlayerPrefs.SetFloat("TapPower", value);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("Powerups").Child("TapPower").SetValueAsync(value);
    }

    public int GetLeafMultiplier()
    {
        return PlayerPrefs.GetInt("LeafMultiplier", 1); // Default to 1 if not found
    }

    public void UpdateLeafMultiplier(int value)
    {
        PlayerPrefs.SetInt("LeafMultiplier", value);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("Powerups").Child("LeafMultiplier").SetValueAsync(value);
    }

    public float GetMinusSpawnTime()
    {
        return PlayerPrefs.GetFloat("MinusSpawnTime", 0.0f); // Default to 0.0 if not found
    }

    public void UpdateMinusSpawnTime(float value)
    {
        PlayerPrefs.SetFloat("MinusSpawnTime", value);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("Powerups").Child("MinusSpawnTime").SetValueAsync(value);
    }

    // Powerups Level Management
    public int GetIncreaseTapLvl()
    {
        return PlayerPrefs.GetInt("IncreaseTapLvl", 1);
    }

    public void UpdateIncreaseTapLvl(int value)
    {
        PlayerPrefs.SetInt("IncreaseTapLvl", value);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("PowerupsLvl").Child("IncreaseTapLvl").SetValueAsync(value);
    }

    public int GetIncreaseLeafMultiplierLvl()
    {
        return PlayerPrefs.GetInt("IncreaseLeafMultiplierLvl", 1);
    }

    public void UpdateIncreaseLeafMultiplierLvl(int value)
    {
        PlayerPrefs.SetInt("IncreaseLeafMultiplierLvl", value);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("PowerupsLvl").Child("IncreaseLeafMultiplierLvl").SetValueAsync(value);
    }

    public int GetIncreaseMinusSpawnTimeLvl()
    {
        return PlayerPrefs.GetInt("IncreaseMinusSpawnTimeLvl", 1);
    }

    public void UpdateIncreaseMinusSpawnTimeLvl(int value)
    {
        PlayerPrefs.SetInt("IncreaseMinusSpawnTimeLvl", value);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("PowerupsLvl").Child("IncreaseMinusSpawnTimeLvl").SetValueAsync(value);
    }

    // Cost Management
    public int GetIncreaseTapCost()
    {
        return PlayerPrefs.GetInt("IncreaseTapCost", 0);
    }

    public void UpdateIncreaseTapCost(int value)
    {
        PlayerPrefs.SetInt("IncreaseTapCost", value);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("Cost").Child("IncreaseTapCost").SetValueAsync(value);
    }

    public int GetIncreaseLeafMultiplierCost()
    {
        return PlayerPrefs.GetInt("IncreaseLeafMultiplierCost", 0);
    }

    public void UpdateIncreaseLeafMultiplierCost(int value)
    {
        PlayerPrefs.SetInt("IncreaseLeafMultiplierCost", value);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("Cost").Child("IncreaseLeafMultiplierCost").SetValueAsync(value);
    }

    public int GetIncreaseMinusSpawnTimeCost()
    {
        return PlayerPrefs.GetInt("IncreaseMinusSpawnTimeCost", 0);
    }

    public void UpdateIncreaseMinusSpawnTimeCost(int value)
    {
        PlayerPrefs.SetInt("IncreaseMinusSpawnTimeCost", value);
        PlayerPrefs.Save();

        // Update Firebase
        databaseReference.Child("users").Child(Instance.userId).Child("Cost").Child("IncreaseMinusSpawnTimeCost").SetValueAsync(value);
    }
}
