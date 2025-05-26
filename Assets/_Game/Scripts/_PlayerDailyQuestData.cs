using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerDailyQuestData : List<PlayerDailyQuestData>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerDailyQuestData.Set(value);
		ProfileManager.SaveAll();
	}

	public int GetNumberReadyQuest()
	{
		int num = 0;
		for (int i = 0; i < base.Count; i++)
		{
			PlayerDailyQuestData playerDailyQuestData = base[i];
			StaticDailyQuestData data = GameData.staticDailyQuestData.GetData(playerDailyQuestData.type);
			if (playerDailyQuestData.progress >= data.value && !playerDailyQuestData.isClaimed)
			{
				num++;
			}
		}
		return num;
	}

	public bool IsAlreadyClaimed(DailyQuestType type)
	{
		for (int i = 0; i < base.Count; i++)
		{
			PlayerDailyQuestData playerDailyQuestData = base[i];
			if (playerDailyQuestData.type == type)
			{
				return playerDailyQuestData.isClaimed;
			}
		}
		return false;
	}
}
