using System;

public class AVM_OwnSpecialGun : BaseAchievement
{
	public override void Init()
	{
		base.Init();
	}

	public override void SetProgressToDefault()
	{
		int numberOfSpecialGun = GameData.playerGuns.GetNumberOfSpecialGun();
		if (GameData.playerAchievements.ContainsKey(this.type))
		{
			GameData.playerAchievements[this.type].progress = numberOfSpecialGun;
		}
		else
		{
			GameData.playerAchievements.Add(this.type, new PlayerAchievementData(this.type, numberOfSpecialGun, 0, false));
		}
		this.progress = GameData.playerAchievements[this.type].progress;
	}
}
