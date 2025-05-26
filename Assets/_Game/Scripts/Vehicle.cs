using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class Vehicle : BaseUnit
{
	[Header("VEHICLE")]
	public float fuel;

	public bool infinityFuel = true;

	private readonly BaseUnit _Player_k__BackingField;

	public virtual BaseUnit Player
	{
		get
		{
			return this._Player_k__BackingField;
		}
	}

	public new abstract void Idle();

	public abstract void GetIn(Rambo rambo);

	public abstract void GetOut();
}
