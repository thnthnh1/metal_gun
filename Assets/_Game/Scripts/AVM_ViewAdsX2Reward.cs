using System;
using UnityEngine;

public class AVM_ViewAdsX2Reward : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.ViewAdsx2CoinEndGame, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerAchievements.Save();
		});
	}
}
