using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerCampaignRewardProgressData : Dictionary<MapType, List<bool>>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(value);
		ProfileManager.SaveAll();
	}

	public void AddNewProgress(MapType map)
	{
		if (!base.ContainsKey(map))
		{
			List<bool> list = new List<bool>(3);
			for (int i = 0; i < 3; i++)
			{
				list.Add(false);
			}
			base.Add(map, list);
			this.Save();
		}
	}

	public void ClaimReward(MapType map, int boxIndex)
	{
		if (base.ContainsKey(map))
		{
			for (int i = 0; i < base[map].Count; i++)
			{
				base[map][boxIndex] = true;
			}
			this.Save();
		}
	}
}
