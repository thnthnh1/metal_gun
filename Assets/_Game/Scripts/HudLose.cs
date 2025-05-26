using System;
using UnityEngine;
using UnityEngine.UI;

public class HudLose : MonoBehaviour
{
	public Text textNotiButtonHome;

	public Button btnSelectStage;

	public Button btnHome;

	public Button btnRetry;

	public void Open()
	{
		base.gameObject.SetActive(true);
		this.SetNotification();
		this.ShowButtons(true);
		Singleton<UIController>.Instance.ActiveIngameUI(false);
		this.CheckTutorial();
	}

	private void CheckTutorial()
	{
		if (!GameData.playerTutorials.IsCompletedStep(TutorialType.RecommendUpgradeWeapon))
		{
			if (GameData.playerTutorials.IsCompletedStep(TutorialType.Weapon))
			{
				GameData.playerTutorials.SetComplete(TutorialType.RecommendUpgradeWeapon);
			}
			else
			{
				Singleton<TutorialGamePlayController>.Instance.ShowTutorialRecommendUpgradeWeapon();
			}
		}
		else if (!GameData.playerTutorials.IsCompletedStep(TutorialType.RecommendUpgradeCharacter))
		{
			if (GameData.playerTutorials.IsCompletedStep(TutorialType.Character))
			{
				GameData.playerTutorials.SetComplete(TutorialType.RecommendUpgradeCharacter);
			}
			else
			{
				Singleton<TutorialGamePlayController>.Instance.ShowTutorialRecommendUpgradeCharacter();
			}
		}
	}

	public void Retry()
	{
		Time.timeScale = 1f;
		SoundManager.Instance.PlaySfxClick();
		SceneFading.Instance.FadeOutAndLoadScene("GamePlay", true, 2f);
	}

	public void SelectStage()
	{
		SoundManager.Instance.PlaySfxClick();
		MainMenu.navigation = MainMenuNavigation.OpenWorldMap;
		MapChooser.navigation = WorldMapNavigation.None;
		SceneFading.Instance.FadeOutAndLoadScene("Menu", true, 2f);
	}

	public void BackToMainMenu()
	{
		SoundManager.Instance.PlaySfxClick();
		Singleton<UIController>.Instance.BackToMainMenu();
	}

	public void GoUpgradeWeapon()
	{
		MainMenu.navigation = MainMenuNavigation.ShowUpgradeWeapon;
		SceneFading.Instance.FadeOutAndLoadScene("Menu", true, 2f);
		if (GameData.isShowingTutorial)
		{
			EventDispatcher.Instance.PostEvent(EventID.SubStepGoUpgradeWeaponFromLose);
		}
	}

	public void GoUpgradeSoldier()
	{
		MainMenu.navigation = MainMenuNavigation.ShowUpgradeSoldier;
		SceneFading.Instance.FadeOutAndLoadScene("Menu", true, 2f);
		if (GameData.isShowingTutorial)
		{
			EventDispatcher.Instance.PostEvent(EventID.SubStepGoUpgradeCharacterFromLose);
		}
	}

	public void GoUpgradeSkill()
	{
		if (GameData.playerTutorials.IsCompletedStep(TutorialType.Character))
		{
			MainMenu.navigation = MainMenuNavigation.ShowUpgradeSkill;
			SceneFading.Instance.FadeOutAndLoadScene("Menu", true, 2f);
		}
		else
		{
			this.GoUpgradeSoldier();
		}
	}

	private void ShowButtons(bool isShow)
	{
		this.btnSelectStage.gameObject.SetActive(isShow);
		this.btnHome.gameObject.SetActive(isShow);
		this.btnRetry.gameObject.SetActive(isShow);
	}

	private void SetNotification()
	{
		int numberReadyQuest = GameData.playerDailyQuests.GetNumberReadyQuest();
		int numberReadyAchievement = GameData.playerAchievements.GetNumberReadyAchievement();
		this.textNotiButtonHome.transform.parent.gameObject.SetActive(numberReadyQuest > 0 || numberReadyAchievement > 0);
	}
}
