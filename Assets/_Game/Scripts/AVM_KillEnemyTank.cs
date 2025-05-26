using System;
using UnityEngine;

public class AVM_KillEnemyTank : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyTank, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
