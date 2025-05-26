using System;
using UnityEngine;

public class SO_BossMegatankStats : SO_BaseUnitStats
{
	[SerializeField]
	private float _plasmaDuration;

	[SerializeField]
	private float _rocketDamage;

	[SerializeField]
	private float _rocketRadius;

	[SerializeField]
	private float _goreDamage;

	[SerializeField]
	private float _rageGunDamage;

	[SerializeField]
	private float _rageRocketDamage;

	[SerializeField]
	private float _rageGoreDamage;

	[SerializeField]
	private float _rageAttackTimeSecond;

	[SerializeField]
	private float _rageBulletSpeed;

	public float PlasmaDuration
	{
		get
		{
			return this._plasmaDuration;
		}
	}

	public float RocketDamage
	{
		get
		{
			return this._rocketDamage;
		}
	}

	public float RocketRadius
	{
		get
		{
			return this._rocketRadius;
		}
	}

	public float GoreDamage
	{
		get
		{
			return this._goreDamage;
		}
	}

	public float RageGunDamage
	{
		get
		{
			return this._rageGunDamage;
		}
	}

	public float RageRocketDamage
	{
		get
		{
			return this._rageRocketDamage;
		}
	}

	public float RageGoreDamage
	{
		get
		{
			return this._rageGoreDamage;
		}
	}

	public float RageAttackTimeSecond
	{
		get
		{
			return this._rageAttackTimeSecond;
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
