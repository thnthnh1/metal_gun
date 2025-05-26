using System;
using UnityEngine;

public class DQ_BuyAmmo : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.BuyAmmo, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerDailyQuests.Save();
		});
	}
}
