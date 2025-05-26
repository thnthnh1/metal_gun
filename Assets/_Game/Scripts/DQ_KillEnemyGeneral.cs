using System;
using UnityEngine;

public class DQ_KillEnemyGeneral : BaseDailyQuest
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
