using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerRamboSkillData : Dictionary<int, PlayerRamboSkillData>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerRamboSkillData.Set(value);
		ProfileManager.SaveAll();
	}

	public PlayerRamboSkillData GetRamboSkillProgress(int ramboId)
	{
		if (base.ContainsKey(ramboId))
		{
			return base[ramboId];
		}
		return null;
	}

	public int GetUsedSkillPoints(int ramboId)
	{
		int num = 0;
		PlayerRamboSkillData ramboSkillProgress = this.GetRamboSkillProgress(ramboId);
		foreach (KeyValuePair<int, int> current in ramboSkillProgress)
		{
			if (current.Value > 0)
			{
				num++;
			}
		}
		return num;
	}

	public int GetUnusedSkillPoints(int ramboId)
	{
		int usedSkillPoints = this.GetUsedSkillPoints(ramboId);
		int ramboLevel = GameData.playerRambos.GetRamboLevel(ramboId);
		return Mathf.Clamp(ramboLevel - 1 - usedSkillPoints, 0, ramboLevel - 1);
	}
}
