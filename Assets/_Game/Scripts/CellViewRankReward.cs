using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellViewRankReward : MonoBehaviour
{
	public TournamentRank rankType;

	public Image rankIcon;

	public Text rankName;

	public Text rankPoint;

	public Button btnClaim;

	public GameObject labelAchieved;

	public Image background;

	public Sprite[] bgSprites;

	public RewardElement[] rewardCells;

	private List<RewardData> rewards;

	public void Load()
	{
		StaticTournamentRankData data = GameData.staticTournamentRankData.GetData((int)this.rankType);
		TournamentRank currentRank = GameData.staticTournamentRankData.GetCurrentRank(GameData.playerTournamentData.score);
		this.background.sprite = this.bgSprites[(int)this.rankType];
		this.rankIcon.sprite = GameResourcesUtils.GetTournamentRankImage((int)this.rankType);
		this.rankIcon.SetNativeSize();
		this.rankName.text = this.rankType.ToString().ToUpper();
		this.rankPoint.text = ((data.score <= 0) ? string.Empty : string.Format("Score: {0}", data.score));
		this.rewards = data.rewards;
		for (int i = 0; i < this.rewardCells.Length; i++)
		{
			RewardElement rewardElement = this.rewardCells[i];
			rewardElement.gameObject.SetActive(i < data.rewards.Count);
			if (i < data.rewards.Count)
			{
				RewardData data2 = data.rewards[i];
				rewardElement.SetInformation(data2, false);
			}
		}
		this.btnClaim.gameObject.SetActive(currentRank >= this.rankType);
		switch (this.rankType)
		{
		case TournamentRank.Ducky:
			this.labelAchieved.SetActive(false);
			this.btnClaim.gameObject.SetActive(false);
			break;
		case TournamentRank.Bronze:
			this.labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank1);
			if (ProfileManager.UserProfile.isClaimedRank1)
			{
				this.btnClaim.gameObject.SetActive(false);
			}
			break;
		case TournamentRank.Silver:
			this.labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank2);
			if (ProfileManager.UserProfile.isClaimedRank2)
			{
				this.btnClaim.gameObject.SetActive(false);
			}
			break;
		case TournamentRank.Gold:
			this.labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank3);
			if (ProfileManager.UserProfile.isClaimedRank3)
			{
				this.btnClaim.gameObject.SetActive(false);
			}
			break;
		case TournamentRank.Platinum:
			this.labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank4);
			if (ProfileManager.UserProfile.isClaimedRank4)
			{
				this.btnClaim.gameObject.SetActive(false);
			}
			break;
		case TournamentRank.Diamond:
			this.labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank5);
			if (ProfileManager.UserProfile.isClaimedRank5)
			{
				this.btnClaim.gameObject.SetActive(false);
			}
			break;
		case TournamentRank.Legend:
			this.labelAchieved.SetActive(ProfileManager.UserProfile.isClaimedRank6);
			if (ProfileManager.UserProfile.isClaimedRank6)
			{
				this.btnClaim.gameObject.SetActive(false);
			}
			break;
		}
		if (currentRank == this.rankType)
		{
			this.background.rectTransform.localScale = new Vector3(1.05f, 1f, 1f);
		}
		else
		{
			this.background.rectTransform.localScale = Vector3.one;
		}
	}

	public bool IsAvailableClaim()
	{
		bool result = false;
		TournamentRank currentRank = GameData.staticTournamentRankData.GetCurrentRank(GameData.playerTournamentData.score);
		if (currentRank >= this.rankType)
		{
			switch (this.rankType)
			{
			case TournamentRank.Bronze:
				return !ProfileManager.UserProfile.isClaimedRank1;
			case TournamentRank.Silver:
				return !ProfileManager.UserProfile.isClaimedRank2;
			case TournamentRank.Gold:
				return !ProfileManager.UserProfile.isClaimedRank3;
			case TournamentRank.Platinum:
				return !ProfileManager.UserProfile.isClaimedRank4;
			case TournamentRank.Diamond:
				return !ProfileManager.UserProfile.isClaimedRank5;
			case TournamentRank.Legend:
				return !ProfileManager.UserProfile.isClaimedRank6;
			}
		}
		return result;
	}

	public void Claim()
	{
		if (this.rewards != null)
		{
			RewardUtils.Receive(this.rewards);
			Singleton<Popup>.Instance.ShowReward(this.rewards, null, null);
		}
		this.btnClaim.gameObject.SetActive(false);
		this.labelAchieved.gameObject.SetActive(true);
		switch (this.rankType)
		{
		case TournamentRank.Bronze:
			ProfileManager.UserProfile.isClaimedRank1.Set(true);
			break;
		case TournamentRank.Silver:
			ProfileManager.UserProfile.isClaimedRank2.Set(true);
			break;
		case TournamentRank.Gold:
			ProfileManager.UserProfile.isClaimedRank3.Set(true);
			break;
		case TournamentRank.Platinum:
			ProfileManager.UserProfile.isClaimedRank4.Set(true);
			break;
		case TournamentRank.Diamond:
			ProfileManager.UserProfile.isClaimedRank5.Set(true);
			break;
		case TournamentRank.Legend:
			ProfileManager.UserProfile.isClaimedRank6.Set(true);
			break;
		}
		EventDispatcher.Instance.PostEvent(EventID.ClaimTournamentRankReward, this.rankType);
		EventLogger.LogEvent("N_ClaimRankReward", new object[]
		{
			this.rankType.ToString()
		});
	}
}
