using System;
using UnityEngine;

public class DQ_KillFinalBoss : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, delegate(Component sender, object param)
		{
			if (GameData.mode == GameMode.Campaign)
			{
				this.IncreaseProgress();
			}
		});
	}
}
