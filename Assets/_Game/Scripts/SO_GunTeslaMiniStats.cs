using System;
using UnityEngine;

public class SO_GunTeslaMiniStats : SO_GunStats
{
	[SerializeField]
	private float _stunChance;

	[SerializeField]
	private float _stunDuration;

	public float StunChance
	{
		get
		{
			return this._stunChance;
		}
	}

	public float StunDuration
	{
		get
		{
			return this._stunDuration;
		}
	}
}
