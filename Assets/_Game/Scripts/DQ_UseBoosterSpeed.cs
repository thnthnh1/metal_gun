using System;
using UnityEngine;

public class DQ_UseBoosterSpeed : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterSpeed, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
