using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudSelectBooster : MonoBehaviour
{
	public GameObject popup;

	public RectTransform rectTransBoosters;

	[Header("MONEY")]
	public ResourcesChangeText changeTextPrefab;

	public Text textCoin;

	public Text textGem;

	[Header("BOOSTER")]
	public Text textBoosterName;

	public Text textBoosterDescription;

	public Text textIntroduce;

	[Header("STAGE INFO")]
	public Text textStageNameId;

	public Text textDifficulty;

	public Color32 colorDifficultyNormal;

	public Color32 colorDifficultyHard;

	public Color32 colorDifficultyCrazy;

	[Header("AMMO")]
	public Image imgGun;

	public Text textPriceAmmo;

	public Text textCurrentAmmo;

	public Text textMaxAmmo;

	public Button btnBuyFullAmmo;

	public GameObject labelNoSpecialGun;

	public Color32 colorFullAmmo;

	private int costBuyFullAmmo;

	private int maxAmmoCount;

	[Header("PLAYER INFO")]
	public TMP_Text _textName;
	public Text textRamboLevel;

	public Text textGrenadeQuantity;

	public GameObject[] selectedBoosters;

	private void Awake()
	{
		EventDispatcher.Instance.RegisterListener(EventID.SelectBooster, delegate (Component sender, object param)
		{
			this.OnSelectBooster((StaticBoosterData)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ConsumeCoin, delegate (Component sender, object param)
		{
			this.OnConsumeCoin((int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, delegate (Component sender, object param)
		{
			this.OnBuyBooster((BoosterType)param);
		});
	}

	public void Open()
	{
		this.popup.SetActive(true);
		this.SetLayout();
		this.SetCoinAndGem();
		this.SetPlayerInfo();
		this.SetBuyAmmoInfo();
		this.SetActiveBoosters();
		this.ShowGuideText(true);
		if (GameData.mode == GameMode.Campaign)
		{
			this.textStageNameId.gameObject.SetActive(true);
			this.textDifficulty.gameObject.SetActive(true);
			this.SetStageInfo();
			if (GameData.mode == GameMode.Campaign && !GameData.playerTutorials.IsCompletedStep(TutorialType.Booster) && ProfileManager.UserProfile.gunSpecialId == -1)
			{
				Singleton<UIController>.Instance.tutorialGamePlay.ShowTutorialBooster();
			}
		}
		else if (GameData.mode == GameMode.Survival)
		{
			this.textStageNameId.gameObject.SetActive(false);
			this.textDifficulty.gameObject.SetActive(false);
		}
	}

	public void Close()
	{
		this.popup.SetActive(false);
	}

	public void ClosePowerUpAndStartGame()
	{
		this.Close();
		for (int i = 0; i < GameData.selectingBoosters.Count; i++)
		{
			BoosterType type = GameData.selectingBoosters[i];
			GameData.playerBoosters.Consume(type, 1);
		}
		SoundManager.Instance.PlaySfx("sfx_start_mission", 0f);
		GameData.selectingBoosters.Save();
		EventDispatcher.Instance.PostEvent(EventID.SelectBoosterDone);
		if (GameData.isShowingTutorial && string.Compare(GameData.currentStage.id, "1.1") == 0)
		{
			EventDispatcher.Instance.PostEvent(EventID.CompleteStep, TutorialType.Booster);
		}
	}

	public void Home()
	{
		Singleton<UIController>.Instance.BackToMainMenu();
	}

	public void BuyFullAmmo()
	{
		if (this.costBuyFullAmmo <= 0)
		{
			Singleton<Popup>.Instance.ShowToastMessage("Gun has max ammo", ToastLength.Normal);
			return;
		}
		GameData.playerResources.ConsumeCoin(this.costBuyFullAmmo);
		SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
		GameData.playerGuns.SetGunAmmo(ProfileManager.UserProfile.gunSpecialId, this.maxAmmoCount);
		this.textCurrentAmmo.text = this.maxAmmoCount.ToString("n0");
		this.textCurrentAmmo.color = this.colorFullAmmo;
		this.costBuyFullAmmo = 0;
		this.textPriceAmmo.text = this.costBuyFullAmmo.ToString("n0");
		EventDispatcher.Instance.PostEvent(EventID.BuyAmmo);
	}

	private void SetLayout()
	{
		if (ProfileManager.UserProfile.gunSpecialId == -1)
		{
			Vector3 localPosition = this.rectTransBoosters.localPosition;
			localPosition.x = 0f;
			this.rectTransBoosters.localPosition = localPosition;
			this.labelNoSpecialGun.transform.parent.gameObject.SetActive(false);
		}
	}

	private void SetCoinAndGem()
	{
		this.textCoin.text = GameData.playerResources.coin.ToString("n0");
		this.textGem.text = GameData.playerResources.gem.ToString("n0");
	}

	private void OnConsumeCoin(int value)
	{
		this.textCoin.text = GameData.playerResources.coin.ToString("n0");
		ResourcesChangeText resourcesChangeText = Header.poolTextChange.New();
		if (resourcesChangeText == null)
		{
			resourcesChangeText = UnityEngine.Object.Instantiate<ResourcesChangeText>(this.changeTextPrefab);
		}
		resourcesChangeText.Active(false, value, this.textCoin.rectTransform.position, base.transform);
	}

	private void OnSelectBooster(StaticBoosterData data)
	{
		this.ShowGuideText(false);
		this.textBoosterName.text = data.boosterName;
		this.textBoosterDescription.text = data.description;
		this.SetActiveBoosters();
	}

	private void OnBuyBooster(BoosterType type)
	{
		if (type == BoosterType.Grenade)
		{
			int num = ProfileManager.UserProfile.grenadeId;
			if (num == 500)
			{
				int quantityHave = GameData.playerGrenades.GetQuantityHave(num);
				this.textGrenadeQuantity.text = string.Format("x{0:n0}", quantityHave);
			}
		}
		StaticBoosterData data = GameData.staticBoosterData.GetData(type);
		this.ShowGuideText(false);
		this.textBoosterDescription.text = data.description;
		this.textBoosterName.text = data.boosterName;
		this.SetActiveBoosters();
	}

	private void ShowGuideText(bool isShow)
	{
		this.textIntroduce.gameObject.SetActive(isShow);
		this.textBoosterDescription.gameObject.SetActive(!isShow);
		this.textBoosterName.gameObject.SetActive(!isShow);
	}

	private void SetActiveBoosters()
	{
		for (int i = 0; i < this.selectedBoosters.Length; i++)
		{
			GameObject gameObject = this.selectedBoosters[i];
			BoosterType item = (BoosterType)int.Parse(gameObject.name);
			if (GameData.selectingBoosters.Contains(item))
			{
				if (!gameObject.activeInHierarchy)
				{
					gameObject.SetActive(false);
					gameObject.SetActive(true);
				}
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}

	private void SetQuestInfo()
	{
	}

	private void SetPlayerInfo()
	{
		int key = ProfileManager.UserProfile.ramboId;
		int num = (!GameData.playerRambos.ContainsKey(key)) ? 0 : GameData.playerRambos[key].level;
		_textName.text = PlayerPrefs.GetString("playerName");
		this.textRamboLevel.text = string.Format("Lv: {0}", num);
		int id = ProfileManager.UserProfile.grenadeId;
		int quantityHave = GameData.playerGrenades.GetQuantityHave(id);
		this.textGrenadeQuantity.text = string.Format("x{0:n0}", quantityHave);
	}

	private void SetStageInfo()
	{
		this.textStageNameId.text = string.Format("STAGE {0}", GameData.currentStage.id);
		this.textDifficulty.text = GameData.currentStage.difficulty.ToString();
		Difficulty difficulty = GameData.currentStage.difficulty;
		if (difficulty != Difficulty.Normal)
		{
			if (difficulty != Difficulty.Hard)
			{
				if (difficulty == Difficulty.Crazy)
				{
					this.textDifficulty.color = this.colorDifficultyCrazy;
				}
			}
			else
			{
				this.textDifficulty.color = this.colorDifficultyHard;
			}
		}
		else
		{
			this.textDifficulty.color = this.colorDifficultyNormal;
		}
	}

	private void SetBuyAmmoInfo()
	{
		int num = ProfileManager.UserProfile.gunSpecialId;
		this.labelNoSpecialGun.SetActive(num == -1);
		this.imgGun.transform.parent.gameObject.SetActive(num != -1);
		if (num != -1)
		{
			this.imgGun.sprite = GameResourcesUtils.GetGunImage(num);
			this.imgGun.SetNativeSize();
			StaticGunData data = GameData.staticGunData.GetData(num);
			int gunLevel = GameData.playerGuns.GetGunLevel(num);
			int gunAmmo = GameData.playerGuns.GetGunAmmo(num);
			SO_GunStats baseStats = GameData.staticGunData.GetBaseStats(num, gunLevel);
			this.costBuyFullAmmo = (baseStats.Ammo - gunAmmo) * data.ammoPrice;
			this.maxAmmoCount = baseStats.Ammo;
			this.textMaxAmmo.text = this.maxAmmoCount.ToString("n0");
			this.textCurrentAmmo.text = gunAmmo.ToString("n0");
			this.textCurrentAmmo.color = ((this.maxAmmoCount - gunAmmo <= 0) ? this.colorFullAmmo : StaticValue.color32NotEnoughMoney);
			this.textPriceAmmo.text = this.costBuyFullAmmo.ToString("n0");
			this.textPriceAmmo.color = ((GameData.playerResources.coin < this.costBuyFullAmmo) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.btnBuyFullAmmo.enabled = (GameData.playerResources.coin >= this.costBuyFullAmmo);
		}
	}
}
