using System;
using UnityEngine;

public class AVM_KillFinalBossCrazyMode : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, delegate(Component sender, object param)
		{
			if (GameData.mode == GameMode.Campaign && GameData.currentStage.difficulty == Difficulty.Crazy)
			{
				this.IncreaseProgress();
			}
		});
	}
}
