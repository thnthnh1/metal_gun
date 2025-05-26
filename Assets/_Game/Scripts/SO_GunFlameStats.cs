using System;
using UnityEngine;

public class SO_GunFlameStats : SO_GunStats
{
	[SerializeField]
	private float _timeApplyDamage;

	public float TimeApplyDamage
	{
		get
		{
			return this._timeApplyDamage;
		}
	}
}
