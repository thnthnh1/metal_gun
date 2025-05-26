using System;

public class AVM_GetRankLevel : BaseAchievement
{
	public override void Init()
	{
		base.Init();
	}

	public override void SetProgressToDefault()
	{
		int level = GameData.playerProfile.level;
		if (GameData.playerAchievements.ContainsKey(this.type))
		{
			GameData.playerAchievements[this.type].progress = level;
		}
		else
		{
			GameData.playerAchievements.Add(this.type, new PlayerAchievementData(this.type, level, 0, false));
		}
		this.progress = GameData.playerAchievements[this.type].progress;
	}
}
