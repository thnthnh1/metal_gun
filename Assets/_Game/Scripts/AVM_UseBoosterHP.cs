using System;
using UnityEngine;

public class AVM_UseBoosterHP : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterHP, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
