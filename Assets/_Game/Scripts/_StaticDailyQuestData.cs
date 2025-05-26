using System;
using System.Collections.Generic;

public class _StaticDailyQuestData : List<StaticDailyQuestData>
{
	public StaticDailyQuestData GetData(DailyQuestType type)
	{
		for (int i = 0; i < base.Count; i++)
		{
			StaticDailyQuestData staticDailyQuestData = base[i];
			if (staticDailyQuestData.type == type)
			{
				return staticDailyQuestData;
			}
		}
		return null;
	}
}
