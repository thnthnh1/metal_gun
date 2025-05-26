using System;
using UnityEngine;

public class AVM_UseGrenadeKillEnemyAtOnce : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.GrenadeKillEnemyAtOnce, delegate(Component sender, object param)
		{
			if ((int)param >= 3)
			{
				this.IncreaseProgress();
			}
		});
	}
}
