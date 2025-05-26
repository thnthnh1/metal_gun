using System;
using UnityEngine;

public class SO_BossSubmarineStats : SO_BaseUnitStats
{
	[SerializeField]
	private int _numberOfBullet;

	[SerializeField]
	private int _numberOfRocket;

	[SerializeField]
	private float _timeDelayRocket;

	[SerializeField]
	private float _rocketSpeed;

	[SerializeField]
	private float _rocketDamage;

	[SerializeField]
	private int _numberOfMarine;

	[SerializeField]
	private float _timeDelaySpawnMarine;

	[SerializeField]
	private int _marineLevel;

	[SerializeField]
	private float _goreDamage;

	[SerializeField]
	private int _rageNumberOfRocket;

	[SerializeField]
	private float _rageRocketDamage;

	[SerializeField]
	private float _rageRocketSpeed;

	[SerializeField]
	private int _rageNumberOfMarine;

	[SerializeField]
	private int _rageMarineLevel;

	[SerializeField]
	private float _rageGoreDamage;

	public int NumberOfBullet
	{
		get
		{
			return this._numberOfBullet;
		}
	}

	public int NumberOfRocket
	{
		get
		{
			return this._numberOfRocket;
		}
	}

	public float TimeDelayRocket
	{
		get
		{
			return this._timeDelayRocket;
		}
	}

	public float RocketSpeed
	{
		get
		{
			return this._rocketSpeed;
		}
	}

	public float RocketDamage
	{
		get
		{
			return this._rocketDamage;
		}
	}

	public int NumberOfMarine
	{
		get
		{
			return this._numberOfMarine;
		}
	}

	public float TimeDelaySpawnMarine
	{
		get
		{
			return this._timeDelaySpawnMarine;
		}
	}

	public int MarineLevel
	{
		get
		{
			return this._marineLevel;
		}
	}

	public float GoreDamage
	{
		get
		{
			return this._goreDamage;
		}
	}

	public int RageNumberOfRocket
	{
		get
		{
			return this._rageNumberOfRocket;
		}
	}

	public float RageRocketDamage
	{
		get
		{
			return this._rageRocketDamage;
		}
	}

	public float RageRocketSpeed
	{
		get
		{
			return this._rageRocketSpeed;
		}
	}

	public int RageNumberOfMarine
	{
		get
		{
			return this._rageNumberOfMarine;
		}
	}

	public int RageMarineLevel
	{
		get
		{
			return this._rageMarineLevel;
		}
	}

	public float RageGoreDamage
	{
		get
		{
			return this._rageGoreDamage;
		}
	}
}
