using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerCampaignStageProgressData : Dictionary<string, List<bool>>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerCampaignStageProgessData.Set(value);
		ProfileManager.SaveAll();
	}

	public List<bool> GetProgress(string stageId)
	{
		if (base.ContainsKey(stageId))
		{
			return base[stageId];
		}
		return new List<bool>
		{
			false,
			false,
			false
		};
	}
}
