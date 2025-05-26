using System;
using UnityEngine;

public class AVM_WinStageWithoutReviving : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.CompleteStageWithoutReviving, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
		});
	}
}
