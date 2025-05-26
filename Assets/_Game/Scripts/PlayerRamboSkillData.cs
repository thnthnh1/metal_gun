using System;
using System.Collections.Generic;

public class PlayerRamboSkillData : Dictionary<int, int>
{
	public int GetSkillLevel(int skillId)
	{
		if (base.ContainsKey(skillId))
		{
			return base[skillId];
		}
		return 0;
	}

	public void IncreaseLevel(int skillId)
	{
		if (base.ContainsKey(skillId))
		{
			base[skillId]++;
			GameData.playerRamboSkills.Save();
		}
	}

	public void Reset()
	{
		List<int> list = new List<int>(base.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			int key = list[i];
			base[key] = 0;
		}
		GameData.playerRamboSkills.Save();
	}
}
