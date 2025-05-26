using System;
using UnityEngine;

public class DQ_PlayTournament : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.QuitSurvivalSession, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerDailyQuests.Save();
		});
		EventDispatcher.Instance.RegisterListener(EventID.CompleteSurvivalSession, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerDailyQuests.Save();
		});
	}
}
