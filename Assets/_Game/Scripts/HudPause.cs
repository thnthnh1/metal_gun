using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HudPause : MonoBehaviour
{
	public GameObject popupPause;

	public GameObject popupPauseCampaign;

	public GameObject popupPauseSurvival;

	public Text stageNameId;

	private static Action<ShowResult> __f__am_cache0;

	private static UnityAction __f__am_cache1;

	private static Action<ShowResult> __f__am_cache2;

	public void Open()
	{
		this.popupPause.SetActive(true);
		this.popupPauseCampaign.SetActive(GameData.mode == GameMode.Campaign);
		this.popupPauseSurvival.SetActive(GameData.mode == GameMode.Survival);
		if (GameData.mode == GameMode.Campaign)
		{
			this.stageNameId.text = string.Format("STAGE {0} - {1}", GameData.currentStage.id, GameData.currentStage.difficulty.ToString().ToUpper());
		}
		this.Pause();
	}

	public void Pause()
	{
		Singleton<GameController>.Instance.modeController.PauseGame();
	}

	public void Leave()
	{
		if (GameData.mode == GameMode.Campaign)
		{
			// Time.timeScale = 1f;
			// if (!ProfileManager.UserProfile.isRemoveAds)
			// {
			// 	// AdMob Remove
			// 	Singleton<AdmobController>.Instance.ManualResetTryLoadCount();
			// 	Singleton<AdmobController>.Instance.ShowInterstitialAd(delegate(ShowResult result)
			// 	{
			// 		Time.timeScale = 1f;
			// 		Singleton<UIController>.Instance.BackToMainMenu();
			// 	});
			// }
			// else
			{
				Singleton<UIController>.Instance.BackToMainMenu();
			}
		}
		else if (GameData.mode == GameMode.Survival)
		{
            /*
			if (AccessToken.CurrentAccessToken != null)
			{
				Time.timeScale = 1f;
				Singleton<Popup>.Instance.Show("do you really want to quit?\nyour score will be saved.", "NOTICE", PopupType.YesNo, delegate
				{
					EventDispatcher.Instance.PostEvent(EventID.QuitSurvivalSession);
				}, null);
			}
			*/
			//else
			{
				Singleton<UIController>.Instance.BackToMainMenu();
			}
		}
	}

	public void Retry()
	{
		if (GameData.mode == GameMode.Campaign)
		{
			// Time.timeScale = 1f;
			// if (!ProfileManager.UserProfile.isRemoveAds)
			// {
			// 	// AdMob Remove
			// 	Singleton<AdmobController>.Instance.ManualResetTryLoadCount();
			// 	Singleton<AdmobController>.Instance.ShowInterstitialAd(delegate(ShowResult result)
			// 	{
			// 		Time.timeScale = 1f;
			// 		Singleton<UIController>.Instance.Retry();
			// 	});
			// }
			// else
			{
				Singleton<UIController>.Instance.Retry();
			}
		}
	}

	public void Resume()
	{
		Singleton<GameController>.Instance.modeController.ResumeGame();
		this.popupPause.SetActive(false);
	}
}
