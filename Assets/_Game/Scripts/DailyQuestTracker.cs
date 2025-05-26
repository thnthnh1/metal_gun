using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DailyQuestTracker : MonoBehaviour
{
	private static DailyQuestTracker _Instance_k__BackingField;

	public BaseDailyQuest[] questPool;

	public List<BaseDailyQuest> quests = new List<BaseDailyQuest>();

	public static DailyQuestTracker Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (DailyQuestTracker.Instance == null)
		{
			DailyQuestTracker.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			this.MaintainDailyQuest();
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
			EventDispatcher.Instance.RegisterListener(EventID.NewDay, delegate(Component sender, object param)
			{
				this.RefreshDailyQuest();
			});
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		this.Init();
	}

	private void Init()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			BaseDailyQuest baseDailyQuest = this.quests[i];
			if (!baseDailyQuest.IsAlreadyClaimed())
			{
				this.quests[i].Init();
				this.quests[i].SetProgressToDefault();
			}
		}
	}

	private void SetProgressToDefault()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			this.quests[i].SetProgressToDefault();
		}
	}

	public void Save()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			this.quests[i].Save();
		}
		GameData.playerDailyQuests.Save();
	}

	private void MaintainDailyQuest()
	{
		if (GameData.playerDailyQuests.Count <= 0)
		{
			this.RefreshDailyQuest();
		}
		else
		{
			for (int i = 0; i < GameData.playerDailyQuests.Count; i++)
			{
				PlayerDailyQuestData playerDailyQuestData = GameData.playerDailyQuests[i];
				BaseDailyQuest questPrefab = this.GetQuestPrefab(playerDailyQuestData.type);
				if (questPrefab)
				{
					BaseDailyQuest baseDailyQuest = UnityEngine.Object.Instantiate<BaseDailyQuest>(questPrefab, base.transform);
					baseDailyQuest.progress = playerDailyQuestData.progress;
					this.quests.Add(baseDailyQuest);
				}
			}
		}
	}

	private void RefreshDailyQuest()
	{
		for (int i = 0; i < this.quests.Count; i++)
		{
			UnityEngine.Object.Destroy(this.quests[i].gameObject);
		}
		this.quests.Clear();
		GameData.playerDailyQuests.Clear();
		List<DailyQuestType> list = new List<DailyQuestType>();
		int num = Enum.GetNames(typeof(DailyQuestType)).Length;
		int num2 = 0;
		for (int j = 0; j < 5; j++)
		{
			if (j == 0)
			{
				num2 = 0;
			}
			else if (j == 1)
			{
				num2 = 1;
			}
			else if (j == 2)
			{
				num2 = 9;
			}
			else if (j == 3)
			{
				num2 = 11;
			}
			else if (j == 4)
			{
				num2 = 10;
			}
			list.Add((DailyQuestType)num2);
			BaseDailyQuest baseDailyQuest = UnityEngine.Object.Instantiate<BaseDailyQuest>(this.GetQuestPrefab((DailyQuestType)num2), base.transform);
			baseDailyQuest.name = baseDailyQuest.type.ToString();
			this.quests.Add(baseDailyQuest);
			GameData.playerDailyQuests.Add(new PlayerDailyQuestData(baseDailyQuest.type, 0, false));
		}
		GameData.playerDailyQuests.Save();
	}

	private BaseDailyQuest GetQuestPrefab(DailyQuestType type)
	{
		for (int i = 0; i < this.questPool.Length; i++)
		{
			if (type == this.questPool[i].type)
			{
				return this.questPool[i];
			}
		}
		return null;
	}
}
