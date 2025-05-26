using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class AchievementTracker : MonoBehaviour
{
	private static AchievementTracker _Instance_k__BackingField;

	public BaseAchievement[] achievementPrefabs;

	public List<BaseAchievement> achievements = new List<BaseAchievement>();

	public static AchievementTracker Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (AchievementTracker.Instance == null)
		{
			AchievementTracker.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			SceneManager.sceneLoaded += new UnityAction<Scene, LoadSceneMode>(this.OnSceneLoaded);
			this.CreateAchievements();
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

	private void CreateAchievements()
	{
		this.achievements.Clear();
		for (int i = 0; i < this.achievementPrefabs.Length; i++)
		{
			BaseAchievement baseAchievement = UnityEngine.Object.Instantiate<BaseAchievement>(this.achievementPrefabs[i], base.transform);
			baseAchievement.name = baseAchievement.type.ToString();
			baseAchievement.SetProgressToDefault();
			this.achievements.Add(baseAchievement);
		}
	}

	private void Init()
	{
		for (int i = 0; i < this.achievements.Count; i++)
		{
			BaseAchievement baseAchievement = this.achievements[i];
			if (!baseAchievement.IsAlreadyCompleted())
			{
				baseAchievement.Init();
				baseAchievement.SetProgressToDefault();
			}
		}
	}

	public void SetProgressToDefault()
	{
		for (int i = 0; i < this.achievements.Count; i++)
		{
			this.achievements[i].SetProgressToDefault();
		}
	}

	public void Save()
	{
		for (int i = 0; i < this.achievements.Count; i++)
		{
			this.achievements[i].Save();
		}
		GameData.playerAchievements.Save();
	}
}
