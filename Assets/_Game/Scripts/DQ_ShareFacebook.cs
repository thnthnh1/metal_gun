using System;
using UnityEngine;

public class DQ_ShareFacebook : BaseDailyQuest
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.ShareFacebookSuccess, delegate(Component sender, object param)
		{
			this.IncreaseProgress();
			this.Save();
			GameData.playerDailyQuests.Save();
		});
	}
}
