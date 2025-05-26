using System;
using UnityEngine;

public class QuestKillFinalBoss : BaseQuest
{
	public override void Init()
	{
		this.keyDescription = "kill_final_boss";
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, delegate(Component sender, object param)
		{
			base.SetComplete(true);
		});
	}
}
