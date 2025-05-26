using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerBoosterData : Dictionary<BoosterType, int>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerBoosterData.Set(value);
		ProfileManager.SaveAll();
	}

	public int GetQuantityHave(BoosterType type)
	{
		if (base.ContainsKey(type))
		{
			return base[type];
		}
		return 0;
	}

	public void Receive(BoosterType type, int value)
	{
		if (base.ContainsKey(type))
		{
			base[type] += value;
		}
		else
		{
			base.Add(type, value);
		}
		this.Save();
	}

	public void Consume(BoosterType type, int value)
	{
		if (base.ContainsKey(type))
		{
			base[type] -= value;
			if (base[type] < 0)
			{
				base[type] = 0;
			}
		}
		this.Save();
	}
}
