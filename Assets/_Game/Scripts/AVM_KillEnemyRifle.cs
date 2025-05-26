using System;
using UnityEngine;

public class AVM_KillEnemyRifle : BaseAchievement
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
