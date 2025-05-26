using System;
using UnityEngine;

public class SO_BossMonkeyStats : SO_BaseUnitStats
{
	[SerializeField]
	private float _stoneDamage;

	[SerializeField]
	private float _spikeDamage;

	[SerializeField]
	private float _spikeSpeed;

	[SerializeField]
	private float _spikeDelay;

	[SerializeField]
	private int _numberSpikes;

	[SerializeField]
	private int _numberMinions;

	[SerializeField]
	private int _levelMinions;

	[SerializeField]
	private float _hp65_stoneDamage;

	[SerializeField]
	private float _hp65_spikeDamage;

	[SerializeField]
	private float _hp65_spikeSpeed;

	[SerializeField]
	private float _hp65_spikeDelay;

	[SerializeField]
	private int _hp65_numberSpikes;

	[SerializeField]
	private int _hp65_numberMinions;

	[SerializeField]
	private float _hp35_stoneDamage;

	[SerializeField]
	private float _hp35_spikeDamage;

	[SerializeField]
	private float _hp35_spikeSpeed;

	[SerializeField]
	private float _hp35_spikeDelay;

	[SerializeField]
	private int _hp35_numberSpikes;

	[SerializeField]
	private int _hp35_numberMinions;

	public float StoneDamage
	{
		get
		{
			return this._stoneDamage;
		}
	}

	public float SpikeDamage
	{
		get
		{
			return this._spikeDamage;
		}
	}

	public float SpikeSpeed
	{
		get
		{
			return this._spikeSpeed;
		}
	}

	public float SpikeDelay
	{
		get
		{
			return this._spikeDelay;
		}
	}

	public int NumberSpikes
	{
		get
		{
			return this._numberSpikes;
		}
	}

	public int NumberMinions
	{
		get
		{
			return this._numberMinions;
		}
	}

	public int LevelMinions
	{
		get
		{
			return this._levelMinions;
		}
	}

	public float Hp65_StoneDamage
	{
		get
		{
			return this._hp65_stoneDamage;
		}
	}

	public float Hp65_SpikeDamage
	{
		get
		{
			return this._hp65_spikeDamage;
		}
	}

	public float Hp65_SpikeSpeed
	{
		get
		{
			return this._hp65_spikeSpeed;
		}
	}

	public float Hp65_SpikeDelay
	{
		get
		{
			return this._hp65_spikeDelay;
		}
	}

	public int Hp65_NumberSpikes
	{
		get
		{
			return this._hp65_numberSpikes;
		}
	}

	public int Hp65_NumberMinions
	{
		get
		{
			return this._hp65_numberMinions;
		}
	}

	public float Hp35_StoneDamage
	{
		get
		{
			return this._hp35_stoneDamage;
		}
	}

	public float Hp35_SpikeDamage
	{
		get
		{
			return this._hp35_spikeDamage;
		}
	}

	public float Hp35_SpikeSpeed
	{
		get
		{
			return this._hp35_spikeSpeed;
		}
	}

	public float Hp35_SpikeDelay
	{
		get
		{
			return this._hp35_spikeDelay;
		}
	}

	public int Hp35_NumberSpikes
	{
		get
		{
			return this._hp35_numberSpikes;
		}
	}

	public int Hp35_NumberMinions
	{
		get
		{
			return this._hp35_numberMinions;
		}
	}
}
