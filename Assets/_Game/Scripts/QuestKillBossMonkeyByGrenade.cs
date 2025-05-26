using System;
using UnityEngine;

public class QuestKillBossMonkeyByGrenade : BaseQuest
{
	public override void Init()
	{
		this.keyDescription = "kill_boss_monkey_by_grenade";
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, delegate(Component sender, object param)
		{
			UnitDieData unitDieData = (UnitDieData)param;
			if (unitDieData != null && GameData.mode == GameMode.Campaign && unitDieData.attackData.weapon == WeaponType.Grenade && unitDieData.unit.id == 1005)
			{
				base.SetComplete(true);
			}
		});
	}
}
