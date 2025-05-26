using System;
using UnityEngine;

public class SO_GunRocketChaserStats : SO_GunStats
{
	[SerializeField]
	private float _radiusDealDamage;

	public float RadiusDealDamage
	{
		get
		{
			return this._radiusDealDamage;
		}
	}
}
