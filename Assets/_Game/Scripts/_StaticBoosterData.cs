using System;
using System.Collections.Generic;

public class _StaticBoosterData : Dictionary<BoosterType, StaticBoosterData>
{
	public StaticBoosterData GetData(BoosterType type)
	{
		if (base.ContainsKey(type))
		{
			return base[type];
		}
		return null;
	}
}
