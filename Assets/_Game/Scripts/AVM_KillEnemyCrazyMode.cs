using System;
using UnityEngine;

public class AVM_KillEnemyCrazyMode : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, delegate(Component sender, object param)
		{
			if (GameData.mode == GameMode.Campaign && GameData.currentStage.difficulty == Difficulty.Crazy)
			{
				this.IncreaseProgress();
			}
		});
	}
}
