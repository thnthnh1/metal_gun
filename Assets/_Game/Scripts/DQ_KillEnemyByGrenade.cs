using System;
using UnityEngine;

public class DQ_KillEnemyByGrenade : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByGrenade, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
