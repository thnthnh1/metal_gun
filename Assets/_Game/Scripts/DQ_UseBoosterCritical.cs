using System;
using UnityEngine;

public class DQ_UseBoosterCritical : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterCritical, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
