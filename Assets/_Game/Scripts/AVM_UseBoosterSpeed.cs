using System;
using UnityEngine;

public class AVM_UseBoosterSpeed : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterSpeed, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
