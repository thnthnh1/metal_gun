using System;
using UnityEngine;

public abstract class BaseWeapon : BaseEquipment
{
	public AudioClip[] attackSounds;

	protected float damage;

	protected float attackTimePerSecond;

	protected float criticalRate;

	protected float criticalDamageBonus;

	public override void Init(int level)
	{
		this.level = level;
		if (!this.isLoadedScriptableObject)
		{
			this.isLoadedScriptableObject = true;
			this.LoadScriptableObject();
		}
	}

	public override void ApplyOptions(BaseUnit unit)
	{
		this.Init(this.level);
		unit.stats.AdjustStats(this.damage, StatsType.Damage);
		unit.stats.AdjustStats(this.attackTimePerSecond, StatsType.AttackTimePerSecond);
		unit.stats.AdjustStats(this.criticalRate, StatsType.CriticalRate);
		unit.stats.AdjustStats(this.criticalDamageBonus, StatsType.CriticalDamageBonus);
	}

	public abstract void Attack(AttackData attackData);

	public virtual void PlaySoundAttack()
	{
		if (this.attackSounds.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, this.attackSounds.Length);
			SoundManager.Instance.PlaySfx(this.attackSounds[num], 0f);
		}
	}
}
