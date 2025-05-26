using System;
using UnityEngine;

public class SO_GunKamePowerStats : SO_GunStats
{
	[SerializeField]
	private float _chargeTime;

	public float ChargeTime
	{
		get
		{
			return this._chargeTime;
		}
	}
}
