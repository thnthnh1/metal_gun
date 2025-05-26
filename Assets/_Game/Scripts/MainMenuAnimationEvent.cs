using System;
using UnityEngine;

public class MainMenuAnimationEvent : MonoBehaviour
{
	public void OnAnimationComplete()
	{
		if (PlayerPrefs.GetInt("NotifyTutorial") == 0)
		{
			Debug.Log("Nik log return 2");
			return;
		}
		if (!GameData.playerTutorials.IsCompletedStep(TutorialType.WorldMap))
		{
			Singleton<TutorialMenuController>.Instance.ShowTutorial(TutorialType.WorldMap);
		}
		else if (!GameData.playerTutorials.IsCompletedStep(TutorialType.Mission))
		{
			Singleton<TutorialMenuController>.Instance.ShowTutorial(TutorialType.Mission);
		}
		else if (!GameData.playerTutorials.IsCompletedStep(TutorialType.FreeGift))
		{
			Singleton<TutorialMenuController>.Instance.ShowTutorial(TutorialType.FreeGift);
		}
	}
}
