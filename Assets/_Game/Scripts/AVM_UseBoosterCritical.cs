using System;
using UnityEngine;

public class AVM_UseBoosterCritical : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterCritical, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
