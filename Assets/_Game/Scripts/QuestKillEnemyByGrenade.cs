using System;
using UnityEngine;

public class QuestKillEnemyByGrenade : BaseQuest
{
	public int requirement;

	private int enemyKilledByGrenade;

	public override void Init()
	{
		this.keyDescription = "kill_enemy_by_grenade";
		base.Init();
		this.enemyKilledByGrenade = 0;
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByGrenade, delegate(Component sender, object param)
		{
			this.enemyKilledByGrenade++;
		});
	}

	public override bool IsCompleted()
	{
		this.isCompleted = (this.enemyKilledByGrenade >= this.requirement);
		return this.isCompleted;
	}

	public override string GetDescription()
	{
		return string.Format(this.description, this.requirement);
	}

	public override string GetCurrentProgress()
	{
		return string.Format("{0}/{1}", this.enemyKilledByGrenade, this.requirement);
	}
}
