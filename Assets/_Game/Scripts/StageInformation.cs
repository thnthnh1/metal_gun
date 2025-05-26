using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageInformation : MonoBehaviour
{
	public Text textStageNameId;

	public GameObject btnStartEnable;

	public GameObject btnStartDisable;

	public RewardElement[] rewardCells;

	public GameObject[] stars;

	public GameObject[] highlights;

	public GameObject[] locks;

	public GameObject[] ticks;

	private string stageId;

	private Difficulty selectingDifficulty;

	private Difficulty highestPlayableDifficulty;

	public void Open(string stageId)
	{
		this.stageId = stageId;
		this.textStageNameId.text = string.Format("STAGE {0}", stageId);
		this.highestPlayableDifficulty = MapUtils.GetHighestPlayableDifficulty(stageId);
		this.selectingDifficulty = this.highestPlayableDifficulty;
		List<bool> progress = GameData.playerCampaignStageProgress.GetProgress(stageId);
		for (int i = 0; i < 3; i++)
		{
			this.locks[i].SetActive(i > (int)this.highestPlayableDifficulty);
			this.ticks[i].SetActive(progress[i]);
		}
		int numberOfStar = MapUtils.GetNumberOfStar(stageId);
		for (int j = 0; j < this.stars.Length; j++)
		{
			this.stars[j].SetActive(j < numberOfStar);
		}
		this.SetInformation();
		base.gameObject.SetActive(true);
	}

	public void Close()
	{
		base.gameObject.SetActive(false);
	}

	public void SelectDifficulty(int difficulty)
	{
		if (this.selectingDifficulty == (Difficulty)difficulty)
		{
			return;
		}
		this.selectingDifficulty = (Difficulty)difficulty;
		this.SetInformation();
	}

	public void Play()
	{
		GameData.mode = GameMode.Campaign;
		this.StartMission();
		if (GameData.isShowingTutorial && string.Compare(this.stageId, "1.1") == 0)
		{
			EventDispatcher.Instance.PostEvent(EventID.CompleteStep, TutorialType.WorldMap);
		}
	}

	private void StartMission()
	{
		GameData.currentStage = new StageData(this.stageId, this.selectingDifficulty);
		SoundManager.Instance.PlaySfx("sfx_start_mission", 0f);
		Loading.nextScene = "GamePlay";
		Singleton<Popup>.Instance.loading.Show();
	}

	private void SetInformation()
	{
		for (int i = 0; i < this.highlights.Length; i++)
		{
			this.highlights[i].SetActive(i == (int)this.selectingDifficulty);
		}
		List<RewardData> list = new List<RewardData>();
		if (MapUtils.IsStagePassed(this.stageId, this.selectingDifficulty))
		{
			list = MapUtils.GetStaticRewards(this.stageId, this.selectingDifficulty);
		}
		else
		{
			list = MapUtils.GetFirstTimeRewards(this.stageId, this.selectingDifficulty);
		}
		for (int j = 0; j < this.rewardCells.Length; j++)
		{
			RewardElement rewardElement = this.rewardCells[j];
			rewardElement.gameObject.SetActive(false);
			rewardElement.gameObject.SetActive(j < list.Count);
			if (j < list.Count)
			{
				RewardData data = list[j];
				rewardElement.SetInformation(data, false);
			}
		}
		this.btnStartEnable.SetActive(this.selectingDifficulty <= this.highestPlayableDifficulty);
		this.btnStartDisable.SetActive(this.selectingDifficulty > this.highestPlayableDifficulty);
	}
}
