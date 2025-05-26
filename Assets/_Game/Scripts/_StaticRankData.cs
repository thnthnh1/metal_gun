using System;
using System.Collections.Generic;

public class _StaticRankData : List<StaticRankData>
{
	public StaticRankData GetData(int level)
	{
		for (int i = 0; i < base.Count; i++)
		{
			StaticRankData staticRankData = base[i];
			if (staticRankData.level == level)
			{
				return staticRankData;
			}
		}
		return null;
	}

	public string GetRankName(int level)
	{
		if (GameData.rankNames.ContainsKey(level))
		{
			return GameData.rankNames[level].ToUpper();
		}
		return string.Empty;
	}

	public int GetExpOfLevel(int level)
	{
		for (int i = 0; i < base.Count; i++)
		{
			StaticRankData staticRankData = base[i];
			if (staticRankData.level == level)
			{
				return staticRankData.exp;
			}
		}
		return 0;
	}

	public int GetLevelByExp(int exp)
	{
		for (int i = base.Count - 1; i >= 0; i--)
		{
			StaticRankData staticRankData = base[i];
			if (staticRankData.exp <= exp)
			{
				return staticRankData.level;
			}
		}
		return base[base.Count - 1].level;
	}
}
