using System;
using UnityEngine;

public class SO_EnemyHelicopterStats : SO_EnemyHasProjectileStats
{
	[SerializeField]
	private float _projectileSpeed;

	[SerializeField]
	private int _numberOfProjectilePerShot;

	public float ProjectileSpeed
	{
		get
		{
			return this._projectileSpeed;
		}
	}

	public int NumberOfProjectilePerShot
	{
		get
		{
			return this._numberOfProjectilePerShot;
		}
	}
}
