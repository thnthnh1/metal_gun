using System;
using UnityEngine;

public class DQ_UseBoosterCoinMagnet : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterCoinMagnet, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
