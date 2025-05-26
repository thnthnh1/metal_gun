using EnhancedUI.EnhancedScroller;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CellViewAchievement : EnhancedScrollerCellView
{
	public Text textTitle;

	public Text textDescription;

	public Text textProgress;

	public Text textTarget;

	public Image imageProgress;

	public Button btnClaim;

	public GameObject labelAchieved;

	public RewardElement[] rewardCells;

	private CellViewAchievementData _data;

	public void SetData(CellViewAchievementData data)
	{
		this._data = data;
		this.UpdateInformation();
	}

	private void UpdateInformation()
	{
		this.textTitle.text = this._data.title.ToUpper();
		this.textDescription.text = this._data.description;
		for (int i = 0; i < this.rewardCells.Length; i++)
		{
			RewardElement rewardElement = this.rewardCells[i];
			rewardElement.gameObject.SetActive(i < this._data.rewards.Count);
			if (i < this._data.rewards.Count)
			{
				RewardData data = this._data.rewards[i];
				rewardElement.SetInformation(data, false);
			}
		}
		this.labelAchieved.SetActive(this._data.isCompleted);
		if (this._data.isCompleted)
		{
			this.btnClaim.gameObject.SetActive(false);
			this.imageProgress.transform.parent.gameObject.SetActive(false);
		}
		else if (this._data.progress >= this._data.target)
		{
			this.btnClaim.gameObject.SetActive(true);
			this.imageProgress.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			this.btnClaim.gameObject.SetActive(false);
			this.imageProgress.transform.parent.gameObject.SetActive(true);
			this.imageProgress.fillAmount = Mathf.Clamp01((float)this._data.progress / (float)this._data.target);
			this.textProgress.text = this._data.progress.ToString("n0");
			this.textTarget.text = this._data.target.ToString("n0");
		}
	}

	public override void RefreshCellView()
	{
		base.RefreshCellView();
		this.UpdateInformation();
	}

	public void ClaimReward()
	{
		StaticAchievementData data = GameData.staticAchievementData.GetData(this._data.type);
		int num = (!GameData.playerAchievements.ContainsKey(this._data.type)) ? 0 : GameData.playerAchievements[this._data.type].claimTimes;
		if (num >= data.milestones.Count - 1)
		{
			this._data.isCompleted = true;
		}
		else
		{
			num++;
			AchievementMilestone achievementMilestone = data.milestones[num];
			this._data.description = string.Format(data.description, achievementMilestone.requirement.ToString("n0"));
			this._data.progress = ((!GameData.playerAchievements.ContainsKey(data.type)) ? 0 : GameData.playerAchievements[data.type].progress);
			this._data.target = achievementMilestone.requirement;
			this._data.rewards = achievementMilestone.rewards;
			this._data.isCompleted = false;
		}
		if (GameData.playerAchievements.ContainsKey(this._data.type))
		{
			GameData.playerAchievements[this._data.type].claimTimes++;
			GameData.playerAchievements.Save();
		}
		EventDispatcher.Instance.PostEvent(EventID.ClaimAchievementReward, this._data);
	}
}
