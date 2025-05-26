using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class VideoReward : MonoBehaviour {

	public string adID;

    public int coinsReward;
    public int ticketsReward;
    

	void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown("q"))
		{
            GameData.playerResources.ReceiveTournamentTicket(1);
        }
	}

    public void ShowCoinsVideo(int value) 
    {
        coinsReward = value;
        ticketsReward = 0;
        // AdMob Remove
        Singleton<AdmobController>.Instance.ShowRewardedVideoAd(delegate(ShowResult showResult)
		{
			if (showResult == ShowResult.Finished)
			{
                Time.timeScale = 1;
				UnityEngine.Debug.Log("NIk Log is the Reward Complete");
                Invoke("DelayReward", 0.1f);
			}
			else
			{
				UnityEngine.Debug.Log("NIk Log is the Reward Fail");
			}
		});
    }

    public void DelayReward()
    {
        UnityEngine.Debug.Log("NIk Log is DelayReward Enter");
        GameData.playerResources.ReceiveCoin(coinsReward);
        Singleton<Popup>.Instance.ShowToastMessage(coinsReward + " Coins of Reward", ToastLength.Normal);

        //GameData.playerResources.ReceiveCoin(coinsReward);

        Debug.Log(coinsReward + " Coins Rewarded");
        coinsReward = 0;
        UnityEngine.Debug.Log("NIk Log is DelayReward Complete Reward");
    }

    public void ShowTicketVideo(int value)
    {
        coinsReward = 0;
        ticketsReward = value;

        // Singleton<AdmobController>.Instance.ShowRewardedVideoAd(delegate(ShowResult showResult)
		// {
        //     Time.timeScale = 1;
		// 	if (showResult == ShowResult.Finished)
		// 	{
        //         Time.timeScale = 1;
		// 		UnityEngine.Debug.Log("NIk Log is the Reward Complete");
        //         GameData.playerResources.ReceiveTournamentTicket(ticketsReward);
        //         Singleton<Popup>.Instance.ShowToastMessage(ticketsReward + " Tickets of Reward", ToastLength.Normal);

        //         //GameData.playerResources.ReceiveTournamentTicket(ticketsReward);

        //         Debug.Log(ticketsReward + " Tickets Rewarded");
        //         ticketsReward = 0;
				
		// 	}
		// 	else
		// 	{
		// 		UnityEngine.Debug.Log("NIk Log is the Reward Fail");
		// 	}
        //     Mp_Armory.instance.RefreshTickets();
		// });
    }

    IEnumerator ApplyReward(int coins, int tickets) 
    {
        //Reward Here
        if (coinsReward > 0)
        {
            GameData.playerResources.ReceiveCoin(coinsReward);
            Singleton<Popup>.Instance.ShowToastMessage(coinsReward + " Coins of Reward", ToastLength.Normal);

            //GameData.playerResources.ReceiveCoin(coinsReward);

            Debug.Log(coinsReward + " Coins Rewarded");
            coinsReward = 0;
        }
        if (ticketsReward > 0)
        {
            GameData.playerResources.ReceiveTournamentTicket(ticketsReward);
            Singleton<Popup>.Instance.ShowToastMessage(ticketsReward + " Tickets of Reward", ToastLength.Normal);

            //GameData.playerResources.ReceiveTournamentTicket(ticketsReward);

            Debug.Log(ticketsReward + " Tickets Rewarded");
            ticketsReward = 0;
        }

        Mp_Armory.instance.RefreshTickets();

        Invoke("LoadRewardVideo", 5f);

        yield return null;
    }
}
