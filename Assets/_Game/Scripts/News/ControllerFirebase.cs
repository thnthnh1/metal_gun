/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase.Auth;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
//using Firebase.Storage;
// using Firebase.Unity.Editor;
using System.Threading;

public class ControllerFirebase : MonoBehaviour
{
    public static ControllerFirebase controllerFirebase;
    [Header("FireBase Settings")]
    // public string firebaseDatabaseURL = "https://Example.firebaseio.com/";
    public string firebaseDatabaseURL = "gs://metal-gun-multiplayer.appspot.com/";

    // public string firebaseStorageURL = "https://Example.firebaseio.com/";

    [Header("Main Login Screen")]
    public GameObject mainLoginScreenObj;
    public InputField userText;
    public InputField passwordText;

    [Header("Create User Screen")]
    public GameObject createUserScreenObj;
    public InputField createUserText;
    public InputField createUsernameText;
    public InputField createPasswordText;
    public GameObject createUserObj;

    [Header("Loged Screen")]
    public Text welcomeText;
    public GameObject logedScreen;

    [Header("Firebase Message Screen")]
    public GameObject messageScreen;
    public Text messageText;

    [Header("Player room invitation received")]
    public GameObject invitedPlayerRoomScreen;
    public Text invitationPlayerRoomText;
    private string playerRoomToJoin;

    [Header("Global chat")]
    public InputField globalChatMessage;
    public RectTransform messageglobalChatContentWindow;
    public Text bodyContenglobalChatText;
    private bool ignoreFirtsMessage = true;

    [Header("Sounds")]
    public AudioSource selectAudio;
    public AudioSource signtInAudio;
    public AudioSource notificationAudio;

    [Header("Local Facebook Profile")]
    public FacebookUserInfo currentFacebookUser;
    public FacebookUserInfo remoteUser;

    [Header("LocalStore GameDetails")]

    public GameDetails gameDetails;

    public GameDetails FilledGameDetails;

    //References
    // DatabaseReference databaseReference;

    [HideInInspector]
    public FirebaseUser savedUser;

    public bool IsInitialize = false;

