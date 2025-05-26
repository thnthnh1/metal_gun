using System;

public class AVM_OwnNormalGun : BaseAchievement
{
	public override void Init()
	{
		base.Init();
	}

	public override void SetProgressToDefault()
	{
		int numberOfNormalGun = GameData.playerGuns.GetNumberOfNormalGun();
		if (GameData.playerAchievements.ContainsKey(this.type))
		{
			GameData.playerAchievements[this.type].progress = numberOfNormalGun;
		}
		else
		{
			GameData.playerAchievements.Add(this.type, new PlayerAchievementData(this.type, numberOfNormalGun, 0, false));
		}
		this.progress = GameData.playerAchievements[this.type].progress;
	}
}
