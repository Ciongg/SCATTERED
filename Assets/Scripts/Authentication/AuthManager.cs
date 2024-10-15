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

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    public TMP_Text signedInAs;

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
                        UIManager.instance.LoginScreen();
                        warningRegisterText.gameObject.SetActive(false);
                    }
                }
            }
        }
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
