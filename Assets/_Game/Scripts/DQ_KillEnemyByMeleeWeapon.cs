using System;
using UnityEngine;

public class DQ_KillEnemyByMeleeWeapon : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByKnife, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
