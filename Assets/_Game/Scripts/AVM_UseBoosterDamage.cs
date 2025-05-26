using System;
using UnityEngine;

public class AVM_UseBoosterDamage : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterDamage, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