    // Start is called before the first frame update
    void Start()
    {
        //Get Static reference 
        controllerFirebase = this;

        if (FilledGameDetails.BannerId == "")
        {
            // FilledGameDetails.AppId = "ca-app-pub-7937098131590091~3687501246";
            FilledGameDetails.BannerId = "ca-app-pub-7937098131590091/4576915233";
            FilledGameDetails.InterstitialId = "ca-app-pub-7937098131590091/1675102496";
            FilledGameDetails.RewardedId = "ca-app-pub-7937098131590091/1950751897";
        }

        //Init Database
        // FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(firebaseDatabaseURL);
        // FirebaseApp.GetInstance(firebaseDatabaseURL);
        // Firebase.AppOptions.DatabaseUrl = new Uri(firebaseDatabaseURL);
        // AppOptions options = new AppOptions();
        // options.DatabaseUrl = new Uri(firebaseDatabaseURL);
        // FirebaseApp app = FirebaseApp.Create(options);
        Debug.Log("Nik Log Is the the firebase Url call");
        IsInitialize = false;


        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                // Get And Set GameDetails Data
                CheckGameDetailsData();
            }
            else
            {
                Debug.LogError(System.String.Format("Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });


        //Invoke("ComprobateLogin", 2);
    }

    public void CheckGameDetailsData()
    {
        Debug.Log("Nik Enter The getgamedetailsData ");
        // StartCoroutine(CheckData());
        // FirebaseDatabase.DefaultInstance.GetReference("GameDetails").GetValueAsync().ContinueWith(task =>
        // {
        //     if (task.Result.Child("BannerId").Exists)
        //     {

        //         Debug.Log("This user already exist! : " + 
        //                     task.Result.Child("AppId") + 
        //                     task.Result.Child("BannerId") +
        //                     task.Result.Child("InterstitialId") +
        //                     task.Result.Child("RewardedId"));

        //         // if (gameDetails.BannerId != AdmobController.Instance.AdmobMediationBannerID ||
        //         //     gameDetails.InterstitialId != AdmobController.Instance.AdmobMediationInterstitialID||
        //         //     gameDetails.RewardedId != AdmobController.Instance.AdmobMediationRewardvideoID)
        //         // {
        //             Debug.Log("Nik Data Set");
        //             // gameDetails.AppId = task.Result.Child("AppId").Value.ToString();
        //             gameDetails.BannerId = task.Result.Child("BannerId").Value.ToString();
        //             gameDetails.InterstitialId = task.Result.Child("InterstitialId").Value.ToString();
        //             gameDetails.RewardedId = task.Result.Child("RewardedId").Value.ToString();

        CallAdsInitialize();
        //         // }

        //         // AdmobController.Instance.AdsInitialize(gameDetails.BannerId,
        //         //             gameDetails.InterstitialId,gameDetails.RewardedId);
        //         Debug.Log("Nik All Complete Ads Value");
        //     }
        // 	else
        // 	{
        //         Debug.Log("This user Not exist! ");
        //         UnityMainThreadDispatcher.Instance().Enqueue(SetGameDetailsData());
        //     }
        // });


        //     // }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    // public IEnumerator CheckData()
    // {
    //     Debug.Log("Nik Enter The Check data ");
    //     var task = FirebaseDatabase.DefaultInstance.GetReference("GameDetails").GetValueAsync();

    //     yield return new WaitUntil(()=> task.IsCompleted);

    //     if (task.Result.Child("AppId").Exists)
    //     {
    //         Debug.Log("This user already exist! : " + 
    //                     task.Result.Child("AppId") + 
    //                     task.Result.Child("BannerId") +
    //                     task.Result.Child("InterstitialId") +
    //                     task.Result.Child("RewardedId"));
    //         gameDetails.AppId = task.Result.Child("AppId").Value.ToString();
    //         gameDetails.BannerId = task.Result.Child("BannerId").Value.ToString();
    //         gameDetails.InterstitialId = task.Result.Child("InterstitialId").Value.ToString();
    //         gameDetails.RewardedId = task.Result.Child("RewardedId").Value.ToString();
    //         CallAdsInitialize();
    //         Debug.Log("Nik All Complete Ads Value");
    //     }
    //     else
    //     {
    //         Debug.Log("This user Not exist! ");
    //         UnityMainThreadDispatcher.Instance().Enqueue(SetGameDetailsData());
    //     }
    // }

    public bool _initializeData = false;

    void CallAdsInitialize()
    {
        Debug.Log("Nik Enter Call Initialize");
        // if (gameDetails.BannerId != Singleton<AdmobController>.Instance.AdmobMediationBannerID ||
        //     gameDetails.InterstitialId != Singleton<AdmobController>.Instance.AdmobMediationInterstitialID||
        //     gameDetails.RewardedId != Singleton<AdmobController>.Instance.AdmobMediationRewardvideoID)
        // {
        // Singleton<AdmobController>.Instance._initialize = false;
        _initializeData = true;
        // }
        Debug.Log("Nik Exit Initialize");
    }

    public IEnumerator SetGameDetailsData()
    {
        Debug.Log("Writing GameDetailsData");
        // String AppId = FilledGameDetails.AppId;
        String BannerId = FilledGameDetails.BannerId;
        String InterstitialId = FilledGameDetails.InterstitialId;
        String RewardedId = FilledGameDetails.RewardedId;

        // string GameDetails = JsonUtility.ToJson(AppId);
        // databaseReference.Child("GameDetails").Child("AppId").SetValueAsync(AppId);
        // databaseReference.Child("GameDetails").Child("BannerId").SetValueAsync(BannerId);
        // databaseReference.Child("GameDetails").Child("InterstitialId").SetValueAsync(InterstitialId);
        // databaseReference.Child("GameDetails").Child("RewardedId").SetValueAsync(RewardedId);

        Debug.Log("Firebase Write data Id : " + BannerId + " : " + InterstitialId + " : " + RewardedId);

        // Call Ads Initialize
        // AdmobController.Instance.AdsInitialize(BannerId,InterstitialId,RewardedId);
        AdmobController.Instance.AdsInitialize();

        yield return null;
    }

    public void ComprobateLogin()
    {
        ////Try relogin
        //if (GameObject.FindGameObjectWithTag("Progress").GetComponent<Progress>().email != "")
        //{
        //    userText.text = GameObject.FindGameObjectWithTag("Progress").GetComponent<Progress>().email;
        //    passwordText.text = GameObject.FindGameObjectWithTag("Progress").GetComponent<Progress>().password;

        //    //Relog 
        //    SigntUser();
        //}
        //else
        //{
        //    mainLoginScreenObj.GetComponent<Animator>().SetTrigger("active");
        //}
    }

    //Retrieve data from Firebase
    public void RetrieveFaceBookInfoInDatabase(string username)
    {
        // Debug.Log("Enter This RetrieveFaceBookInfoInDatabase" + username + ":" + DisplayHighscores.instance.firebaseUserCount);
        // FirebaseDatabase.DefaultInstance.GetReference("FacebookUsers").Child(username).GetValueAsync().//ContinueWith(task =>
        // ContinueWithOnMainThread(task =>
        // {

        //     // remoteUser = JsonUtility.FromJson<FacebookUserInfo>(task.Result.Value.ToString());
        //     remoteUser = JsonUtility.FromJson<FacebookUserInfo>(task.Result.Value.ToString());

        //     Debug.Log("NIk Check" + task.Result.Value.ToString());

        //     // UnityMainThreadDispatcher.Instance().Enqueue(SendInfo());
        //     if (task.IsCompleted)
        //     {
        //         DataSnapshot DS = task.Result;
        //         // remoteUser = JsonUtility.FromJson<FacebookUserInfo>task.Result.Value.ToString();
        //         Debug.Log("Success" + DS);
        //         UnityMainThreadDispatcher.Instance().Enqueue(SendInfo());
        //         // StartCoroutine(SendInfo());
        //     }
        //     else
        //     {
        //         Debug.Log("Fail");
        //     }

        // }); 
        // }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());

        // StartCoroutine(DelayData(username));
    }

    IEnumerator DelayData(string username)
    {
        var task = FirebaseDatabase.DefaultInstance.GetReference("FacebookUsers").Child(username).GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        var Use = task.Result;
        if (task.Result != null)
        {
            if (Use.Value != null)
            {
                remoteUser = JsonUtility.FromJson<FacebookUserInfo>(task.Result.Value.ToString());
            }
            else
            {
                remoteUser.facebookName = "Fetching...";
                remoteUser.facebookPhoto = "";
            }
            // Debug.Log("checked value " + task.Result.ToString());
            var Data = task.Result;
            // remoteUser = JsonUtility.FromJson<FacebookUserInfo>(task.Result.GetRawJsonValue());
            // Debug.Log("Remote user  " + remoteUser.facebookName);
            // UnityMainThreadDispatcher.Instance().Enqueue(SendInfo());
            StartCoroutine(SendInfo());
        }

    }
    public IEnumerator SendInfo()
    {
        Debug.Log("Enter SendInfo");
        // yield return new WaitForSeconds(0.5f);
        DisplayHighscores.instance.GetFirebaseInfo();
        yield return null;
    }

    //Put user facebook data in database
    public void WriteFaceBookInfoInDatabase()
    {
        Debug.Log("Writing Info in database");
        FirebaseDatabase.DefaultInstance.GetReference("FacebookUsers").GetValueAsync().ContinueWith(task =>
        {
            Debug.Log("Writing Name is " + currentFacebookUser.facebookID);
            if (task.Result.Child(currentFacebookUser.facebookID).Exists)
            {
                Debug.Log("This user already exist! : " + task.Result.Child(currentFacebookUser.facebookID));
                UnityMainThreadDispatcher.Instance().Enqueue(FinallyWriteFacebookInfo());
                return;
            }
            else
            {
                Debug.Log("This user Not exist! ");
                UnityMainThreadDispatcher.Instance().Enqueue(FinallyWriteFacebookInfo());
            }
        });
    }

    public void CallGoogleLoginWritedata()
    {
        Debug.Log("Enter The GoogleLoginWritedata");
        StartCoroutine(FinallyWriteFacebookInfo());
    }

    public IEnumerator FinallyWriteFacebookInfo()
    {
        Debug.Log("Writing!");
        //Get info od player settings
        currentFacebookUser.facebookID = Mp_playerSettings.instance.facebookID;
        currentFacebookUser.facebookName = Mp_playerSettings.instance.facebookName;
        currentFacebookUser.facebookPhoto = Mp_playerSettings.instance.facebookPhotoURL;

        string facebookUserInforJson = JsonUtility.ToJson(currentFacebookUser);
        // databaseReference.Child("FacebookUsers").Child(currentFacebookUser.facebookID).SetValueAsync(facebookUserInforJson);

        Debug.Log("Firebase Write data Id : " + currentFacebookUser.facebookID + " Name " + currentFacebookUser.facebookName + " url " + currentFacebookUser.facebookPhoto + " Json String " + facebookUserInforJson);

        //Save DatabaseInfo in PlayerPref
        Mp_playerSettings.instance.SaveFacebookInfo();
        yield return null;
    }

    public void CreateNewUser()
    {

        //Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //Comrpobate if this Username exist in database
        //Comprobate this name for this clan
        UnityMainThreadDispatcher.Instance().Enqueue(StartingFinallyCreateUser());

        FirebaseDatabase.DefaultInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
        {
            if (task.Result.Child(createUsernameText.text).Exists)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(ErrorFinallyCreateUser());
                UnityMainThreadDispatcher.Instance().Enqueue(localUserMessage("This name is used for other user, please enter diferent name"));
                return;
            }
        });

        auth.CreateUserWithEmailAndPasswordAsync(createUserText.text, createPasswordText.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(ErrorFinallyCreateUser());
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                UnityMainThreadDispatcher.Instance().Enqueue(localUserMessage("CreateUserWithEmailAndPasswordAsync was canceled."));

                return;
            }
            if (task.IsFaulted)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(ErrorFinallyCreateUser());
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                UnityMainThreadDispatcher.Instance().Enqueue(localUserMessage("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception));

                return;
            }
            if (task.IsCompleted)
            {
                //Set Username
                Firebase.Auth.FirebaseUser user = auth.CurrentUser;

                if (user != null)
                {
                    Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                    {
                        DisplayName = createUsernameText.text,
                        PhotoUrl = new System.Uri("https://example.com/jane-q-user/profile.jpg"),
                    };
                    user.UpdateUserProfileAsync(profile).ContinueWith(task2 =>
                    {
                        if (task2.IsCanceled)
                        {
                            Debug.LogError("UpdateUserProfileAsync was canceled.");
                            return;
                        }
                        if (task2.IsFaulted)
                        {
                            Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                            return;
                        }
                        if (task2.IsCompleted)
                        {
                            UnityMainThreadDispatcher.Instance().Enqueue(FinallyCreateUser());
                        }
                    });
                }
                else
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(localUserMessage("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception));
                }
            }
        });
    }

    public IEnumerator StartingFinallyCreateUser()
    {
        createUserScreenObj.GetComponent<Animator>().SetTrigger("inactive");

        yield return null;
    }
    public IEnumerator ErrorFinallyCreateUser()
    {
        createUserScreenObj.GetComponent<Animator>().SetTrigger("active");

        yield return null;
    }

    IEnumerator FinallyCreateUser()
    {
        //Create Notification Channels for This User
        // databaseReference.Child("notificationJoinRoom").Child(createUsernameText.text).SetValueAsync("");

        //Account Created
        StartCoroutine(localUserMessage("Account Created Succefully!"));

        createUserScreenObj.SetActive(false);

        //Dissable Creater user button
        createUserScreenObj.GetComponent<Animator>().SetTrigger("inactive");

        mainLoginScreenObj.GetComponent<Animator>().SetTrigger("active");

        Debug.Log("User profile updated successfully.");

        yield return null;
    }

    public void SigntUser()
    {
        Debug.Log("Sight User");
        //Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        UnityMainThreadDispatcher.Instance().Enqueue(StartingInitUser());

        auth.SignInWithEmailAndPasswordAsync(userText.text, passwordText.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                UnityMainThreadDispatcher.Instance().Enqueue(localUserMessage("SignInWithEmailAndPasswordAsync was canceled."));

                UnityMainThreadDispatcher.Instance().Enqueue(ErrorInitUser());
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                UnityMainThreadDispatcher.Instance().Enqueue(localUserMessage("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception));

                UnityMainThreadDispatcher.Instance().Enqueue(ErrorInitUser());
                return;
            }
            if (task.IsCompleted)
            {
                // Firebase.Auth.FirebaseUser newUser = task.Result;
                // Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

                // savedUser = task.Result;

                Firebase.Auth.AuthResult newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.User.DisplayName, newUser.User.UserId);

                savedUser = newUser.User;

                UnityMainThreadDispatcher.Instance().Enqueue(InitUser());
            }
        });
    }

