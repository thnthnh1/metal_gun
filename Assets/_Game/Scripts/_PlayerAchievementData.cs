using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerAchievementData : Dictionary<AchievementType, PlayerAchievementData>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerAchievementData.Set(value);
		ProfileManager.SaveAll();
	}

	public int GetNumberReadyAchievement()
	{
		int num = 0;
		foreach (PlayerAchievementData current in base.Values)
		{
			StaticAchievementData data = GameData.staticAchievementData.GetData(current.type);
			if (current.claimTimes < data.milestones.Count)
			{
				AchievementMilestone achievementMilestone = data.milestones[current.claimTimes];
				if (current.progress >= achievementMilestone.requirement)
				{
					num++;
				}
			}
		}
		return num;
	}

	public bool IsAlreadyCompleted(AchievementType type)
	{
		StaticAchievementData data = GameData.staticAchievementData.GetData(type);
		return base.ContainsKey(type) && base[type].claimTimes >= data.milestones.Count;
	}
}
