using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class TicketChecker : MonoBehaviour
{
	private bool alreadyCheck;

	public double passedTime;
	public double remainingTime;
	public Text remainingTimeText;

	public double timer = 3600;
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

					double totalHours = TimeSpan.FromTicks(dateTime.Ticks - dateTime2.Ticks).TotalMinutes;
					passedTime = totalHours;

					if (totalHours >= 60)
					{
						
						if (GameData.playerResources.tournamentTicket < 5)
						{
							GameData.playerResources.ReceiveTournamentTicket(1);
						}
						

						totalHours -= 60;

						ProfileManager.UserProfile.dateLastLogin.Set(dateTime);

					}
					if (totalHours >= 120)
					{
						if (GameData.playerResources.tournamentTicket < 5)
						{
							GameData.playerResources.ReceiveTournamentTicket(1);
						}

						totalHours -= 60;

						ProfileManager.UserProfile.dateLastLogin.Set(dateTime);

					}
					if (totalHours >= 180)
					{
						if (GameData.playerResources.tournamentTicket < 5)
						{
							GameData.playerResources.ReceiveTournamentTicket(1);
						}

						totalHours -= 60;

						ProfileManager.UserProfile.dateLastLogin.Set(dateTime);

					}
					if (totalHours >= 240)
					{
						if (GameData.playerResources.tournamentTicket < 5)
						{
							GameData.playerResources.ReceiveTournamentTicket(1);
						}

						totalHours -= 60;

						ProfileManager.UserProfile.dateLastLogin.Set(dateTime);

					}
					if (totalHours >= 300)
					{
						if (GameData.playerResources.tournamentTicket < 5)
						{
							GameData.playerResources.ReceiveTournamentTicket(1);
						}

						totalHours -= 60;
					}
					if (totalHours >= 360)
					{
						if (GameData.playerResources.tournamentTicket < 5)
						{
							GameData.playerResources.ReceiveTournamentTicket(1);
						}

						totalHours -= 60;

						ProfileManager.UserProfile.dateLastLogin.Set(dateTime);

					}

					EventDispatcher.Instance.PostEvent(EventID.CheckTimeNewDayDone);
					if (response.code != 0)
					{
						EventDispatcher.Instance.PostEvent(EventID.NewVersionAvailable);
					}

					if (GameData.playerResources.tournamentTicket>5)
					{
						//GameData.playerResources.tournamentTicket = 5;
					}

					remainingTime = totalHours;

					timer -= remainingTime*60;

					StartCoroutine(CountTimer());

					Mp_Armory.instance.RefreshTickets();
				}
			});
		}
		else
		{
			EventDispatcher.Instance.PostEvent(EventID.CheckTimeNewDayDone);
		}
	}

	IEnumerator CountTimer ()
	{
		yield return new WaitForSeconds(1);

		if (GameData.playerResources.tournamentTicket<5)
		{
			if (timer > 0)
			{
				timer --;
			}	
			else
			{
				timer = 3600;
				GameData.playerResources.ReceiveTournamentTicket(1);

				Mp_Armory.instance.RefreshTickets();
			}	

		
			TimeSpan timeSpan = TimeSpan.FromSeconds(timer);

			remainingTimeText.text = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds );
		}
		else
		{
			remainingTimeText.text = "";
		}

		StartCoroutine(CountTimer());
	}
}
