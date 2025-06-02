using GoogleMobileAds.Api;
using GoogleMobileAds;
using System;
using UnityEngine;
using System.Collections.Generic;
// using AudienceNetwork;
// FacebookSDK Remove
// using Facebook.Unity;
using System.Collections;
// using GoogleMobileAdsMediationTestSuite.Api;

public class AdmobController : Singleton<AdmobController>
{
    [SerializeField]
    private bool useBanner;

    [SerializeField]
    private bool useInterstital;

    [SerializeField]
    private bool useRewarded;

    [Range(1f, 10f), SerializeField]
    private int retryLoad = 2;

    // [SerializeField]
    // private AdPosition bannerPosition;

    [Header("Ads Info"), SerializeField]
    private string androidAppId;

    [SerializeField]
    private string iosAppId;

    // [Header("Ads Unit"), SerializeField]
    // private AdmobUnit androidAdsUnit;

    // [SerializeField]
    // private AdmobUnit iosAdsUnit;


    public string facebookInterstitial = "2599466807037610_2600482986935992";
    public string facebookRewardVideo = "2599466807037610_2599468643704093";
    [SerializeField]
    private GameObject InterstitialPrefab;
    [SerializeField]
    private GameObject RewardVideoPrefab;
    // private BannerView bannerView;

    // AdMob Remove
    private InterstitialAd interstitial;
    //facebook interstitial
    // public AudienceNetwork.InterstitialAd interstitialAd; // facebook
    public GameObject InterstitialObject;
    public GameObject RewardVideoObject;
    private bool didClose;
    private bool isLoaded;

    // AdMob Remove
    private RewardedAd rewardBasedVideo;
    // AdMob Remove
    private BannerView bannerView;

    //facebook reward video
    // private AudienceNetwork.RewardedVideoAd rewardedVideoAd;
    private bool isLoadedVideo;
    private bool didCloseVideo;
    private bool isBannerAdLoaded;

    private bool isInterstitialAdLoading;

    private bool isRewardedAdsLoaded;

    private bool isRewardedAdLoading;

    private int tryLoadBannerCount;

    private int tryLoadVideoCount;

    private int tryLoadInterstitialCount;

    private Action<ShowResult> rewardedAdsCallback;

    private Action<ShowResult> interstitialAdsCallback;

    public bool isAdRewarded;

    private bool userConsent;

#if UNITY_IOS
    string DefaultBannerID = "ca-app-pub-7937098131590091/8679993927";
    string DefaultInterstitialID = "ca-app-pub-7937098131590091/7734292501";
    string DefaultRewardvideoID = "ca-app-pub-7937098131590091/7583330838";
#else
    string DefaultBannerID = "ca-app-pub-7937098131590091/4576915233";
    string DefaultInterstitialID = "ca-app-pub-7937098131590091/1675102496";
    string DefaultRewardvideoID = "ca-app-pub-7937098131590091/1950751897";
#endif

