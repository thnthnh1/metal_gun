using System;
using UnityEngine;

[Serializable]
public class UnitStats
{
	[SerializeField]
	private float damage;

	[SerializeField]
	private float hp;

	[SerializeField]
	private float maxHp;

	[SerializeField]
	private float moveSpeed;

	[SerializeField]
	private float attackTimePerSecond;

	[SerializeField]
	private float criticalRate;

	[SerializeField]
	private float criticalDamageBonus;

	private SO_BaseUnitStats baseStats;

	public float Damage
	{
		get
		{
			return this.damage;
		}
	}

	public float HP
	{
		get
		{
			return this.hp;
		}
	}

	public float MaxHp
	{
		get
		{
			return this.maxHp;
		}
	}

	public float MoveSpeed
	{
		get
		{
			return this.moveSpeed;
		}
	}

	public float AttackTimePerSecond
	{
		get
		{
			return this.attackTimePerSecond;
		}
	}

	public float AttackRate
	{
		get
		{
			return 1f / this.attackTimePerSecond;
		}
	}

	public float CriticalRate
	{
		get
		{
			return this.criticalRate;
		}
	}

	public float CriticalDamageBonus
	{
		get
		{
			return this.criticalDamageBonus;
		}
	}

	public void Init(SO_BaseUnitStats baseStats)
	{
		this.baseStats = baseStats;
		this.ResetToBaseStats();
		this.hp = this.maxHp;
	}

	public void ResetToBaseStats()
	{
		this.maxHp = this.baseStats.HP;
		this.damage = this.baseStats.Damage;
		this.attackTimePerSecond = this.baseStats.AttackTimePerSecond;
		this.criticalRate = this.baseStats.CriticalRate;
		this.criticalDamageBonus = this.baseStats.CriticalDamageBonus;
		this.moveSpeed = this.baseStats.MoveSpeed;
	}

	public void AdjustStats(float value, StatsType type)
	{
		switch (type)
		{
		case StatsType.Damage:
			this.damage += value;
			if (this.damage <= 0f)
			{
				this.damage = 0f;
			}
			break;
		case StatsType.Hp:
			this.hp += value;
			if (this.hp <= 0f)
			{
				this.hp = 0f;
			}
			break;
		case StatsType.MaxHp:
			this.maxHp += value;
			if (this.maxHp <= 0f)
			{
				this.maxHp = 0f;
			}
			break;
		case StatsType.MoveSpeed:
			this.moveSpeed += value;
			if (this.moveSpeed <= 0.2f)
			{
				this.moveSpeed = 0.2f;
			}
			break;
		case StatsType.AttackTimePerSecond:
			this.attackTimePerSecond += value;
			if (this.attackTimePerSecond <= 0f)
			{
				this.attackTimePerSecond = 0f;
			}
			break;
		case StatsType.CriticalRate:
			this.criticalRate += value;
			if (this.criticalRate <= 0f)
			{
				this.criticalRate = 0f;
			}
			break;
		case StatsType.CriticalDamageBonus:
			this.criticalDamageBonus += value;
			if (this.criticalDamageBonus <= 0f)
			{
				this.criticalDamageBonus = 0f;
			}
			break;
		}
	}

	public void SetStats(float value, StatsType type)
	{
		switch (type)
		{
		case StatsType.Damage:
			this.damage = value;
			break;
		case StatsType.Hp:
			this.hp = value;
			break;
		case StatsType.MoveSpeed:
			this.moveSpeed = value;
			break;
		case StatsType.AttackTimePerSecond:
			this.attackTimePerSecond = value;
			break;
		case StatsType.CriticalRate:
			this.moveSpeed = value;
			break;
		case StatsType.CriticalDamageBonus:
			this.moveSpeed = value;
			break;
		}
	}
}
