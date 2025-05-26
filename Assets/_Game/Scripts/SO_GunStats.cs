using System;
using UnityEngine;

public class SO_GunStats : ScriptableObject
{
	[SerializeField]
	private bool _hasCartouche = true;

	[SerializeField]
	private float _damage;

	[SerializeField]
	private float _attackTimePerSecond;

	[SerializeField]
	private float _bulletSpeed;

	[SerializeField]
	private int _bulletPerShoot;

	[SerializeField]
	private int _ammo;

	[SerializeField]
	private float _criticalRate;

	[SerializeField]
	private float _criticalDamageBonus;

	public bool HasCartouche
	{
		get
		{
			return this._hasCartouche;
		}
	}

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

	public float BulletSpeed
	{
		get
		{
			return this._bulletSpeed;
		}
	}

	public int BulletPerShoot
	{
		get
		{
			return this._bulletPerShoot;
		}
	}

	public int Ammo
	{
		get
		{
			return this._ammo;
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
