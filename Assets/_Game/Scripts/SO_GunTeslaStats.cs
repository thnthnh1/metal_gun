using System;
using UnityEngine;

public class SO_GunTeslaStats : SO_GunStats
{
	[SerializeField]
	private float _timeApplyDamage;

	[SerializeField]
	private int _numberEnemyChain;

	public float TimeApplyDamage
	{
		get
		{
			return this._timeApplyDamage;
		}
	}

	public int NumberEnemyChain
	{
		get
		{
			return this._numberEnemyChain;
		}
	}
}
