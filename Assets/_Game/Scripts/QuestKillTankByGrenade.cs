using System;
using UnityEngine;

public class QuestKillTankByGrenade : BaseQuest
{
	public int numberTankRequirement;

	private int tankKilledByGrenade;

	public override void Init()
	{
		this.keyDescription = "kill_tank_by_grenade";
		base.Init();
		this.tankKilledByGrenade = 0;
		EventDispatcher.Instance.RegisterListener(EventID.KillTankByGrenade, delegate(Component sender, object param)
		{
			this.tankKilledByGrenade++;
		});
	}

	public override bool IsCompleted()
	{
		this.isCompleted = (this.tankKilledByGrenade >= this.numberTankRequirement);
		return this.isCompleted;
	}

	public override string GetDescription()
	{
		return string.Format(this.description, this.numberTankRequirement);
	}

	public override string GetCurrentProgress()
	{
		return string.Format("{0}/{1}", this.tankKilledByGrenade, this.numberTankRequirement);
	}
}
