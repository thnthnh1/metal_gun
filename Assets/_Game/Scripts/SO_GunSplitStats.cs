using System;
using UnityEngine;

public class SO_GunSplitStats : SO_GunStats
{
	[SerializeField]
	private float _damageSplit;

	public float DamageSplit
	{
		get
		{
			return this._damageSplit;
		}
	}
}
