using System;
using UnityEngine;

public class DQ_CompleteAllQuests : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.CompleteAllDailyQuests, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
