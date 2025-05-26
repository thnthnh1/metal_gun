using System;
using UnityEngine;

public class DQ_KillEnemyRifle : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyRifle, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
