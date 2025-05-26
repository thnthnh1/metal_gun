using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapChooser : MonoBehaviour
{
	public static WorldMapNavigation navigation;

	public StageInformation stageInfoController;

	[Header("MAP")]
	public int currentMapIndex;

	public GameObject btnNextMap;

	public GameObject btnPreviousMap;

	public Text currentStar;

	public Text maxStar;

	public Image starProgress;

	public MapOverview[] mapOverviews;

	public CampaignBoxReward[] boxes;

	[Header("MAP PAGE")]
	public Sprite pageActive;

	public Sprite pageDeactive;

	public Image[] pageMap;

	private int totalMap;

	private int totalDifficulty;

	private string selectingStageId;

	private Difficulty currentDifficulty;

	private void Awake()
	{
		this.totalMap = Enum.GetNames(typeof(MapType)).Length;
		this.totalDifficulty = Enum.GetNames(typeof(Difficulty)).Length;
		for (int i = 0; i < this.mapOverviews.Length; i++)
		{
			this.mapOverviews[i].Init();
		}
	}

	private void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ClickStageOnWorldMap, delegate(Component sender, object param)
		{
			this.ShowStageInformation((string)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ClaimCampaignBox, delegate(Component sender, object param)
		{
			this.OnClaimBoxReward((int)param);
		});
	}

	private void OnEnable()
	{
		string stageId = string.Empty;
		WorldMapNavigation worldMapNavigation = MapChooser.navigation;
		if (worldMapNavigation != WorldMapNavigation.None)
		{
			if (worldMapNavigation == WorldMapNavigation.NextStageFromGame)
			{
				stageId = MapUtils.GetNextStage(GameData.currentStage);
				MapType mapType = MapUtils.GetMapType(stageId);
				this.currentMapIndex = mapType - MapType.Map_1_Desert;
				this.ShowStageInformation(stageId);
			}
		}
		else
		{
			stageId = MapUtils.GetCurrentProgressStageId();
			MapType mapType = MapUtils.GetMapType(stageId);
			this.currentMapIndex = mapType - MapType.Map_1_Desert;
		}
		MapChooser.navigation = WorldMapNavigation.None;
		this.UpdateWorldMapInformation();
	}

	private void OnDisable()
	{
		if (this.stageInfoController)
		{
			this.stageInfoController.Close();
		}
	}

	public void NextMap()
	{
		SoundManager.Instance.PlaySfxClick();
		this.currentMapIndex++;
		this.UpdateWorldMapInformation();
	}

	public void PreviousMap()
	{
		SoundManager.Instance.PlaySfxClick();
		this.currentMapIndex--;
		this.UpdateWorldMapInformation();
	}

	private void ShowStageInformation(string stageId)
	{
		if (string.IsNullOrEmpty(stageId))
		{
			return;
		}
		this.stageInfoController.Open(stageId);
	}

	private void UpdateWorldMapInformation()
	{
		MapType mapType = this.GetMapType(this.currentMapIndex);
		for (int i = 0; i < this.totalMap; i++)
		{
			this.mapOverviews[i].Active(i == this.currentMapIndex);
			this.pageMap[i].sprite = ((i != this.currentMapIndex) ? this.pageDeactive : this.pageActive);
		}
		int numberOfStage = MapUtils.GetNumberOfStage(mapType);
		int numberOfStar = MapUtils.GetNumberOfStar(mapType);
		int num = numberOfStage * this.totalDifficulty;
		this.maxStar.text = num.ToString();
		this.currentStar.text = numberOfStar.ToString();
		this.btnNextMap.SetActive(this.currentMapIndex < this.mapOverviews.Length - 1);
		this.btnPreviousMap.SetActive(this.currentMapIndex > 0);
		if (!GameData.playerCampaignRewardProgress.ContainsKey(mapType))
		{
			GameData.playerCampaignRewardProgress.AddNewProgress(mapType);
		}
		this.StartActionEndOfFrame(new Action(this.LoadBoxRewardState));
	}

	private void LoadBoxRewardState()
	{
		MapType mapType = this.GetMapType(this.currentMapIndex);
		List<bool> progress = GameData.playerCampaignRewardProgress[mapType];
		int numberOfStar = MapUtils.GetNumberOfStar(mapType);
		for (int i = 0; i < this.boxes.Length; i++)
		{
			this.boxes[i].LoadState(numberOfStar, progress);
		}
	}

	private void OnClaimBoxReward(int index)
	{
		MapType mapType = this.GetMapType(this.currentMapIndex);
		List<RewardData> rewards = GameData.staticCampaignBoxRewardData.GetRewards(mapType, index);
		RewardUtils.Receive(rewards);
		Singleton<Popup>.Instance.ShowReward(rewards, null, null);
		SoundManager.Instance.PlaySfx("sfx_get_reward", 0f);
		GameData.playerCampaignRewardProgress.ClaimReward(mapType, index);
		this.LoadBoxRewardState();
		EventLogger.LogEvent("N_ClaimBoxCampaign", new object[]
		{
			string.Format("Map {0} - Box {1}", (int)mapType, index + 1)
		});
	}

	private MapType GetMapType(int mapIndex)
	{
		return mapIndex + MapType.Map_1_Desert;
	}
}
