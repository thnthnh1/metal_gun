using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public abstract class BaseModifier
{
	private float _Value_k__BackingField;

	private ModifierType _Type_k__BackingField;

	private StatsType _Stats_k__BackingField;

	public abstract int Priority
	{
		get;
	}

	public float Value
	{
		get;
		set;
	}

	public ModifierType Type
	{
		get;
		protected set;
	}

	public StatsType Stats
	{
		get;
		protected set;
	}

	public BaseModifier(float value, StatsType stats)
	{
		this.Value = value;
		this.Stats = stats;
	}

	public abstract float GetModStatsValue(float stats, float baseStats);
}
