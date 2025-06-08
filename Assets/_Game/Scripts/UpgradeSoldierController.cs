using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSoldierController : MonoBehaviour
{
	const int COST_RAMBO_1 = 60000;

	[Header("Buy")]
	[SerializeField] GameObject _btnUpgrade;
	[SerializeField] GameObject _btnBuy;
	[SerializeField] GameObject _btnTakeGift;

	[Header("Characters")]
	[SerializeField] Transform[] _tfCharacters;

	public Text textRamboPrice;
	public Text textRamboName;

	public Text textRamboLevel;

	public Text textCurHp;

	public Text textMaxHp;

	public Text textCurSpeed;

	public Text textMaxSpeed;

	public Text textCoinUpgrade;

	public GameObject notification;

	public Color32 colorNormal;

	public Color32 colorMax;

	private int requireCoinUpgrade;

	private int _SelectingRamboId_k__BackingField;

	public int SelectingRamboId
	{
		get;
		set;
	}

	private void OnEnable()
	{
		this.SelectingRamboId = ProfileManager.UserProfile.ramboId;
		this.UpdateRamboInfomation();
		this.UpdateCharacters();
		this.CheckTutorial();
		this.RefreshUI();
	}

	private void CheckTutorial()
	{
		if (PlayerPrefs.GetInt("NotifyTutorial") == 0)
		{
			UnityEngine.Debug.Log("Nik log return 3");
			return;
		}
		if (!GameData.playerTutorials.IsCompletedStep(TutorialType.Character))
		{
			StaticRamboData data = GameData.staticRamboData.GetData(ProfileManager.UserProfile.ramboId);
			int num = data.upgradeInfo[1];
			if (GameData.playerResources.coin >= num)
			{
				Singleton<TutorialMenuController>.Instance.ShowTutorial(TutorialType.Character);
			}
			else
			{
				GameData.playerTutorials.SetComplete(TutorialType.Character);
			}
		}
	}

	public void UpgradeRambo()
	{
		if (GameData.playerResources.coin < this.requireCoinUpgrade)
		{
			Singleton<Popup>.Instance.ShowToastMessage("Not enough coins", ToastLength.Normal);
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeCoin(this.requireCoinUpgrade);
			this.UpgradeRamboSuccess();
			SoundManager.Instance.PlaySfx("sfx_upgrade_success", 0f);
		}
	}

	private void UpgradeRamboSuccess()
	{
		GameData.playerRambos.IncreaseRamboLevel(this.SelectingRamboId);
		this.UpdateRamboInfomation();
		this.UpdateCharacters();
		this.RefreshUI();

		if (GameData.isShowingTutorial && GameData.playerRambos.GetRamboLevel(this.SelectingRamboId) == 2)
		{
			EventDispatcher.Instance.PostEvent(EventID.SubStepUpgradeRamboToLevel2);
		}

		EventLogger.LogEvent("N_UpgradeRambo", new object[]
		{
			"ToLevel=" + GameData.playerRambos[this.SelectingRamboId].level
		});
	}

	private void UpdateRamboInfomation()
	{
		StaticRamboData data = GameData.staticRamboData.GetData(this.SelectingRamboId);
		this.textRamboName.text = data.ramboName;
		if (GameData.playerRambos.ContainsKey(this.SelectingRamboId))
		{
			int ramboLevel = GameData.playerRambos.GetRamboLevel(this.SelectingRamboId);
			bool flag = ramboLevel >= data.upgradeInfo.Length;
			string path = string.Format("Scriptable Object/Rambo/{0}/rambo_{0}_lv{1}", this.SelectingRamboId, ramboLevel);
			SO_BaseUnitStats sO_BaseUnitStats = Resources.Load<SO_BaseUnitStats>(path);
			path = string.Format("Scriptable Object/Rambo/{0}/rambo_{0}_lv{1}", this.SelectingRamboId, data.upgradeInfo.Length);
			SO_BaseUnitStats sO_BaseUnitStats2 = Resources.Load<SO_BaseUnitStats>(path);
			this.textRamboLevel.gameObject.SetActive(true);
			this.textRamboLevel.text = string.Format("Level {0}", ramboLevel);
			this.textCurHp.text = string.Format("{0:n0}", sO_BaseUnitStats.HP * 10f);
			this.textMaxHp.text = string.Format("{0:n0}", sO_BaseUnitStats2.HP * 10f);
			this.textCurHp.color = ((!flag) ? this.colorNormal : this.colorMax);
			this.textCurSpeed.text = (sO_BaseUnitStats.MoveSpeed * 100f).ToString("n0");
			this.textMaxSpeed.text = (sO_BaseUnitStats2.MoveSpeed * 100f).ToString("n0");
			this.textCurSpeed.color = ((!flag) ? this.colorNormal : this.colorMax);
			this.textCoinUpgrade.transform.parent.gameObject.SetActive(!flag);
			this.textRamboPrice.text = COST_RAMBO_1.ToString("N0");
			if (!flag)
			{
				this.requireCoinUpgrade = data.upgradeInfo[ramboLevel];
				this.textCoinUpgrade.text = this.requireCoinUpgrade.ToString("n0");
				this.textCoinUpgrade.color = ((GameData.playerResources.coin < this.requireCoinUpgrade) ? StaticValue.colorNotEnoughMoney : Color.white);
			}
			_btnUpgrade.SetActive(GameData.playerRambos.GetRamboState(this.SelectingRamboId) == PlayerRamboState.Unlock);
			_btnBuy.SetActive(GameData.playerRambos.GetRamboState(this.SelectingRamboId) == PlayerRamboState.IAP);
			_btnTakeGift.SetActive(GameData.playerRambos.GetRamboState(this.SelectingRamboId) == PlayerRamboState.Gift);
		}
		this.CheckNotification();
	}

	public void RefreshUI()
    {
		int ramboLevel = GameData.playerRambos.GetRamboLevel(this.SelectingRamboId);
		_btnUpgrade.SetActive(!_btnTakeGift.activeSelf & ramboLevel < 10);
	}

	public void UpdateCharacters()
	{
		for (int i = 0; i < _tfCharacters.Length; i++)
		{
			_tfCharacters[i].SetSiblingIndex(this.SelectingRamboId == i ? 3 : 1);
			_tfCharacters[i].transform.localScale = Vector2.one * (this.SelectingRamboId == i ? 1.2f : 1f);
		}
	}

	private void CheckNotification()
	{
		int unusedSkillPoints = GameData.playerRamboSkills.GetUnusedSkillPoints(this.SelectingRamboId);
		this.notification.SetActive(unusedSkillPoints > 0);
	}

	public void OnSelectRambo(int id)
	{
		this.SelectingRamboId = id;
		this.UpdateRamboInfomation();
		this.UpdateCharacters();
		this.RefreshUI();

		if (GameData.playerRambos[SelectingRamboId].state == PlayerRamboState.Unlock)
		{
			S.Instance.characterDat.curRambo = id;
			ProfileManager.UserProfile.ramboId.Set(S.Instance.characterDat.curRambo);
			S.Instance.Save();
		}
	}

	public void OnBuyRambo()
	{
		if (GameData.playerResources.gem >= COST_RAMBO_1)
		{
			Singleton<Popup>.Instance.Show(string.Format("Would you like to buy this hero by <color=#00ffffff>{0:n0}</color> gems?", COST_RAMBO_1), PopupTitleID.Confirmation, PopupType.YesNo, delegate
			{
				GameData.playerResources.ConsumeGem(COST_RAMBO_1);

				GameData.playerRambos[SelectingRamboId].state = PlayerRamboState.Unlock;
				GameData.playerRambos.Save();
				this.UpdateRamboInfomation();
				this.RefreshUI();

				SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
			}, null);
		}
		else
		{
			Singleton<Popup>.Instance.Show(string.Format("Not enough gems, would you like to buy some?", new object[0]), PopupTitleID.Confirmation, PopupType.YesNo, delegate
			{
				MainMenu.instance.ShowBuyGemPack();
			}, null);
		}
	}

	public void OnOpenGift()
	{
		MainMenu.instance.ShowNewGiftCodeUI();
	}
}