    public void SignOutUser()
    {
        FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        //Play signt
        selectAudio.Play();

        //Reset UI
        //mainLoginScreenObj.GetComponent<DefaultPopup>().OpenPanel();

        //Dissable Creater user button
        logedScreen.GetComponent<Animator>().SetTrigger("inactive");

        mainLoginScreenObj.GetComponent<Animator>().SetTrigger("active");

        auth.SignOut();
    }

    public IEnumerator localUserMessage(string message)
    {
        //Play Notification audio
        notificationAudio.Play();

        messageScreen.SetActive(true);
        messageText.text = message;

        yield return null;
    }

    public IEnumerator StartingInitUser()
    {
        //if (GameObject.FindGameObjectWithTag("Progress").GetComponent<Progress>().email == "")
        //{
        //    mainLoginScreenObj.GetComponent<Animator>().SetTrigger("inactive");
        //}


        yield return null;
    }
    public IEnumerator ErrorInitUser()
    {
        mainLoginScreenObj.GetComponent<Animator>().SetTrigger("active");

        yield return null;
    }

    public IEnumerator InitUser()
    {
        signtInAudio.Play();

        logedScreen.GetComponent<Animator>().SetTrigger("active");

        //Send Player to save temporally data
        //GameObject.FindGameObjectWithTag("Progress").GetComponent<Progress>().playerName = savedUser.DisplayName;
        //GameObject.FindGameObjectWithTag("Progress").GetComponent<Progress>().email = userText.text;
        //GameObject.FindGameObjectWithTag("Progress").GetComponent<Progress>().password = passwordText.text;

        //Set welcome text
        welcomeText.text = "Welcome, " + savedUser.DisplayName;

        //Start tracking paths
        FirebaseDatabase.DefaultInstance.GetReference("notificationJoinRoom").Child(savedUser.DisplayName).ValueChanged += TrackNotificationJoinRoomPath;

        //Start tracking Global Chat
        FirebaseDatabase.DefaultInstance.GetReference("GlobalChat").ValueChanged += TrackGlobalChatMessages;

        //Reset Main Screen Texts
        userText.text = "";
        passwordText.text = "";

        yield return null;
    }

