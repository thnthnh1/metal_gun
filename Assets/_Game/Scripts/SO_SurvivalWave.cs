using System;
using System.Collections.Generic;
using UnityEngine;

public class SO_SurvivalWave : ScriptableObject
{
	[SerializeField]
	private int _waveId;

	[SerializeField]
	private bool _isBossWave;

	[SerializeField]
	private BossType _bossType;

	[SerializeField]
	private int _minLevelUnit;

	[SerializeField]
	private int _maxLevelUnit;

	[SerializeField]
	private List<TimeData> _time;

	[SerializeField]
	private List<TimeDropItemData> _timeDropItem;

	public int WaveId
	{
		get
		{
			return this._waveId;
		}
	}

	public bool IsBossWave
	{
		get
		{
			return this._isBossWave;
		}
	}

	public BossType BossType
	{
		get
		{
			return this._bossType;
		}
	}

	public int MinLevelUnit
	{
		get
		{
			return this._minLevelUnit;
		}
	}

	public int MaxLevelUnit
	{
		get
		{
			return this._maxLevelUnit;
		}
	}

	public List<TimeData> Time
	{
		get
		{
			return this._time;
		}
	}

	public List<TimeDropItemData> TimeDropItem
	{
		get
		{
			return this._timeDropItem;
		}
	}
}
