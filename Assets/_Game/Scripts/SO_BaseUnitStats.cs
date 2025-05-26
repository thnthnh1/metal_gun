using System;
using UnityEngine;

public class SO_BaseUnitStats : ScriptableObject
{
	[SerializeField]
	private float _damage;

	[SerializeField]
	private float _bulletSpeed;

	[SerializeField]
	private float _hp;

	[SerializeField]
	private float _moveSpeed;

	[SerializeField]
	private float _attackTimePerSecond;

	[SerializeField]
	private float _criticalRate;

	[SerializeField]
	private float _criticalDamageBonus;

	public float Damage
	{
		get
		{
			return this._damage;
		}
	}

	public float HP
	{
		get
		{
			return this._hp;
		}
	}

	public float MoveSpeed
	{
		get
		{
			return this._moveSpeed;
		}
	}

	public float AttackTimePerSecond
	{
		get
		{
			return this._attackTimePerSecond;
		}
	}

	public float BulletSpeed
	{
		get
		{
			return this._bulletSpeed;
		}
	}

	public float CriticalRate
	{
		get
		{
			return this._criticalRate;
		}
	}

	public float CriticalDamageBonus
	{
		get
		{
			return this._criticalDamageBonus;
		}
	}
}
