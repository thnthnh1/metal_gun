using System;
using UnityEngine;

public class AVM_KillEnemyFire : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyFire, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
