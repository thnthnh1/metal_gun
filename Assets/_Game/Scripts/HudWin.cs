using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudWin : MonoBehaviour
{
	public Button btnRetry;

	public Button btnSelectStage;

	public Button btnHome;

	public Button btnNextStage;

	public Button btnWatchAds;

	public Text textNotiButtonHome;

	public GameObject[] difficultyIcons;

	public GameObject[] stars;

	public RewardElement[] rewardCells;

	private List<RewardData> winRewards = new List<RewardData>();

	public void Open(List<RewardData> rewards)
	{
		this.winRewards = rewards;
		base.gameObject.SetActive(true);
		this.SetStar();
		this.SetIconDifficulty();
		this.SetNotification();
		for (int i = 0; i < this.rewardCells.Length; i++)
		{
			RewardElement rewardElement = this.rewardCells[i];
			rewardElement.gameObject.SetActive(false);
			rewardElement.gameObject.SetActive(i < rewards.Count);
			if (i < rewards.Count)
			{
				RewardData data = rewards[i];
				rewardElement.SetInformation(data, false);
			}
		}
		this.ShowButtons(true);
		Singleton<UIController>.Instance.ActiveIngameUI(false);
		SoundManager.Instance.PlaySfx("sfx_text_typing", 0f);
		int num = UnityEngine.Random.Range(1, 101);
		this.btnWatchAds.gameObject.SetActive(num <= 40);
	}

	public void SelectStage()
	{
		SoundManager.Instance.PlaySfxClick();
		MainMenu.navigation = MainMenuNavigation.OpenWorldMap;
		MapChooser.navigation = WorldMapNavigation.None;
		SceneFading.Instance.FadeOutAndLoadScene("Menu", true, 2f);
	}

	public void NextStage()
	{
		SoundManager.Instance.PlaySfxClick();
		MainMenu.navigation = MainMenuNavigation.OpenWorldMap;
		MapChooser.navigation = WorldMapNavigation.NextStageFromGame;
		SceneFading.Instance.FadeOutAndLoadScene("Menu", true, 2f);
	}

	public void BackToMainMenu()
	{
		SoundManager.Instance.PlaySfxClick();
		Singleton<UIController>.Instance.BackToMainMenu();
	}

	public void Retry()
	{
		Time.timeScale = 1f;
		SoundManager.Instance.PlaySfxClick();
		SceneFading.Instance.FadeOutAndLoadScene("GamePlay", true, 2f);
	}

	public void WatchAdsX2Reward()
	{
		SoundManager.Instance.PlaySfxClick();
		this.btnWatchAds.interactable = false;
		// AdMob Remove
		Singleton<AdmobController>.Instance.ShowRewardedVideoAd(delegate(ShowResult showResult)
		{
			Time.timeScale = 1f;
			if (showResult == ShowResult.Finished)
			{
				UnityEngine.Debug.Log("NIk Log is the Reward Complete");
				// StartCoroutine(DelayReward());
				Time.timeScale = 1f;
				Invoke("DelayReward", 0.1f);
			}
			else
			{
				this.btnWatchAds.interactable = true;
			}
		});
	}

	public void DelayReward()
	{
		EventDispatcher.Instance.PostEvent(EventID.ViewAdsx2CoinEndGame, true);
		this.btnWatchAds.gameObject.SetActive(false);
		this.ShowButtons(true);
		RewardUtils.Receive(this.winRewards);
		Singleton<Popup>.Instance.ShowReward(this.winRewards, null, null);
		SoundManager.Instance.PlaySfx("sfx_get_reward", 0f);
		EventLogger.LogEvent("N_ViewAdsX2Reward", new object[0]);
	}

	private void SetStar()
	{
		Difficulty difficulty = GameData.currentStage.difficulty;
		for (int i = 0; i < this.stars.Length; i++)
		{
			this.stars[i].SetActive(i <= (int)difficulty);
		}
	}

	private void SetIconDifficulty()
	{
		Difficulty difficulty = GameData.currentStage.difficulty;
		for (int i = 0; i < this.difficultyIcons.Length; i++)
		{
			this.difficultyIcons[i].SetActive(i == (int)difficulty);
		}
	}

	private void ShowButtons(bool isShow)
	{
		this.btnRetry.gameObject.SetActive(isShow);
		this.btnHome.gameObject.SetActive(isShow);
		this.btnNextStage.gameObject.SetActive(isShow);
	}

	private void SetNotification()
	{
		int numberReadyQuest = GameData.playerDailyQuests.GetNumberReadyQuest();
		int numberReadyAchievement = GameData.playerAchievements.GetNumberReadyAchievement();
		this.textNotiButtonHome.transform.parent.gameObject.SetActive(numberReadyQuest > 0 || numberReadyAchievement > 0);
	}
}
