using System;
using UnityEngine;

public class DQ_GetCriticalHit : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.GetCriticalHit, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
