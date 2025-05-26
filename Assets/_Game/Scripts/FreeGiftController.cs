using System;
using UnityEngine;

public class FreeGiftController : MonoBehaviour
{
	public GameObject[] notifications;

	public CellViewFreeGift[] freeGifts;

	public void Init()
	{
		for (int i = 0; i < this.freeGifts.Length; i++)
		{
			this.freeGifts[i].Init();
		}
		EventDispatcher.Instance.RegisterListener(EventID.NewDay, delegate(Component sender, object param)
		{
			this.OnNewDay();
		});
	}

	public void Open()
	{
		base.gameObject.SetActive(true);
		SoundManager.Instance.PlaySfxClick();
	}

	public void Close()
	{
		base.gameObject.SetActive(false);
		SoundManager.Instance.PlaySfxClick();
	}

	public void CheckNotification()
	{
		int num = ProfileManager.UserProfile.countViewAdsFreeCoin;
		for (int i = 0; i < this.notifications.Length; i++)
		{
			this.notifications[i].SetActive(num < GameData.staticFreeGiftData.Count);
		}
	}

	private void UpdateState()
	{
		for (int i = 0; i < this.freeGifts.Length; i++)
		{
			this.freeGifts[i].UpdateState();
		}
	}

	private void OnNewDay()
	{
		this.UpdateState();
		this.CheckNotification();
	}
}
