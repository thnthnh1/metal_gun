using System;
using System.Collections.Generic;

public class _StaticFreeGiftData : List<List<RewardData>>
{
	public List<RewardData> GetRewards(int times)
	{
		if (times <= base.Count)
		{
			return base[times - 1];
		}
		return base[base.Count - 1];
	}
}
