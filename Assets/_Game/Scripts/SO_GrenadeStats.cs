using System;
using UnityEngine;

public class SO_GrenadeStats : ScriptableObject
{
	[SerializeField]
	private float _damage;

	[SerializeField]
	private float _radius;

	[SerializeField]
	private float _cooldown;

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

	public float Radius
	{
		get
		{
			return this._radius;
		}
	}

	public float Cooldown
	{
		get
		{
			return this._cooldown;
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
