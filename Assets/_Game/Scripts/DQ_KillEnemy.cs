using System;
using UnityEngine;

public class DQ_KillEnemy : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
