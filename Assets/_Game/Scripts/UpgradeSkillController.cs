using System;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeSkillController : MonoBehaviour
{
	public static int ramboId;

	public Image ramboIcon;

	public Image skillIcon;

	public Text textGuide;

	public Text textSkillName;

	public Text textSkillDescriptionMain;

	public Text textSkillDescriptionSub;

	public Text textPriceUpgrade;

	public Text textSkillPoint;

	public Text textPriceResetSkill;

	public Button btnTrain;

	public Button btnReset;

	public Button btnUpgrade;

	public NodeSkill[] nodeOffense;

	public NodeSkill[] nodeDefense;

	public NodeSkill[] nodeUtility;

	private int selectingSkillId = -1;

	private int priceUpgrade;

	private int priceReset;

	private int remainingPoints;

	private void Awake()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ClickNodeSkill, delegate(Component sender, object param)
		{
			this.OnSelectNodeSkill((int)param);
		});
	}

	private void OnEnable()
	{
		this.ResetUI();
		this.CalculatePoints();
		this.LoadSkillTree();
	}

	public void ClickRamboIcon()
	{
		if (this.selectingSkillId == -1)
		{
			return;
		}
		this.ResetUI();
	}

	public void TrainSkill()
	{
		if (this.remainingPoints <= 0)
		{
			SoundManager.Instance.PlaySfxClick();
			Singleton<Popup>.Instance.ShowToastMessage("not enough skill point", ToastLength.Normal);
		}
		else
		{
			this.remainingPoints--;
			this.SetTextSkillPoints();
			this.UpgradeSkillSuccess();
		}
	}

	public void UpgradeSkill()
	{
		if (GameData.playerResources.gem < this.priceUpgrade)
		{
			Singleton<Popup>.Instance.ShowToastMessage("Not enough gems", ToastLength.Normal);
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeGem(this.priceUpgrade);
			this.UpgradeSkillSuccess();
		}
	}

	public void ResetPoints()
	{
		SoundManager.Instance.PlaySfxClick();
		if (GameData.playerResources.gem < this.GetCostResetSkill())
		{
			Singleton<Popup>.Instance.ShowToastMessage("Not enough gems", ToastLength.Normal);
			return;
		}
		int usedSkillPoints = GameData.playerRamboSkills.GetUsedSkillPoints(UpgradeSkillController.ramboId);
		if (usedSkillPoints <= 0)
		{
			Singleton<Popup>.Instance.ShowToastMessage("you did not spend any skill points", ToastLength.Normal);
		}
		else
		{
			Singleton<Popup>.Instance.Show(string.Format("Use <color=yellow>{0} gems</color> to reset skill points?\nGems spent on upgrading are non-refundable", this.GetCostResetSkill()), "reset skill", PopupType.YesNo, new UnityAction(this.ResetPointsSuccess), null);
			EventLogger.LogEvent("N_ResetSkillPoints", new object[]
			{
				usedSkillPoints
			});
		}
	}

	private void ResetPointsSuccess()
	{
		GameData.playerResources.ConsumeGem(this.GetCostResetSkill());
		GameData.playerRamboSkills[UpgradeSkillController.ramboId].Reset();
		this.CalculatePoints();
		this.LoadSkillTree();
		this.ResetUI();
	}

	private void UpgradeSkillSuccess()
	{
		GameData.playerRamboSkills[UpgradeSkillController.ramboId].IncreaseLevel(this.selectingSkillId);
		this.LoadSkillTree();
		this.UpdateInformation();
		this.UpdateResetSkillCost();
		SoundManager.Instance.PlaySfx("sfx_upgrade_success", 0f);
		EventDispatcher.Instance.PostEvent(EventID.UpgradeSkillSuccess, this.selectingSkillId);
		if (GameData.isShowingTutorial && this.selectingSkillId == 6 && GameData.playerRamboSkills[UpgradeSkillController.ramboId].GetSkillLevel(this.selectingSkillId) == 1)
		{
			EventDispatcher.Instance.PostEvent(EventID.SubStepUnlockSkillPhoenixDown);
		}
		StaticRamboSkillData data = GameData.staticRamboSkillData.GetData(this.selectingSkillId);
		if (data != null)
		{
			EventLogger.LogEvent("N_UpgradeSkill", new object[]
			{
				data.skillName
			});
			if (data.type == SkillType.Active && GameData.playerRamboSkills.GetRamboSkillProgress(UpgradeSkillController.ramboId).GetSkillLevel(this.selectingSkillId) == 1)
			{
				EventLogger.LogEvent("N_UnlockActiveSkill", new object[]
				{
					data.catergory.ToString()
				});
			}
		}
	}

	private void ResetUI()
	{
		this.selectingSkillId = -1;
		this.priceUpgrade = 0;
		this.skillIcon.sprite = this.ramboIcon.sprite;
		this.skillIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(120f, 120f);
		this.textGuide.gameObject.SetActive(true);
		this.textSkillName.text = string.Empty;
		this.textSkillDescriptionMain.text = string.Empty;
		this.textSkillDescriptionSub.text = string.Empty;
		this.UpdateResetSkillCost();
		this.btnTrain.gameObject.SetActive(false);
		this.btnUpgrade.gameObject.SetActive(false);
		EventDispatcher.Instance.PostEvent(EventID.ResetUISkillTree);
	}

	private void CalculatePoints()
	{
		int ramboLevel = GameData.playerRambos.GetRamboLevel(UpgradeSkillController.ramboId);
		int usedSkillPoints = GameData.playerRamboSkills.GetUsedSkillPoints(UpgradeSkillController.ramboId);
		this.remainingPoints = Mathf.Clamp(ramboLevel - 1 - usedSkillPoints, 0, ramboLevel - 1);
		this.SetTextSkillPoints();
	}

	private void SetTextSkillPoints()
	{
		this.textSkillPoint.text = string.Format("POINTS:  <color=yellow>{0}</color>", this.remainingPoints);
	}

	private void LoadSkillTree()
	{
		PlayerRamboSkillData ramboSkillProgress = GameData.playerRamboSkills.GetRamboSkillProgress(UpgradeSkillController.ramboId);
		this.LoadSkillOffense(ramboSkillProgress);
		this.LoadSkillDefense(ramboSkillProgress);
		this.LoadSkillUtility(ramboSkillProgress);
	}

	private void LoadSkillOffense(PlayerRamboSkillData progress)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < GameData.staticRamboSkillData.Count; i++)
		{
			StaticRamboSkillData staticRamboSkillData = GameData.staticRamboSkillData[i];
			if (staticRamboSkillData.ramboId == UpgradeSkillController.ramboId && staticRamboSkillData.catergory == SkillCatergory.Offense)
			{
				list.Add(staticRamboSkillData.id);
			}
		}
		list.Sort();
		for (int j = 0; j < this.nodeOffense.Length; j++)
		{
			int num = list[j];
			int level = progress[num];
			this.nodeOffense[j].Load(num, level);
		}
	}

	private void LoadSkillDefense(PlayerRamboSkillData progress)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < GameData.staticRamboSkillData.Count; i++)
		{
			StaticRamboSkillData staticRamboSkillData = GameData.staticRamboSkillData[i];
			if (staticRamboSkillData.ramboId == UpgradeSkillController.ramboId && staticRamboSkillData.catergory == SkillCatergory.Defense)
			{
				list.Add(staticRamboSkillData.id);
			}
		}
		list.Sort();
		for (int j = 0; j < this.nodeDefense.Length; j++)
		{
			int num = list[j];
			int level = progress[num];
			this.nodeDefense[j].Load(num, level);
		}
	}

	private void LoadSkillUtility(PlayerRamboSkillData progress)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < GameData.staticRamboSkillData.Count; i++)
		{
			StaticRamboSkillData staticRamboSkillData = GameData.staticRamboSkillData[i];
			if (staticRamboSkillData.ramboId == UpgradeSkillController.ramboId && staticRamboSkillData.catergory == SkillCatergory.Utility)
			{
				list.Add(staticRamboSkillData.id);
			}
		}
		list.Sort();
		for (int j = 0; j < this.nodeUtility.Length; j++)
		{
			int num = list[j];
			int level = progress[num];
			this.nodeUtility[j].Load(num, level);
		}
	}

	private void UpdateResetSkillCost()
	{
		int costResetSkill = this.GetCostResetSkill();
		this.textPriceResetSkill.text = costResetSkill.ToString("n0");
		this.textPriceResetSkill.color = ((GameData.playerResources.gem < costResetSkill) ? StaticValue.colorNotEnoughMoney : Color.white);
	}

	private void UpdateInformation()
	{
		StaticRamboSkillData data = GameData.staticRamboSkillData.GetData(this.selectingSkillId);
		if (data != null)
		{
			PlayerRamboSkillData ramboSkillProgress = GameData.playerRamboSkills.GetRamboSkillProgress(UpgradeSkillController.ramboId);
			int skillLevel = ramboSkillProgress.GetSkillLevel(this.selectingSkillId);
			bool flag = skillLevel >= data.maxLevel;
			this.textSkillName.text = data.skillName.ToUpper();
			this.skillIcon.sprite = GameResourcesUtils.GetSkillUnlockImage(this.selectingSkillId);
			this.skillIcon.SetNativeSize();
			string text = data.descriptionSub;
			for (int i = 0; i < data.values.Length; i++)
			{
				float num = data.values[i];
				if (skillLevel == i + 1)
				{
					text += string.Format("<color=green>{0}</color>", num);
				}
				else
				{
					text += num.ToString();
				}
				if (i < data.values.Length - 1)
				{
					text += "/";
				}
			}
			this.textSkillDescriptionSub.text = text;
			if (skillLevel > 0)
			{
				this.textSkillDescriptionMain.text = string.Format(data.descriptionMain, data.values[skillLevel - 1]);
				this.btnTrain.gameObject.SetActive(false);
				this.btnUpgrade.gameObject.SetActive(!flag);
				if (!flag)
				{
					this.priceUpgrade = data.upgradePrice[skillLevel];
					this.textPriceUpgrade.text = this.priceUpgrade.ToString("n0");
					this.textPriceUpgrade.color = ((GameData.playerResources.gem < this.priceUpgrade) ? StaticValue.colorNotEnoughMoney : Color.white);
				}
			}
			else
			{
				this.textSkillDescriptionMain.text = string.Format(data.descriptionMain, data.values[0]);
				bool active = !data.isRequirePreviousSkill || ramboSkillProgress.GetSkillLevel(data.requireSkillId) > 0;
				this.btnTrain.gameObject.SetActive(active);
				this.btnUpgrade.gameObject.SetActive(false);
			}
		}
	}

	private int GetCostResetSkill()
	{
		int usedSkillPoints = GameData.playerRamboSkills.GetUsedSkillPoints(UpgradeSkillController.ramboId);
		return usedSkillPoints * 30;
	}

	private void OnSelectNodeSkill(int id)
	{
		if (id == this.selectingSkillId)
		{
			return;
		}
		this.selectingSkillId = id;
		this.UpdateInformation();
		this.textGuide.gameObject.SetActive(false);
	}
}
