using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerRamboData : Dictionary<int, PlayerRamboData>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerRamboData.Set(value);
		ProfileManager.SaveAll();
	}

	public int GetRamboLevel(int id)
	{
		int result = 1;
		if (base.ContainsKey(id))
		{
			result = base[id].level;
		}
		return result;
	}

	public PlayerRamboState GetRamboState(int id)
	{
		PlayerRamboState result = PlayerRamboState.Unlock;
		if (base.ContainsKey(id))
		{
			result = base[id].state;
		}
		return result;
	}

	public void IncreaseRamboLevel(int id)
	{
		if (base.ContainsKey(id))
		{
			PlayerRamboData playerRamboData = base[id];
			playerRamboData.level++;
			this.Save();
		}
	}
}
