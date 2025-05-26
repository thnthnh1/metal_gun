using System;
using UnityEngine;

public class DQ_BuyGrenade : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.BuyGrenade, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerDailyQuests.Save();
		});
	}
}
