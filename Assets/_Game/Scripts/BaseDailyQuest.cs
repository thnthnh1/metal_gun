using System;
using UnityEngine;

public class BaseDailyQuest : MonoBehaviour
{
	public DailyQuestType type;

	public int progress;

	public virtual void Init()
	{
	}

	public virtual void SetProgressToDefault()
	{
		for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
		{
			PlayerDailyQuestData playerDailyQuestData = GameData.playerDailyQuests[i];
			if (playerDailyQuestData.type == this.type)
			{
				this.progress = playerDailyQuestData.progress;
				break;
			}
		}
	}

	public virtual void Save()
	{
		for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
		{
			PlayerDailyQuestData playerDailyQuestData = GameData.playerDailyQuests[i];
			if (playerDailyQuestData.type == this.type)
			{
				playerDailyQuestData.progress = this.progress;
				break;
			}
		}
	}

	public virtual bool IsAlreadyClaimed()
	{
		return GameData.playerDailyQuests.IsAlreadyClaimed(this.type);
	}

	protected virtual void IncreaseProgress()
	{
		this.progress++;
	}
}
