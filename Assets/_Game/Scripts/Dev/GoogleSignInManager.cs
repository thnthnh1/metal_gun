// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Firebase;
// using Firebase.Auth;
// // using MB.Game.UI;
// using Google;
using UnityEngine;
// // using MB.Constants;
// // using MB.Networking;
// // using static MB.Game.Constants.Enums;
public class GoogleSignInManager : MonoBehaviour
{
//     //public TextMeshProUGUI infoText;
//     public string webClientId = "<your client id here>";
//     internal bool forceLogin = false;
//     private FirebaseAuth auth;
//     private GoogleSignInConfiguration configuration;
//     internal string userId;
//     internal bool isSyncing;
//     private void Awake()
//     {
//         configuration = new GoogleSignInConfiguration
//         {WebClientId = webClientId, RequestEmail = true, RequestIdToken = true};
        
//         CheckFirebaseDependencies();
//     }
//     private void CheckFirebaseDependencies()
//     {
//         FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//         {
//             if (task.IsCompleted)
//             {
//                 if (task.Result == DependencyStatus.Available)
//                     auth = FirebaseAuth.DefaultInstance;
//                 else
//                     AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
//                 }
//             else
//             {
//                 AddToInformation("Dependency check was not completed. Error : " +
//                 task.Exception.Message);
//             }
//         });
//     }
//     public void SignInWithGoogle()
//     {
//         OnSignIn();
//     }
//     public void SignOutFromGoogle()
//     {
//         OnSignOut();
//         Debug.Log("GOOGLE LOGOUT");
//     }
//     private void OnSignIn()
//     {
//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.Configuration.UseGameSignIn = false;
//         GoogleSignIn.Configuration.RequestIdToken = true;
//         AddToInformation("Calling SignIn");
//         GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
//     }
//     private void OnSignOut()
//     {
//         AddToInformation("Calling SignOut");
//         GoogleSignIn.DefaultInstance.SignOut();
//     }
//     public void OnDisconnect()
//     {
//         AddToInformation("Calling Disconnect");
//         GoogleSignIn.DefaultInstance.Disconnect();
//     }
//     internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
//     {
//         if (task.IsFaulted)
//         {
//             using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
//             {
//                 if (enumerator.MoveNext())
//                 {
//                     GoogleSignIn.SignInException error = (GoogleSignIn.SignInException) enumerator.Current;
//                     AddToInformation("Got Error: " + error.Status + " " + error.Message);
//                 }
//                 else
//                 {
//                     AddToInformation("Got Unexpected Exception?!?" + task.Exception);
//                 }
//             }
//         }
//         else if (task.IsCanceled)
//         {
//             AddToInformation("Canceled");
//         }
//         else
//         {
//             AddToInformation("Welcome: " + task.Result.DisplayName + "!");
//             AddToInformation("Email = " + task.Result.Email);
//             //AddToInformation("Google ID Token = " + task.Result.IdToken);
//             AddToInformation("Email = " + task.Result.Email);
//             // if (PlayerPrefs.GetString(PlayerPrefsConstants.LOGIN_TYPE) != PlayerPrefsConstants.GOOGLE)
//             // {
//             SignInWithGoogleOnFirebase(task.Result.IdToken, task.Result.DisplayName, task.Result.UserId);
//             // }
//         // else
//         // {
//         //     AddToInformation("Sign In Successful.2");
//         //     PlayerPrefs.SetString(PlayerPrefsConstants.LOGIN_TYPE, PlayerPrefsConstants.GOOGLE);
//         //     if (!isSyncing)
//         //     {
//         //         userId = task.Result.UserId;
//         //         GoogleSignInHandler.Login(LoginType.GOOGLE,
//         //         task.Result.DisplayName, task.Result.UserId, forceLogin);
//         //         forceLogin = false;
//         //     }
//         //     else
//         //     {
//         //         userId = task.Result.UserId;
//         //         LoginHandler.CheckCloudAndTryLogin(2, googleId: task.Result.UserId);
//         //         isSyncing = false;
//         //     }
//         //     }
//         }
//     }
//     private void SignInWithGoogleOnFirebase(string idToken, string userName, string userId)
//     {
//         Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
//         auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
//         {
//             AggregateException ex = task.Exception;
//             if (ex != null)
//             {
//                 if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
//                     AddToInformation("\nError code = " + inner.ErrorCode + " Message =" + inner.Message);
//             }
//             else
//             {
//                 AddToInformation("Sign In Successful.1");
//                 // PlayerPrefs.SetString(PlayerPrefsConstants.LOGIN_TYPE, PlayerPrefsConstants.GOOGLE);
//             if (!isSyncing)
//             {
//                 this.userId = userId;
//                 // GoogleSignInHandler.Login(LoginType.GOOGLE, userName, userId, forceLogin);
//                 forceLogin = false;
//                 }
//                 else
//                 {
//                 this.userId = userId;
//                 // LoginHandler.CheckCloudAndTryLogin(2, googleId: userId);
//                 isSyncing = false;
//                 }
//             }
//         });
//     }
//     public void OnSignInSilently()
//     {
//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.Configuration.UseGameSignIn = false;
//         GoogleSignIn.Configuration.RequestIdToken = true;
//         AddToInformation("Calling SignIn Silently");

//         GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
//     }
//     public void OnGamesSignIn()
//     {
//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.Configuration.UseGameSignIn = true;
//         GoogleSignIn.Configuration.RequestIdToken = false;
//         AddToInformation("Calling Games SignIn");
//         GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
//     }
//     private void AddToInformation(string str)
//     {
//     //infoText.text += "\n" + str;
//     Debug.Log(str);
//     }
}