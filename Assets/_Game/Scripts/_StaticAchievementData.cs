using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class _StaticAchievementData : List<StaticAchievementData>
{
	private static Func<StaticAchievementData, bool> __f__am_cache0;

	private static Func<StaticAchievementData, bool> __f__am_cache1;

	private static Func<StaticAchievementData, AchievementType> __f__am_cache2;

	public StaticAchievementData GetData(AchievementType type)
	{
		for (int i = 0; i < base.Count; i++)
		{
			StaticAchievementData staticAchievementData = base[i];
			if (staticAchievementData.type == type)
			{
				return staticAchievementData;
			}
		}
		return null;
	}

	public AchievementMilestone GetMilestone(AchievementType type, int index)
	{
		int i = 0;
		while (i < base.Count)
		{
			StaticAchievementData staticAchievementData = base[i];
			if (staticAchievementData.type == type)
			{
				if (index < staticAchievementData.milestones.Count)
				{
					return staticAchievementData.milestones[index];
				}
				return null;
			}
			else
			{
				i++;
			}
		}
		return null;
	}

	public void SortByState()
	{
		for (int i = 0; i < base.Count; i++)
		{
			StaticAchievementData staticAchievementData = base[i];
			int index = 0;
			if (GameData.playerAchievements.ContainsKey(staticAchievementData.type))
			{
				index = Mathf.Clamp(GameData.playerAchievements[staticAchievementData.type].claimTimes, 0, staticAchievementData.milestones.Count - 1);
			}
			AchievementMilestone achievementMilestone = staticAchievementData.milestones[index];
			int num = (!GameData.playerAchievements.ContainsKey(staticAchievementData.type)) ? 0 : GameData.playerAchievements[staticAchievementData.type].progress;
			int requirement = achievementMilestone.requirement;
			staticAchievementData.isCompleted = (GameData.playerAchievements.ContainsKey(staticAchievementData.type) && GameData.playerAchievements[staticAchievementData.type].claimTimes >= staticAchievementData.milestones.Count);
			staticAchievementData.isReady = (!staticAchievementData.isCompleted && num >= requirement);
		}
		List<StaticAchievementData> list = (from x in this
		orderby x.isCompleted, x.isReady descending, x.type
		select x).ToList<StaticAchievementData>();
		base.Clear();
		for (int j = 0; j < list.Count; j++)
		{
			base.Add(list[j]);
		}
	}
}
