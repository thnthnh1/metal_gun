using System;
using UnityEngine;

public class AVM_PlayStage : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.GameEnd, delegate(Component sender, object param)
		{
			if (GameData.mode == GameMode.Campaign)
			{
				this.IncreaseProgress();
				this.Save();
			}
		});
	}
}
