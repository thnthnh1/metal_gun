using System;
using UnityEngine;

public class AVM_KillEnemy : BaseAchievement
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
