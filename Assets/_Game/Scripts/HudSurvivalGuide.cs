using System;
using UnityEngine;

public class HudSurvivalGuide : MonoBehaviour
{
	public GameObject popup;

	public void Open()
	{
		this.popup.SetActive(true);
	}

	public void Close()
	{
		this.popup.SetActive(false);
	}

	public void StartSurvival()
	{
		this.Close();
		SoundManager.Instance.PlaySfx("sfx_start_mission", 0f);
		EventDispatcher.Instance.PostEvent(EventID.StartFirstWave);
	}
}