    void TrackNotificationJoinRoomPath(object sender, ValueChangedEventArgs args)
    {
        //If Path is Empty, return
        if (args.Snapshot.Value.ToString() == "")
        {
            print("Path Empty");
            return;
        }

        //Play Notification audio
        notificationAudio.Play();

        //Invitation of Other friend!
        invitedPlayerRoomScreen.SetActive(true);

        invitationPlayerRoomText.text = args.Snapshot.Value.ToString();

        playerRoomToJoin = args.Snapshot.Value.ToString();

        //Reset Path in Database
        // databaseReference.Child("notificationJoinRoom").Child(savedUser.DisplayName).SetValueAsync("");
    }

    //Global Chat
    public void SendGlobalChatMessage()
    {
        if (globalChatMessage.text == "")
        {
            return;
        }

        //Send clan message
        // databaseReference.Child("GlobalChat").SetValueAsync(savedUser.DisplayName + " : " + globalChatMessage.text);

        //Renew Input Field
        globalChatMessage.text = "";
    }

    void TrackGlobalChatMessages(object sender, ValueChangedEventArgs args)
    {
        if (ignoreFirtsMessage)
        {
            ignoreFirtsMessage = false;
            return;
        }
        //Play Notification audio
        notificationAudio.Play();

        //Setting new position to rectTransform Message Content
        messageglobalChatContentWindow.GetComponent<RectTransform>().offsetMax =
        new Vector2(0, messageglobalChatContentWindow.GetComponent<RectTransform>().offsetMax.y + 30);

        //Show Message
        bodyContenglobalChatText.GetComponent<Text>().text += "\n \n" + args.Snapshot.Value.ToString();
    }

    public void AcceptInvitationToRoom()
    {
        //Player Select audio
        //selectAudio.Play();

        Launcher.launcher.JoinCustomRoom(playerRoomToJoin);
        invitedPlayerRoomScreen.SetActive(false);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class FacebookUserInfo
{
    public string facebookName;
    public string facebookID;
    public string facebookPhoto;
}

[System.Serializable]
public class GameDetails
{
    // public string AppId;
    public string BannerId;
    public string InterstitialId;
    public string RewardedId;
}*/