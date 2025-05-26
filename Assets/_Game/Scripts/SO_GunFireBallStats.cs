using System;
using UnityEngine;

public class SO_GunFireBallStats : SO_GunStats
{
	[SerializeField]
	private float _timeApplyDamage;

	[SerializeField]
	private float _distance;

	public float TimeApplyDamage
	{
		get
		{
			return this._timeApplyDamage;
		}
	}

	public float Distance
	{
		get
		{
			return this._distance;
		}
	}
}
