using System;
using UnityEngine;

public class DQ_UseBooster : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBooster, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
