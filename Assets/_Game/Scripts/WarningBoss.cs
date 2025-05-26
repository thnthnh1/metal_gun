using System;
using UnityEngine;

public class WarningBoss : MonoBehaviour
{
	public void Active()
	{
		base.gameObject.SetActive(true);
		SoundManager.Instance.PlaySfx("sfx_warning", 0f);
	}

	public void Deactive()
	{
		base.gameObject.SetActive(false);
		EventDispatcher.Instance.PostEvent(EventID.WarningBossDone);
	}
}
