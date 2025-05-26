using System;
using UnityEngine;

public class AVM_KillEnemyGeneral : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyGeneral, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
