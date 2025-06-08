using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopIAP : Singleton<ShopIAP>
{
    public ShopIAP instance;
    private sealed class _ExchangeMedalToCoin_c__AnonStorey0
    {
        internal int medal;

        internal int coin;

        internal void __m__0()
        {
            GameData.playerResources.ConsumeMedal(this.medal);
            GameData.playerResources.ReceiveCoin(this.coin);
            SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
            EventLogger.LogEvent("N_ExchangeMedalToCoin", new object[]
            {
                "N_Medal=" + this.medal
            });
        }
    }

    private sealed class _ExchangeGemToCoin_c__AnonStorey1
    {
        internal int gem;

        internal int coin;

        internal ShopIAP _this;

        internal void __m__0()
        {
            GameData.playerResources.ConsumeGem(this.gem);
            GameData.playerResources.ReceiveCoin(this.coin);
            SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
            EventLogger.LogEvent("N_ExchangeGemToCoin", new object[]
            {
                "N_Gem=" + this.gem
            });
        }

        internal void __m__1()
        {
            this._this.OpenGemShop();
        }
    }

    public GameObject gemPanel;

    public GameObject coinPanel;

    public GameObject ticketPanel;

    // public GameObject essentialPanel;

    public GameObject bonusGem100;

    public GameObject bonusGem300;

    public GameObject bonusGem500;

    public GameObject bonusGem1000;

    public GameObject bonusGem2500;

    public GameObject bonusGem5000;

    public GameObject btnRestorePurchase;

    public GameObject btnRemoveAds;

    public TMP_Text[] priceLabels;

    public GameObject[] x2Banners;

    public GameObject[] dissableObjects;

    public GameObject enoughCoinScreen;

    public bool IsShowing
    {
        get
        {
            return base.enabled;
        }
    }

    private void Start()
    {
        InAppPurchaseController.Instance.InitializePurchasing(ProductDefine.GetListProducts(), new UnityAction<string>(this.BuyIapSuccessCallback), new UnityAction(this.OnInitialized));

        // this.btnRemoveAds.SetActive(!ProfileManager.UserProfile.isRemoveAds);
        this.btnRemoveAds.SetActive(false);
        //testing coins and gems 
        //GameData.playerResources.ReceiveGem(99999);
        //GameData.playerResources.ReceiveCoin(99999);

        //GameData.playerResources.gem = 0;
        //GameData.playerResources.coin = 0;

        //GameData.playerResources.ReceiveGem(0);
        //GameData.playerResources.ReceiveCoin(0);

        instance = this;
    }

    public void RestorePurchased()
    {
        InAppPurchaseController.Instance.RestorePurchases((result, reason) =>
        {
            if (result)
            {
                this.btnRestorePurchase.SetActive(false);
                PlayerPrefs.SetInt("RestorePurchasedSuccess", 1);
                PlayerPrefs.Save();
            }
        });
    }

    public void ExchangeMedalToCoin(int medal)
    {
        int coin = 0;
        if (medal == 25)
        {
            coin = 2500;
        }
        if (GameData.playerResources.medal >= medal)
        {
            Singleton<Popup>.Instance.Show(string.Format("would you like to exchange <color=#ffff00ff>{0:n0}</color> medal to <color=#ffff00ff>{1:n0}</color> coins?", medal, coin), PopupTitleID.Confirmation, PopupType.YesNo, delegate
            {
                GameData.playerResources.ConsumeMedal(medal);
                GameData.playerResources.ReceiveCoin(coin);
                SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
                EventLogger.LogEvent("N_ExchangeMedalToCoin", new object[]
                {
                    "N_Medal=" + medal
                });
            }, null);
        }
        else
        {
            Singleton<Popup>.Instance.ShowToastMessage("not enough medals", ToastLength.Normal);
        }
        SoundManager.Instance.PlaySfxClick();
    }

    public void ExchangeGemToCoin(int gem)
    {
        int coin = 0;
        if (gem != 25)
        {
            if (gem != 50)
            {
                if (gem != 100)
                {
                    if (gem != 250)
                    {
                        if (gem != 500)
                        {
                            if (gem == 1000)
                            {
                                coin = 150000;
                            }
                        }
                        else
                        {
                            coin = 70000;
                        }
                    }
                    else
                    {
                        coin = 32500;
                    }
                }
                else
                {
                    coin = 12000;
                }
            }
            else
            {
                coin = 5000;
            }
        }
        else
        {
            coin = 2500;
        }

        if (GameData.playerResources.gem >= gem)
        {
            Singleton<Popup>.Instance.Show(string.Format("would you like to exchange <color=#00ffffff>{0:n0}</color> gems to <color=#ffff00ff>{1:n0}</color> coins?", gem, coin), PopupTitleID.Confirmation, PopupType.YesNo, delegate
            {
                GameData.playerResources.ConsumeGem(gem);
                GameData.playerResources.ReceiveCoin(coin);
                SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
                EventLogger.LogEvent("N_ExchangeGemToCoin", new object[]
                {
                    "N_Gem=" + gem
                });
            }, null);
        }
        else
        {
            Singleton<Popup>.Instance.Show(string.Format("not enough gems, would you like to buy some?", new object[0]), PopupTitleID.Confirmation, PopupType.YesNo, delegate
            {
                this.OpenGemShop();
            }, null);
        }
    }

    public void ExchangeGemToTicket(int gem)
    {
        int ticket = 0;
        if (gem != 25)
        {
            if (gem != 50)
            {
                if (gem != 100)
                {
                    if (gem == 150)
                    {
                        ticket = 4;
                    }
                }
                else
                {
                    ticket = 3;
                }
            }
            else
            {
                ticket = 2;
            }
        }
        else
        {
            ticket = 1;
        }

        if (GameData.playerResources.gem >= gem)
        {
            Singleton<Popup>.Instance.Show(string.Format("would you like to exchange <color=#00ffffff>{0:n0}</color> gems to <color=#ffff00ff>{1:n0}</color> tickets?", gem, ticket), PopupTitleID.Confirmation, PopupType.YesNo, delegate
            {
                GameData.playerResources.ConsumeGem(gem);
                GameData.playerResources.ReceiveTournamentTicket(ticket);
                SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);

                //Mp_Armory.instance.RefreshTickets();
            }, null);
        }
        else
        {
            Singleton<Popup>.Instance.Show(string.Format("not enough gems, would you like to buy some?", new object[0]), PopupTitleID.Confirmation, PopupType.YesNo, delegate
            {
                this.OpenGemShop();
            }, null);
        }
    }

    public void BuyGem100()
    {
#if CHEAT_ENABLED
        ProcessBuyGem40();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.GEM_40.productId, null);
    }

    public void BuyGem300()
    {
#if CHEAT_ENABLED
        ProcessBuyGem80();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.GEM_80.productId, null);
    }

    public void BuyGem500()
    {
#if CHEAT_ENABLED
        ProcessBuyGem160();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.GEM_160.productId, null);
    }

    public void BuyGem1000()
    {
#if CHEAT_ENABLED
        ProcessBuyGem350();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.GEM_350.productId, null);
    }

    public void BuyGem2500()
    {
#if CHEAT_ENABLED
        ProcessBuyGem700();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.GEM_700.productId, null);
    }

    public void BuyGem5000()
    {
#if CHEAT_ENABLED
        ProcessBuyGem1250();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.GEM_1250.productId, null);
    }

    public void BuyTicket0()
    {
#if CHEAT_ENABLED
        ProcessBuyTicket5();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.TICKET_5.productId, null);
    }
    public void BuyTicket1()
    {
#if CHEAT_ENABLED
        ProcessBuyTicket17();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.TICKET_17.productId, null);
    }
    public void BuyTicket2()
    {
#if CHEAT_ENABLED
        ProcessBuyTicket35();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.TICKET_35.productId, null);
    }
    public void BuyTicket3()
    {
#if CHEAT_ENABLED
        ProcessBuyTicket80();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.TICKET_80.productId, null);
    }

    public void BuyEverybodyFavorite()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.EVERY_FAVORITE.productId, null);
    }

    public void BuyDragonBreath()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.DRAGON_BREATH.productId, null);
    }

    public void BuyLetThereBeFire()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.LET_THERE_BE_FIRE.productId, null);
    }

    public void BuySnippingForDummies()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.SNIPPING_FOR_DUMMIES.productId, null);
    }

    public void BuyTaserLaser()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.TASER_LASER.productId, null);
    }

    public void BuyShockingSale()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.SHOCKING_SALE.productId, null);
    }

    public void BuyUpgradeEnthusiast()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.UPGRADE_ENTHUSIAST.productId, null);
    }

    public void BuyBattleEssentials1()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.BATTLE_ESSENTIALS_1.productId, null);
    }

    public void BuyBattleEssentials2()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.BATTLE_ESSENTIALS_2.productId, null);
    }

    public void BuyBattleEssentials3()
    {
        // InAppPurchaseController.Instance.BuyProductID(ProductDefine.BATTLE_ESSENTIALS_3.productId, null);
    }

    public void BuyStarterPack()
    {
#if CHEAT_ENABLED
        ProcessBuyStarterPack();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
        return;
#endif

        InAppPurchaseController.Instance.BuyProductID(ProductDefine.STARTER_PACK.productId, null);
    }

    public void BuyRemoveAds()
    {
        InAppPurchaseController.Instance.BuyProductID(ProductDefine.REMOVE_ADS.productId, null);
    }

    public void OpenGemShop()
    {
        this.gemPanel.SetActive(true);
        this.coinPanel.SetActive(false);
        // this.essentialPanel.SetActive(false);
        this.ticketPanel.SetActive(false);
        this.CheckLabelBonusGem();
        SoundManager.Instance.PlaySfxClick();

        ChangeStatusObjects(false);
    }

    public void OpenCoinShop()
    {
        this.gemPanel.SetActive(false);
        this.coinPanel.SetActive(true);
        // this.essentialPanel.SetActive(false);
        this.ticketPanel.SetActive(false);
        SoundManager.Instance.PlaySfxClick();

        ChangeStatusObjects(false);
    }

    public void OpenEssentialShop()
    {
        this.gemPanel.SetActive(false);
        this.coinPanel.SetActive(false);
        // this.essentialPanel.SetActive(true);
        this.ticketPanel.SetActive(false);
        this.CheckPurchased();
        SoundManager.Instance.PlaySfxClick();

        ChangeStatusObjects(false);
    }
    public void OpenTicketsShop()
    {
        this.gemPanel.SetActive(false);
        this.coinPanel.SetActive(false);
        // this.essentialPanel.SetActive(false);
        this.ticketPanel.SetActive(true);
        this.CheckPurchased();
        SoundManager.Instance.PlaySfxClick();

        ChangeStatusObjects(false);
    }

    public void ChangeStatusObjects(bool status)
    {
        if (dissableObjects.Length > 0)
        {
            for (int i = 0; i < dissableObjects.Length; i++)
            {
                dissableObjects[i].SetActive(status);
            }
        }

    }

    public void Hide()
    {
        base.gameObject.SetActive(false);
    }

    private void CheckPurchased()
    {
    }

    private void CheckLabelBonusGem()
    {
        //this.bonusGem100.SetActive(!ProfileManager.UserProfile.isFirstBuyGem100);
        //this.bonusGem300.SetActive(!ProfileManager.UserProfile.isFirstBuyGem300);
        //this.bonusGem500.SetActive(!ProfileManager.UserProfile.isFirstBuyGem500);
        //this.bonusGem1000.SetActive(!ProfileManager.UserProfile.isFirstBuyGem1000);
        //this.bonusGem2500.SetActive(!ProfileManager.UserProfile.isFirstBuyGem2500);
        //this.bonusGem5000.SetActive(!ProfileManager.UserProfile.isFirstBuyGem5000);
    }

    private void BuyIapSuccessCallback(string productId)
    {
        Debug.Log("buyIAP Callback");
        if (string.IsNullOrEmpty(productId))
        {
            return;
        }
        if (string.Equals(productId, ProductDefine.GEM_40.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyGem40();
        }
        else if (string.Equals(productId, ProductDefine.GEM_80.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyGem80();
        }
        else if (string.Equals(productId, ProductDefine.GEM_160.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyGem160();
        }
        else if (string.Equals(productId, ProductDefine.GEM_350.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyGem350();
        }
        else if (string.Equals(productId, ProductDefine.GEM_700.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyGem700();
        }
        else if (string.Equals(productId, ProductDefine.GEM_1250.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyGem1250();
        }
        else if (string.Equals(productId, ProductDefine.STARTER_PACK.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyStarterPack();
        }
        else if (string.Equals(productId, ProductDefine.TICKET_5.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyTicket5();
        }
        else if (string.Equals(productId, ProductDefine.TICKET_17.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyTicket17();
        }
        else if (string.Equals(productId, ProductDefine.TICKET_35.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyTicket35();
        }
        else if (string.Equals(productId, ProductDefine.TICKET_80.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyTicket80();
        }
        // else if (string.Equals(productId, ProductDefine.EVERY_FAVORITE.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuyEverybodyFavorite();
        // }
        // else if (string.Equals(productId, ProductDefine.DRAGON_BREATH.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuyDragonBreath();
        // }
        // else if (string.Equals(productId, ProductDefine.LET_THERE_BE_FIRE.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuyLetThereBeFire();
        // }
        // else if (string.Equals(productId, ProductDefine.SNIPPING_FOR_DUMMIES.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuySnippingForDummies();
        // }
        // else if (string.Equals(productId, ProductDefine.TASER_LASER.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuyTaserLaser();
        // }
        // else if (string.Equals(productId, ProductDefine.SHOCKING_SALE.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuyShockingSale();
        // }
        // else if (string.Equals(productId, ProductDefine.UPGRADE_ENTHUSIAST.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuyUpgradeEnthusiast();
        // }
        // else if (string.Equals(productId, ProductDefine.BATTLE_ESSENTIALS_1.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuyBattleEssentials_1();
        // }
        // else if (string.Equals(productId, ProductDefine.BATTLE_ESSENTIALS_2.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuyBattleEssentials_2();
        // }
        // else if (string.Equals(productId, ProductDefine.BATTLE_ESSENTIALS_3.productId, StringComparison.Ordinal))
        // {
        //     this.ProcessBuyBattleEssentials_3();
        // }
        else if (string.Equals(productId, ProductDefine.REMOVE_ADS.productId, StringComparison.Ordinal))
        {
            this.ProcessBuyRemoveAds();
        }
        this.CheckPurchased();
        ProfileManager.SaveAll();
        SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
    }

    private void OnInitialized()
    {
        ProductIAP[] listProducts = ProductDefine.GetListProducts();
        for (int i = 0; i < this.priceLabels.Length; i++)
        {
            string localizedPriceString = InAppPurchaseController.Instance.GetProduct(listProducts[i].productId).metadata.localizedPriceString;
            this.priceLabels[i].text = localizedPriceString;
            Debug.Log("product id - " + listProducts[i].productId + " price - " + this.priceLabels[i].text);
        }

        x2Banners[0].SetActive(!ProfileManager.UserProfile.isFirstBuyGem40);
        x2Banners[1].SetActive(!ProfileManager.UserProfile.isFirstBuyGem80);
        x2Banners[2].SetActive(!ProfileManager.UserProfile.isFirstBuyGem160);
        x2Banners[3].SetActive(!ProfileManager.UserProfile.isFirstBuyGem350);
        x2Banners[4].SetActive(!ProfileManager.UserProfile.isFirstBuyGem700);
        x2Banners[5].SetActive(!ProfileManager.UserProfile.isFirstBuyGem1250);
    }

    private void ProcessBuyGem40()
    {
        int num = (!ProfileManager.UserProfile.isFirstBuyGem40) ? 80 : 40;
        GameData.playerResources.ReceiveGem(num);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} gems", num), ToastLength.Normal);
        ProfileManager.UserProfile.isFirstBuyGem40.Set(true);
        this.CheckLabelBonusGem();
        EventLogger.LogEvent("N_BuyGem", new object[]
        {
            40
        });
        x2Banners[0].SetActive(false);
    }

    private void ProcessBuyGem80()
    {
        int num = (!ProfileManager.UserProfile.isFirstBuyGem80) ? 160 : 80;
        GameData.playerResources.ReceiveGem(num);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} gems", num), ToastLength.Normal);
        ProfileManager.UserProfile.isFirstBuyGem80.Set(true);
        this.CheckLabelBonusGem();
        EventLogger.LogEvent("N_BuyGem", new object[]
        {
            80
        });
        x2Banners[1].SetActive(false);
    }

    private void ProcessBuyGem160()
    {
        int num = (!ProfileManager.UserProfile.isFirstBuyGem160) ? 320 : 160;
        GameData.playerResources.ReceiveGem(num);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} gems", num), ToastLength.Normal);
        ProfileManager.UserProfile.isFirstBuyGem160.Set(true);
        this.CheckLabelBonusGem();
        EventLogger.LogEvent("N_BuyGem", new object[]
        {
            160
        });
        x2Banners[2].SetActive(false);
    }

    private void ProcessBuyGem350()
    {
        int num = (!ProfileManager.UserProfile.isFirstBuyGem350) ? 700 : 350;
        GameData.playerResources.ReceiveGem(num);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} gems", num), ToastLength.Normal);
        ProfileManager.UserProfile.isFirstBuyGem350.Set(true);
        this.CheckLabelBonusGem();
        EventLogger.LogEvent("N_BuyGem", new object[]
        {
            350
        });
        x2Banners[3].SetActive(false);
    }

    private void ProcessBuyGem700()
    {
        int num = (!ProfileManager.UserProfile.isFirstBuyGem700) ? 1400 : 700;
        GameData.playerResources.ReceiveGem(num);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} gems", num), ToastLength.Normal);
        ProfileManager.UserProfile.isFirstBuyGem700.Set(true);
        this.CheckLabelBonusGem();
        EventLogger.LogEvent("N_BuyGem", new object[]
        {
            700
        });
        x2Banners[4].SetActive(false);
    }

    private void ProcessBuyGem1250()
    {
        int num = (!ProfileManager.UserProfile.isFirstBuyGem1250) ? 2500 : 1250;
        GameData.playerResources.ReceiveGem(num);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} gems", num), ToastLength.Normal);
        ProfileManager.UserProfile.isFirstBuyGem1250.Set(true);
        this.CheckLabelBonusGem();
        EventLogger.LogEvent("N_BuyGem", new object[]
        {
            1250
        });
        x2Banners[5].SetActive(false);
    }

    private void ProcessBuyTicket5()
    {
        GameData.playerResources.ReceiveTournamentTicket(5);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} tickets", 5), ToastLength.Normal);
        // ProfileManager.UserProfile.isFirstBuyGem5000.Set(true);
        this.CheckLabelBonusGem();
    }
    private void ProcessBuyTicket17()
    {
        GameData.playerResources.ReceiveTournamentTicket(17);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} tickets", 17), ToastLength.Normal);
        // ProfileManager.UserProfile.isFirstBuyGem5000.Set(true);
        this.CheckLabelBonusGem();
    }
    private void ProcessBuyTicket35()
    {
        GameData.playerResources.ReceiveTournamentTicket(35);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} tickets", 35), ToastLength.Normal);
        this.CheckLabelBonusGem();
    }
    private void ProcessBuyTicket80()
    {
        GameData.playerResources.ReceiveTournamentTicket(80);
        Singleton<Popup>.Instance.ShowToastMessage(string.Format("Received {0} tickets", 80), ToastLength.Normal);
        // ProfileManager.UserProfile.isFirstBuyGem5000.Set(true);
        this.CheckLabelBonusGem();
    }

    private void ProcessBuyEverybodyFavorite()
    {
        if (ProfileManager.UserProfile.isPurchasedPackEverybodyFavorite)
        {
            return;
        }
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_everybody_favorite");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        ProfileManager.UserProfile.isPurchasedPackEverybodyFavorite.Set(true);
        EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);
        EventLogger.LogEvent("N_Buy_EverybodyFavorite", new object[0]);
    }

    private void ProcessBuyDragonBreath()
    {
        if (ProfileManager.UserProfile.isPurchasedPackDragonBreath)
        {
            return;
        }
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_dragon_breath");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        ProfileManager.UserProfile.isPurchasedPackDragonBreath.Set(true);
        EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);
        EventLogger.LogEvent("N_Buy_DragonBreath", new object[0]);
    }

    private void ProcessBuyLetThereBeFire()
    {
        if (ProfileManager.UserProfile.isPurchasedPackLetThereBeFire)
        {
            return;
        }
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_let_there_be_fire");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        ProfileManager.UserProfile.isPurchasedPackLetThereBeFire.Set(true);
        EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);
        EventLogger.LogEvent("N_Buy_LetBeFire", new object[0]);
    }

    private void ProcessBuySnippingForDummies()
    {
        if (ProfileManager.UserProfile.isPurchasedPackSnippingForDummies)
        {
            return;
        }
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_snipping_for_dummies");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        ProfileManager.UserProfile.isPurchasedPackSnippingForDummies.Set(true);
        EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);
        EventLogger.LogEvent("N_Buy_SnippingDummies", new object[0]);
    }

    private void ProcessBuyTaserLaser()
    {
        if (ProfileManager.UserProfile.isPurchasedPackTaserLaser)
        {
            return;
        }
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_taser_laser");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        ProfileManager.UserProfile.isPurchasedPackTaserLaser.Set(true);
        EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);
        EventLogger.LogEvent("N_Buy_TaserLaser", new object[0]);
    }

    private void ProcessBuyShockingSale()
    {
        if (ProfileManager.UserProfile.isPurchasedPackShockingSale)
        {
            return;
        }
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_shocking_sale");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        ProfileManager.UserProfile.isPurchasedPackShockingSale.Set(true);
        EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);
        EventLogger.LogEvent("N_Buy_ShockingSale", new object[0]);
    }

    private void ProcessBuyUpgradeEnthusiast()
    {
        if (ProfileManager.UserProfile.isPurchasedPackUpgradeEnthusiast)
        {
            return;
        }
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_upgrade_enthusiast");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        ProfileManager.UserProfile.isPurchasedPackUpgradeEnthusiast.Set(true);
        EventDispatcher.Instance.PostEvent(EventID.BuySpecialOffer);
        EventLogger.LogEvent("N_Buy_Enthusiast", new object[0]);
    }

    private void ProcessBuyBattleEssentials_1()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_battle_essentitals_1");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        EventLogger.LogEvent("N_Buy_Essential_1", new object[0]);
    }

    private void ProcessBuyBattleEssentials_2()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_battle_essentitals_2");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        EventLogger.LogEvent("N_Buy_Essential_2", new object[0]);
    }

    private void ProcessBuyBattleEssentials_3()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_battle_essentitals_3");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        EventLogger.LogEvent("N_Buy_Essential_3", new object[0]);
    }

    public void ProcessBuyStarterPack()
    {
        if (ProfileManager.UserProfile.isPurchasedStarterPack)
        {
            return;
        }
        TextAsset textAsset = Resources.Load<TextAsset>("JSON/IAP/iap_pack_starter");
        List<RewardData> rewards = JsonConvert.DeserializeObject<List<RewardData>>(textAsset.text);
        RewardUtils.Receive(rewards);
        Singleton<Popup>.Instance.ShowReward(rewards, null, null);
        ProfileManager.UserProfile.isPurchasedStarterPack.Set(true);
        EventDispatcher.Instance.PostEvent(EventID.BuyStarterPack);
        EventLogger.LogEvent("N_Buy_Starter_Pack", new object[0]);
    }

    public void ProcessBuyRemoveAds()
    {
        if (ProfileManager.UserProfile.isRemoveAds)
        {
            return;
        }
        this.btnRemoveAds.SetActive(false);
        Singleton<Popup>.Instance.Show("Your purchase was successful.\nYou now no longer receive ads.", "Remove ads", PopupType.Ok, null, null);
        ProfileManager.UserProfile.isRemoveAds.Set(true);
        EventLogger.LogEvent("N_Buy_RemoveAds", new object[0]);
    }
}
