using System;
using UnityEngine;

public class SO_BossProfessorStats : SO_BaseUnitStats
{
	[SerializeField]
	private float _satelliteHp;

	[SerializeField]
	private float _rotateSpeed;

	[SerializeField]
	private float _shootDuration;

	[SerializeField]
	private float _energyPulseDamage;

	[SerializeField]
	private int _numberEnemySpawn;

	[SerializeField]
	private int _enemyMinLevel;

	[SerializeField]
	private int _enemyMaxLevel;

	public float SatelliteHp
	{
		get
		{
			return this._satelliteHp;
		}
	}

	public float RotateSpeed
	{
		get
		{
			return this._rotateSpeed;
		}
	}

	public float ShootDuration
	{
		get
		{
			return this._shootDuration;
		}
	}

	public float EnergyPulseDamage
	{
		get
		{
			return this._energyPulseDamage;
		}
	}

	public float NumberEnemySpawn
	{
		get
		{
			return (float)this._numberEnemySpawn;
		}
	}

	public int EnemyMinLevel
	{
		get
		{
			return this._enemyMinLevel;
		}
	}

	public int EnemyMaxLevel
	{
		get
		{
			return this._enemyMaxLevel;
		}
	}
}
