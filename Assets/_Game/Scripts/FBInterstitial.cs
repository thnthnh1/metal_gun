using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using AudienceNetwork;
//using AudienceNetwork.Utility;
using System;
public class FBInterstitial : MonoBehaviour
{

    private void Awake()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        //AudienceNetworkAds.Initialize();
#endif
    }

    //     public void LoadInterstitial()
    //     {
    //         Debug.Log("Loading interstitial ad... " + Singleton<AdmobController>.Instance.facebookInterstitial);
    //         // Create the interstitial unit with a placement ID (generate your own on the Facebook app settings).
    //         // Use different ID for each ad placement in your app.
    //         interstitialAd = new InterstitialAd(Singleton<AdmobController>.Instance.facebookInterstitial);

    //         interstitialAd.Register(gameObject);

    //         // Set delegates to get notified on changes or when the user interacts with the ad.
    //         interstitialAd.InterstitialAdDidLoad = delegate ()
    //         {
    //             Debug.Log("Interstitial ad loaded.");
    //             isLoaded = true;
    //             didClose = false;
    //             string isAdValid = interstitialAd.IsValid() ? "valid" : "invalid";
    //             Debug.Log("facebook - ad loaded and is " + isAdValid + ". Click show to present!");
    //         };
    //         interstitialAd.InterstitialAdDidLoad += this.HandleInterstitialLoaded;

    //         interstitialAd.InterstitialAdDidFailWithError = delegate (string error)
    //         {
    //             Debug.Log("facebook - Interstitial ad failed to load with error: " + error);
    //         };
    //         interstitialAd.InterstitialAdWillLogImpression = delegate ()
    //         {
    //             Debug.Log("facebook - Interstitial ad logged impression.");
    //         };
    //         interstitialAd.InterstitialAdDidClick = delegate ()
    //         {
    //             Debug.Log("facebook - Interstitial ad clicked.");
    //         };
    //         interstitialAd.InterstitialAdDidClose = delegate ()
    //         {
    //             Debug.Log("facebook - Interstitial ad did close.");
    //             didClose = true;
    //             if (interstitialAd != null)
    //             {
    //                 interstitialAd.Dispose();
    //             }
    //         };

    // #if UNITY_ANDROID
    //         /*
    //          * Only relevant to Android.
    //          * This callback will only be triggered if the Interstitial activity has
    //          * been destroyed without being properly closed. This can happen if an
    //          * app with launchMode:singleTask (such as a Unity game) goes to
    //          * background and is then relaunched by tapping the icon.
    //          */
    //         interstitialAd.interstitialAdActivityDestroyed = delegate ()
    //         {
    //             if (!didClose)
    //             {
    //                 Debug.Log("facebook - Interstitial activity destroyed without being closed first.");
    //                 Debug.Log("facebook - Game should resume.");
    //             }
    //         };
    // #endif

    //         // Initiate the request to load the ad.
    //         interstitialAd.LoadAd();
    //     }

    //     public void ShowInterstitial()
    //     {
    //         if (isLoaded)
    //         {
    //             interstitialAd.Show();
    //             isLoaded = false;
    //             Debug.Log("facebook - Showing Interstitial");
    //         }
    //         else
    //         {
    //             Debug.Log("facebook - Interstitial Ad not loaded. Click load to request an ad.");
    //         }
    //     }

    //     public void ShowInterstitialAd(Action<ShowResult> callback = null)
    //     {
    //         if (this.interstitialAd != null && isLoaded)
    //         {
    //             SoundManager.Instance.SetMute(true);
    //             isLoaded = false;
    //             // Time.timeScale = 0f;
    //             this.interstitialAdsCallback = callback;
    //             interstitialAd.Show();
    //             Debug.Log("facebook - Showing Interstitial");
    //         }
    //         else
    //         {
    //             // this.RequestInterstitial();
    //             if (callback != null)
    //             {
    //                 callback(ShowResult.Failed);
    //             }
    //             Debug.Log("facebook - Interstitial Ad not loaded. Click load to request an ad.");
    //         }
    //     }

    //     private void HandleInterstitialLoaded()
    //     {

    //     }

    void OnDestroy()
    {
        // Dispose of interstitial ad when the scene is destroyed
        Debug.Log("InterstitialAdTest was destroyed!");
    }
}
