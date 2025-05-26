using System;
using UnityEngine;

public class AVM_KillEnemySniper : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemySniper, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
