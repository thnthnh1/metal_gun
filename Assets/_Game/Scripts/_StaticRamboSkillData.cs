using System;
using System.Collections.Generic;

public class _StaticRamboSkillData : List<StaticRamboSkillData>
{
	public StaticRamboSkillData GetData(int skillId)
	{
		for (int i = 0; i < base.Count; i++)
		{
			StaticRamboSkillData staticRamboSkillData = base[i];
			if (staticRamboSkillData.id == skillId)
			{
				return staticRamboSkillData;
			}
		}
		return null;
	}
}
