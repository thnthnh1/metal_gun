using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerSelectingBooster : List<BoosterType>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerSelectingBooster.Set(value);
		ProfileManager.SaveAll();
	}
}
