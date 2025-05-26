using System;
using UnityEngine;

public class SO_EnemyHasProjectileStats : SO_BaseUnitStats
{
	[SerializeField]
	private float _projectileDamage;

	[SerializeField]
	private float _projectileDamageRadius;

	public float ProjectileDamage
	{
		get
		{
			return this._projectileDamage;
		}
	}

	public float ProjectileDamageRadius
	{
		get
		{
			return this._projectileDamageRadius;
		}
	}
}
