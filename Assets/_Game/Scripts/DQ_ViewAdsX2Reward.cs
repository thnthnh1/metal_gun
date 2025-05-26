using System;
using UnityEngine;

public class DQ_ViewAdsX2Reward : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.ViewAdsx2CoinEndGame, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerDailyQuests.Save();
		});
	}
}
