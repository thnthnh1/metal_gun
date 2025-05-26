using System;
using UnityEngine;

public class Rate : MonoBehaviour
{
	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	public void Hide()
	{
		SoundManager.Instance.PlaySfxClick();
		base.gameObject.SetActive(false);
		EventLogger.LogEvent("N_HideRate", new object[0]);
	}

	public void GoToRate()
	{
		SoundManager.Instance.PlaySfxClick();
		ProfileManager.UserProfile.isNoLongerRate.Set(true);
		base.gameObject.SetActive(false);
		UtilityUnity.OpenStore();
		EventLogger.LogEvent("N_GoStoreRate", new object[0]);
	}
}
