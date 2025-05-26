using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerCampaignProgressData : Dictionary<Difficulty, PlayerCampaignProgressData>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerCampaignProgessData.Set(value);
		ProfileManager.SaveAll();
	}
}
