using System;
using UnityEngine;

public class DQ_BuyBoosterSpeed : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, delegate(Component sender, object param)
		{
			if ((BoosterType)param == BoosterType.Speed)
			{
				this.IncreaseProgress();
				this.Save();
				GameData.playerDailyQuests.Save();
			}
		});
	}
}
