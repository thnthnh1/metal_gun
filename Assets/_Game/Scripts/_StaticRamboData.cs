using System;
using System.Collections.Generic;

public class _StaticRamboData : Dictionary<int, StaticRamboData>
{
	public StaticRamboData GetData(int id)
	{
		if (base.ContainsKey(id))
		{
			return base[id];
		}
		return null;
	}
}
