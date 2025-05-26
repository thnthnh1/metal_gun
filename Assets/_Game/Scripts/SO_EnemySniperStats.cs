using System;
using UnityEngine;

public class SO_EnemySniperStats : SO_BaseUnitStats
{
	[SerializeField]
	private float _knifeDamage;

	[SerializeField]
	private float _knifeAttackTimePerSecond;

	public float KnifeDamage
	{
		get
		{
			return this._knifeDamage;
		}
	}

	public float KnifeAttackTimePerSecond
	{
		get
		{
			return this._knifeAttackTimePerSecond;
		}
	}
}
