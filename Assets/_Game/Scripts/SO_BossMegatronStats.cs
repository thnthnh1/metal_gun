using System;
using UnityEngine;

public class SO_BossMegatronStats : SO_BaseUnitStats
{
	[SerializeField]
	private float _smashDamage;

	[SerializeField]
	private float _jumpDamage;

	public float SmashDamage
	{
		get
		{
			return this._smashDamage;
		}
	}

	public float JumpDamage
	{
		get
		{
			return this._jumpDamage;
		}
	}
}
