using System;
using UnityEngine;

public class AVM_GetFreeCoin : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.ViewAdsGetFreeCoin, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerAchievements.Save();
		});
	}
}
