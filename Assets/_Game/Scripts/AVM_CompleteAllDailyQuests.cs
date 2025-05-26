using System;
using UnityEngine;

public class AVM_CompleteAllDailyQuests : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.CompleteAllDailyQuests, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerAchievements.Save();
		});
	}
}
