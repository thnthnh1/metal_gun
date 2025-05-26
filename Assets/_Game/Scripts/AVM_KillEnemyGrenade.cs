using System;
using UnityEngine;

public class AVM_KillEnemyGrenade : BaseAchievement
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
