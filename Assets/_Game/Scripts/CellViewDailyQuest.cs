using EnhancedUI.EnhancedScroller;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CellViewDailyQuest : EnhancedScrollerCellView
{
	public Text textTitle;

	public Text textDescription;

	public Text textProgress;

	public Text textTarget;

	public Image imageProgress;

	public Button btnClaim;

	public GameObject labelAchieved;

	public RewardElement[] rewardCells;

	private CellViewDailyQuestData _data;

	public void SetData(CellViewDailyQuestData data)
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
		this.labelAchieved.SetActive(this._data.isClaimed);
		if (this._data.isClaimed)
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
		this._data.isClaimed = true;
		for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
		{
			PlayerDailyQuestData playerDailyQuestData = GameData.playerDailyQuests[i];
			if (playerDailyQuestData.type == this._data.type)
			{
				playerDailyQuestData.isClaimed = true;
				break;
			}
		}
		GameData.playerDailyQuests.Save();
		EventDispatcher.Instance.PostEvent(EventID.ClaimDailyQuestReward, this._data);
	}
}
