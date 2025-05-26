using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Net.Http;
using Google;
using UnityEngine.Events;
using Firebase;
using Firebase.Auth;
using UnityEngine.Networking;

public class GoogleController : MonoBehaviour
{
    public static GoogleController _instance;

    // private string G_Web_Api = "452833067648-tif1e8picomd0b0f2f4u1apfj47jupmp.apps.googleusercontent.com";
    private string G_Web_Api = "493812243643-ic80okpikkktj9lfnp84gos9dq3ddh6t.apps.googleusercontent.com";
    // private GoogleSignInConfiguration configuration;

    // FireBase Remove
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            UnityEngine.Object.DontDestroyOnLoad(this);
        }
        else
        {
            UnityEngine.Object.Destroy(this);
        }
        if (!PlayerPrefs.HasKey("GoogleSignIn"))
        {
            PlayerPrefs.SetInt("GoogleSignIn", 0);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        Debug.Log("WCI " + G_Web_Api);
        // configuration = new GoogleSignInConfiguration
        // {
        //     WebClientId = G_Web_Api,
        //     RequestIdToken = true,
        //     RequestProfile = true,
        //     RequestEmail = true
        // };

        // CheckFirebaseDependencies();
    }

    // public void LoginWithGoogle()
    public void LoginWithGoogle(UnityAction<bool> callback = null)
    {
        // Debug.Log("Google Sing in Go . . . ");
        // GoogleSignIn.Configuration = configuration;
        // GoogleSignIn.Configuration.UseGameSignIn = true;
        // GoogleSignIn.Configuration.RequestIdToken = true;
        // GoogleSignIn.Configuration.RequestEmail = true;
        // GoogleSignIn.Configuration.RequestProfile = true;

        // GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleLoginFinished);
        // SignInWithGoogle();
    }





    public IEnumerator LoadImage(string url)
    {
        WWW www = new WWW(url);
        // UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www;
        HudTournamentRanking.instance.imgPlayerAvatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

        // FireBase Remove
        //write data in firebase
        // ControllerFirebase.controllerFirebase.WriteFaceBookInfoInDatabase();
    }

    internal bool forceLogin = false;
    internal string userId;
    internal bool isSyncing;

    public void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    auth = FirebaseAuth.DefaultInstance;

                    // SignInWithGoogle();
                }
                else
                    Debug.Log("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                Debug.Log("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }
    // public void SignInWithGoogle()
    // {
    //     OnSignIn();
    // }
    // public void SignOutFromGoogle()
    // {
    //     OnSignOut();
    //     Debug.Log("GOOGLE LOGOUT");
    // }
    // private void OnSignIn()
    // {
    //     GoogleSignIn.Configuration = configuration;
    //     GoogleSignIn.Configuration.UseGameSignIn = false;
    //     GoogleSignIn.Configuration.RequestIdToken = true;
    //     Debug.Log("Calling SignIn");
    //     GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    // }
    // private void OnSignOut()
    // {
    //     Debug.Log("Calling SignOut");
    //     GoogleSignIn.DefaultInstance.SignOut();
    // }
    // public void OnDisconnect()
    // {
    //     Debug.Log("Calling Disconnect");
    //     GoogleSignIn.DefaultInstance.Disconnect();
    // }
    // internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    // {
    //     if (task.IsFaulted)
    //     {
    //         Debug.Log("Signin an error : " + task.Exception);
    //     }
    //     else if (task.IsCanceled)
    //     {
    //         Debug.Log("Canceled");
    //     }
    //     else
    //     {
    //         Debug.Log("Welcome: " + task.Result.DisplayName + "!");
    //         Debug.Log("Email = " + task.Result.Email);
    //         Debug.Log("Google ID Token = " + task.Result.IdToken);

    //         // user = auth.CurrentUser;

    //         // Mp_playerSettings.instance.googleName = user.DisplayName;
    //         // Mp_playerSettings.instance.googleEmail = user.Email;
    //         // Mp_playerSettings.instance.googleID = user.UserId;

    //         // // Debug.Log("Nik Log Is the ProviderId " + user.ProviderId);

    //         // Mp_playerSettings.instance.googlePhotoURL = user.PhotoUrl.ToString();

    //         // Mp_playerSettings.instance.facebookName = user.DisplayName;
    //         // Mp_playerSettings.instance.facebookEmail = user.Email;
    //         // Mp_playerSettings.instance.facebookID = user.UserId;
    //         // Mp_playerSettings.instance.facebookPhotoURL = user.PhotoUrl.ToString();

    //         // StartCoroutine(LoadImage(user.PhotoUrl.ToString()));
    //         Debug.Log("All Complete");
    //         // Debug.Log("Email = " + task.Result.Email);
    //         SignInWithGoogleOnFirebase(task.Result.IdToken, task.Result.DisplayName, task.Result.UserId);
    //     }

    // }
    // private void SignInWithGoogleOnFirebase(string idToken, string userName, string userId)
    // {
    //     Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(idToken, null);
    //     Debug.Log("Google IDToken : " + idToken);

    //     auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(task =>
    //     {
    //         if (task.IsCanceled)
    //         {
    //             Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
    //             return;
    //         }
    //         if (task.IsFaulted)
    //         {
    //             Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
    //             return;
    //         }

    //         Firebase.Auth.AuthResult result = task.Result;
    //         Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);

    //         user = auth.CurrentUser;

    //         LoadData();
    //       // Mp_playerSettings.instance.googleName = user.DisplayName;
    //       // Mp_playerSettings.instance.googleEmail = user.Email;
    //       // Mp_playerSettings.instance.googleID = user.UserId;

    //       // // Debug.Log("Nik Log Is the ProviderId " + user.ProviderId);

    //       // Mp_playerSettings.instance.googlePhotoURL = user.PhotoUrl.ToString();

    //       // Mp_playerSettings.instance.facebookName = user.DisplayName;
    //       // Mp_playerSettings.instance.facebookEmail = user.Email;
    //       // Mp_playerSettings.instance.facebookID = user.UserId;
    //       // Mp_playerSettings.instance.facebookPhotoURL = user.PhotoUrl.ToString();

    //       // Mp_playerSettings.instance.SaveFacebookInfo();

    //       // StartCoroutine(LoadImage(user.PhotoUrl.ToString()));
    //         Debug.Log("All Complete 2");
    //         PlayerPrefs.SetInt("GoogleSignIn", 1);
    //         PlayerPrefs.Save();
    //     });
    //     // }, TaskScheduler.FromCurrentSynchronizationContext());
    // }

    // void LoadData()
    // {
    //     Debug.Log("Enter >>>>>");
    //     Mp_playerSettings.instance.googleName = user.DisplayName;
    //     Mp_playerSettings.instance.googleEmail = user.Email;
    //     Mp_playerSettings.instance.googleID = user.UserId;

    //     Mp_playerSettings.instance.playerName = user.DisplayName;
    //     Debug.Log("Play Name : " + Mp_playerSettings.instance.playerName);

    //     // Debug.Log("Nik Log Is the ProviderId " + user.ProviderId);

    //     Mp_playerSettings.instance.googlePhotoURL = user.PhotoUrl.ToString();

    //     Mp_playerSettings.instance.facebookName = user.DisplayName;
    //     Mp_playerSettings.instance.facebookEmail = user.Email;
    //     Mp_playerSettings.instance.facebookID = user.UserId;
    //     Mp_playerSettings.instance.facebookPhotoURL = user.PhotoUrl.ToString();

    //     // Mp_playerSettings.instance.SaveFacebookInfo();

    //     ControllerFirebase.controllerFirebase.CallGoogleLoginWritedata();

    //     MainMenu.instance.IsFBLogin = true;
    //     Debug.Log("Fill Data");
    // }
    // public void OnSignInSilently()
    // {
    //     GoogleSignIn.Configuration = configuration;
    //     GoogleSignIn.Configuration.UseGameSignIn = false;
    //     GoogleSignIn.Configuration.RequestIdToken = true;
    //     Debug.Log("Calling SignIn Silently");

    //     GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    // }
    // public void OnGamesSignIn()
    // {
    //     GoogleSignIn.Configuration = configuration;
    //     GoogleSignIn.Configuration.UseGameSignIn = true;
    //     GoogleSignIn.Configuration.RequestIdToken = false;
    //     Debug.Log("Calling Games SignIn");
    //     GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    // }
}
