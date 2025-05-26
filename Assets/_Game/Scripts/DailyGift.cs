using System;
using UnityEngine;
using UnityEngine.UI;

public class DailyGift : MonoBehaviour
{
	[Header("DAY 1")]
	public GameObject day1_item;

	public GameObject day1_mark;

	[Header("DAY 2")]
	public GameObject day2_item;

	public GameObject day2_mark;

	[Header("DAY 3")]
	public GameObject day3_item1;

	public GameObject day3_item2;

	public GameObject day3_mark1;

	public GameObject day3_mark2;

	[Header("DAY 4")]
	public GameObject day4_item;

	public GameObject day4_mark;

	[Header("DAY 5")]
	public GameObject day5_item;

	public GameObject day5_mark;

	[Header("DAY 6")]
	public GameObject day6_item;

	public GameObject day6_mark;

	[Header("DAY 7")]
	public GameObject day7_item;

	public GameObject day7_mark;

	[Header("CONTROL")]
	public Button btnCollect;

	public static DateTime date;

	private void OnEnable()
	{
		this.day1_item.SetActive(true);
		this.day2_item.SetActive(true);
		if (ProfileManager.UserProfile.isPassFirstWeek)
		{
			this.day3_item1.SetActive(false);
			this.day3_item2.SetActive(true);
		}
		else
		{
			this.day3_item1.SetActive(true);
			this.day3_item2.SetActive(false);
		}
		this.day4_item.SetActive(true);
		this.day5_item.SetActive(true);
		this.day6_item.SetActive(true);
		this.day7_item.SetActive(true);
		switch (ProfileManager.UserProfile.getDailyGiftDay)
		{
		case 2:
			this.day1_mark.SetActive(true);
			break;
		case 3:
			this.day1_mark.SetActive(true);
			this.day2_mark.SetActive(true);
			break;
		case 4:
			this.day1_mark.SetActive(true);
			this.day2_mark.SetActive(true);
			if (!ProfileManager.UserProfile.isPassFirstWeek)
			{
				this.day3_mark1.SetActive(true);
			}
			else
			{
				this.day3_mark2.SetActive(true);
			}
			break;
		case 5:
			this.day1_mark.SetActive(true);
			this.day2_mark.SetActive(true);
			if (!ProfileManager.UserProfile.isPassFirstWeek)
			{
				this.day3_mark1.SetActive(true);
			}
			else
			{
				this.day3_mark2.SetActive(true);
			}
			this.day4_mark.SetActive(true);
			break;
		case 6:
			this.day1_mark.SetActive(true);
			this.day2_mark.SetActive(true);
			if (!ProfileManager.UserProfile.isPassFirstWeek)
			{
				this.day3_mark1.SetActive(true);
			}
			else
			{
				this.day3_mark2.SetActive(true);
			}
			this.day4_mark.SetActive(true);
			this.day5_mark.SetActive(true);
			break;
		case 7:
			this.day1_mark.SetActive(true);
			this.day2_mark.SetActive(true);
			if (!ProfileManager.UserProfile.isPassFirstWeek)
			{
				this.day3_mark1.SetActive(true);
			}
			else
			{
				this.day3_mark2.SetActive(true);
			}
			this.day4_mark.SetActive(true);
			this.day5_mark.SetActive(true);
			this.day6_mark.SetActive(true);
			this.day7_mark.SetActive(ProfileManager.UserProfile.isReceivedDailyGiftToday);
			break;
		}
		this.btnCollect.gameObject.SetActive(!ProfileManager.UserProfile.isReceivedDailyGiftToday);
	}

	public void Collected()
	{
		SoundManager.Instance.PlaySfx("sfx_get_reward", 0f);
		int num = ProfileManager.UserProfile.getDailyGiftDay;
		switch (num)
		{
		case 1:
			GameData.playerResources.ReceiveCoin(5000);
			this.day1_mark.SetActive(true);
			break;
		case 2:
			GameData.playerResources.ReceiveGem(70);
			this.day2_mark.SetActive(true);
			break;
		case 3:
			if (this.day3_item1.activeInHierarchy)
			{
				GameData.playerGuns.ReceiveNewGun(102);
				this.day3_mark1.SetActive(true);
			}
			else if (this.day3_item2.activeInHierarchy)
			{
				GameData.playerGrenades.Receive(500, 5);
				GameData.playerResources.ReceiveCoin(5000);
				GameData.playerBoosters.Receive(BoosterType.Hp, 2);
				GameData.playerBoosters.Receive(BoosterType.CoinMagnet, 2);
				GameData.playerBoosters.Receive(BoosterType.Critical, 2);
				GameData.playerBoosters.Receive(BoosterType.Damage, 2);
				GameData.playerBoosters.Receive(BoosterType.Speed, 2);
				this.day3_mark2.SetActive(true);
			}
			break;
		case 4:
			GameData.playerResources.ReceiveTournamentTicket(5);
			this.day4_mark.SetActive(true);
			break;
		case 5:
			GameData.playerGrenades.Receive(500, 20);
			this.day5_mark.SetActive(true);
			break;
		case 6:
			GameData.playerBoosters.Receive(BoosterType.Hp, 5);
			GameData.playerBoosters.Receive(BoosterType.CoinMagnet, 5);
			GameData.playerBoosters.Receive(BoosterType.Critical, 5);
			GameData.playerBoosters.Receive(BoosterType.Damage, 5);
			GameData.playerBoosters.Receive(BoosterType.Speed, 5);
			this.day6_mark.SetActive(true);
			break;
		case 7:
			GameData.playerResources.ReceiveGem(200);
			GameData.playerResources.ReceiveCoin(50000);
			this.day7_mark.SetActive(true);
			break;
		}
		num++;
		ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(true);
		ProfileManager.UserProfile.getDailyGiftDay.Set(num);
		ProfileManager.UserProfile.dateGetGift.Set(DailyGift.date);
		ProfileManager.SaveAll();
		EventDispatcher.Instance.PostEvent(EventID.ClaimDailyGift);
		this.btnCollect.gameObject.SetActive(false);
	}
}