    public bool _initialize = false;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Tutotial") || !PlayerPrefs.HasKey("AfterTutotial"))
        {
            Debug.Log("Player's pref not creted");
            PlayerPrefs.SetInt("Tutotial", 0);
            PlayerPrefs.SetInt("AfterTutotial", 1);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("NotifyTutorial"))
        {
            Debug.Log("Player's pref not creted");
            PlayerPrefs.SetInt("NotifyTutorial", 0);
            PlayerPrefs.Save();
        }
        // this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        // this.rewardBasedVideo.OnAdLoaded += new EventHandler<EventArgs>(this.HandleRewardBasedVideoLoaded);
        // this.rewardBasedVideo.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.HandleRewardBasedVideoFailedToLoad);
        // this.rewardBasedVideo.OnAdOpening += new EventHandler<EventArgs>(this.HandleRewardBasedVideoOpened);
        // this.rewardBasedVideo.OnAdStarted += new EventHandler<EventArgs>(this.HandleRewardBasedVideoStarted);
        // this.rewardBasedVideo.OnAdRewarded += new EventHandler<Reward>(this.HandleRewardBasedVideoRewarded);
        // this.rewardBasedVideo.OnAdClosed += new EventHandler<EventArgs>(this.HandleRewardBasedVideoClosed);
        // this.rewardBasedVideo.OnAdLeavingApplication += new EventHandler<EventArgs>(this.HandleRewardBasedVideoLeftApplication);
        // this.userConsent = PlayerPrefs.GetString("user_consent", "PERSONALIZED").Equals("PERSONALIZED");
        UnityEngine.Object.DontDestroyOnLoad(this);
    }


    private void Start()
    {

        // MediationTestSuite.OnMediationTestSuiteDismissed += this.HandleMediationTestSuiteDismissed;
        // AudienceNetworkAds.Initialize();
        // ChartboostSDK.Chartboost.isInitialized();
        // Initialize the Mobile Ads SDK.

        // New Code
        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        break;
                }
            }
        });

        // FacebookSDK Remove
        // FB.Init();
        // RequestInterstitial();
        // RequestRewardBasedVideo();
    }

    void Update()
    {
        /*if (!ControllerFirebase.controllerFirebase)
        {
            return;
        }*/
        if (!_initialize)
        {
            //ControllerFirebase.controllerFirebase._initializeData = false;
            AdsInitialize();
        }
    }
    // public void AdsInitialize(String BannerId, String InterstitialId, String RewardedId)
    public void AdsInitialize()
    {
        //ControllerFirebase CF = ControllerFirebase.controllerFirebase;
        /*if (CF.gameDetails.BannerId == null || CF.gameDetails.InterstitialId == null || CF.gameDetails.RewardedId == null)
        {
            Debug.Log("Nik Ads Return By empty Id");
            return;
        }*/


        // Debug.Log("Nik Ad Id Banner : " + CF.gameDetails.BannerId + "Interstitial : " + CF.gameDetails.InterstitialId + "Rewarded : " + CF.gameDetails.RewardedId);
        // AdmobMediationBannerID = CF.gameDetails.BannerId;
        // AdmobMediationInterstitialID = CF.gameDetails.InterstitialId;
        // AdmobMediationRewardvideoID = CF.gameDetails.RewardedId;

        MobileAds.Initialize((initStatus) =>
        {
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        // The adapter initialization did not complete.
                        MonoBehaviour.print("Adapter: " + className + " not ready.");
                        break;
                    case AdapterState.Ready:
                        // The adapter was successfully initialized.
                        MonoBehaviour.print("Adapter: " + className + " is initialized.");
                        break;
                }
            }
            _initialize = true;
        });

        // RequestInterstitial();
        RequestRewardBasedVideo();
    }
    //     private void ShowMediationTestSuite()
    //     {
    //         // MediationTestSuite.Show();
    //     }
    //     public void HandleMediationTestSuiteDismissed(object sender, EventArgs args)
    //     {
    //         MonoBehaviour.print("HandleMediationTestSuiteDismissed event received");
    //     }

    //     public void Preload()
    //     {
    //         if (this.useBanner)
    //         {
    //             this.RequestBanner();
    //         }
    //         if (this.useInterstital)
    //         {
    //             this.RequestInterstitial();
    //         }
    //         if (this.useRewarded)
    //         {
    //             this.RequestRewardBasedVideo();
    //         }
    //     }

    public void ManualResetTryLoadCount()
    {
        this.tryLoadBannerCount = 0;
        this.tryLoadInterstitialCount = 0;
        this.tryLoadVideoCount = 0;
    }

    //     public bool IsRewardedAdsLoaded()
    //     {
    //         return this.isRewardedAdsLoaded;
    //     }

    //     public bool IsInitInterstitial()
    //     {
    //         // return this.interstitial != null && this.interstitial.IsLoaded();
    //         return false;
    //     }

    //     public void SetAndroidAdsUnit(string bannerId, string interstitialId, string rewardedId)
    //     {
    //         this.androidAdsUnit.bannerAdUnit = bannerId;
    //         this.androidAdsUnit.interstitialAdUnit = interstitialId;
    //         this.androidAdsUnit.rewardedAdUnit = rewardedId;
    //     }

    //     public void SetIosAdsUnit(string bannerId, string interstitialId, string rewardedId)
    //     {
    //         this.iosAdsUnit.bannerAdUnit = bannerId;
    //         this.iosAdsUnit.interstitialAdUnit = interstitialId;
    //         this.iosAdsUnit.rewardedAdUnit = rewardedId;
    //     }

    //     private void RewardedAdCallback()
    //     {
    //         SoundManager.Instance.SetMute(false);
    //         Debug.Log("facebook - its Time for reward ");
    //         if (this.rewardedAdsCallback != null)
    //         {
    //             Debug.Log("facebook - giving reward ");
    //             ShowResult obj = (!this.isAdRewarded) ? ShowResult.Skipped : ShowResult.Finished;
    //             // ShowResult obj = ShowResult.Finished;
    //             this.rewardedAdsCallback(obj);
    //             this.rewardedAdsCallback = null;
    //         }
    //         this.isRewardedAdLoading = false;
    //         this.isAdRewarded = false;
    //         this.RequestRewardBasedVideo();
    //     }

    //     private void InterstitialAdCallback()
    //     {
    //         SoundManager.Instance.SetMute(false);
    //         if (this.interstitialAdsCallback != null)
    //         {
    //             this.interstitialAdsCallback(ShowResult.Finished);
    //             this.interstitialAdsCallback = null;
    //         }
    //         this.isInterstitialAdLoading = false;
    //         this.RequestInterstitial();
    //     }

    #region Banner Ads
    // Request Banner
    private void RequestBanner()
    {
        // if (this.tryLoadBannerCount > this.retryLoad)
        // {
        //     return;
        // }
        // string bannerAdUnit = this.androidAdsUnit.bannerAdUnit;
        // UnityEngine.Debug.Log("RequestBanner");
        // this.isBannerAdLoaded = false;
        // this.bannerView = new BannerView(bannerAdUnit, AdSize.SmartBanner, this.bannerPosition);
        // this.bannerView.OnAdLoaded += new EventHandler<EventArgs>(this.HandleBannerAdLoaded);
        // this.bannerView.OnAdFailedToLoad += new EventHandler<AdFailedToLoadEventArgs>(this.HandleBannerAdFailedToLoad);
        // this.bannerView.OnAdLoaded += new EventHandler<EventArgs>(this.HandleBannerAdOpened);
        // this.bannerView.OnAdClosed += new EventHandler<EventArgs>(this.HandleBannerAdClosed);
        // this.bannerView.OnAdLeavingApplication += new EventHandler<EventArgs>(this.HandleBannerAdLeftApplication);
        // AdRequest request = (!this.userConsent) ? new AdRequest.Builder().AddExtra("npa", "1").Build() : new AdRequest.Builder().Build();
        // this.bannerView.LoadAd(request);
        // this.bannerView.Hide();

        // Admob Banner
        // bannerView = new BannerView(AdmobMediationBannerID, GoogleMobileAds.Api.AdSize.SmartBanner, GoogleMobileAds.Api.AdPosition.Bottom);
        // // Register for ad events.
        // bannerView.OnAdLoaded += HandleAdLoaded;
        // bannerView.OnAdFailedToLoad += HandleAdFailedToLoad;
        // bannerView.OnAdOpening += HandleAdOpened;
        // bannerView.OnAdClosed += HandleAdClosed;
        // bannerView.OnPaidEvent += HandleAdPaidEvent;
        // // Load a banner ad.
        // bannerView.LoadAd(createAdRequest());

        // New Code
        Debug.Log("Creating banner view");

        Debug.Log("Rewarded Id : " + DefaultBannerID);

        // If we already have a banner, destroy the old one.
        if (bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(DefaultBannerID, AdSize.SmartBanner, AdPosition.Bottom);
    }

    public void DestroyAd()
    {
        if (bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            bannerView.Destroy();
            bannerView = null;
        }
    }

    // CallBack Banner
    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        bannerView.OnBannerAdLoaded += () =>
        {
            isBannerAdLoaded = true;
            Debug.Log("Banner view loaded an ad with response : "
                + bannerView.GetResponseInfo());
        };
        // Raised when an ad fails to load into the banner view.
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            isBannerAdLoaded = false;
            Debug.LogError("Banner view failed to load an ad with error : "
                + error);
        };
        // Raised when the ad is estimated to have earned money.
        bannerView.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Banner view paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        bannerView.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Banner view recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        bannerView.OnAdClicked += () =>
        {
            Debug.Log("Banner view was clicked.");
        };
        // Raised when an ad opened full screen content.
        bannerView.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Banner view full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        bannerView.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Banner view full screen content closed.");
        };
    }

    #endregion

    //     public void ShowBanerAd()
    //     {
    //         if (this.isBannerAdLoaded && this.bannerView != null)
    //         {
    //             this.bannerView.Show();
    //         }
    //         else
    //         {
    //             this.RequestBanner();
    //         }
    //     }

    //     public void HideBannerAd()
    //     {
    //         if (this.bannerView != null)
    //         {
    //             this.bannerView.Hide();
    //         }
    //         else
    //         {
    //             this.RequestBanner();
    //         }
    //     }

    //     public void DestoryBanner()
    //     {
    //         if (this.bannerView != null)
    //         {
    //             this.bannerView.Destroy();
    //         }
    //     }

    #region interstitial Ads
    // interstitial Request
    public void RequestInterstitial()
    {
        // Old Code Remove
        // // Admob Ads
        // // interstitial = new InterstitialAd(androidAdsUnit.interstitialAdUnit);
        // // interstitial = new InterstitialAd("ca-app-pub-3940256099942544/1033173712");
        // interstitial = new GoogleMobileAds.Api.InterstitialAd(AdmobMediationInterstitialID);
        // // Register for ad events.
        // interstitial.OnAdLoaded += HandleInterstitialLoaded;
        // interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
        // interstitial.OnAdOpening += HandleInterstitialOpened;
        // interstitial.OnAdClosed += HandleInterstitialClosed;
        // interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
        // interstitial.OnAdDidRecordImpression += HandleInterstitialDidRecordImpression;
        // interstitial.OnPaidEvent += HandleInterstitialPaidEvent;
        // // Load an interstitial ad.
        // interstitial.LoadAd(createAdRequest());

        // New Code
        // Clean up the old ad before loading a new one.
        if (interstitial != null)
        {
            interstitial.Destroy();
            interstitial = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        // adRequest.Keywords.Add("unity-admob-sample");

        Debug.Log("interstitial Id : " + DefaultInterstitialID);
        // send the request to load the ad.
        InterstitialAd.Load(DefaultInterstitialID, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            // if error is not null, the load request failed.
            if (error != null || ad == null)
            {
                Debug.LogError("interstitial ad failed to load an ad " +
                                "with error : " + error);
                return;
            }

            Debug.Log("Interstitial ad loaded with response : "
                        + ad.GetResponseInfo());

            interstitial = ad;
        });
    }

    // interstitial Show
    public void ShowInterstitialAd(Action<ShowResult> callback = null)
    {
        // Old Code Remove
        // if (this.interstitial.IsLoaded())
        // {
        //     SoundManager.Instance.SetMute(true);
        //     // Time.timeScale = 0f;
        //     this.interstitialAdsCallback = callback;
        //     this.interstitial.Show();
        // }
        // else
        // {
        //     this.RequestInterstitial();
        //     if (callback != null)
        //     {
        //         callback(ShowResult.Failed);
        //     }
        // }

        if (interstitial != null && interstitial.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            this.interstitialAdsCallback = callback;
            interstitial.Show();

            base.Invoke("RequestInterstitial", 0.1f);
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            this.RequestInterstitial();
            if (callback != null)
            {
                callback(ShowResult.Failed);
            }
        }
    }

    // CallBack interstitial
    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
            // Loaded Ads
            this.tryLoadInterstitialCount = 0;
            this.isInterstitialAdLoading = false;
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            this.interstitialAdsCallback(ShowResult.Finished);
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            // Closed Ads
            Time.timeScale = 1f;
            base.Invoke("InterstitialAdCallback", 0.1f);
            Debug.Log("Interstitial ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            // OnAdFailedToLoad
            this.tryLoadInterstitialCount++;
            this.isInterstitialAdLoading = false;
            this.interstitialAdsCallback(ShowResult.Failed);
            Debug.LogError("Interstitial ad failed to open full screen content " +
                        "with error : " + error);
        };
    }

    #endregion
    public void RequestRewardBasedVideo()
    {
        // Old Code
        // rewardBasedVideo = new RewardedAd(androidAdsUnit.rewardedAdUnit);
        // rewardBasedVideo = new RewardedAd("ca-app-pub-3940256099942544/5224354917");
        // rewardBasedVideo = new RewardedAd(AdmobMediationRewardvideoID);

        // rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // rewardBasedVideo.OnAdFailedToShow += HandleVideoFailedToShow;
        // rewardBasedVideo.OnAdClosed += HandleVideoClose;
        // rewardBasedVideo.OnAdOpening += HandleVideoOpening;
        // rewardBasedVideo.OnPaidEvent += HandleVideoPaidEvent;
        // rewardBasedVideo.OnUserEarnedReward += HandleVideoUserEarnedReward;

        // rewardBasedVideo.LoadAd(createAdRequest());


        // New Code
        // Clean up the old ad before loading a new one.
        if (rewardBasedVideo != null)
        {
            rewardBasedVideo.Destroy();
            rewardBasedVideo = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        // adRequest.Keywords.Add("unity-admob-sample");

        Debug.Log("Rewarded Id : " + DefaultRewardvideoID);

        // send the request to load the ad.
        RewardedAd.Load(DefaultRewardvideoID, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                    "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

                rewardBasedVideo = ad;
            });
    }

    public bool IsRewardedAdLoaded()
    {
        return rewardBasedVideo != null && rewardBasedVideo.CanShowAd();
    }

    public void ShowRewardedVideoAd(Action<ShowResult> callback)
    {
        // Old Code Remove
        this.isAdRewarded = false;
        // // if (this.rewardBasedVideo != null && this.rewardBasedVideo.IsLoaded())
        // if (this.rewardBasedVideo.IsLoaded())
        // {
        //     SoundManager.Instance.SetMute(true);
        //     Time.timeScale = 0f;
        //     this.rewardedAdsCallback = callback;
        //     this.rewardBasedVideo.Show();
        // }
        // else
        // {
        //     this.RequestRewardBasedVideo();
        //     if (callback != null)
        //     {
        //         callback(ShowResult.Failed);
        //     }
        // }

        // New Code
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardBasedVideo != null && rewardBasedVideo.CanShowAd())
        {
            rewardBasedVideo.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
                this.rewardedAdsCallback(ShowResult.Finished);

                base.Invoke("RequestRewardBasedVideo", 0.1f);
            });
            SoundManager.Instance.SetMute(true);
            Time.timeScale = 0f;
            this.rewardedAdsCallback = callback;
        }
        else
        {
            this.RequestRewardBasedVideo();
            if (callback != null)
            {
                callback(ShowResult.Failed);
            }
        }
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            // Loaded
            this.tryLoadVideoCount = 0;
            this.isRewardedAdLoading = false;
            this.isRewardedAdsLoaded = true;
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            this.isAdRewarded = true;
            this.rewardedAdsCallback(ShowResult.Finished);
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Time.timeScale = 1f;
            base.Invoke("RewardedAdCallback", 0.1f);
            Debug.Log("Rewarded ad full screen content closed.");
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Time.timeScale = 1f;
            this.rewardedAdsCallback(ShowResult.Failed);
            Debug.LogError("Rewarded ad failed to open full screen content " +
                        "with error : " + error);
        };
    }


    // #region  Banner Call back
    //     public void HandleAdLoaded(object sender, EventArgs args)
    //     {
    //         isBannerAdLoaded = true;
    //         MonoBehaviour.print("HandleAdLoaded event received");
    //     }

    //     public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    //     {
    //         isBannerAdLoaded = false;
    //         MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.LoadAdError);
    //     }

    //     public void HandleAdOpened(object sender, EventArgs args)
    //     {
    //         MonoBehaviour.print("HandleAdOpened event received");
    //     }

    //     public void HandleAdClosed(object sender, EventArgs args)
    //     {
    //         MonoBehaviour.print("HandleAdClosed event received");
    //     }

    //     public void HandleAdPaidEvent(object sender, EventArgs args)
    //     {
    //         MonoBehaviour.print("HandleAdLeavingApplication event received");
    //     }
    // #endregion

    // #region admob Interstitial callback
    //     private void HandleInterstitialLoaded(object sender, EventArgs e)
    //     {
    //         Debug.Log("Interstitial start app ad loaded ");
    //         this.tryLoadInterstitialCount = 0;
    //         this.isInterstitialAdLoading = false;
    //         Debug.Log("Nik Log is the HandleInterstitialLoaded");
    //     }

    //     // // private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    //     private void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    //     {
    //         Debug.Log("Nik Log Interstitial start app failed to load " + e.LoadAdError);
    //         this.tryLoadInterstitialCount++;
    //         this.isInterstitialAdLoading = false;
    //         // this.RequestInterstitial();
    //         Debug.Log("Nik Log is the HandleInterstitialFailedToLoad");
    //     }

    //     private void HandleInterstitialOpened(object sender, EventArgs e)
    //     {
    //         Debug.Log("Nik Log is the HandleInterstitialOpened");
    //     }

    //     private void HandleInterstitialClosed(object sender, EventArgs e)
    //     {
    //         Time.timeScale = 1f;
    //         base.Invoke("InterstitialAdCallback", 0.1f);
    //         Debug.Log("Nik Log is the HandleInterstitialClosed");
    //     }

    //     private void HandleInterstitialFailedToShow(object sender, EventArgs e)
    //     {
    //         Debug.Log("Nik Log is the HandleInterstitialFailedToShow");
    //     }

    //     private void HandleInterstitialDidRecordImpression(object sender, EventArgs e)
    //     {
    //         Debug.Log("Nik Log is the HandleInterstitialDidRecordImpression");
    //         // this.interstitialAdsCallback(ShowResult.Finished);
    //     }
    //     private void HandleInterstitialPaidEvent(object sender, EventArgs e)
    //     {
    //         Debug.Log("Nik Log is the HandleInterstitialPaidEvent");
    //     }
    // #endregion

    // #region Admob Video callback

    //     private void HandleRewardBasedVideoLoaded(object sender, EventArgs e)
    //     {
    //         this.tryLoadVideoCount = 0;
    //         this.isRewardedAdLoading = false;
    //         this.isRewardedAdsLoaded = true;
    //         Debug.Log("Admob Video Ads Ready");
    //         Debug.Log("Nik Log is the HandleRewardBasedVideoLoaded");
    //     }

    //     private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    //     {
    //         this.tryLoadVideoCount++;
    //         this.isRewardedAdLoading = false;
    //         this.isRewardedAdsLoaded = false;
    //         // this.RequestRewardBasedVideo();
    //         Debug.Log("Nik Log Admob Video Ads Fail " + e.LoadAdError);
    //         Debug.Log("Nik Log is the HandleRewardBasedVideoFailedToLoad");
    //     }

    //     private void HandleVideoFailedToShow(object sender, EventArgs e)
    //     {
    //         Debug.Log("Nik Log is the HandleVideoFailedToShow");
    //     }
    //     private void HandleVideoClose(object sender, EventArgs e)
    //     {
    //         if (isAdRewarded)
    //         {
    //             // this.rewardedAdsCallback(ShowResult.Finished);
    //             // base.Invoke("RewardedAdCallback", 0.1f);
    //             // Invoke("DelayReward", 0.1f);
    //             DelayReward();
    //             isAdRewarded = false;
    //             StartCoroutine(DelayRequest());
    //             Debug.Log("Nik Log is the HandleVideoClose is REWARD");
    //         }
    //         else
    //         {
    //             Debug.Log("Nik Log is the HandleVideoClose is not REWARD");
    //         }
    //         Debug.Log("Nik Log is the HandleVideoClose");
    //     }

    //     IEnumerator DelayRequest()
    //     {
    //         yield return new WaitForSeconds(0.2f);
    //         RequestRewardBasedVideo();
    //     }

    //     private void HandleVideoOpening(object sender, EventArgs e)
    //     {
    //         Debug.Log("Admob Video Ads Opening");
    //         Debug.Log("Nik Log is the HandleVideoOpening");
    //     }

    //     private void HandleVideoUserEarnedReward(object sender, Reward e)
    //     {
    //         this.isAdRewarded = true;
    //         isAdRewarded = true;
    //         // this.rewardedAdsCallback(ShowResult.Finished);
    //         // Invoke("DelayReward", 1);
    //         Debug.Log("Admob Video Ads EarnedReward");
    //         Debug.Log("Nik Log is the HandleVideoUserEarnedReward");
    //     }

    //     public void DelayReward()
    //     {
    //         Debug.Log("Nik log is the DelayReward in AdmobController in Enter");
    //         this.rewardedAdsCallback(ShowResult.Finished);
    //         Debug.Log("Nik log is the DelayReward in AdmobController in Exit");
    //     }

    //     private void HandleVideoPaidEvent(object sender, EventArgs e)
    //     {
    //         Debug.Log("Nik Log is the HandleVideoPaidEvent");
    //         Time.timeScale = 1f;
    //         base.Invoke("RewardedAdCallback", 0.1f);
    //         Debug.Log("Admob Video Ads PaidEvent");
    //     }
    // #endregion

    // #region facebook_Interstitial_callback
    //     // private void FBHandleInterstitialLoaded()
    //     // {
    //     //     this.tryLoadInterstitialCount = 0;
    //     //     this.isInterstitialAdLoading = false;

    //     //     isLoaded = true;
    //     //     didClose = false;
    //     // }

    //     // private void FBHandleInterstitialFailedToLoad(string error)
    //     // {
    //     //     this.tryLoadInterstitialCount++;
    //     //     this.isInterstitialAdLoading = false;
    //     //     this.RequestInterstitial();
    //     //     Debug.Log("facebook - Interstitial ad failed to load with error: " + error);
    //     // }

    //     // private void FBHandleInterstitialImpression()
    //     // {
    //     //     Debug.Log("facebook - Interstitial FBHandleInterstitialImpression: ");
    //     // }
    //     // private void FBHandleInterstitialClicked()
    //     // {
    //     //     Debug.Log("facebook - Interstitial FBHandleInterstitialClicked: ");
    //     // }

    //     // public void FBHandleInterstitialClosed()
    //     // {
    //     //     Time.timeScale = 1f;
    //     //     Debug.Log("facebook - Interstitial ad did close.");
    //     //     didClose = true;
    //     //     if (interstitialAd != null)
    //     //     {
    //     //        interstitialAd.Dispose();
    //     //     }
    //     //     if (InterstitialObject)
    //     //        Destroy(InterstitialObject);
    //     //     base.Invoke("InterstitialAdCallback", 0.1f);

    //     // }
    //     #endregion facebook_Interstitial_callback

    // #region facebook_Rewardvideo_callback


    //     // private void FBHandleRewardBasedVideoLoaded()
    //     // {
    //     //     this.tryLoadVideoCount = 0;
    //     //     this.isRewardedAdLoading = false;
    //     //     this.isRewardedAdsLoaded = true;

    //     //     Debug.Log("facebook - RewardedVideo ad loaded.");
    //     //     isLoadedVideo = true;
    //     //     didCloseVideo = false;
    //     //     string isAdValid = rewardedVideoAd.IsValid() ? "valid" : "invalid";
    //     //     Debug.Log("Ad loaded and is " + isAdValid + ". Click show to present!");
    //     // }

    //     // private void FBHandleRewardBasedVideoFailedToLoad(string error)
    //     // {
    //     //     this.tryLoadVideoCount++;
    //     //     this.isRewardedAdLoading = false;
    //     //     this.isRewardedAdsLoaded = false;
    //     //     this.RequestRewardBasedVideo();
    //     //     Debug.Log("facebook - RewardedVideo ad failed to load with error: " + error);
    //     // }

    //     // private void FBHandleRewardBasedVideoImpressioned()
    //     // {
    //     //     Debug.Log("facebook - RewardedVideo ad logged impression.");
    //     // }

    //     // private void FBHandleRewardBasedVideoClicked()
    //     // {
    //     //     Debug.Log("facebook - RewardedVideo ad clicked.");
    //     // }

    //     // private void FBHandleRewardBasedVideoRewarded()
    //     // {
    //     //     this.isAdRewarded = true;
    //     //     Debug.Log("facebook - Rewarded video ad validated by server");
    //     // }
    //     // private void FBHandleRewardBasedVideoFailedToRewarded()
    //     // {
    //     //     this.isAdRewarded = false;
    //     //     Debug.Log(" facebook - Rewarded video ad not validated, or no response from server");
    //     // }

    //     // private void FBHandleRewardBasedVideoClosed()
    //     // {
    //     //     Time.timeScale = 1f;
    //     //     Debug.Log("facebook Rewarded video ad did close.");
    //     //     base.Invoke("RewardedAdCallback", 0.1f);
    //     //     base.Invoke("DisposeRewardVideo", 0.1f);
    //     // }

    //     // private void DisposeRewardVideo()
    //     // {
    //     //     Debug.Log("Disposing reward video");
    //     //     didCloseVideo = true;

    //     //     if (rewardedVideoAd != null)
    //     //     {
    //     //        rewardedVideoAd.Dispose();
    //     //     }
    //     //     //// if (RewardVideoObject)
    //     //     ////  Destroy(RewardVideoObject);
    //     // }

    //     // private void FBHandleRewardBasedVideoLeftApplication()
    //     // {
    //     // }
    // #endregion facebook_Rewardvideo_callback

    // #region Nik Ads Controller

    //     public void Nik_RequestInterstitial()
    //     {
    //         // // Admob Ads
    //         // interstitial = new InterstitialAd(androidAdsUnit.interstitialAdUnit);
    //         // // Register for ad events.
    //         // interstitial.OnAdLoaded += HandleInterstitialLoaded;
    //         // interstitial.OnAdFailedToLoad += HandleInterstitialFailedToLoad;
    //         // interstitial.OnAdOpening += HandleInterstitialOpened;
    //         // interstitial.OnAdClosed += HandleInterstitialClosed;
    //         // interstitial.OnAdFailedToShow += HandleInterstitialFailedToShow;
    //         // interstitial.OnAdDidRecordImpression += HandleInterstitialDidRecordImpression;
    //         // interstitial.OnPaidEvent += HandleInterstitialPaidEvent;
    //         // // Load an interstitial ad.
    //         // interstitial.LoadAd(createAdRequest());

    //         // // FB Ads
    //         // Debug.Log ("Nik - FB - Loading interstitial ad...");

    // 		// interstitialAd = new AudienceNetwork.InterstitialAd (facebookInterstitial);

    // 		// interstitialAd.Register (this.gameObject);

    // 		// this.interstitialAd.InterstitialAdDidLoad = (delegate() {
    // 		// 	Debug.Log ("Nik - FB - Interstitial ad loaded.");
    // 		// 	this.isLoaded = true;
    // 		// });
    // 		// interstitialAd.InterstitialAdDidFailWithError = (delegate(string error) {
    // 		// 	Debug.Log ("Nik - FB - Interstitial ad failed to load with error: " + error);
    // 		// });
    // 		// interstitialAd.InterstitialAdWillLogImpression = (delegate() {
    // 		// 	Debug.Log ("Nik - FB - Interstitial ad logged impression.");
    //         //     this.interstitialAdsCallback(ShowResult.Finished);
    // 		// });
    // 		// interstitialAd.InterstitialAdDidClick = (delegate() {
    // 		// 	Debug.Log ("Nik - FB - Interstitial ad clicked.");
    // 		// });

    // 		// // Initiate the request to load the ad.
    // 		// this.interstitialAd.LoadAd ();
    //     }
    //     public void Nik_RequestRewardBasedVideo()
    //     {
    //         // // Admob Ads
    //         // rewardBasedVideo = new RewardedAd(androidAdsUnit.rewardedAdUnit);

    //         // rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
    //         // rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
    //         // rewardBasedVideo.OnAdFailedToShow += HandleVideoFailedToShow;
    //         // rewardBasedVideo.OnAdOpening += HandleVideoOpening;
    //         // rewardBasedVideo.OnPaidEvent += HandleVideoPaidEvent;
    //         // rewardBasedVideo.OnUserEarnedReward += HandleVideoUserEarnedReward;

    //         // rewardBasedVideo.LoadAd(createAdRequest());

    //         // // FB Ads
    //         // this.rewardedVideoAd = new AudienceNetwork.RewardedVideoAd(facebookRewardVideo);

    //         // this.rewardedVideoAd.Register(this.gameObject);

    //         // // Set delegates to get notified on changes or when the user interacts with the ad.
    //         // this.rewardedVideoAd.RewardedVideoAdDidLoad = (delegate() {
    //         //     Debug.Log("Nik - RewardedVideo ad loaded.");
    //         //     this.isLoadedVideo = true;
    //         // });
    //         // this.rewardedVideoAd.RewardedVideoAdDidFailWithError = (delegate(string error) {
    //         //     Debug.Log("Nik - RewardedVideo ad failed to load with error: " + error);
    //         // });
    //         // this.rewardedVideoAd.RewardedVideoAdWillLogImpression = (delegate() {
    //         //     Debug.Log("Nik - RewardedVideo ad logged impression.");
    //         //     this.rewardedAdsCallback(ShowResult.Finished);
    //         // });
    //         // this.rewardedVideoAd.RewardedVideoAdDidClick = (delegate() {
    //         //     Debug.Log("Nik - RewardedVideo ad clicked.");
    //         // });

    //         // this.rewardedVideoAd.RewardedVideoAdDidClose = (delegate() {
    //         //     Debug.Log("Nik - Rewarded video ad did close.");
    //         //     if (this.rewardedVideoAd != null) {
    //         //         this.rewardedVideoAd.Dispose();
    //         //     }
    //         // });

    //         // // Initiate the request to load the ad.
    //         // this.rewardedVideoAd.LoadAd();
    //     }

    //     private AdRequest createAdRequest()
    //     {
    //         return new AdRequest.Builder()
    //                 .Build();

    //     //     // MediationTestSuite.AdRequest = new AdRequest.Builder()
    //     //     // .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
    //     //     // .Build();

    //     //     AdRequest adRequestBuilder = new AdRequest.Builder();
    //     //     MediationTestSuite.AdRequest = adRequestBuilder.Build();
    //     //     return adRequestBuilder;
    //     }

    // #endregion 

}
