using System;
using UnityEngine;

public class DQ_BuyBoosterCritical : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, delegate(Component sender, object param)
		{
			if ((BoosterType)param == BoosterType.Critical)
			{
				this.IncreaseProgress();
				this.Save();
				GameData.playerDailyQuests.Save();
			}
		});
	}
}
