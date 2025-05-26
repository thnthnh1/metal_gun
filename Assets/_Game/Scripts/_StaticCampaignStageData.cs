using System;
using System.Collections.Generic;
using UnityEngine;

public class _StaticCampaignStageData : List<StaticCampaignStageData>
{
	public int GetLevelEnemy(string id, Difficulty difficulty)
	{
		int num = 1;
		if (GameData.campaignStageLevelData.ContainsKey(id))
		{
			num = GameData.campaignStageLevelData[id];
			if (difficulty == Difficulty.Hard)
			{
				num += 2;
			}
			else if (difficulty == Difficulty.Crazy)
			{
				num += 7;
			}
		}
		return Mathf.Clamp(num, 1, 20);
	}

	public int GetCoinDrop(string id, Difficulty difficulty)
	{
		int num = 0;
		for (int i = 0; i < base.Count; i++)
		{
			if (string.Compare(base[i].stageNameId, id) == 0)
			{
				List<RewardData> list = base[i].rewards[difficulty];
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].type == RewardType.Coin)
					{
						num += list[j].value;
					}
				}
			}
		}
		return num;
	}

	public List<RewardData> GetFirstTimeRewards(string id, Difficulty difficulty)
	{
		for (int i = 0; i < base.Count; i++)
		{
			if (string.Compare(base[i].stageNameId, id) == 0)
			{
				return base[i].firstTimeRewards[difficulty];
			}
		}
		return null;
	}

	public int GetCoinCompleteStage(string id, Difficulty difficulty)
	{
		int result = 0;
		for (int i = 0; i < base.Count; i++)
		{
			if (string.Compare(base[i].stageNameId, id) == 0)
			{
				result = base[i].coinCompleteStage[(int)difficulty];
			}
		}
		return result;
	}
}
