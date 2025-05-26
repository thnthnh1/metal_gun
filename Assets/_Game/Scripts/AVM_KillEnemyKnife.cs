using System;
using UnityEngine;

public class AVM_KillEnemyKnife : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyKnife, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
