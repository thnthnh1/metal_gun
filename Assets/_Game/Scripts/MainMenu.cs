using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;


    private sealed class _StartChallenge_c__AnonStorey0
    {
        internal int priceEntrance;

        internal MainMenu _this;

        internal void __m__0(MasterInfoResponse res)
        {
            Singleton<Popup>.Instance.HideInstantLoading();
            if (res != null)
            {
                if (res.code == 2)
                {
                    Singleton<Popup>.Instance.Show("Please update to the latest version to join Tournament", "Update", PopupType.Ok, delegate
                    {
                        UtilityUnity.OpenStore();
                        this._this.isNewVersionAvailable = false;
                    }, null);
                }
                else
                {
                    //Singleton<FireBaseDatabase>.Instance.timeStartChallenge = Singleton<MasterInfo>.Instance.GetCurrentDateTime();
                    int num = ProfileManager.UserProfile.countPlayTournament;
                    num++;
                    ProfileManager.UserProfile.countPlayTournament.Set(num);
                    GameData.playerResources.ConsumeGem(this.priceEntrance);
                    if (this.priceEntrance <= 0)
                    {
                        GameData.playerResources.ConsumeTournamentTicket(1);
                    }
                    GameData.mode = GameMode.Survival;
                    Loading.nextScene = "GamePlay";
                    Singleton<Popup>.Instance.loading.Show();
                    EventLogger.LogEvent("N_StartTournament", new object[]
                    {
                        "Times=" + num
                    });
                }
            }
            else
            {
                Singleton<Popup>.Instance.Show("Data verification failed. Please try again!", "NOTICE", PopupType.Ok, null, null);
            }
        }

        internal void __m__1()
        {
            UtilityUnity.OpenStore();
            this._this.isNewVersionAvailable = false;
        }
    }

    private sealed class _FillTournamentData_c__AnonStorey1
    {
        internal List<TournamentData> data;

        internal MainMenu _this;

        internal void __m__0(int score)
        {
            Singleton<Popup>.Instance.HideInstantLoading();
            if (GameData.playerTournamentData.score < score)
            {
                GameData.playerTournamentData.score = score;
            }
            //this._this.hudTournament.Open(this.data);
        }
    }

    public static MainMenuNavigation navigation;

    public GameObject panelMainMenu;

    public GameObject panelSoldier;

    public GameObject panelWeapon;

    public GameObject panelSelectStage;

    public GameObject panelQuests;

    public GameObject panelIap;

    public GameObject panelSkillTree;

    public ChestUI panelOpenchestUI;

    public GiftCodeUI giftCodeUI;

    public NewCharaterUI panelNewCharaterUI;

    public MainMenuSwitchCharacterButton switchCharacterButton;

    [Header("DAILY GIFT")]
    public GameObject panelDailyGift;

    public Button btnShowDailyGift;

    [Header("FREE GIFT")]
    public FreeGiftController freeGiftController;
    public Button btnShowSoildierGiftCode;

    [Header("NEWBIE PACK")]
    public Button btnNewbiePack;
    //public NewbieController newbieController;

    [Header("SKILL")]
    public GameObject btnSkill;

    [Header("SPECIAL OFFER")]
    public Button btnStarterPack;
    public GameObject panelStarterPack;
    public TMP_Text textPriceStarterPack;

    public SpecialOfferController specialOfferController;

    [Header("NOTIFICATION")]
    public GameObject notiWeapon;

    public GameObject notiSoldier;

    public GameObject notiDailyGift;

    public Text notiQuests;

    public GameObject notiTournament;

    public Text numberUnusedSkillPoints;

    //[Header("TOURNAMENT")]
    //public HudTournamentRanking hudTournament;

    public GameObject popupLoginFacebook;

    [Space(15f)]
    public Animation[] starterAnims;

    [SerializeField]
    private SceneMenu currentScene;

    private bool isWaitingShowDailyGift;

    private bool isNewVersionAvailable;

    private static UnityAction __f__am_cache0;

    private bool _isOpenFromCharacter = false;

    private void Awake()
    {
        Time.timeScale = 1f;
        EventDispatcher.Instance.RegisterListener(EventID.CheckTimeNewDayDone, delegate (Component sender, object param)
        {
            this.OnCheckTimeNewDayDone();
        });
        EventDispatcher.Instance.RegisterListener(EventID.ViewAdsGetFreeCoin, delegate (Component sender, object param)
        {
            this.CheckAllNotifications();
        });
        EventDispatcher.Instance.RegisterListener(EventID.ClaimDailyGift, delegate (Component sender, object param)
        {
            this.CheckAllNotifications();
        });
        EventDispatcher.Instance.RegisterListener(EventID.BuySpecialOffer, delegate (Component sender, object param)
        {
            this.CheckAllNotifications();
        });
        EventDispatcher.Instance.RegisterListener(EventID.ClickStartTournament, delegate (Component sender, object param)
        {
            this.StartChallenge((int)param);
        });
        EventDispatcher.Instance.RegisterListener(EventID.BuyStarterPack, delegate (Component sender, object param)
        {
            this.CheckAllNotifications();
            this.ShowStarterPack(false);
            this.btnStarterPack.gameObject.SetActive(false);
        });
        EventDispatcher.Instance.RegisterListener(EventID.ClaimNewbiePackage, delegate (Component sender, object param)
        {
            this.ShowNewbiePack(false);
            this.btnNewbiePack.gameObject.SetActive(false);
        });
        EventDispatcher.Instance.RegisterListener(EventID.NewDay, delegate (Component sender, object param)
        {
            DateTime date = (param == null) ? StaticValue.defaultDate : ((DateTime)param);
            DailyGift.date = date;
            if (this.currentScene == SceneMenu.Main)
            {
                this.ShowDailyGift();
            }
            else
            {
                this.isWaitingShowDailyGift = true;
            }
        });
        EventDispatcher.Instance.RegisterListener(EventID.NewVersionAvailable, delegate (Component sender, object param)
        {
            Debug.Log("new version " + param + " - " + sender);
            this.isNewVersionAvailable = true;
        });

        instance = this;
    }

    private void Start()
    {
        SoundManager.Instance.StopSfx();
        SoundManager.Instance.PlayMusic("music_menu", 0f);
        switch (MainMenu.navigation)
        {
            case MainMenuNavigation.OpenWorldMap:
                this.ShowCampaign();
                break;
            case MainMenuNavigation.ShowUpgradeWeapon:
                this.ShowWeapon();
                break;
            case MainMenuNavigation.ShowUpgradeSoldier:
                this.ShowSoldiers();
                break;
            case MainMenuNavigation.ShowUpgradeSkill:
                this.ShowSkillTree();
                break;
        }
        MainMenu.navigation = MainMenuNavigation.None;
        this.freeGiftController.Init();
        this.CheckAllNotifications();
        for (int i = 0; i < this.starterAnims.Length; i++)
        {
            this.starterAnims[i].Play();
        }
        this.btnStarterPack.gameObject.SetActive(!ProfileManager.UserProfile.isPurchasedStarterPack);
        this.btnSkill.gameObject.SetActive(GameData.playerTutorials.IsCompletedStep(TutorialType.Character));
        this.btnNewbiePack.gameObject.SetActive(!ProfileManager.UserProfile.isClaimNewbiePack);
        this.btnShowSoildierGiftCode.gameObject.SetActive(!ProfileManager.UserProfile.isClaimHeroPack);
        this.ShowRecommends();
        this.ShowClaimNewbie();
        this.ShowClaimHero();
        Singleton<Popup>.Instance.setting.AutoBackupData();
        switchCharacterButton.UpdateText();
    }
    private void Update()
    {
        if (UnityEngine.Input.GetKeyUp(KeyCode.Escape))
        {
            this.Back();
        }
    }

    public void Back()
    {
        ShopIAP.Instance.ChangeStatusObjects(true);
        if (GameData.isShowingTutorial)
        {
            return;
        }
        SoundManager.Instance.PlaySfxClick();
        if (this.popupLoginFacebook.activeSelf)
        {
            this.popupLoginFacebook.SetActive(false);
            return;
        }
        if (Singleton<Popup>.Instance.IsInstantLoading)
        {
            return;
        }
        if (Singleton<Popup>.Instance.IsShowing)
        {
            Singleton<Popup>.Instance.Hide();
            return;
        }
        if (this.panelIap.activeSelf)
        {
            switch (this.currentScene)
            {
                case SceneMenu.Main:
                    this.ShowMainMenu();
                    break;
                case SceneMenu.Campaign:
                    this.panelSelectStage.SetActive(true);
                    break;
                case SceneMenu.Soldier:
                    this.panelSoldier.SetActive(true);
                    break;
                case SceneMenu.Weapons:
                    this.panelWeapon.SetActive(true);
                    break;
                case SceneMenu.Setting:
                    this.ShowMainMenu();
                    break;
                case SceneMenu.Quests:
                    this.panelQuests.SetActive(true);
                    break;
                case SceneMenu.SkillTree:
                    this.ShowSkillTree();
                    break;
            }
            this.panelIap.SetActive(false);
            return;
        }
        if (this.panelSkillTree.activeSelf)
        {
            this.panelSkillTree.SetActive(false);
            this.ShowSoldiers();
            return;
        }
        if (this.panelDailyGift.activeSelf)
        {
            this.HideDailyGift();
            return;
        }
        if (this.freeGiftController.gameObject.activeSelf)
        {
            this.freeGiftController.Close();
            return;
        }
        /*if (this.newbieController.View.activeSelf)
        {
            this.newbieController.Close();
            return;
        }*/
        if (this.panelSoldier.activeSelf)
        {
            _isOpenFromCharacter = false;
            this.panelSoldier.SetActive(false);
            this.ShowMainMenu();
            return;
        }
        if (this.panelQuests.activeSelf)
        {
            this.panelQuests.SetActive(false);
            this.ShowMainMenu();
            return;
        }
        if (this.panelWeapon.activeSelf)
        {
            this.panelWeapon.SetActive(false);
            this.ShowMainMenu();
            return;
        }
        if (this.panelSelectStage.activeSelf)
        {
            this.panelSelectStage.SetActive(false);
            this.ShowMainMenu();
            return;
        }
        /*if (this.hudTournament.popupRank.activeSelf)
        {
            this.hudTournament.Close();
            this.ShowMainMenu();
            return;
        }
        if (this.hudTournament.popupRank.activeSelf)
        {
            this.hudTournament.Close();
            this.ShowMainMenu();
            return;
        }*/
        if (this.panelOpenchestUI.gameObject.activeSelf)
        {
            this.panelOpenchestUI.Close();
            this.ShowMainMenu();
            return;
        }
        if (this.panelNewCharaterUI.gameObject.activeSelf)
        {
            this.panelNewCharaterUI.Close();
            this.ShowMainMenu();
            return;
        }
        if (this.panelMainMenu.activeSelf)
        {
            Singleton<Popup>.Instance.Show("do you want to exit game?", PopupTitleID.Confirmation, PopupType.YesNo, delegate
            {
                Application.Quit();
            }, null);
            return;
        }
        if (this.giftCodeUI.gameObject.activeSelf)
        {
            this.giftCodeUI.Close();
            if (_isOpenFromCharacter)
                this.ShowSoldiers();
            else
                this.ShowMainMenu();
            return;
        }
    }

    public void ShowBuyCoinPack()
    {
        this.panelMainMenu.SetActive(false);
        this.panelSoldier.SetActive(false);
        this.panelWeapon.SetActive(false);
        this.panelSelectStage.SetActive(false);
        this.panelDailyGift.SetActive(false);
        this.panelSkillTree.SetActive(false);
        this.panelIap.SetActive(true);
        Singleton<ShopIAP>.Instance.OpenCoinShop();
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowBuyGemPack()
    {
        this.panelMainMenu.SetActive(false);
        this.panelSoldier.SetActive(false);
        this.panelWeapon.SetActive(false);
        this.panelSelectStage.SetActive(false);
        this.panelDailyGift.SetActive(false);
        this.panelSkillTree.SetActive(false);
        this.panelIap.SetActive(true);
        Singleton<ShopIAP>.Instance.OpenGemShop();
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowBuyIapPacks()
    {
        this.panelMainMenu.SetActive(false);
        this.panelSoldier.SetActive(false);
        this.panelWeapon.SetActive(false);
        this.panelSelectStage.SetActive(false);
        this.panelDailyGift.SetActive(false);
        this.panelSkillTree.SetActive(false);
        this.panelIap.SetActive(true);
        Singleton<ShopIAP>.Instance.OpenEssentialShop();
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowBuyTicketPack()
    {
        this.panelMainMenu.SetActive(false);
        this.panelSoldier.SetActive(false);
        this.panelWeapon.SetActive(false);
        this.panelSelectStage.SetActive(false);
        this.panelDailyGift.SetActive(false);
        this.panelSkillTree.SetActive(false);
        this.panelIap.SetActive(true);
        Singleton<ShopIAP>.Instance.OpenTicketsShop();
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowSetting()
    {
        Singleton<Popup>.Instance.setting.Show();
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowStarterPack(bool isShow)
    {
        this.panelStarterPack.SetActive(isShow);
        if (isShow)
        {
            this.textPriceStarterPack.text = InAppPurchaseController.Instance.GetProduct(ProductDefine.STARTER_PACK.productId).metadata.localizedPriceString;
            SoundManager.Instance.PlaySfx("sfx_show_dialog", 0f);
        }
    }

    public void ShowSkillTree()
    {
        this.currentScene = SceneMenu.SkillTree;
        this.panelSoldier.SetActive(false);
        this.panelSkillTree.SetActive(true);
        SoundManager.Instance.PlaySfxClick();
        if (GameData.isShowingTutorial)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepEnterSkillTree);
        }
    }

    public void ShowTournament()
    {
        SoundManager.Instance.PlaySfxClick();
        Singleton<Popup>.Instance.ShowInstantLoading(15);
        Singleton<MasterInfo>.Instance.StartGetData(false, delegate (MasterInfoResponse res)
        {
            if (res != null)
            {
                /*
				if (FB.IsLoggedIn)
				{
					if (Singleton<FireBaseDatabase>.Instance.IsAuthenticated)
					{
						Singleton<FireBaseDatabase>.Instance.GetTopTournament(50, Singleton<MasterInfo>.Instance.GetCurrentWeekRangeString(), delegate(List<TournamentData> data)
						{
							this.FillTournamentData(data);
						});
					}
					else
					{
						Singleton<FireBaseDatabase>.Instance.AuthenWithFacebook(AccessToken.CurrentAccessToken.UserId, AccessToken.CurrentAccessToken.TokenString, new UnityAction<UserInfo>(this.AuthenFirebaseCallback));
					}
				}
				*/
                //else
                {
                    Singleton<Popup>.Instance.HideInstantLoading();
                    this.popupLoginFacebook.SetActive(true);
                }
            }
            else
            {
                Singleton<Popup>.Instance.HideInstantLoading();
                Singleton<Popup>.Instance.ShowToastMessage("Fetching data failed", ToastLength.Long);
            }
        });
    }

    public void ShowSoldiers()
    {
        _isOpenFromCharacter = true;
        this.currentScene = SceneMenu.Soldier;
        this.panelMainMenu.SetActive(false);
        this.panelSoldier.SetActive(true);
        this.panelWeapon.SetActive(false);
        this.panelSelectStage.SetActive(false);
        this.panelQuests.SetActive(false);
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowCampaign()
    {
        this.currentScene = SceneMenu.Campaign;
        this.panelMainMenu.SetActive(false);
        this.panelSoldier.SetActive(false);
        this.panelWeapon.SetActive(false);
        this.panelSelectStage.SetActive(true);
        this.panelQuests.SetActive(false);
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowWeapon()
    {
        this.currentScene = SceneMenu.Weapons;
        this.panelMainMenu.SetActive(false);
        this.panelSoldier.SetActive(false);
        this.panelWeapon.SetActive(true);
        this.panelSelectStage.SetActive(false);
        this.panelQuests.SetActive(false);
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowQuests()
    {
        this.currentScene = SceneMenu.Quests;
        this.panelMainMenu.SetActive(false);
        this.panelSoldier.SetActive(false);
        this.panelWeapon.SetActive(false);
        this.panelSelectStage.SetActive(false);
        this.panelQuests.SetActive(true);
        SoundManager.Instance.PlaySfxClick();
        if (GameData.isShowingTutorial)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepClickMission);
        }
    }
    public void ShowChestUI() => ShowAdditionalUI(SceneMenu.OpenBox);
    public void ShowNewHeroUI() => ShowSingleUI(SceneMenu.NewHero);
    public void ShowSingleUI(SceneMenu sceneMenu)
    {
        this.currentScene = sceneMenu;
        this.panelMainMenu.SetActive(currentScene == SceneMenu.Main);
        this.panelSoldier.SetActive(currentScene == SceneMenu.Soldier);
        this.panelWeapon.SetActive(currentScene == SceneMenu.Weapons);
        this.panelSelectStage.SetActive(currentScene == SceneMenu.Campaign);
        this.panelQuests.SetActive(currentScene == SceneMenu.Quests);
        this.panelOpenchestUI.gameObject.SetActive(currentScene == SceneMenu.OpenBox);
        this.panelNewCharaterUI.gameObject.SetActive(currentScene == SceneMenu.NewHero);
        this.giftCodeUI.gameObject.SetActive(currentScene == SceneMenu.GiftCode);
        SoundManager.Instance.PlaySfxClick();
    }
    public void ShowAdditionalUI(SceneMenu sceneMenu)
    {
        this.currentScene = sceneMenu;
        switch (sceneMenu)
        {
            case SceneMenu.Main:
                break;
            case SceneMenu.Campaign:
                break;
            case SceneMenu.Soldier:
                break;
            case SceneMenu.Weapons:
                break;
            case SceneMenu.Setting:
                break;
            case SceneMenu.Quests:
                break;
            case SceneMenu.Tournament:
                break;
            case SceneMenu.SkillTree:
                break;
            case SceneMenu.OpenBox:
                this.panelOpenchestUI.gameObject.SetActive(currentScene == SceneMenu.OpenBox);
                break;
            case SceneMenu.NewHero:
                break;
            default:
                break;
        }
    }



    public void ShowDailyGift()
    {
        this.panelDailyGift.SetActive(true);
        this.isWaitingShowDailyGift = false;
        SoundManager.Instance.PlaySfx("sfx_show_daily_gift", 0f);
    }

    public void HideDailyGift()
    {
        this.panelDailyGift.SetActive(false);
        if (!ProfileManager.UserProfile.isReceivedDailyGiftToday)
        {
            this.btnShowDailyGift.enabled = true;
        }
    }

    public void ShowFreeGift()
    {
        this.freeGiftController.Open();
        SoundManager.Instance.PlaySfxClick();
        if (GameData.isShowingTutorial && this.currentScene == SceneMenu.Main)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepClickButtonFreeGift);
        }
    }

    public void ShowNewbiePack(bool isShow)
    {
        /*if (isShow)
            this.newbieController.Open();
        else
            this.newbieController.Close();

        SoundManager.Instance.PlaySfxClick();*/
    }

    public void ShowNewGiftCodeUI()
    {
        ShowSingleUI(SceneMenu.GiftCode);
        SoundManager.Instance.PlaySfxClick();
    }

    public void ShowMainMenu()
    {
        switchCharacterButton.UpdateText();
        this.panelMainMenu.SetActive(true);
        this.currentScene = SceneMenu.Main;
        if (this.isWaitingShowDailyGift)
        {
            this.ShowDailyGift();
        }
        else if (this.isNewVersionAvailable)
        {
            this.ShowNewVersionUpdate();
        }
        this.CheckTutorial();
        this.CheckAllNotifications();
    }

    private void CheckTutorial()
    {
        if (PlayerPrefs.GetInt("NotifyTutorial") == 0)
        {
            Debug.Log("Nik log return 1");
            return;
        }
        if (GameData.playerTutorials.IsCompletedStep(TutorialType.WorldMap))
        {
            if (!GameData.playerTutorials.IsCompletedStep(TutorialType.Mission))
            {
                Singleton<TutorialMenuController>.Instance.ShowTutorial(TutorialType.Mission);
            }
            else if (!GameData.playerTutorials.IsCompletedStep(TutorialType.FreeGift))
            {
                Singleton<TutorialMenuController>.Instance.ShowTutorial(TutorialType.FreeGift);
            }
        }
    }

    public void StartChallenge(int priceEntrance)
    {
        Singleton<Popup>.Instance.ShowInstantLoading(15);
        Singleton<MasterInfo>.Instance.StartGetData(true, delegate (MasterInfoResponse res)
        {
            Singleton<Popup>.Instance.HideInstantLoading();
            if (res != null)
            {
                if (res.code == 2)
                {
                    Singleton<Popup>.Instance.Show("Please update to the latest version to join Tournament", "Update", PopupType.Ok, delegate
                    {
                        UtilityUnity.OpenStore();
                        this.isNewVersionAvailable = false;
                    }, null);
                }
                else
                {
                    //Singleton<FireBaseDatabase>.Instance.timeStartChallenge = Singleton<MasterInfo>.Instance.GetCurrentDateTime();
                    int num = ProfileManager.UserProfile.countPlayTournament;
                    num++;
                    ProfileManager.UserProfile.countPlayTournament.Set(num);
                    GameData.playerResources.ConsumeGem(priceEntrance);
                    if (priceEntrance <= 0)
                    {
                        GameData.playerResources.ConsumeTournamentTicket(1);
                    }
                    GameData.mode = GameMode.Survival;
                    Loading.nextScene = "GamePlay";
                    Singleton<Popup>.Instance.loading.Show();
                    EventLogger.LogEvent("N_StartTournament", new object[]
                    {
                        "Times=" + num
                    });
                }
            }
            else
            {
                Singleton<Popup>.Instance.Show("Data verification failed. Please try again!", "NOTICE", PopupType.Ok, null, null);
            }
        });
    }

    public void PlaySurvival()
    {
        GameData.mode = GameMode.Survival;
        Loading.nextScene = "GamePlay";
        Singleton<Popup>.Instance.loading.Show();
    }

    public void LoginFacebookToJoinTournament()
    {
        /*this.popupLoginFacebook.SetActive(false);
        Singleton<Popup>.Instance.ShowInstantLoading(15);
        List<TournamentData> data = new List<TournamentData>();
        //data.Add(new TournamentData("1", 50, 0, true));
        this.hudTournament.Open(data);
        // FacebookSDK Remove
        FbController.Instance.LoginWithReadPermission(new UnityAction<bool>(this.LoginFacebookCallback));*/
    }

    public void LoginGoogleToJoinTournament()
    {
        /*this.popupLoginFacebook.SetActive(false);
        Singleton<Popup>.Instance.ShowInstantLoading(15);
        List<TournamentData> data = new List<TournamentData>();
        //data.Add(new TournamentData("1", 50, 0, true));
        this.hudTournament.Open(data);
        // GoogleController._instance.LoginWithGoogle();
        GoogleController._instance.CheckFirebaseDependencies();
        // GoogleController._instance.LoginWithGoogle(new UnityAction<bool>(this.LoginFacebookCallback));
        // FbController.Instance.LoginWithReadPermission(new UnityAction<bool>(this.LoginFacebookCallback));*/
    }

    private void FillTournamentData(List<TournamentData> data)
    {
        this.currentScene = SceneMenu.Tournament;
        /*
		Singleton<FireBaseDatabase>.Instance.GetTournamentScore(AccessToken.CurrentAccessToken.UserId, Singleton<MasterInfo>.Instance.GetCurrentWeekRangeString(), delegate(int score)
		{
			Singleton<Popup>.Instance.HideInstantLoading();
			if (GameData.playerTournamentData.score < score)
			{
				GameData.playerTournamentData.score = score;
			}
			this.hudTournament.Open(data);
		});
		FirebaseAnalyticsHelper.LogEvent("N_EnterMenuTournament", new object[0]);
		*/

    }

    private bool IsAvailablePlayTournament()
    {
        return MapUtils.IsStagePassed("1.3", Difficulty.Normal);
    }

    private void OnCheckTimeNewDayDone()
    {
        this.btnShowDailyGift.enabled = true;
        this.CheckAllNotifications();
    }

    private void CheckAllNotifications()
    {
        this.CheckNotificationWeapon();
        this.CheckNotificationSoldier();
        this.CheckNotificationQuests();
        this.CheckNotificationDailyGift();
        this.CheckNotificationFreeGifts();
        this.CheckNotificationTournament();
    }

    private void CheckNotificationWeapon()
    {
        bool active = false;
        foreach (KeyValuePair<int, PlayerGunData> current in GameData.playerGuns)
        {
            if (current.Value.isNew)
            {
                active = true;
                break;
            }
        }
        this.notiWeapon.SetActive(active);
    }

    private void CheckNotificationSoldier()
    {
        int num = 0;
        foreach (KeyValuePair<int, PlayerRamboSkillData> current in GameData.playerRamboSkills)
        {
            int unusedSkillPoints = GameData.playerRamboSkills.GetUnusedSkillPoints(current.Key);
            num += unusedSkillPoints;
        }
        if (num > 0)
        {
            this.numberUnusedSkillPoints.text = num.ToString();
            this.numberUnusedSkillPoints.transform.parent.gameObject.SetActive(true);
            this.notiSoldier.SetActive(true);
        }
        else
        {
            this.numberUnusedSkillPoints.transform.parent.gameObject.SetActive(false);
            this.notiSoldier.SetActive(false);
        }
    }

    private void CheckNotificationQuests()
    {
        int numberReadyQuest = GameData.playerDailyQuests.GetNumberReadyQuest();
        int numberReadyAchievement = GameData.playerAchievements.GetNumberReadyAchievement();
        int num = numberReadyQuest + numberReadyAchievement;
        this.notiQuests.text = num.ToString();
        this.notiQuests.transform.parent.gameObject.SetActive(num > 0);
    }

    private void CheckNotificationDailyGift()
    {
        this.notiDailyGift.SetActive(!ProfileManager.UserProfile.isReceivedDailyGiftToday);
    }

    private void CheckNotificationFreeGifts()
    {
        this.freeGiftController.CheckNotification();
    }

    private void CheckNotificationTournament()
    {
        /*
		if (AccessToken.CurrentAccessToken != null)
		{
			bool flag = GameData.playerResources.tournamentTicket > 0 && ProfileManager.UserProfile.countPlayTournament < 5;
			int unclaimRankRewards = this.hudTournament.GetUnclaimRankRewards();
			this.notiTournament.SetActive(flag || unclaimRankRewards > 0);
		}
		*/
        //else
        {
            this.notiTournament.SetActive(false);
        }
    }

    private void ShowNewVersionUpdate()
    {
        Singleton<Popup>.Instance.Show("New version available, please update the game", "Update", PopupType.Ok, delegate
        {
            UtilityUnity.OpenStore();
            this.isNewVersionAvailable = false;
        }, null);
    }

    private void ShowRecommends()
    {
        if (this.currentScene != SceneMenu.Main)
        {
            return;
        }
        if (!GameData.isShowStarterPack)
        {
            if (MapUtils.IsStagePassed("1.1", Difficulty.Normal))
            {
                GameData.isShowStarterPack = true;
                if (!ProfileManager.UserProfile.isPurchasedStarterPack)
                {
                    this.ShowStarterPack(true);
                }
            }
        }
        else if (!GameData.isShowSpecialOffer && MapUtils.IsStagePassed("1.1", Difficulty.Normal))
        {
            GameData.isShowSpecialOffer = true;
            //commented by hardik - no need to show special offer
            // if (this.specialOfferController.isShowPack)
            // {
            // 	this.specialOfferController.Show(true);
            // }
        }
    }

    private void ShowClaimNewbie()
    {
        if (this.currentScene != SceneMenu.Main)
        {
            return;
        }

        this.btnNewbiePack.gameObject.SetActive(!ProfileManager.UserProfile.isClaimNewbiePack);
    }

    public void ShowClaimHero()
    {
        if (this.currentScene != SceneMenu.Main)
        {
            return;
        }

        this.btnShowSoildierGiftCode.gameObject.SetActive(!ProfileManager.UserProfile.isClaimHeroPack);
    }

    public bool IsFBLogin = false;

    private void LoginFacebookCallback(bool success)
    {
        if (success)
        {
            /*
			FbController.Instance.GetLoggedInUserInfomation(delegate(FbUserInfo info)
			{
				Singleton<FireBaseDatabase>.Instance.AuthenWithFacebook(AccessToken.CurrentAccessToken.UserId, AccessToken.CurrentAccessToken.TokenString, new UnityAction<UserInfo>(this.AuthenFirebaseCallback));
			});
			*/
            Debug.Log("Nik Is the FB LogIn SuccessFully");
            IsFBLogin = true;
        }
        else
        {
            Singleton<Popup>.Instance.HideInstantLoading();
            Singleton<Popup>.Instance.ShowToastMessage("Failed to login to Facebook!", ToastLength.Normal);
        }
    }

    private void AuthenFirebaseCallback(UserInfo authUserInfo)
    {
        if (authUserInfo != null)
        {
            GameData.playerTournamentData.id = authUserInfo.id;
            GameData.playerTournamentData.fbName = authUserInfo.name;
            if (GameData.playerTournamentData.sprAvatar == null)
            {
                /*
				FbController.Instance.GetProfilePictureById(authUserInfo.id, delegate(Sprite sprite)
				{
					GameData.playerTournamentData.sprAvatar = sprite;
					this.hudTournament.SetPlayerFbAvatar(sprite);
				});
				*/
            }
            /*
			Singleton<FireBaseDatabase>.Instance.GetTopTournament(50, Singleton<MasterInfo>.Instance.GetCurrentWeekRangeString(), delegate(List<TournamentData> data)
			{
				this.FillTournamentData(data);
			});
			*/
        }
        else
        {
            Singleton<Popup>.Instance.HideInstantLoading();
            Singleton<Popup>.Instance.Show("Authentication failed!", "NOTICE", PopupType.Ok, null, null);
        }
    }
    #region Character

    #endregion
}
public enum SceneMenu
{
    Main,
    Campaign,
    Soldier,
    Weapons,
    Setting,
    Quests,
    Tournament,
    SkillTree,
    OpenBox,
    NewHero,
    GiftCode,
}
public enum ShowType
{
    Single,
    Additional,
}