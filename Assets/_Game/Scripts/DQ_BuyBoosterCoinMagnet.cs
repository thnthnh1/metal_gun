using System;
using UnityEngine;

public class DQ_BuyBoosterCoinMagnet : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.BuyBooster, delegate(Component sender, object param)
		{
			if ((BoosterType)param == BoosterType.CoinMagnet)
			{
				this.IncreaseProgress();
				this.Save();
				GameData.playerDailyQuests.Save();
			}
		});
	}
}
