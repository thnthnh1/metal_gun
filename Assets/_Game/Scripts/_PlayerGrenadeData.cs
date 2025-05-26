using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerGrenadeData : Dictionary<int, PlayerGrenadeData>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerGrenadeData.Set(value);
		ProfileManager.SaveAll();
	}

	public int GetGrenadeLevel(int id)
	{
		int result = 1;
		if (base.ContainsKey(id))
		{
			result = base[id].level;
		}
		return result;
	}

	public int GetQuantityHave(int id)
	{
		int result = 0;
		if (base.ContainsKey(id))
		{
			result = base[id].quantity;
		}
		return result;
	}

	public void Receive(int id, int quantity)
	{
		if (base.ContainsKey(id))
		{
			base[id].quantity += quantity;
		}
		else if (GameData.staticGrenadeData.ContainsKey(id))
		{
			base.Add(id, new PlayerGrenadeData(id, 1, quantity)
			{
				isNew = true
			});
		}
		this.Save();
	}

	public void Consume(int id, int quantity)
	{
		if (base.ContainsKey(id))
		{
			base[id].quantity -= quantity;
			if (base[id].quantity < 0)
			{
				base[id].quantity = 0;
			}
		}
		this.Save();
	}

	public void RemoveGrenade(int id)
	{
		if (base.ContainsKey(id))
		{
			base.Remove(id);
			this.Save();
		}
	}

	public void IncreaseGrenadeLevel(int id)
	{
		if (base.ContainsKey(id))
		{
			int num = base[id].level;
			num++;
			base[id].level = num;
			this.Save();
		}
	}

	public void SetNew(int id, bool isNew)
	{
		if (base.ContainsKey(id))
		{
			base[id].isNew = isNew;
			this.Save();
		}
	}
}
