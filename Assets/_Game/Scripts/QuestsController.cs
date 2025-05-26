using System;
using UnityEngine;

public class QuestsController : MonoBehaviour
{
	public HudDailyQuest dailyQuest;

	public HudAchievement achievement;

	public QuestTab tabAchievement;

	public QuestTab tabDailyQuest;

	private void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ClaimDailyQuestReward, delegate(Component sender, object param)
		{
			this.CalculateNotiDailyQuest();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ClaimAchievementReward, delegate(Component sender, object param)
		{
			this.CalculateNotiAchievement();
		});
	}

	private void OnEnable()
	{
		this.SwitchDailyQuest();
		this.CalculateNotiAchievement();
		this.CalculateNotiDailyQuest();
	}

	public void SwitchAchievement()
	{
		this.tabAchievement.Highlight(true);
		this.achievement.gameObject.SetActive(true);
		this.tabDailyQuest.Highlight(false);
		this.dailyQuest.gameObject.SetActive(false);
		SoundManager.Instance.PlaySfxClick();
		if (GameData.isShowingTutorial)
		{
			EventDispatcher.Instance.PostEvent(EventID.SubStepClickAchievement);
		}
	}

	public void SwitchDailyQuest()
	{
		this.tabAchievement.Highlight(false);
		this.achievement.gameObject.SetActive(false);
		this.tabDailyQuest.Highlight(true);
		this.dailyQuest.gameObject.SetActive(true);
		SoundManager.Instance.PlaySfxClick();
	}

	private void CalculateNotiAchievement()
	{
		this.achievement.CalculateNotification();
	}

	private void CalculateNotiDailyQuest()
	{
		this.dailyQuest.CalculateNotification();
	}
}
