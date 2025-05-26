using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerProfile
{
	public int level;

	public int exp;

	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerProfile.Set(value);
		ProfileManager.SaveAll();
	}

	public void ReceiveExp(int value)
	{
		if (value > 0)
		{
			this.exp += value;
			int levelByExp = GameData.staticRankData.GetLevelByExp(this.exp);
			if (levelByExp > this.level)
			{
				this.level = levelByExp;
				EventDispatcher.Instance.PostEvent(EventID.LevelUp, value);
				StaticRankData data = GameData.staticRankData.GetData(this.level);
				string content = string.Format("RANK UP TO LEVEL {0}\n<color=yellow>{1}</color>", data.level, GameData.staticRankData.GetRankName(data.level));
				List<RewardData> rewards = data.rewards;
				Singleton<Popup>.Instance.ShowReward(rewards, content, null);
			}
			this.Save();
			EventDispatcher.Instance.PostEvent(EventID.ReceiveExp, value);
		}
	}
}
