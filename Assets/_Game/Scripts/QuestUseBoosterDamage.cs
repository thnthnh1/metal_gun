using System;
using UnityEngine;

public class QuestUseBoosterDamage : BaseQuest
{
	private int useTimes;

	public override void Init()
	{
		this.keyDescription = "kill_enemy_by_knife";
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterDamage, delegate(Component sender, object param)
		{
			base.SetComplete(true);
		});
	}
}
