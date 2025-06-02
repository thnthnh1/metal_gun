using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// FacebookSDK Remove
// using Facebook.Unity;

public class Mp_RewardsMenu : MonoBehaviour
{
	public static Mp_RewardsMenu instance;
	public List<Rewards> rewards;

	public bool gettedPositionInLeader;
	public int posInLeaderBoard;

	[Header("Battle reward Panel")]
	public GameObject battleRewardMenue;
	public Text trophyText;
	public Text coinsText;

	[Header("Tournament reward Panel")]
	public GameObject tournamentRewardMenue;
	public Text gemsTournamentText;
	public Text coinsTournamentText;

	public bool isMe = false;
	void Awake()
    {
        instance = this;

		Invoke("VerifyBattleReward", 2);

		Debug.Log("Nik log is the Start OB Name is  : " + this.gameObject.name);
    }

	public void ClaimRewards()
	{
		// if (!FB.IsLoggedIn && !isMe)
		// {
		// 	Debug.LogError("Reward Null Return");
		// 	Debug.Log("<color>LB - Log in Not Set Database Entry</color>");
		// 	return;
		// }

		Debug.Log("Try claim rewards");
		StartCoroutine(ComprobateLeaderBoard());

	}

	IEnumerator ComprobateLeaderBoard() 
	{
		yield return new WaitForSeconds(1);
		// FacebookSDK Remove
		if (/*!FB.IsLoggedIn &&*/ !isMe && PlayerPrefs.GetInt("GoogleSignIn") == 0)
		{
			Debug.LogError("Reward Null Return");
			// Debug.Log("<color>LB - Log in Not Set Database Entry</color>" + " FB : " + FB.IsLoggedIn + " ISME : " + isMe);
			yield break;
		}

		Debug.Log("LB - Log In Enter Reward");

		if (gettedPositionInLeader)
		{
			Debug.Log("Reward to Receive " + posInLeaderBoard);
			Debug.Log("Diamons " + rewards[posInLeaderBoard].diamonsReward);
			Debug.Log("Coins " + rewards[posInLeaderBoard].coinsReward);

			PlayerPrefs.SetInt("Trophys",0);

			int diamonsReward = rewards[posInLeaderBoard].diamonsReward;
			int coinsReward = rewards[posInLeaderBoard].coinsReward;

			GameData.playerResources.ReceiveGem(diamonsReward);
			GameData.playerResources.ReceiveCoin(coinsReward);

			tournamentRewardMenue.SetActive(true);

			gemsTournamentText.text = "" + rewards[posInLeaderBoard].diamonsReward;
			coinsTournamentText.text = "" + rewards[posInLeaderBoard].coinsReward;

			//HudTournamentRanking.instance.DeleteHighscore();
			//Debug.Log("Nik Log Is the tournamentReward is number : " + DisplayHighscores.instance.slotNumber);
			//Debug.Log("Nik Log Is the tournamentReward is Name : " + DisplayHighscores.instance.playerName);
			//Debug.Log("Nik Log Is the tournamentReward is Trophys : " + DisplayHighscores.instance.score);

			GameData.playerTournamentData.isReceivedTopRankReward = true;

		}
		else
		{
			Debug.Log("No Getted highscore slot");
			StartCoroutine(ComprobateLeaderBoard());
		}
	}

	public void VerifyBattleReward() 
	{
		if (Mp_playerSettings.instance.coins != 0 || Mp_playerSettings.instance.trophy != 0)
		{
			battleRewardMenue.SetActive(true);

			trophyText.text = "" + Mp_playerSettings.instance.trophy;
			coinsText.text = "" + Mp_playerSettings.instance.coins;
		}
	}

	public void ResetBattleReward()
	{
		Mp_playerSettings.instance.trophy = 0;
		Mp_playerSettings.instance.coins = 0;
	}
}

[System.Serializable]
public class Rewards
{
	public int diamonsReward;
	public int coinsReward;
}
