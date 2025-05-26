using System;
using UnityEngine;

public class DQ_UseBoosterDamage : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterDamage, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
