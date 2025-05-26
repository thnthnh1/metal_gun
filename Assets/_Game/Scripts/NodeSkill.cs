using Spine.Unity;
using System;
using UnityEngine;
using UnityEngine.UI;

public class NodeSkill : MonoBehaviour
{
	public Image icon;

	public Text textLevel;

	public GameObject notiCanLearn;

	public GameObject highlight;

	public SkeletonGraphic effectUpgrade;

	private int id;

	private int level;

	private void Awake()
	{
		EventDispatcher.Instance.RegisterListener(EventID.UpgradeSkillSuccess, delegate(Component sender, object param)
		{
			this.OnUpgradeSkillSuccess((int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ResetUISkillTree, delegate(Component sender, object param)
		{
			this.ActiveHighlight(false);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ClickNodeSkill, delegate(Component sender, object param)
		{
			this.ActiveHighlight((int)param == this.id);
		});
		this.effectUpgrade.gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		this.ActiveHighlight(false);
	}

	public void Load(int id, int level)
	{
		this.id = id;
		this.level = level;
		this.icon.sprite = ((level <= 0) ? GameResourcesUtils.GetSkillLockImage(id) : GameResourcesUtils.GetSkillUnlockImage(id));
		this.icon.SetNativeSize();
		this.textLevel.text = level.ToString();
		this.textLevel.transform.parent.gameObject.SetActive(level > 0);
		StaticRamboSkillData data = GameData.staticRamboSkillData.GetData(id);
		if (data != null)
		{
			int requireSkillId = data.requireSkillId;
		}
		if (level > 0)
		{
			this.notiCanLearn.SetActive(false);
		}
		else
		{
			int unusedSkillPoints = GameData.playerRamboSkills.GetUnusedSkillPoints(data.ramboId);
			if (unusedSkillPoints <= 0)
			{
				this.notiCanLearn.SetActive(false);
			}
			else
			{
				PlayerRamboSkillData ramboSkillProgress = GameData.playerRamboSkills.GetRamboSkillProgress(data.ramboId);
				if (!data.isRequirePreviousSkill || ramboSkillProgress.GetSkillLevel(data.requireSkillId) > 0)
				{
					this.notiCanLearn.SetActive(true);
				}
				else
				{
					this.notiCanLearn.SetActive(false);
				}
			}
		}
	}

	public void OnClick()
	{
		EventDispatcher.Instance.PostEvent(EventID.ClickNodeSkill, this.id);
		if (GameData.isShowingTutorial && this.id == 6)
		{
			EventDispatcher.Instance.PostEvent(EventID.SubStepSelectSkillPhoenixDown);
		}
	}

	private void OnUpgradeSkillSuccess(int id)
	{
		if (this.id == id)
		{
			this.effectUpgrade.gameObject.SetActive(true);
			this.effectUpgrade.AnimationState.SetAnimation(0, "animation", false);
		}
	}

	private void ActiveHighlight(bool isActive)
	{
		this.highlight.SetActive(isActive);
	}
}
