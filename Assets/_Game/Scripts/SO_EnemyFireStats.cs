using System;
using UnityEngine;

public class SO_EnemyFireStats : SO_BaseUnitStats
{
	[SerializeField]
	private float _timeApplyDamage;

	[SerializeField]
	private float _slowPercent;

	public float TimeApplyDamage
	{
		get
		{
			return this._timeApplyDamage;
		}
	}

	public float SlowPercent
	{
		get
		{
			return this._slowPercent;
		}
	}
}
