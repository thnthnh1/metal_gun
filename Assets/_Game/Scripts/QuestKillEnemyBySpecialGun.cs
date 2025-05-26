using System;
using UnityEngine;

public class QuestKillEnemyBySpecialGun : BaseQuest
{
	public int requirement;

	private int enemyKilledBySpecialGun;

	public override void Init()
	{
		this.keyDescription = "kill_enemy_by_special_gun";
		base.Init();
		this.enemyKilledBySpecialGun = 0;
		EventDispatcher.Instance.RegisterListener(EventID.KillEnemyBySpecialGun, delegate(Component sender, object param)
		{
			this.enemyKilledBySpecialGun++;
		});
	}

	public override bool IsCompleted()
	{
		this.isCompleted = (this.enemyKilledBySpecialGun >= this.requirement);
		return this.isCompleted;
	}

	public override string GetDescription()
	{
		return string.Format(this.description, this.requirement);
	}

	public override string GetCurrentProgress()
	{
		return string.Format("{0}/{1}", this.enemyKilledBySpecialGun, this.requirement);
	}
}
