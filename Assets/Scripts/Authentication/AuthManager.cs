using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using Firebase.Database;

public class AuthManager : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;

    public DatabaseReference databaseReference;
    public PlayerDataManager playerDataManager;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    public TMP_Text signedInAs;

    public GameObject playBtn;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;




    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are available Initialize Firebase
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
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(CheckUsernameAndRegister(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

   private IEnumerator CheckUsernameAndRegister(string _email, string _password, string _username)
{
    Debug.Log("Checking username availability...");
 
    // Reference to the specific username in the database
    var usernameRef = databaseReference.Child("usernames").Child(_username);
    Debug.Log("Database reference: " + usernameRef);

    // Check if the username exists
    var usernameTask = usernameRef.GetValueAsync();
    yield return new WaitUntil(() => usernameTask.IsCompleted);

    if (usernameTask.IsFaulted)
    {
        Debug.LogError("Error checking username: " + usernameTask.Exception);
    }
    else if (usernameTask.IsCompleted)
    {
        DataSnapshot snapshot = usernameTask.Result;
        Debug.Log("Snapshot retrieved: " + snapshot.Exists);

        if (snapshot.Exists) // Username already exists
        {
            ShowWarning(warningRegisterText, "Username already taken. Please choose another.");
        }
        else // Username is unique, proceed to register
        {
            Debug.Log("Username available, registering...");
            StartCoroutine(Register(_email, _password, _username));
        }
    }
}



    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        Task<AuthResult> LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            ShowWarning(warningLoginText, message);
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.gameObject.SetActive(false);
            confirmLoginText.gameObject.SetActive(true);
            confirmLoginText.text = "Logged In";
            signedInAs.gameObject.SetActive(true);
            signedInAs.text = "Signed in as: " + User.DisplayName;
            
            yield return SetPlayerPrefsFromDatabase(User.UserId);
            PlayerDataManager.Instance.SetUserId(User.UserId);
            playBtn.SetActive(true);
            
            
        }
    }

   private IEnumerator SetPlayerPrefsFromDatabase(string userId)
{
    var userRef = databaseReference.Child("users").Child(userId);
    Task<DataSnapshot> snapshotTask = userRef.GetValueAsync();

    // Wait until the snapshot task completes
    yield return new WaitUntil(() => snapshotTask.IsCompleted);

    if (snapshotTask.Exception != null)
    {
        Debug.LogError("Failed to retrieve user data: " + snapshotTask.Exception);
        yield break; // Exit if there was an error
    }

    DataSnapshot snapshot = snapshotTask.Result;
    Debug.Log(snapshot.GetRawJsonValue()); // Logs the entire snapshot as a JSON string

    if (snapshot.Exists)
    {
        // Set the player prefs to values from the database
        PlayerPrefs.SetInt("ecoCoinCount", int.Parse(snapshot.Child("Leaderboards").Child("ecoCoinCount").Value.ToString()));
        PlayerPrefs.SetInt("LeafCount", int.Parse(snapshot.Child("Leaderboards").Child("LeafCount").Value.ToString()));
        PlayerPrefs.SetInt("totalTrashThrown", int.Parse(snapshot.Child("Leaderboards").Child("totalTrashThrown").Value.ToString()));

        PlayerPrefs.SetFloat("TapPower", float.Parse(snapshot.Child("Powerups").Child("TapPower").Value.ToString()));
        PlayerPrefs.SetInt("LeafMultiplier", int.Parse(snapshot.Child("Powerups").Child("LeafMultiplier").Value.ToString()));
        PlayerPrefs.SetFloat("MinusSpawnTime", float.Parse(snapshot.Child("Powerups").Child("MinusSpawnTime").Value.ToString()));

        PlayerPrefs.SetInt("IncreaseTapLvl", int.Parse(snapshot.Child("PowerupsLvl").Child("IncreaseTapLvl").Value.ToString()));
        PlayerPrefs.SetInt("IncreaseLeafMultiplierLvl", int.Parse(snapshot.Child("PowerupsLvl").Child("IncreaseLeafMultiplierLvl").Value.ToString()));
        PlayerPrefs.SetInt("IncreaseMinusSpawnTimeLvl", int.Parse(snapshot.Child("PowerupsLvl").Child("IncreaseMinusSpawnTimeLvl").Value.ToString()));

        PlayerPrefs.SetInt("IncreaseTapCost", int.Parse(snapshot.Child("Cost").Child("IncreaseTapCost").Value.ToString()));
        PlayerPrefs.SetInt("IncreaseLeafMultiplierCost", int.Parse(snapshot.Child("Cost").Child("IncreaseLeafMultiplierCost").Value.ToString()));
        PlayerPrefs.SetInt("IncreaseMinusSpawnTimeCost", int.Parse(snapshot.Child("Cost").Child("IncreaseMinusSpawnTimeCost").Value.ToString()));

        PlayerPrefs.Save();
    }
    else
    {
        Debug.Log("User data does not exist in the database.");
    }
}


    

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            ShowWarning(warningRegisterText, "Missing Username");
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            ShowWarning(warningRegisterText, "Password Does Not Match!");
        }
        else 
        {
            //Call the Firebase auth signin function passing the email and password
            Task<AuthResult> RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                ShowWarning(warningRegisterText, message);
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result.User;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    Task ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowWarning(warningRegisterText, "Username Set Failed!");
                        
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        databaseReference.Child("usernames").Child(_username).SetValueAsync(true);
                        databaseReference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);
                        ResetPlayerPrefs();
                        InitializePlayerPrefs(User.UserId);

                        UIManager.instance.LoginScreen();
                        warningRegisterText.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private void ResetPlayerPrefs(){

        // This will clear all saved PlayerPrefs
        PlayerPrefs.DeleteAll();  
        


        PlayerPrefs.SetInt("ecoCoins", 0);
        PlayerPrefs.SetInt("LeafCount", 0);
        PlayerPrefs.SetInt("totalTrashThrown", 0);
        PlayerPrefs.SetFloat("TapPower", 1.0f);
        PlayerPrefs.SetInt("LeafMultiplier", 1);
        PlayerPrefs.SetFloat("MinusSpawnTime", 0.0f);

        // Initialize levels and costs
        PlayerPrefs.SetInt("IncreaseTapLvl", 1);
        PlayerPrefs.SetInt("IncreaseLeafMultiplierLvl", 1);
        PlayerPrefs.SetInt("IncreaseMinusSpawnTimeLvl", 1);

        PlayerPrefs.SetInt("IncreaseTapCost", 10);
        PlayerPrefs.SetInt("IncreaseLeafMultiplierCost", 10);
        PlayerPrefs.SetInt("IncreaseMinusSpawnTimeCost", 10);
    }

    private void InitializePlayerPrefs(string userId)
{
    // Reference to the user data in the database
    var userRef = databaseReference.Child("users").Child(userId);

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

    private void ShowWarning(TMP_Text warningText, string message)
    {
        warningText.gameObject.SetActive(true);
        warningText.text = message;
        StartCoroutine(HideWarning(warningText, 10f)); // Hide after 10 seconds
    }

    private IEnumerator HideWarning(TMP_Text warningText, float delay)
    {
        yield return new WaitForSeconds(delay);
        warningText.gameObject.SetActive(false);
    }
}
