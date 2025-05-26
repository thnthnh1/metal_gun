using System;
using UnityEngine;

public class SO_MeleeWeaponStats : ScriptableObject
{
	[SerializeField]
	private float _damage;

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

	public float AttackTimePerSecond
	{
		get
		{
			return this._attackTimePerSecond;
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
