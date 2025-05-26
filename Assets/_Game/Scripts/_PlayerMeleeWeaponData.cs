using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerMeleeWeaponData : Dictionary<int, PlayerMeleeWeaponData>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerMeleeWeaponData.Set(value);
		ProfileManager.SaveAll();
	}

	public void ReceiveNewMeleeWeapon(int id)
	{
		if (GameData.staticMeleeWeaponData.ContainsKey(id))
		{
			if (base.ContainsKey(id))
			{
				PlayerMeleeWeaponData playerMeleeWeaponData = base[id];
				playerMeleeWeaponData.level += 2;
				playerMeleeWeaponData.level = Mathf.Clamp(playerMeleeWeaponData.level, 1, GameData.staticMeleeWeaponData[id].upgradeInfo.Length);
			}
			else
			{
				StaticMeleeWeaponData data = GameData.staticMeleeWeaponData.GetData(id);
				base.Add(id, new PlayerMeleeWeaponData
				{
					id = id,
					level = 1,
					isNew = true
				});
			}
			this.Save();
		}
	}

	public int GetMeleeWeaponLevel(int id)
	{
		int result = 1;
		if (base.ContainsKey(id))
		{
			result = base[id].level;
		}
		return result;
	}

	public void IncreaseMeleeWeaponLevel(int id)
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
