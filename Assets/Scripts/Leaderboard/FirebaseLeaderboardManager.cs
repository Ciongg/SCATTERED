using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System.Linq;
using System.Threading.Tasks;

public class FirebaseLeaderboardManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseUser User;
    public DatabaseReference DBreference;

    [Header("UserData")]
    public TMP_InputField ecoCoinsField;
    public TMP_InputField trashThrownField;
    public GameObject scoreElement;
    public Transform scoreboardContent;

    private string currentUsernameField;


    void Start(){
        ScoreboardButton();
    }

    void Awake()
    {
        // Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // If they are available Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        // Set the authentication instance object
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        User = auth.CurrentUser;

        if (User != null)
        {
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;

            // Retrieve the username from the database for the current logged-in user
            StartCoroutine(LoadUsername());
            StartCoroutine(LoadUserData()); // Load user data here
        }
        else
        {
            Debug.LogWarning("No user is signed in.");
        }
    }

    private IEnumerator LoadUsername()
    {
        // Load the username for the logged-in user
        Task<DataSnapshot> DBTask = DBreference.Child("users").Child(User.UserId).Child("username").GetValueAsync();

        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to retrieve username with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value != null)
        {
            currentUsernameField = DBTask.Result.Value.ToString(); // Store the username
        }
        else
        {
            Debug.LogWarning("Username not found for the logged-in user.");
        }
    }

    private IEnumerator LoadUserData()
    {
        // Get the currently logged-in user data
        Task<DataSnapshot> DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            // No data exists yet
            ecoCoinsField.text = "0";
            trashThrownField.text = "0";
        }
        else
        {
            // Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;
            ecoCoinsField.text = snapshot.Child("ecoCoins").Value.ToString();
            trashThrownField.text = snapshot.Child("trashThrown").Value.ToString();
        }
    }

    public void ScoreboardButton()
    {
        StartCoroutine(LoadScoreboardData());
    }

    private IEnumerator LoadScoreboardData()
    {
        // Get all the users' data ordered by ecoCoins amount
        Task<DataSnapshot> DBTask = DBreference.Child("users").OrderByChild("ecoCoins").GetValueAsync();

        yield return new WaitUntil(() => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning($"Failed to load scoreboard data with {DBTask.Exception}");
        }
        else
        {
            // Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            // Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            // Loop through every user
            int rank = 1; // Start ranking from 1
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                // string username = childSnapshot.Child("username").Value.ToString();
                 string username = childSnapshot.Child("username").Value != null ? 
                  childSnapshot.Child("username").Value.ToString() : "Unknown User";

                int ecoCoins = int.Parse(childSnapshot.Child("ecoCoins").Value.ToString());
                int trashThrown = int.Parse(childSnapshot.Child("trashThrown").Value.ToString());

                // Instantiate new scoreboard elements
                Debug.Log("Instantianting");
                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, ecoCoins, trashThrown, rank);
                Debug.Log("username " + username + "ecoCoins " + ecoCoins + "trashThrown " + trashThrown + "rank " + rank);
                rank++; // Increment rank
            }

            // Go to the scoreboard screen if necessary
        }
    }

    public void SaveDataButton()
    {
        StartCoroutine(UpdateEcoCoins(int.Parse(ecoCoinsField.text)));
        StartCoroutine(UpdateTrashThrown(int.Parse(trashThrownField.text)));
    }

    private IEnumerator UpdateEcoCoins(int _ecoCoins)
    {
        // Set the currently logged in user ecoCoins
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("ecoCoins").SetValueAsync(_ecoCoins);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            // EcoCoins are now updated
        }
    }

    private IEnumerator UpdateTrashThrown(int _trashThrown)
    {
        // Set the currently logged in user trashThrown
        Task DBTask = DBreference.Child("users").Child(User.UserId).Child("trashThrown").SetValueAsync(_trashThrown);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            // TrashThrown is now updated
        }
    }
}
