using System;
using UnityEngine;

public class AVM_UseBoosterCoinMagnet : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterCoinMagnet, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
