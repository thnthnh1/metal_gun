using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapUtils
{
	private static Dictionary<string, MapData> mapDatas = new Dictionary<string, MapData>();

	public static MapData GetMapData(string nameId)
	{
		if (MapUtils.mapDatas.ContainsKey(nameId))
		{
			return MapUtils.mapDatas[nameId];
		}
		string path = "JSON/Map Enemy Data/" + nameId;
		TextAsset textAsset = Resources.Load<TextAsset>(path);
		MapData mapData = JsonConvert.DeserializeObject<MapData>(textAsset.text);
		MapUtils.mapDatas.Add(nameId, mapData);
		return mapData;
	}

	public static Map GetMapPrefab(string stageId)
	{
		return Resources.Load<Map>("Map/" + stageId);
	}

	public static MapType GetMapType(string stageId)
	{
		string s = stageId.Split(new char[]
		{
			'.'
		}).First<string>();
		return (MapType)int.Parse(s);
	}

	public static string GetMapName(MapType mapType)
	{
		string result = string.Empty;
		if (mapType != MapType.Map_1_Desert)
		{
			if (mapType != MapType.Map_2_Lab)
			{
				if (mapType == MapType.Map_3_Jungle)
				{
					result = "SILENT JUNGLE";
				}
			}
			else
			{
				result = "MUTANT LAB";
			}
		}
		else
		{
			result = "STORM DESERT";
		}
		return result;
	}

	public static string GetNextStage(StageData currentStage)
	{
		int num = int.Parse(currentStage.id.Split(new char[]
		{
			'.'
		}).First<string>());
		int num2 = int.Parse(currentStage.id.Split(new char[]
		{
			'.'
		}).Last<string>());
		string result = string.Empty;
		if (MapUtils.IsLastStageInMap(currentStage.id))
		{
			MapType mapType = MapUtils.GetMapType(currentStage.id);
			if (MapUtils.IsLastMap(mapType))
			{
				result = currentStage.id;
			}
			else
			{
				result = string.Format("{0}.{1}", num + 1, 1);
			}
		}
		else
		{
			result = string.Format("{0}.{1}", num, num2 + 1);
		}
		return result;
	}

	public static string GetCurrentProgressStageId()
	{
		string result = string.Empty;
		if (GameData.playerCampaignStageProgress.Count <= 0)
		{
			result = "1.1";
		}
		else
		{
			string key = GameData.playerCampaignStageProgress.Last<KeyValuePair<string, List<bool>>>().Key;
			int num = int.Parse(key.Split(new char[]
			{
				'.'
			}).First<string>());
			int num2 = int.Parse(key.Split(new char[]
			{
				'.'
			}).Last<string>());
			if (MapUtils.IsLastStageInMap(key))
			{
				MapType mapType = MapUtils.GetMapType(key);
				if (MapUtils.IsLastMap(mapType))
				{
					result = key;
				}
				else
				{
					result = string.Format("{0}.{1}", num + 1, 1);
				}
			}
			else
			{
				result = string.Format("{0}.{1}", num, num2 + 1);
			}
		}
		return result;
	}

	public static Difficulty GetHighestPlayableDifficulty(string stageId)
	{
		Difficulty difficulty = Difficulty.Normal;
		foreach (KeyValuePair<string, List<bool>> current in GameData.playerCampaignStageProgress)
		{
			if (string.Compare(current.Key, stageId) == 0)
			{
				for (int i = 0; i < current.Value.Count; i++)
				{
					if (current.Value[i] && i >= (int)difficulty)
					{
						int num = Mathf.Clamp(i + 1, 0, 2);
						difficulty = (Difficulty)num;
					}
				}
			}
		}
		return difficulty;
	}

	public static List<RewardData> GetFirstTimeRewards(string stageId, Difficulty difficulty)
	{
		for (int i = 0; i < GameData.staticCampaignStageData.Count; i++)
		{
			StaticCampaignStageData staticCampaignStageData = GameData.staticCampaignStageData[i];
			if (string.Compare(staticCampaignStageData.stageNameId, stageId) == 0)
			{
				return staticCampaignStageData.firstTimeRewards[difficulty];
			}
		}
		return null;
	}

	public static List<RewardData> GetStaticRewards(string stageId, Difficulty difficulty)
	{
		for (int i = 0; i < GameData.staticCampaignStageData.Count; i++)
		{
			StaticCampaignStageData staticCampaignStageData = GameData.staticCampaignStageData[i];
			if (string.Compare(staticCampaignStageData.stageNameId, stageId) == 0)
			{
				return staticCampaignStageData.rewards[difficulty];
			}
		}
		return null;
	}

	public static bool IsStagePassed(string stageId, Difficulty difficulty)
	{
		return GameData.playerCampaignStageProgress.ContainsKey(stageId) && GameData.playerCampaignStageProgress[stageId][(int)difficulty];
	}

	public static void UnlockCampaignProgress(StageData stageData)
	{
		if (!MapUtils.IsStagePassed(stageData.id, stageData.difficulty))
		{
			if (GameData.playerCampaignStageProgress.ContainsKey(stageData.id))
			{
				GameData.playerCampaignStageProgress[stageData.id][(int)stageData.difficulty] = true;
			}
			else
			{
				List<bool> list = new List<bool>(3);
				for (int i = 0; i < 3; i++)
				{
					list.Add(i == (int)stageData.difficulty);
				}
				GameData.playerCampaignStageProgress.Add(stageData.id, list);
			}
			GameData.playerCampaignStageProgress.Save();
		}
	}

	public static int GetNumberOfStage(MapType map)
	{
		int num = 0;
		for (int i = 0; i < GameData.staticCampaignStageData.Count; i++)
		{
			string s = GameData.staticCampaignStageData[i].stageNameId.Split(new char[]
			{
				'.'
			}).First<string>();
			if (int.Parse(s) == (int)map)
			{
				num++;
			}
		}
		return num;
	}

	public static int GetNumberOfStar(MapType map)
	{
		int num = 0;
		foreach (KeyValuePair<string, List<bool>> current in GameData.playerCampaignStageProgress)
		{
			if (MapUtils.GetMapType(current.Key) == map)
			{
				for (int i = 0; i < current.Value.Count; i++)
				{
					if (current.Value[i])
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public static int GetNumberOfStar(string stageId)
	{
		int num = 0;
		foreach (KeyValuePair<string, List<bool>> current in GameData.playerCampaignStageProgress)
		{
			if (string.Compare(current.Key, stageId) == 0)
			{
				for (int i = 0; i < current.Value.Count; i++)
				{
					if (current.Value[i])
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	private static bool IsLastDifficulty(Difficulty difficulty)
	{
		int num = Enum.GetNames(typeof(Difficulty)).Length;
		return difficulty == (Difficulty)(num - 1);
	}

	private static bool IsLastMap(MapType mapType)
	{
		int num = Enum.GetNames(typeof(MapType)).Length;
		return mapType == (MapType)num;
	}

	private static bool IsLastStageInMap(string stageId)
	{
		MapType mapType = MapUtils.GetMapType(stageId);
		int numberOfStage = MapUtils.GetNumberOfStage(mapType);
		string s = stageId.Split(new char[]
		{
			'.'
		}).Last<string>();
		return int.Parse(s) == numberOfStage;
	}
}
