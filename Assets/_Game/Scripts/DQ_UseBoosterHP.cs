using System;
using UnityEngine;

public class DQ_UseBoosterHP : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterHP, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
