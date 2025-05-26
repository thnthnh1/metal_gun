using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
//using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWeaponController : MonoBehaviour, IEnhancedScrollerDelegate
{
	public EnhancedScroller scroller;

	public EnhancedScrollerCellView cellViewUpgradeWeapon;

   [Header("ATTRIBUTE")]
	public Text nameStats_1;

	public Text curStats_1;

	public Text maxStats_1;

	public GameObject crossStats_1;

	public Image gradeStats_1;

	public Text nameStats_2;

	public Text curStats_2;

	public Text maxStats_2;

	public GameObject crossStats_2;

	public Image gradeStats_2;

	public Text nameStats_3;

	public Text curStats_3;

	public Text maxStats_3;

	public GameObject crossStats_3;

	public Image gradeStats_3;

	public Text nameStats_4;

	public Text curStats_4;

	public Text maxStats_4;

	public GameObject crossStats_4;

	public Image gradeStats_4;

	public Text remainingAmmo;

	public Text priceBuyFullAmmo;

	public Button btnBuyGrenade;

	public Text quantityGrenade;

	public Text pricePerGrenade;

	public Color32 colorStatsNormal;

	public Color32 colorStatsMax;

    public Text textBattlePower;

	public Sprite[] sprGrades;

	[Header("BUTTONS")]
	public GameObject btnUpgrade;

	public GameObject btnEquip;

	public GameObject btnBuyByBigSalePacks;

	public GameObject btnBuyByStarterPack;

	public GameObject btnGetFromDailyGift;

	public Text priceBuyByCoin;

	public Text priceBuyByGem;

	public Text priceBuyByMedal;

	public Text priceUpgrade;

	[Header("PREVIEW WEAPONS")]
	public RamboPreview rambo;

	[Space(20f)]
	public WeaponTab currentWeaponTab;

	public UpgradeTab[] tabs;

	private int requireMedalBuyWeapon;

	private int requireCoinBuyWeapon;

	private int requireGemBuyWeapon;

	private int requireCoinUpgrade;

	private int requireCoinBuyFullAmmo;

	private int requireCoinBuyPerGrenade;

	private SmallList<CellViewWeaponData> gunData = new SmallList<CellViewWeaponData>();

	private SmallList<CellViewWeaponData> normalGunData = new SmallList<CellViewWeaponData>();

	private SmallList<CellViewWeaponData> specialGunData = new SmallList<CellViewWeaponData>();

	private SmallList<CellViewWeaponData> grenadeData = new SmallList<CellViewWeaponData>();

	private SmallList<CellViewWeaponData> meleeWeaponData = new SmallList<CellViewWeaponData>();

	private SmallList<CellViewWeaponData> currentWeaponData = new SmallList<CellViewWeaponData>();

	private int _SelectingGunId_k__BackingField;

	private int _SelectingGrenadeId_k__BackingField;

	private int _SelectingMeleeWeaponId_k__BackingField;


	public int SelectingGunId
	{
		get;
		set;
	}

	public int SelectingGrenadeId
	{
		get;
		set;
	}

	public int SelectingMeleeWeaponId
	{
		get;
		set;
	}

	private void Awake()
	{
		EventDispatcher.Instance.RegisterListener(EventID.SelectWeaponCellView, new Action<Component, object>(this.OnSelectWeaponCellView));
		EventDispatcher.Instance.RegisterListener(EventID.SwichTabUpgradeWeapon, delegate(Component sender, object param)
		{
			this.OnSwitchTab((WeaponTab)param);
		});
		this.scroller.CreateContainer();
		this.scroller.Delegate = this;
	}

	private void OnEnable()
	{
		this.SelectingGunId = ProfileManager.UserProfile.gunNormalId;
		this.SelectingGrenadeId = ProfileManager.UserProfile.grenadeId;
		this.SelectingMeleeWeaponId = ProfileManager.UserProfile.meleeWeaponId;
		this.CreateGunData();
		this.CreateGrenadeData();
		this.CreateMeleeWeaponData();
		this.currentWeaponData = this.normalGunData;
		this.OnSwitchTab(WeaponTab.Rifle);
		this.UpdateTabNotification();
		this.CheckTutorial();
	}

	private void OnDisable()
	{
		this.currentWeaponTab = WeaponTab.None;
	}

	private void CheckTutorial()
	{
		if (PlayerPrefs.GetInt("NotifyTutorial") == 0)
		{
			UnityEngine.Debug.Log("Nik log return 4");
			return;
		}
		if (!GameData.playerTutorials.IsCompletedStep(TutorialType.Weapon))
		{
			bool flag = GameData.playerGuns.GetGunLevel(0) == 1;
			bool flag2 = !GameData.playerGuns.ContainsKey(107);
			StaticGunData data = GameData.staticGunData.GetData(0);
			int num = data.upgradeInfo[1];
			int num2 = data.upgradeInfo[2];
			bool flag3 = GameData.playerResources.coin >= num + num2;
			if (flag && flag2 && flag3)
			{
				Singleton<TutorialMenuController>.Instance.ShowTutorial(TutorialType.Weapon);
			}
			else
			{
				GameData.playerTutorials.SetComplete(TutorialType.Weapon);
			}
		}
	}

	public void EquipWeapon()
	{
		if (this.currentWeaponTab == WeaponTab.Rifle || this.currentWeaponTab == WeaponTab.Special)
		{
			this.EquipGun();
		}
		else if (this.currentWeaponTab == WeaponTab.Grenade)
		{
			this.EquipGrenade();
		}
		else if (this.currentWeaponTab == WeaponTab.MeleeWeapon)
		{
			this.EquipMeleeWeapon();
		}
		SoundManager.Instance.PlaySfx("sfx_equip_weapon", 0f);
	}

	public void UnlockWeaponByCoin()
	{
		if (this.currentWeaponTab == WeaponTab.Rifle || this.currentWeaponTab == WeaponTab.Special)
		{
			this.UnlockGunByCoin();
		}
		else if (this.currentWeaponTab == WeaponTab.Grenade)
		{
			this.UnlockGrenadeBuyCoin();
		}
		else if (this.currentWeaponTab == WeaponTab.MeleeWeapon)
		{
			this.UnlockMeleeWeaponByCoin();
		}
	}

	public void UnlockWeaponByGem()
	{
		if (this.currentWeaponTab == WeaponTab.Rifle || this.currentWeaponTab == WeaponTab.Special)
		{
			this.UnlockGunByGem();
		}
		else if (this.currentWeaponTab == WeaponTab.Grenade)
		{
			this.UnlockGrenadeByGem();
		}
		else if (this.currentWeaponTab == WeaponTab.MeleeWeapon)
		{
			this.UnlockMeleeWeaponByGem();
		}
	}

	public void UnlockWeaponByMedal()
	{
		if (this.currentWeaponTab == WeaponTab.Rifle || this.currentWeaponTab == WeaponTab.Special)
		{
			this.UnlockGunByMedal();
		}
		else if (this.currentWeaponTab == WeaponTab.Grenade)
		{
			this.UnlockGrenadeByMedal();
		}
		else if (this.currentWeaponTab == WeaponTab.MeleeWeapon)
		{
			this.UnlockMeleeWeaponByMedal();
		}
	}

	public void UpgradeWeapon()
	{
		if (this.currentWeaponTab == WeaponTab.Rifle || this.currentWeaponTab == WeaponTab.Special)
		{
			this.UpgradeGun();
		}
		else if (this.currentWeaponTab == WeaponTab.Grenade)
		{
			this.UpgradeGrenade();
		}
		else if (this.currentWeaponTab == WeaponTab.MeleeWeapon)
		{
			this.UpgradeMeleeWeapon();
		}
	}

	public void OnSwitchTab(WeaponTab tab)
	{
		SoundManager.Instance.PlaySfxClick();
		if (tab == this.currentWeaponTab)
		{
			return;
		}
		switch (tab)
		{
		case WeaponTab.Rifle:
			this.SelectingGunId = ProfileManager.UserProfile.gunNormalId;
			this.currentWeaponData = this.normalGunData;
			for (int i = 0; i < this.gunData.Count; i++)
			{
				this.gunData[i].isSelected = (this.gunData[i].id == this.SelectingGunId);
			}
			this.rambo.EquipGun(this.SelectingGunId);
			break;
		case WeaponTab.Special:
			this.SelectingGunId = ((ProfileManager.UserProfile.gunSpecialId != -1) ? ProfileManager.UserProfile.gunSpecialId : 100);
			this.currentWeaponData = this.specialGunData;
			for (int j = 0; j < this.gunData.Count; j++)
			{
				this.gunData[j].isSelected = (this.gunData[j].id == this.SelectingGunId);
			}
			this.rambo.EquipGun(this.SelectingGunId);
			if (GameData.isShowingTutorial)
			{
				EventDispatcher.Instance.PostEvent(EventID.SubStepSwitchSpecialTab);
			}
			break;
		case WeaponTab.Grenade:
			this.SelectingGrenadeId = ProfileManager.UserProfile.grenadeId;
			this.currentWeaponData = this.grenadeData;
			for (int k = 0; k < this.grenadeData.Count; k++)
			{
				this.grenadeData[k].isSelected = (this.grenadeData[k].id == this.SelectingGrenadeId);
			}
			this.rambo.EquipGrenade(this.SelectingGrenadeId);
			break;
		case WeaponTab.MeleeWeapon:
			this.SelectingMeleeWeaponId = ProfileManager.UserProfile.meleeWeaponId;
			this.currentWeaponData = this.meleeWeaponData;
			for (int l = 0; l < this.meleeWeaponData.Count; l++)
			{
				this.meleeWeaponData[l].isSelected = (this.meleeWeaponData[l].id == this.SelectingMeleeWeaponId);
			}
			this.rambo.EquipMeleeWeapon(this.SelectingMeleeWeaponId);
			break;
		}
		this.currentWeaponTab = tab;
		this.HighLightCurrentTab();
		this.scroller.ReloadData(0f);
		this.UpdateWeaponInformation();
	}

	private void UpdateTabNotification()
	{
		for (int i = 0; i < this.tabs.Length; i++)
		{
			this.tabs[i].UpdateNotification();
		}
	}

	private void HighLightCurrentTab()
	{
		int num = (int)this.currentWeaponTab;
		for (int i = 0; i < this.tabs.Length; i++)
		{
			this.tabs[i].Highlight(i == num - 1);
		}
	}

	private void UpdateWeaponInformation()
	{
		if (this.currentWeaponTab == WeaponTab.Rifle || this.currentWeaponTab == WeaponTab.Special)
		{
			CellViewWeaponData data = this.GetGunData(this.SelectingGunId);
			this.UpdateGunLayout();
			this.UpdateGunAttribute(data);
			this.UpdateGunPrice(data);
		}
		else if (this.currentWeaponTab == WeaponTab.Grenade)
		{
			CellViewWeaponData data2 = this.GetGrenadeData(this.SelectingGrenadeId);
			this.UpdateGrenadeLayout();
			this.UpdateGrenadeAttribute(data2);
			this.UpdateGrenadePrice(data2);
		}
		else if (this.currentWeaponTab == WeaponTab.MeleeWeapon)
		{
			CellViewWeaponData data3 = this.GetMeleeWeaponData(this.SelectingMeleeWeaponId);
			this.UpdateMeleeWeaponLayout();
			this.UpdateMeleeWeaponAttribute(data3);
			this.UpdateMeleeWeaponPrice(data3);
		}

		//Mp_Armory.instance.RefreshRifleWeaponMP();
	}

	private void HideAllButtonsBuy()
	{
		this.priceBuyByCoin.transform.parent.gameObject.SetActive(false);
		this.priceBuyByGem.transform.parent.gameObject.SetActive(false);
		this.priceBuyByMedal.transform.parent.gameObject.SetActive(false);
		this.btnGetFromDailyGift.gameObject.SetActive(false);
		this.btnBuyByBigSalePacks.gameObject.SetActive(false);
		this.btnBuyByStarterPack.gameObject.SetActive(false);
	}

	private void OnSelectWeaponCellView(Component sender, object param)
	{
		SoundManager.Instance.PlaySfxClick();
		CellViewWeaponData cellViewWeaponData = (CellViewWeaponData)param;
		if (GameData.playerGuns.ContainsKey(cellViewWeaponData.id) && GameData.playerGuns[cellViewWeaponData.id].isNew)
		{
			GameData.playerGuns.SetNew(cellViewWeaponData.id, false);
			for (int i = 0; i < this.gunData.Count; i++)
			{
				if (this.gunData[i].id == cellViewWeaponData.id && this.gunData[i].isNew)
				{
					this.gunData[i].isNew = false;
					break;
				}
			}
		}
		if (GameData.playerGrenades.ContainsKey(cellViewWeaponData.id) && GameData.playerGrenades[cellViewWeaponData.id].isNew)
		{
			GameData.playerGrenades.SetNew(cellViewWeaponData.id, false);
			for (int j = 0; j < this.grenadeData.Count; j++)
			{
				if (this.grenadeData[j].id == cellViewWeaponData.id && this.grenadeData[j].isNew)
				{
					this.grenadeData[j].isNew = false;
					break;
				}
			}
		}
		if (GameData.playerMeleeWeapons.ContainsKey(cellViewWeaponData.id) && GameData.playerMeleeWeapons[cellViewWeaponData.id].isNew)
		{
			GameData.playerMeleeWeapons.SetNew(cellViewWeaponData.id, false);
			for (int k = 0; k < this.meleeWeaponData.Count; k++)
			{
				if (this.meleeWeaponData[k].id == cellViewWeaponData.id && this.meleeWeaponData[k].isNew)
				{
					this.meleeWeaponData[k].isNew = false;
					break;
				}
			}
		}
		this.UpdateTabNotification();
		if (this.currentWeaponTab == WeaponTab.Rifle || this.currentWeaponTab == WeaponTab.Special)
		{
			if (this.SelectingGunId == cellViewWeaponData.id)
			{
				return;
			}
			this.SelectingGunId = cellViewWeaponData.id;
			for (int l = 0; l < this.gunData.Count; l++)
			{
				this.gunData[l].isSelected = (this.gunData[l].id == cellViewWeaponData.id);
			}
			this.rambo.EquipGun(this.SelectingGunId);
		}
		else if (this.currentWeaponTab == WeaponTab.Grenade)
		{
			if (this.SelectingGrenadeId == cellViewWeaponData.id)
			{
				return;
			}
			this.SelectingGrenadeId = cellViewWeaponData.id;
			for (int m = 0; m < this.grenadeData.Count; m++)
			{
				this.grenadeData[m].isSelected = (this.grenadeData[m].id == cellViewWeaponData.id);
			}
			this.rambo.EquipGrenade(this.SelectingGrenadeId);
		}
		else if (this.currentWeaponTab == WeaponTab.MeleeWeapon)
		{
			if (this.SelectingMeleeWeaponId == cellViewWeaponData.id)
			{
				return;
			}
			this.SelectingMeleeWeaponId = cellViewWeaponData.id;
			for (int n = 0; n < this.meleeWeaponData.Count; n++)
			{
				this.meleeWeaponData[n].isSelected = (this.meleeWeaponData[n].id == cellViewWeaponData.id);
			}
			this.rambo.EquipMeleeWeapon(this.SelectingMeleeWeaponId);
		}
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
	}

	private void Refresh()
	{
		if (base.gameObject.activeInHierarchy)
		{
			this.CreateGunData();
			this.CreateGrenadeData();
			this.CreateMeleeWeaponData();
			if (this.currentWeaponTab == WeaponTab.Rifle)
			{
				this.currentWeaponData = this.normalGunData;
			}
			else if (this.currentWeaponTab == WeaponTab.Special)
			{
				this.currentWeaponData = this.specialGunData;
			}
			else if (this.currentWeaponTab == WeaponTab.Grenade)
			{
				this.currentWeaponData = this.grenadeData;
			}
			else if (this.currentWeaponTab == WeaponTab.MeleeWeapon)
			{
				this.currentWeaponData = this.meleeWeaponData;
			}
			this.scroller.ReloadData(0f);
			this.UpdateWeaponInformation();
			this.UpdateTabNotification();
		}
	}

	public void BuyFullAmmo()
	{
		if (GameData.playerResources.coin < this.requireCoinBuyFullAmmo)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough gems", ToastLength.Normal);
			SoundManager.Instance.PlaySfxClick();
		}
		else if (this.requireCoinBuyFullAmmo <= 0)
		{
			Singleton<Popup>.Instance.ShowToastMessage("gun has max ammo", ToastLength.Normal);
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeCoin(this.requireCoinBuyFullAmmo);
			this.BuyFullAmmoSuccess();
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
			EventDispatcher.Instance.PostEvent(EventID.BuyAmmo);
		}
	}

	private void UnlockGunByCoin()
	{
		if (GameData.playerResources.coin < this.requireCoinBuyWeapon)
		{
			Singleton<Popup>.Instance.ShowToastMessage("Not enough coins", ToastLength.Normal);
			ShopIAP.Instance.enoughCoinScreen.SetActive(true);
			MainMenu.instance.Back();
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeCoin(this.requireCoinBuyWeapon);
			this.UnlockGunSuccess();
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
			EventLogger.LogEvent("N_UnlockGunByCoin", new object[]
			{
				GameData.staticGunData.GetData(this.SelectingGunId).gunName.Replace(" ", string.Empty)
			});
		}
	}

	private void UnlockGunByGem()
	{
		if (GameData.playerResources.gem < this.requireGemBuyWeapon)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough gems", ToastLength.Normal);
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeGem(this.requireGemBuyWeapon);
			this.UnlockGunSuccess();
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
			EventLogger.LogEvent("N_UnlockGunByGem", new object[]
			{
				GameData.staticGunData.GetData(this.SelectingGunId).gunName.Replace(" ", string.Empty)
			});
		}
	}

	private void UnlockGunByMedal()
	{
		if (GameData.playerResources.medal < this.requireMedalBuyWeapon)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough medals", ToastLength.Normal);
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeMedal(this.requireMedalBuyWeapon);
			this.UnlockGunSuccess();
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
			EventLogger.LogEvent("N_UnlockGunByMedal", new object[]
			{
				GameData.staticGunData.GetData(this.SelectingGunId).gunName.Replace(" ", string.Empty)
			});
		}
	}

	private void UpgradeGun()
	{
		if (GameData.playerResources.coin < this.requireCoinUpgrade)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough coins", ToastLength.Normal);
			ShopIAP.Instance.enoughCoinScreen.SetActive(true);
			MainMenu.instance.Back();
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeCoin(this.requireCoinUpgrade);
			this.UpgradeGunSuccess();
			SoundManager.Instance.PlaySfx("sfx_upgrade_success", 0f);
		}
	}

	private void EquipGun()
	{
		if (this.currentWeaponTab == WeaponTab.Rifle)
		{
			ProfileManager.UserProfile.gunNormalId.Set(this.SelectingGunId);
		}
		else if (this.currentWeaponTab == WeaponTab.Special)
		{
			ProfileManager.UserProfile.gunSpecialId.Set(this.SelectingGunId);
		}
		for (int i = 0; i < this.gunData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.gunData[i];
			cellViewWeaponData.isEquipped = (cellViewWeaponData.id == ProfileManager.UserProfile.gunNormalId || cellViewWeaponData.id == ProfileManager.UserProfile.gunSpecialId);
		}
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
	}

	private void UnlockGunSuccess()
	{
		if (this.currentWeaponTab == WeaponTab.Rifle)
		{
			ProfileManager.UserProfile.gunNormalId.Set(this.SelectingGunId);
		}
		else if (this.currentWeaponTab == WeaponTab.Special)
		{
			ProfileManager.UserProfile.gunSpecialId.Set(this.SelectingGunId);
		}
		for (int i = 0; i < this.gunData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.gunData[i];
			if (cellViewWeaponData.id == this.SelectingGunId)
			{
				this.SelectingGunId = cellViewWeaponData.id;
				cellViewWeaponData.isLock = false;
				cellViewWeaponData.level = 1;
				GameData.playerGuns.ReceiveNewGun(this.SelectingGunId);
			}
			cellViewWeaponData.isEquipped = (cellViewWeaponData.id == ProfileManager.UserProfile.gunNormalId || cellViewWeaponData.id == ProfileManager.UserProfile.gunSpecialId);
			cellViewWeaponData.isSelected = (cellViewWeaponData.id == this.SelectingGunId);
		}
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
	}

	private void UpgradeGunSuccess()
	{
		for (int i = 0; i < this.gunData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.gunData[i];
			if (cellViewWeaponData.id == this.SelectingGunId)
			{
				StaticGunData data = GameData.staticGunData.GetData(cellViewWeaponData.id);
				cellViewWeaponData.level++;
				cellViewWeaponData.level = Mathf.Clamp(cellViewWeaponData.level, 1, data.upgradeInfo.Length);
				cellViewWeaponData.isUpgrading = true;
			}
		}
		GameData.playerGuns.IncreaseGunLevel(this.SelectingGunId);
		EventLogger.LogEvent("N_UpgradeGun", new object[]
		{
			GameData.staticGunData[this.SelectingGunId].gunName.Replace(" ", string.Empty),
			"ToLevel=" + GameData.playerGuns[this.SelectingGunId].level
		});
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
		if (GameData.isShowingTutorial && this.SelectingGunId == 0)
		{
			if (GameData.playerGuns[this.SelectingGunId].level == 2)
			{
				EventDispatcher.Instance.PostEvent(EventID.SubStepUpgradeUziTolevel2);
			}
			else if (GameData.playerGuns[this.SelectingGunId].level == 3)
			{
				EventDispatcher.Instance.PostEvent(EventID.SubStepUpgradeUziTolevel3);
			}
		}
	}

	private void BuyFullAmmoSuccess()
	{
		StaticGunData data = GameData.staticGunData.GetData(this.SelectingGunId);
		int level = GameData.playerGuns[data.id].level;
		string path = string.Format(data.statsPath, level);
		SO_GunStats sO_GunStats = Resources.Load<SO_GunStats>(path);
		GameData.playerGuns.SetGunAmmo(data.id, sO_GunStats.Ammo);
		this.requireCoinBuyFullAmmo = 0;
		this.priceBuyFullAmmo.text = this.requireCoinBuyFullAmmo.ToString("n0");
		this.remainingAmmo.text = string.Format("{0}/{1}", sO_GunStats.Ammo, sO_GunStats.Ammo);
		this.remainingAmmo.color = Color.white;
	}

	private void CreateGunData()
	{
		this.gunData.Clear();
		this.normalGunData.Clear();
		this.specialGunData.Clear();
		foreach (StaticGunData current in GameData.staticGunData.Values)
		{
			CellViewWeaponData cellViewWeaponData = new CellViewWeaponData();
			cellViewWeaponData.id = current.id;
			cellViewWeaponData.weaponName = current.gunName;
			cellViewWeaponData.statsPath = current.statsPath;
			cellViewWeaponData.weaponImage = GameResourcesUtils.GetGunImage(current.id);
			cellViewWeaponData.isLock = !GameData.playerGuns.ContainsKey(current.id);
			cellViewWeaponData.level = ((!cellViewWeaponData.isLock) ? GameData.playerGuns[current.id].level : 0);
			cellViewWeaponData.isSelected = (current.id == this.SelectingGunId);
			cellViewWeaponData.isNew = (GameData.playerGuns.ContainsKey(current.id) && GameData.playerGuns[current.id].isNew);
			if (current.isSpecialGun)
			{
				cellViewWeaponData.isEquipped = (current.id == ProfileManager.UserProfile.gunSpecialId);
				this.specialGunData.Add(cellViewWeaponData);
			}
			else
			{
				cellViewWeaponData.isEquipped = (current.id == ProfileManager.UserProfile.gunNormalId);
				this.normalGunData.Add(cellViewWeaponData);
			}
			this.gunData.Add(cellViewWeaponData);
		}
	}

	private void UpdateGunLayout()
	{
		this.nameStats_1.transform.parent.gameObject.SetActive(true);
	this.nameStats_2.transform.parent.gameObject.SetActive(true);
		this.nameStats_3.transform.parent.gameObject.SetActive(true);
		this.nameStats_4.transform.parent.gameObject.SetActive(true);
		this.crossStats_1.SetActive(true);
		this.crossStats_2.SetActive(false);
		this.crossStats_3.SetActive(false);
		this.crossStats_4.SetActive(false);
		this.nameStats_1.text = "DAMAGE";
	this.nameStats_2.text = "FIRE RATE";
	this.nameStats_3.text = "CRIT RATE";
		this.nameStats_4.text = "CRIT DMG";
		this.btnBuyGrenade.gameObject.SetActive(false);
		this.remainingAmmo.transform.parent.gameObject.SetActive(true);
	}

	private void UpdateGunAttribute(CellViewWeaponData data)
	{
		StaticGunData data2 = GameData.staticGunData.GetData(data.id);
		int num = Mathf.Clamp(data.level, 1, data2.upgradeInfo.Length);
		bool flag = num >= data2.upgradeInfo.Length;
		string path = string.Format(data.statsPath, num);
		SO_GunStats sO_GunStats = Resources.Load<SO_GunStats>(path);
		path = string.Format(data.statsPath, data2.upgradeInfo.Length);
		SO_GunStats sO_GunStats2 = Resources.Load<SO_GunStats>(path);
		this.curStats_1.text = string.Format("{0}", Mathf.RoundToInt(sO_GunStats.Damage * 10f));
		this.curStats_1.color = ((!flag) ? this.colorStatsNormal : this.colorStatsMax);
		this.maxStats_1.text = string.Format("{0}", Mathf.RoundToInt(sO_GunStats2.Damage * 10f));
		WeaponStatsGrade gradeDamage = GameData.staticGunData.GetGradeDamage(sO_GunStats.Damage);
		this.gradeStats_1.sprite = this.sprGrades[(int)gradeDamage];
		this.gradeStats_1.SetNativeSize();
		float fireRate = GameData.staticGunData.GetFireRate(data.id, num);
		this.curStats_2.text = string.Format("{0}", Mathf.RoundToInt(fireRate * 100f));
		this.curStats_2.color = this.colorStatsNormal;
		this.maxStats_2.text = string.Empty;
		WeaponStatsGrade gradeFireRate = GameData.staticGunData.GetGradeFireRate(fireRate);
		this.gradeStats_2.sprite = this.sprGrades[(int)gradeFireRate];
		this.gradeStats_2.SetNativeSize();
		this.curStats_3.text = string.Format("{0}%", sO_GunStats.CriticalRate);
		this.curStats_3.color = this.colorStatsNormal;
		this.maxStats_3.text = string.Empty;
		WeaponStatsGrade gradeCritRate = GameData.staticGunData.GetGradeCritRate(sO_GunStats.CriticalRate);
		this.gradeStats_3.sprite = this.sprGrades[(int)gradeCritRate];
		this.gradeStats_3.SetNativeSize();
		this.curStats_4.text = string.Format("{0}%", sO_GunStats.CriticalDamageBonus + 100f);
		this.curStats_4.color = this.colorStatsNormal;
		this.maxStats_4.text = string.Empty;
		WeaponStatsGrade gradeCritDamage = GameData.staticGunData.GetGradeCritDamage(sO_GunStats.CriticalDamageBonus);
		this.gradeStats_4.sprite = this.sprGrades[(int)gradeCritDamage];
		this.gradeStats_4.SetNativeSize();
		int num2 = Mathf.RoundToInt(GameData.staticGunData.GetBattlePower(data.id, num) * 100f);
	    this.textBattlePower.text = num2.ToString();
	}

	private void UpdateGunPrice(CellViewWeaponData data)
	{
		StaticGunData data2 = GameData.staticGunData.GetData(data.id);
		if (GameData.playerGuns.ContainsKey(data.id))
		{
			this.HideAllButtonsBuy();
			int num = Mathf.Clamp(data.level, 1, data2.upgradeInfo.Length);
			if (num < data2.upgradeInfo.Length)
			{
				this.requireCoinUpgrade = data2.upgradeInfo[num];
				this.priceUpgrade.transform.parent.gameObject.SetActive(true);
				this.priceUpgrade.text = this.requireCoinUpgrade.ToString("n0");
				this.priceUpgrade.color = ((GameData.playerResources.coin < this.requireCoinUpgrade) ? StaticValue.colorNotEnoughMoney : Color.white);
			}
			else
			{
				this.priceUpgrade.transform.parent.gameObject.SetActive(false);
			}
			bool flag = data.id == ProfileManager.UserProfile.gunNormalId || data.id == ProfileManager.UserProfile.gunSpecialId;
			this.btnEquip.gameObject.SetActive(!flag);
			if (data2.isSpecialGun)
			{
				this.remainingAmmo.transform.parent.gameObject.SetActive(true);
				string path = string.Format(data.statsPath, num);
				SO_GunStats sO_GunStats = Resources.Load<SO_GunStats>(path);
				int num2 = Mathf.Clamp(GameData.playerGuns[data.id].ammo, 0, sO_GunStats.Ammo);
				this.remainingAmmo.text = string.Format("{0}/{1}", num2, sO_GunStats.Ammo);
				this.remainingAmmo.color = (((float)num2 / (float)sO_GunStats.Ammo > 0.1f) ? Color.white : StaticValue.colorNotEnoughMoney);
				int num3 = sO_GunStats.Ammo - num2;
				this.requireCoinBuyFullAmmo = num3 * data2.ammoPrice;
				this.priceBuyFullAmmo.text = this.requireCoinBuyFullAmmo.ToString("n0");
				this.priceBuyFullAmmo.color = ((GameData.playerResources.coin < this.requireCoinBuyFullAmmo) ? StaticValue.colorNotEnoughMoney : Color.white);
			}
			else
			{
				this.remainingAmmo.transform.parent.gameObject.SetActive(false);
			}
		}
		else
		{
			this.priceUpgrade.transform.parent.gameObject.SetActive(false);
			this.remainingAmmo.transform.parent.gameObject.SetActive(false);
			this.btnEquip.gameObject.SetActive(false);
			this.priceBuyByCoin.transform.parent.gameObject.SetActive(data2.coinUnlock > 0);
			this.priceBuyByCoin.text = data2.coinUnlock.ToString("n0");
			this.requireCoinBuyWeapon = data2.coinUnlock;
			this.priceBuyByCoin.color = ((GameData.playerResources.coin < this.requireCoinBuyWeapon) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.priceBuyByGem.transform.parent.gameObject.SetActive(data2.gemUnlock > 0);
			this.priceBuyByGem.text = data2.gemUnlock.ToString("n0");
			this.requireGemBuyWeapon = data2.gemUnlock;
			this.priceBuyByGem.color = ((GameData.playerResources.gem < this.requireGemBuyWeapon) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.priceBuyByMedal.transform.parent.gameObject.SetActive(data2.medalUnlock > 0);
			this.priceBuyByMedal.text = data2.medalUnlock.ToString("n0");
			this.requireMedalBuyWeapon = data2.medalUnlock;
			this.priceBuyByMedal.color = ((GameData.playerResources.medal < this.requireMedalBuyWeapon) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.btnGetFromDailyGift.gameObject.SetActive(data2.otherWayObtain.Contains(WayToObtain.DailyQuest));
			this.btnBuyByBigSalePacks.gameObject.SetActive(data2.otherWayObtain.Contains(WayToObtain.BigSalePacks));
			this.btnBuyByStarterPack.gameObject.SetActive(data2.otherWayObtain.Contains(WayToObtain.StarterPack));
		}
	}

	private CellViewWeaponData GetGunData(int gunId)
	{
		for (int i = 0; i < this.gunData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.gunData[i];
			if (cellViewWeaponData.id == gunId)
			{
				return cellViewWeaponData;
			}
		}
		return null;
	}

	public void BuyGrenade()
	{
		if (GameData.playerResources.coin < this.requireCoinBuyPerGrenade)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough coins", ToastLength.Normal);
			ShopIAP.Instance.enoughCoinScreen.SetActive(true);
			MainMenu.instance.Back();
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeCoin(this.requireCoinBuyPerGrenade);
			this.BuyGrenadeSuccess();
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
		}
	}

	private void UnlockGrenadeBuyCoin()
	{
		if (GameData.playerResources.coin < this.requireCoinBuyWeapon)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough coins", ToastLength.Normal);
			ShopIAP.Instance.enoughCoinScreen.SetActive(true);
			MainMenu.instance.Back();
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeCoin(this.requireCoinBuyWeapon);
			this.UnlockGrenadeSuccess(1);
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
		}
	}

	private void UnlockGrenadeByGem()
	{
		if (GameData.playerResources.gem < this.requireGemBuyWeapon)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough gems", ToastLength.Normal);
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeGem(this.requireGemBuyWeapon);
			this.UnlockGrenadeSuccess(1);
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
		}
	}

	private void UnlockGrenadeByMedal()
	{
	}

	private void UpgradeGrenade()
	{
		if (GameData.playerResources.coin < this.requireCoinUpgrade)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough coins", ToastLength.Normal);
			ShopIAP.Instance.enoughCoinScreen.SetActive(true);
			MainMenu.instance.Back();
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeCoin(this.requireCoinUpgrade);
			this.UpgradeGrenadeSuccess();
			SoundManager.Instance.PlaySfx("sfx_upgrade_success", 0f);
		}
	}

	private void EquipGrenade()
	{
		ProfileManager.UserProfile.grenadeId.Set(this.SelectingGrenadeId);
		for (int i = 0; i < this.grenadeData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.grenadeData[i];
			cellViewWeaponData.isEquipped = (cellViewWeaponData.id == ProfileManager.UserProfile.grenadeId);
		}
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
	}

	private void UnlockGrenadeSuccess(int quantity = 1)
	{
		ProfileManager.UserProfile.grenadeId.Set(this.SelectingGrenadeId);
		for (int i = 0; i < this.grenadeData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.grenadeData[i];
			if (cellViewWeaponData.id == this.SelectingGrenadeId)
			{
				this.SelectingGrenadeId = cellViewWeaponData.id;
				cellViewWeaponData.isLock = false;
				cellViewWeaponData.level = 1;
				GameData.playerGrenades.Receive(cellViewWeaponData.id, quantity);
			}
			cellViewWeaponData.isEquipped = (cellViewWeaponData.id == ProfileManager.UserProfile.grenadeId);
			cellViewWeaponData.isSelected = (cellViewWeaponData.id == this.SelectingGrenadeId);
		}
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
	}

	private void UpgradeGrenadeSuccess()
	{
		for (int i = 0; i < this.grenadeData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.grenadeData[i];
			if (cellViewWeaponData.id == this.SelectingGrenadeId)
			{
				StaticGrenadeData data = GameData.staticGrenadeData.GetData(cellViewWeaponData.id);
				cellViewWeaponData.level++;
				cellViewWeaponData.level = Mathf.Clamp(cellViewWeaponData.level, 1, data.upgradeInfo.Length);
			}
		}
		GameData.playerGrenades.IncreaseGrenadeLevel(this.SelectingGrenadeId);
		EventLogger.LogEvent("N_UpgradeGrenade", new object[]
		{
			GameData.staticGrenadeData[this.SelectingGrenadeId].grenadeName,
			"ToLevel=" + GameData.playerGrenades[this.SelectingGrenadeId].level
		});
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
	}

	private void BuyGrenadeSuccess()
	{
		if (GameData.playerGrenades.ContainsKey(this.SelectingGrenadeId))
		{
			GameData.playerGrenades.Receive(this.SelectingGrenadeId, 1);
			this.quantityGrenade.text = GameData.playerGrenades[this.SelectingGrenadeId].quantity.ToString("n0");
			this.quantityGrenade.color = ((GameData.playerGrenades[this.SelectingGrenadeId].quantity <= 0) ? StaticValue.colorNotEnoughMoney : Color.white);
			EventDispatcher.Instance.PostEvent(EventID.BuyGrenade);
		}
	}

	private void CreateGrenadeData()
	{
		this.grenadeData.Clear();
		foreach (StaticGrenadeData current in GameData.staticGrenadeData.Values)
		{
			CellViewWeaponData cellViewWeaponData = new CellViewWeaponData();
			cellViewWeaponData.id = current.id;
			cellViewWeaponData.weaponName = current.grenadeName;
			cellViewWeaponData.statsPath = current.statsPath;
			cellViewWeaponData.weaponImage = GameResourcesUtils.GetGrenadeImage(current.id);
			cellViewWeaponData.isLock = !GameData.playerGrenades.ContainsKey(current.id);
			cellViewWeaponData.level = ((!cellViewWeaponData.isLock) ? GameData.playerGrenades[current.id].level : 0);
			cellViewWeaponData.isNew = (GameData.playerGrenades.ContainsKey(current.id) && GameData.playerGrenades[current.id].isNew);
			cellViewWeaponData.isSelected = (current.id == this.SelectingGrenadeId);
			cellViewWeaponData.isEquipped = (current.id == ProfileManager.UserProfile.grenadeId);
			this.grenadeData.Add(cellViewWeaponData);
		}
	}

	private void UpdateGrenadeLayout()
	{
    this.nameStats_1.transform.parent.gameObject.SetActive(true);
	this.nameStats_2.transform.parent.gameObject.SetActive(true);
		this.nameStats_3.transform.parent.gameObject.SetActive(true);
		this.nameStats_4.transform.parent.gameObject.SetActive(false);
		this.crossStats_1.SetActive(true);
		this.crossStats_2.SetActive(true);
		this.crossStats_3.SetActive(true);
		this.crossStats_4.SetActive(false);
	this.nameStats_1.text = "DAMAGE";
	this.nameStats_2.text = "RADIUS";
		this.nameStats_3.text = "COOLDOWN";
	this.nameStats_4.text = string.Empty;
		this.btnBuyGrenade.gameObject.SetActive(true);
		this.remainingAmmo.transform.parent.gameObject.SetActive(false);
	}

	private void UpdateGrenadeAttribute(CellViewWeaponData data)
	{
		StaticGrenadeData data2 = GameData.staticGrenadeData.GetData(data.id);
		int num = Mathf.Clamp(data.level, 1, data2.upgradeInfo.Length);
		bool flag = num >= data2.upgradeInfo.Length;
		string path = string.Format(data.statsPath, num);
		SO_GrenadeStats sO_GrenadeStats = Resources.Load<SO_GrenadeStats>(path);
		path = string.Format(data.statsPath, data2.upgradeInfo.Length);
		SO_GrenadeStats sO_GrenadeStats2 = Resources.Load<SO_GrenadeStats>(path);
		this.curStats_1.text = string.Format("{0}", Mathf.RoundToInt(sO_GrenadeStats.Damage * 10f));
		this.curStats_1.color = ((!flag) ? this.colorStatsNormal : this.colorStatsMax);
		this.maxStats_1.text = string.Format("{0}", Mathf.RoundToInt(sO_GrenadeStats2.Damage * 10f));
		WeaponStatsGrade gradeDamage = GameData.staticGrenadeData.GetGradeDamage(sO_GrenadeStats.Damage);
		this.gradeStats_1.sprite = this.sprGrades[(int)gradeDamage];
		this.gradeStats_1.SetNativeSize();
		this.curStats_2.text = string.Format("{0}", Mathf.RoundToInt(sO_GrenadeStats.Radius * 100f));
		this.curStats_2.color = ((!flag) ? this.colorStatsNormal : this.colorStatsMax);
		this.maxStats_2.text = string.Format("{0}", Mathf.RoundToInt(sO_GrenadeStats2.Radius * 100f));
		WeaponStatsGrade gradeRadius = GameData.staticGrenadeData.GetGradeRadius(sO_GrenadeStats.Radius);
		this.gradeStats_2.sprite = this.sprGrades[(int)gradeRadius];
		this.gradeStats_2.SetNativeSize();
		this.curStats_3.text = string.Format("{0:f2}s", sO_GrenadeStats.Cooldown);
		this.curStats_3.color = ((!flag) ? this.colorStatsNormal : this.colorStatsMax);
		this.maxStats_3.text = string.Format("{0:f2}s", sO_GrenadeStats2.Cooldown);
		WeaponStatsGrade gradeCooldown = GameData.staticGrenadeData.GetGradeCooldown(sO_GrenadeStats.Cooldown);
		this.gradeStats_3.sprite = this.sprGrades[(int)gradeCooldown];
		this.gradeStats_3.SetNativeSize();
		int num2 = Mathf.RoundToInt(GameData.staticGrenadeData.GetBattlePower(data.id, num) * 100f);
		this.textBattlePower.text = num2.ToString();
	}

	private void UpdateGrenadePrice(CellViewWeaponData data)
	{
		StaticGrenadeData data2 = GameData.staticGrenadeData.GetData(data.id);
		if (GameData.playerGrenades.ContainsKey(data.id))
		{
			this.HideAllButtonsBuy();
			int num = Mathf.Clamp(data.level, 1, data2.upgradeInfo.Length);
			if (num < data2.upgradeInfo.Length)
			{
				this.requireCoinUpgrade = data2.upgradeInfo[num];
				this.priceUpgrade.transform.parent.gameObject.SetActive(true);
				this.priceUpgrade.text = this.requireCoinUpgrade.ToString("n0");
				this.priceUpgrade.color = ((GameData.playerResources.coin < this.requireCoinUpgrade) ? StaticValue.colorNotEnoughMoney : Color.white);
			}
			else
			{
				this.priceUpgrade.transform.parent.gameObject.SetActive(false);
			}
			bool flag = data.id == ProfileManager.UserProfile.grenadeId;
			this.btnEquip.gameObject.SetActive(!flag);
			this.btnBuyGrenade.gameObject.SetActive(true);
			this.quantityGrenade.text = GameData.playerGrenades[data.id].quantity.ToString("n0");
			this.quantityGrenade.color = ((GameData.playerGrenades[data.id].quantity <= 0) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.requireCoinBuyPerGrenade = data2.pricePerUnit;
			this.pricePerGrenade.text = this.requireCoinBuyPerGrenade.ToString("n0");
			this.pricePerGrenade.color = ((GameData.playerResources.coin < this.requireCoinBuyPerGrenade) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.btnBuyGrenade.enabled = true;
		}
		else
		{
			this.priceUpgrade.transform.parent.gameObject.SetActive(false);
			this.btnBuyGrenade.gameObject.SetActive(false);
			this.btnEquip.gameObject.SetActive(false);
			this.priceBuyByCoin.transform.parent.gameObject.SetActive(data2.coinUnlock > 0);
			this.priceBuyByCoin.text = data2.coinUnlock.ToString("n0");
			this.requireCoinBuyWeapon = data2.coinUnlock;
			this.priceBuyByCoin.color = ((GameData.playerResources.coin < this.requireCoinBuyWeapon) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.priceBuyByGem.transform.parent.gameObject.SetActive(data2.gemUnlock > 0);
			this.priceBuyByGem.text = data2.gemUnlock.ToString("n0");
			this.requireGemBuyWeapon = data2.gemUnlock;
			this.priceBuyByGem.color = ((GameData.playerResources.gem < this.requireGemBuyWeapon) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.priceBuyByMedal.transform.parent.gameObject.SetActive(data2.medalUnlock > 0);
			this.priceBuyByMedal.text = data2.medalUnlock.ToString("n0");
			this.requireMedalBuyWeapon = data2.medalUnlock;
			this.priceBuyByMedal.color = ((GameData.playerResources.medal < this.requireMedalBuyWeapon) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.btnGetFromDailyGift.gameObject.SetActive(data2.otherWayObtain.Contains(WayToObtain.DailyQuest));
			this.btnBuyByBigSalePacks.gameObject.SetActive(data2.otherWayObtain.Contains(WayToObtain.BigSalePacks));
			this.btnBuyByStarterPack.gameObject.SetActive(data2.otherWayObtain.Contains(WayToObtain.StarterPack));
		}
	}

	private CellViewWeaponData GetGrenadeData(int gunId)
	{
		for (int i = 0; i < this.grenadeData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.grenadeData[i];
			if (cellViewWeaponData.id == gunId)
			{
				return cellViewWeaponData;
			}
		}
		return null;
	}

	private void UnlockMeleeWeaponByCoin()
	{
		if (GameData.playerResources.coin < this.requireCoinBuyWeapon)
		{
			Singleton<Popup>.Instance.ShowToastMessage("Not enough coins", ToastLength.Normal);
			ShopIAP.Instance.enoughCoinScreen.SetActive(true);
			MainMenu.instance.Back();
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeCoin(this.requireCoinBuyWeapon);
			this.UnlockMeleeWeaponSuccess();
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
			EventLogger.LogEvent("N_UnlockMeleeWeaponByCoin", new object[]
			{
				GameData.staticMeleeWeaponData.GetData(this.SelectingMeleeWeaponId).weaponName
			});
		}
	}

	private void UnlockMeleeWeaponByGem()
	{
		if (GameData.playerResources.gem < this.requireGemBuyWeapon)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough gems", ToastLength.Normal);
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeGem(this.requireGemBuyWeapon);
			this.UnlockMeleeWeaponSuccess();
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
			EventLogger.LogEvent("N_UnlockMeleeWeaponByGem", new object[]
			{
				GameData.staticMeleeWeaponData.GetData(this.SelectingMeleeWeaponId).weaponName
			});
		}
	}

	private void UnlockMeleeWeaponByMedal()
	{
	}

	private void UpgradeMeleeWeapon()
	{
		if (GameData.playerResources.coin < this.requireCoinUpgrade)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough coins", ToastLength.Normal);
			MainMenu.instance.Back();
			ShopIAP.Instance.enoughCoinScreen.SetActive(true);
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeCoin(this.requireCoinUpgrade);
			this.UpgradeMeleeWeaponSuccess();
			SoundManager.Instance.PlaySfx("sfx_upgrade_success", 0f);
		}
	}

	private void EquipMeleeWeapon()
	{
		ProfileManager.UserProfile.meleeWeaponId.Set(this.SelectingMeleeWeaponId);
		for (int i = 0; i < this.meleeWeaponData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.meleeWeaponData[i];
			cellViewWeaponData.isEquipped = (cellViewWeaponData.id == ProfileManager.UserProfile.meleeWeaponId);
		}
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
	}

	private void UnlockMeleeWeaponSuccess()
	{
		ProfileManager.UserProfile.meleeWeaponId.Set(this.SelectingMeleeWeaponId);
		for (int i = 0; i < this.meleeWeaponData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.meleeWeaponData[i];
			if (cellViewWeaponData.id == this.SelectingMeleeWeaponId)
			{
				this.SelectingMeleeWeaponId = cellViewWeaponData.id;
				cellViewWeaponData.isLock = false;
				cellViewWeaponData.level = 1;
				GameData.playerMeleeWeapons.ReceiveNewMeleeWeapon(this.SelectingMeleeWeaponId);
			}
			cellViewWeaponData.isEquipped = (cellViewWeaponData.id == ProfileManager.UserProfile.meleeWeaponId);
			cellViewWeaponData.isSelected = (cellViewWeaponData.id == this.SelectingMeleeWeaponId);
		}
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
	}

	private void UpgradeMeleeWeaponSuccess()
	{
		for (int i = 0; i < this.meleeWeaponData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.meleeWeaponData[i];
			if (cellViewWeaponData.id == this.SelectingMeleeWeaponId)
			{
				StaticMeleeWeaponData data = GameData.staticMeleeWeaponData.GetData(cellViewWeaponData.id);
				cellViewWeaponData.level++;
				cellViewWeaponData.level = Mathf.Clamp(cellViewWeaponData.level, 1, data.upgradeInfo.Length);
			}
		}
		if (GameData.playerMeleeWeapons.ContainsKey(this.SelectingMeleeWeaponId))
		{
			GameData.playerMeleeWeapons.IncreaseMeleeWeaponLevel(this.SelectingMeleeWeaponId);
			EventLogger.LogEvent("N_UpgradeMeleeWeapon", new object[]
			{
				GameData.staticMeleeWeaponData[this.SelectingMeleeWeaponId].weaponName,
				"ToLevel=" + GameData.playerMeleeWeapons[this.SelectingMeleeWeaponId].level
			});
		}
		this.scroller.RefreshActiveCellViews();
		this.UpdateWeaponInformation();
	}

	private void CreateMeleeWeaponData()
	{
		this.meleeWeaponData.Clear();
		foreach (StaticMeleeWeaponData current in GameData.staticMeleeWeaponData.Values)
		{
			CellViewWeaponData cellViewWeaponData = new CellViewWeaponData();
			cellViewWeaponData.id = current.id;
			cellViewWeaponData.weaponName = current.weaponName;
			cellViewWeaponData.statsPath = current.statsPath;
			cellViewWeaponData.weaponImage = GameResourcesUtils.GetMeleeWeaponImage(current.id);
			cellViewWeaponData.isLock = !GameData.playerMeleeWeapons.ContainsKey(current.id);
			cellViewWeaponData.level = ((!cellViewWeaponData.isLock) ? GameData.playerMeleeWeapons[current.id].level : 0);
			cellViewWeaponData.isSelected = (current.id == this.SelectingMeleeWeaponId);
			cellViewWeaponData.isNew = (GameData.playerMeleeWeapons.ContainsKey(current.id) && GameData.playerMeleeWeapons[current.id].isNew);
			cellViewWeaponData.isEquipped = (current.id == ProfileManager.UserProfile.meleeWeaponId);
			this.meleeWeaponData.Add(cellViewWeaponData);
		}
	}

	private void UpdateMeleeWeaponLayout()
	{
		this.nameStats_1.transform.parent.gameObject.SetActive(true);
		this.nameStats_2.transform.parent.gameObject.SetActive(true);
		this.nameStats_3.transform.parent.gameObject.SetActive(true);
		this.nameStats_4.transform.parent.gameObject.SetActive(true);
		this.crossStats_1.SetActive(true);
		this.crossStats_2.SetActive(false);
		this.crossStats_3.SetActive(false);
		this.crossStats_4.SetActive(false);
	this.nameStats_1.text = "DAMAGE";
		this.nameStats_2.text = "ATK RATE";
	this.nameStats_3.text = "CRIT RATE";
	this.nameStats_4.text = "CRIT DMG";
		this.btnBuyGrenade.gameObject.SetActive(false);
		this.remainingAmmo.transform.parent.gameObject.SetActive(false);
	}

	private void UpdateMeleeWeaponAttribute(CellViewWeaponData data)
	{
		StaticMeleeWeaponData data2 = GameData.staticMeleeWeaponData.GetData(data.id);
		int num = Mathf.Clamp(data.level, 1, data2.upgradeInfo.Length);
		bool flag = num >= data2.upgradeInfo.Length;
		string path = string.Format(data.statsPath, num);
		SO_MeleeWeaponStats sO_MeleeWeaponStats = Resources.Load<SO_MeleeWeaponStats>(path);
		path = string.Format(data.statsPath, data2.upgradeInfo.Length);
		SO_MeleeWeaponStats sO_MeleeWeaponStats2 = Resources.Load<SO_MeleeWeaponStats>(path);
		this.curStats_1.text = string.Format("{0}", Mathf.RoundToInt(sO_MeleeWeaponStats.Damage * 10f));
		this.curStats_1.color = ((!flag) ? this.colorStatsNormal : this.colorStatsMax);
		this.maxStats_1.text = string.Format("{0}", Mathf.RoundToInt(sO_MeleeWeaponStats2.Damage * 10f));
		WeaponStatsGrade gradeDamage = GameData.staticMeleeWeaponData.GetGradeDamage(sO_MeleeWeaponStats.Damage);
		this.gradeStats_1.sprite = this.sprGrades[(int)gradeDamage];
		this.gradeStats_1.SetNativeSize();
		this.curStats_2.text = string.Format("{0}", Mathf.RoundToInt(sO_MeleeWeaponStats.AttackTimePerSecond * 100f));
		this.curStats_2.color = this.colorStatsNormal;
		this.maxStats_2.text = string.Empty;
		WeaponStatsGrade gradeAttackSpeed = GameData.staticMeleeWeaponData.GetGradeAttackSpeed(sO_MeleeWeaponStats.AttackTimePerSecond);
		this.gradeStats_2.sprite = this.sprGrades[(int)gradeAttackSpeed];
		this.gradeStats_2.SetNativeSize();
		this.curStats_3.text = string.Format("{0}%", sO_MeleeWeaponStats.CriticalRate);
		this.curStats_3.color = this.colorStatsNormal;
		this.maxStats_3.text = string.Empty;
		WeaponStatsGrade gradeCritRate = GameData.staticMeleeWeaponData.GetGradeCritRate(sO_MeleeWeaponStats.CriticalRate);
		this.gradeStats_3.sprite = this.sprGrades[(int)gradeCritRate];
		this.gradeStats_3.SetNativeSize();
		this.curStats_4.text = string.Format("{0}%", sO_MeleeWeaponStats.CriticalDamageBonus + 100f);
		this.curStats_4.color = this.colorStatsNormal;
		this.maxStats_4.text = string.Empty;
		WeaponStatsGrade gradeCritDamage = GameData.staticMeleeWeaponData.GetGradeCritDamage(sO_MeleeWeaponStats.CriticalDamageBonus);
		this.gradeStats_4.sprite = this.sprGrades[(int)gradeCritDamage];
		this.gradeStats_4.SetNativeSize();
		int num2 = Mathf.RoundToInt(GameData.staticMeleeWeaponData.GetBattlePower(data.id, num) * 100f);
	this.textBattlePower.text = num2.ToString();
	}

	private void UpdateMeleeWeaponPrice(CellViewWeaponData data)
	{
		StaticMeleeWeaponData data2 = GameData.staticMeleeWeaponData.GetData(data.id);
		if (GameData.playerMeleeWeapons.ContainsKey(data.id))
		{
			this.HideAllButtonsBuy();
			int num = Mathf.Clamp(data.level, 1, data2.upgradeInfo.Length);
			if (num < data2.upgradeInfo.Length)
			{
				this.requireCoinUpgrade = data2.upgradeInfo[num];
				this.priceUpgrade.transform.parent.gameObject.SetActive(true);
				this.priceUpgrade.text = this.requireCoinUpgrade.ToString("n0");
				this.priceUpgrade.color = ((GameData.playerResources.coin < this.requireCoinUpgrade) ? StaticValue.colorNotEnoughMoney : Color.white);
			}
			else
			{
				this.priceUpgrade.transform.parent.gameObject.SetActive(false);
			}
			bool flag = data.id == ProfileManager.UserProfile.meleeWeaponId;
			this.btnEquip.gameObject.SetActive(!flag);
		}
		else
		{
			this.priceUpgrade.transform.parent.gameObject.SetActive(false);
			this.btnBuyGrenade.gameObject.SetActive(false);
			this.btnEquip.gameObject.SetActive(false);
			this.priceBuyByCoin.transform.parent.gameObject.SetActive(data2.coinUnlock > 0);
			this.priceBuyByCoin.text = data2.coinUnlock.ToString("n0");
			this.requireCoinBuyWeapon = data2.coinUnlock;
			this.priceBuyByCoin.color = ((GameData.playerResources.coin < this.requireCoinBuyWeapon) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.priceBuyByGem.transform.parent.gameObject.SetActive(data2.gemUnlock > 0);
			this.priceBuyByGem.text = data2.gemUnlock.ToString("n0");
			this.requireGemBuyWeapon = data2.gemUnlock;
			this.priceBuyByGem.color = ((GameData.playerResources.gem < this.requireGemBuyWeapon) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.priceBuyByMedal.transform.parent.gameObject.SetActive(data2.medalUnlock > 0);
			this.priceBuyByMedal.text = data2.medalUnlock.ToString("n0");
			this.requireMedalBuyWeapon = data2.medalUnlock;
			this.priceBuyByMedal.color = ((GameData.playerResources.medal < this.requireMedalBuyWeapon) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.btnGetFromDailyGift.gameObject.SetActive(data2.otherWayObtain.Contains(WayToObtain.DailyQuest));
			this.btnBuyByBigSalePacks.gameObject.SetActive(data2.otherWayObtain.Contains(WayToObtain.BigSalePacks));
			this.btnBuyByStarterPack.gameObject.SetActive(data2.otherWayObtain.Contains(WayToObtain.StarterPack));
		}
	}

	private CellViewWeaponData GetMeleeWeaponData(int id)
	{
		for (int i = 0; i < this.meleeWeaponData.Count; i++)
		{
			CellViewWeaponData cellViewWeaponData = this.meleeWeaponData[i];
			if (cellViewWeaponData.id == id)
			{
				return cellViewWeaponData;
			}
		}
		return null;
	}

	public int GetNumberOfCells(EnhancedScroller scroller)
	{
		return this.currentWeaponData.Count;
	}

	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 126f;
	}

	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		CellViewWeapon cellViewWeapon = scroller.GetCellView(this.cellViewUpgradeWeapon) as CellViewWeapon;
		cellViewWeapon.name = this.currentWeaponData[dataIndex].weaponName;
		cellViewWeapon.SetData(this.currentWeaponData[dataIndex]);
		return cellViewWeapon;
	}
}
