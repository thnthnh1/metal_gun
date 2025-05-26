using System;
using UnityEngine;

public class DQ_CompleteStageWith3Stars : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.FinishStageWith3Stars, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
