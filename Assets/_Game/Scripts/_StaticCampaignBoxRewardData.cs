using System;
using System.Collections.Generic;

public class _StaticCampaignBoxRewardData : List<StaticCampaignBoxRewardData>
{
	public List<RewardData> GetRewards(MapType map, int boxIndex)
	{
		for (int i = 0; i < base.Count; i++)
		{
			if (base[i].map == map)
			{
				return base[i].rewards[boxIndex];
			}
		}
		return null;
	}
}
