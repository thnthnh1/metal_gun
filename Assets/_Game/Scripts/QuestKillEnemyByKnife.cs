using System;
using UnityEngine;

public class QuestKillEnemyByKnife : BaseQuest
{
	public int requirement;

	private int enemyKilledByKnife;

	public override void Init()
	{
		this.keyDescription = "kill_enemy_by_knife";
		base.Init();
		this.enemyKilledByKnife = 0;
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyByKnife, delegate(Component sender, object param)
		{
			this.enemyKilledByKnife++;
		});
	}

	public override bool IsCompleted()
	{
		this.isCompleted = (this.enemyKilledByKnife >= this.requirement);
		return this.isCompleted;
	}

	public override string GetDescription()
	{
		return string.Format(this.description, this.requirement);
	}

	public override string GetCurrentProgress()
	{
		return string.Format("{0}/{1}", this.enemyKilledByKnife, this.requirement);
	}
}
