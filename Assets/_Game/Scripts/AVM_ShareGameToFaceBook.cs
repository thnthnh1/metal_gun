using System;
using UnityEngine;

public class AVM_ShareGameToFaceBook : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.ShareFacebookSuccess, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerAchievements.Save();
		});
	}
}
