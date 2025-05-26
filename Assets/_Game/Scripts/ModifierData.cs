using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public class ModifierData
{
	private StatsType _stats_k__BackingField;

	private ModifierType _type_k__BackingField;

	private float _value_k__BackingField;

	public StatsType stats
	{
		get;
		private set;
	}

	public ModifierType type
	{
		get;
		private set;
	}

	public float value
	{
		get;
		set;
	}

	public ModifierData(StatsType stats, ModifierType type, float value)
	{
		this.stats = stats;
		this.type = type;
		this.value = value;
	}
}
