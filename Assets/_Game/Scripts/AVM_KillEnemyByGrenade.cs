using System;
using UnityEngine;

public class AVM_KillEnemyByGrenade : BaseAchievement
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
