using System;
using UnityEngine;

public class AVM_WinStage : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.GameEnd, delegate(Component sender, object param)
		{
			if ((bool)param && GameData.mode == GameMode.Campaign)
			{
				this.IncreaseProgress();
				this.Save();
			}
		});
	}
}
