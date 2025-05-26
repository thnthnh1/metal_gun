using System;
using UnityEngine;

public class DQ_KillEnemyGrenade : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyGrenade, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
