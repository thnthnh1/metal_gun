using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HudAchievement : MonoBehaviour, IEnhancedScrollerDelegate
{
	public EnhancedScroller scroller;

	public EnhancedScrollerCellView cellViewAchievement;

	public Text countNotification;

	private SmallList<CellViewAchievementData> achievementData = new SmallList<CellViewAchievementData>();

	private void Awake()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ClaimAchievementReward, delegate(Component sender, object param)
		{
			this.OnClaimReward((CellViewAchievementData)param);
		});
		this.scroller.CreateContainer();
		this.scroller.Delegate = this;
	}

	private void OnEnable()
	{
		GameData.staticAchievementData.SortByState();
		this.CreateAchievementData();
		this.scroller.ReloadData(0f);
	}

	public void CalculateNotification()
	{
		int numberReadyAchievement = GameData.playerAchievements.GetNumberReadyAchievement();
		if (numberReadyAchievement > 0)
		{
			this.countNotification.transform.parent.gameObject.SetActive(true);
			this.countNotification.text = numberReadyAchievement.ToString();
		}
		else
		{
			this.countNotification.transform.parent.gameObject.SetActive(false);
		}
	}

	private void CreateAchievementData()
	{
		this.achievementData.Clear();
		for (int i = 0; i < GameData.staticAchievementData.Count; i++)
		{
			StaticAchievementData staticAchievementData = GameData.staticAchievementData[i];
			int index = 0;
			if (GameData.playerAchievements.ContainsKey(staticAchievementData.type))
			{
				index = Mathf.Clamp(GameData.playerAchievements[staticAchievementData.type].claimTimes, 0, staticAchievementData.milestones.Count - 1);
			}
			AchievementMilestone achievementMilestone = staticAchievementData.milestones[index];
			CellViewAchievementData cellViewAchievementData = new CellViewAchievementData();
			cellViewAchievementData.type = staticAchievementData.type;
			cellViewAchievementData.title = staticAchievementData.title.ToUpper();
			cellViewAchievementData.description = string.Format(staticAchievementData.description, achievementMilestone.requirement.ToString("n0"));
			cellViewAchievementData.progress = ((!GameData.playerAchievements.ContainsKey(staticAchievementData.type)) ? 0 : GameData.playerAchievements[staticAchievementData.type].progress);
			cellViewAchievementData.target = achievementMilestone.requirement;
			cellViewAchievementData.rewards = achievementMilestone.rewards;
			cellViewAchievementData.isCompleted = staticAchievementData.isCompleted;
			this.achievementData.Add(cellViewAchievementData);
		}
	}

	private void OnClaimReward(CellViewAchievementData data)
	{
		this.scroller.RefreshActiveCellViews();
		RewardUtils.Receive(data.rewards);
		Singleton<Popup>.Instance.ShowToastMessage("Claim reward successfully", ToastLength.Normal);
		SoundManager.Instance.PlaySfx("sfx_get_reward", 0f);
	}

	public int GetNumberOfCells(EnhancedScroller scroller)
	{
		return this.achievementData.Count;
	}

	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		return 98f;
	}

	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		CellViewAchievement cellViewAchievement = scroller.GetCellView(this.cellViewAchievement) as CellViewAchievement;
		cellViewAchievement.name = this.achievementData[dataIndex].type.ToString();
		cellViewAchievement.SetData(this.achievementData[dataIndex]);
		return cellViewAchievement;
	}
}
