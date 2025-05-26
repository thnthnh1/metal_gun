using System;
using UnityEngine;

public class SO_BossVenomStats : SO_BaseUnitStats
{
	[SerializeField]
	private int _shootTimes;

	[SerializeField]
	private float _delayShootTime;

	[SerializeField]
	private float _laserDamage;

	[SerializeField]
	private float _rageLaserDamage;

	[SerializeField]
	private int _rageShootTimes;

	[SerializeField]
	private float _rageDamage;

	[SerializeField]
	private float _rageBulletSpeed;

	public int ShootTimes
	{
		get
		{
			return this._shootTimes;
		}
	}

	public float DelayShootTime
	{
		get
		{
			return this._delayShootTime;
		}
	}

	public float LaserDamage
	{
		get
		{
			return this._laserDamage;
		}
	}

	public float RageLaserDamage
	{
		get
		{
			return this._rageLaserDamage;
		}
	}

	public int RageShootTimes
	{
		get
		{
			return this._rageShootTimes;
		}
	}

	public float RageDamage
	{
		get
		{
			return this._rageDamage;
		}
	}

	public float RageBulletSpeed
	{
		get
		{
			return this._rageBulletSpeed;
		}
	}
}
