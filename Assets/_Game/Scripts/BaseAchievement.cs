using System;
using UnityEngine;

public class BaseAchievement : MonoBehaviour
{
	public AchievementType type;

	public int progress;

	public virtual void Init()
	{
	}

	public virtual void SetProgressToDefault()
	{
		if (GameData.playerAchievements.ContainsKey(this.type))
		{
			this.progress = GameData.playerAchievements[this.type].progress;
		}
		else
		{
			this.progress = 0;
		}
	}

	public virtual void Save()
	{
		if (GameData.playerAchievements.ContainsKey(this.type))
		{
			GameData.playerAchievements[this.type].progress = this.progress;
		}
		else
		{
			GameData.playerAchievements.Add(this.type, new PlayerAchievementData(this.type, this.progress, 0, false));
		}
	}

	public virtual bool IsAlreadyCompleted()
	{
		return GameData.playerAchievements.IsAlreadyCompleted(this.type);
	}

	protected virtual void IncreaseProgress()
	{
		this.progress++;
	}
}
