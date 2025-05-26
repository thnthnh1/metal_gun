using System;
using UnityEngine;

public class DailyChecker : MonoBehaviour
{
	private bool alreadyCheck;
	private void Start()
	{
		if (!this.alreadyCheck)
		{
			Singleton<MasterInfo>.Instance.StartGetData(false, delegate (MasterInfoResponse response)
			{
				if (response != null)
				{
					this.alreadyCheck = true;
					DateTime dateTime = new DateTime(response.data.dateTime.Year, response.data.dateTime.Month, response.data.dateTime.Day, response.data.dateTime.Hour, response.data.dateTime.Minute, response.data.dateTime.Second);
					DateTime dateTime2 = ProfileManager.UserProfile.dateLastLogin;
#if UNITY_EDITOR
					double totalHours = 30f;
#else
					double totalHours = TimeSpan.FromTicks(dateTime.Ticks - dateTime2.Ticks).TotalHours;
#endif
					if (totalHours >= 24.0)
					{
						ProfileManager.UserProfile.countViewAdsFreeCoin.Set(0);
						ProfileManager.UserProfile.countShareFacebook.Set(0);
						ProfileManager.UserProfile.countPlayTournament.Set(0);
						ProfileManager.UserProfile.countRewardInterstitialAds.Set(0);
						ProfileManager.UserProfile.dateLastLogin.Set(dateTime);
						GameData.playerResources.ResetTicketNewday();
						if (ProfileManager.UserProfile.getDailyGiftDay > 7 && ProfileManager.UserProfile.isReceivedDailyGiftToday)
						{
							ProfileManager.UserProfile.getDailyGiftDay.Set(1);
							if (!ProfileManager.UserProfile.isPassFirstWeek)
							{
								ProfileManager.UserProfile.isPassFirstWeek.Set(true);
							}
						}
						ProfileManager.UserProfile.isReceivedDailyGiftToday.Set(false);
						EventDispatcher.Instance.PostEvent(EventID.NewDay, dateTime);
					}
					EventDispatcher.Instance.PostEvent(EventID.CheckTimeNewDayDone);
					if (response.code != 0)
					{
						EventDispatcher.Instance.PostEvent(EventID.NewVersionAvailable);
					}
				}
			});
		}
		else
		{
			EventDispatcher.Instance.PostEvent(EventID.CheckTimeNewDayDone);
		}
	}
}
