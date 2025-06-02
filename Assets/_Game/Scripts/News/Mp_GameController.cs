using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Mp_GameController : MonoBehaviour
{
	public static Mp_GameController instance;

	[Header("Game Times")]
	public int timeToStart;
	public int gameTime;
	private int minutes;
	private int seconds;

	[Header("Game UI Info")]
	public Text startingGameText;
	public Text gameTimeText;

	[Header("GameOver Screen")]
	public GameObject leaderBoardScreen;
	public GameObject leaderBoardScreenCloseButton;

	[Header("Game states")]
	public bool gameStarted = false;
	public bool gameOver = false;
	public bool synckingTime = false;
	public bool runningTime = false;

	[Header("All bots")]
	public List<GameObject> bots;

	public PhotonView pv;

	// Only Use Demo
	public GameObject IsbotPrefab;

	public void BDS()
	{
		StartCoroutine(BotSpawnDelay());
	}

	public IEnumerator BotSpawnDelay()
	{
		yield return new WaitForSeconds(1);
		BotSpawn();
	}

	public void BotSpawn()
	{
		Instantiate(IsbotPrefab, new Vector3(66.29f, -0.16f, 0),Quaternion.identity);
	}
    // Start is called before the first frame update
    void Awake()
    {
		instance = this;

		pv = GetComponent<PhotonView>();
		StartCoroutine(CountDownStartGame());

		minutes = TimeSpan.FromSeconds(gameTime).Minutes;
		seconds = TimeSpan.FromSeconds(gameTime).Seconds;

		gameTimeText.text = minutes + " : " + seconds;

		SoundManager.Instance.PlayMusic("music_map_1");

		Mp_playerSettings.instance.startMainMenuInMultiplayerScreen = true;
	}

	IEnumerator CountDownStartGame()
	{
		yield return new WaitForSeconds(1);

		timeToStart--;
		if (SceneManager.GetActiveScene().name == "Demo")
		{
			Debug.Log("Nik Lof Is the Tutorial Time");
			StartGameCountDownRPC_Local(timeToStart);
		}
		else
		{
			pv.RPC("StartGameCountDownRPC", RpcTarget.AllBuffered, timeToStart);
		}
	}

	[PunRPC]
	public void StartGameCountDownRPC(int newTime)
	{
		if (newTime <= 0)
		{
			startingGameText.text = "";

			if (PhotonNetwork.IsMasterClient && !gameStarted && !gameOver)
			{
				StartCoroutine(CountDownGaming());
			}

			gameStarted = true;
		}
		else
		{
			if (PhotonNetwork.IsMasterClient)
			{
				StartCoroutine(CountDownStartGame());
			}

			startingGameText.text = "Game Start in : " + newTime;
		}
	}
	public void StartGameCountDownRPC_Local(int newTime)
	{
		if (newTime <= 0)
		{
			startingGameText.text = "";
			gameStarted = true;

			if (newTime == 0)
			{
				// FindObjectOfType<Nik_Demo>().Start_Demo();
				Debug.Log("<color>Nik log is Tutorial Step Change 1</color>");
				Nik_Demo.instance.StartTutorialStep1();
				// return;
			}

			if (!gameStarted && !gameOver)
			{
				StartCoroutine(CountDownGaming());
			}

			gameStarted = true;
		}
		else
		{

			StartCoroutine(CountDownStartGame());

			startingGameText.text = "Game Start in : " + newTime;
		}
	}

	IEnumerator CountDownGaming()
	{
		yield return new WaitForSeconds(1);

		startingGameText.text = "";

		gameTime--;

		Debug.Log("Count Time");

		minutes = TimeSpan.FromSeconds(gameTime).Minutes;
		seconds = TimeSpan.FromSeconds(gameTime).Seconds;

		gameTimeText.text = minutes + " : " + seconds;

		if (SceneManager.GetActiveScene().name == "Demo")
		{
			if (gameTime > 0)
			{
				StartCoroutine(CountDownGaming());
			}
			if (gameTime == 0)
			{
				Debug.Log("Nik log stop tutorial time Game Start");
				StopAllCoroutines();
			}
		}
		else
		{
			if (PhotonNetwork.IsMasterClient && !synckingTime)
			{
				StartCoroutine(SynckingTimer());

				synckingTime = true;
			}

			if (gameTime > 0 && PhotonNetwork.IsMasterClient)
			{
				StartCoroutine(CountDownGaming());
			}
		}
	}

	IEnumerator SynckingTimer() 
	{
		yield return new WaitForSeconds(3);

		if (PhotonNetwork.IsMasterClient)
		{
			pv.RPC("SyncTimer", RpcTarget.All, gameTime);
		}
		//  Remove Code
		StartCoroutine(SynckingTimer());
	}

	[PunRPC]
	public void SyncTimer(int time) 
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			gameTime = time;
			minutes = TimeSpan.FromSeconds(gameTime).Minutes;
			seconds = TimeSpan.FromSeconds(gameTime).Seconds;

			gameTimeText.text = minutes + " : " + seconds;

			if (!runningTime)
			{
				StartCoroutine(FakeCountdown());
				runningTime = true;
			}
		}
		if (time == 0)
		{
			Debug.Log("Nik Time is : " + time);
			time = -1;
			//**** Game Over ****//
			startingGameText.text = "Game Finishied!";
			int playerNumber = 0;
			if (SceneManager.GetActiveScene().name == "Demo")
			{
				playerNumber = GameLeaderboard.instance.GetPlayerPoints(Mp_playerSettings.instance.playerName);
			}
			else
			{
				//Save Points and trophy
				playerNumber = GameLeaderboard.instance.GetPlayerPoints(HudManager.instance.localPlayerReference.pv.Owner.NickName);

				PlayerPrefs.SetFloat("PlayerExp", Mp_playerSettings.instance.playerExp + GameLeaderboard.instance.playerTotalList[playerNumber]);
				PlayerPrefs.SetInt("Trophys", Mp_playerSettings.instance.trophys + GameLeaderboard.instance.playerTrophyList[playerNumber]);
			}
			//Give rewards
			GameData.playerTournamentData.score += Mp_playerSettings.instance.trophys + GameLeaderboard.instance.playerTrophyList[playerNumber];
			int coinsToGive = (GameLeaderboard.instance.playerTrophyList[playerNumber] / 2) * 250;
			GameData.playerResources.ReceiveCoin(coinsToGive);

			if (SceneManager.GetActiveScene().name != "Demo")
			{
				Mp_playerSettings.instance.trophy = GameLeaderboard.instance.playerTrophyList[playerNumber];
				Mp_playerSettings.instance.coins = coinsToGive;
			}
			

			//Open leaderboard
			leaderBoardScreen.SetActive(true);
			leaderBoardScreenCloseButton.SetActive(false);
			Debug.Log("Nik Name And Score Display In Log : " + Mp_playerSettings.instance.facebookID + " : " + GameData.playerTournamentData.score + 
			" Test Score : " + GameLeaderboard.instance.playerTrophyList[playerNumber]);// Or Mp_playerSettings.instance.trophy
			//Highscores.instance.AddNewHighscore(Mp_playerSettings.instance.facebookID, GameData.playerTournamentData.score);

			gameOver = true;

			Invoke("ReturnToMainScreen", 5);
		}
	}
	

	IEnumerator FakeCountdown()
	{
		yield return new WaitForSeconds(1);
		gameTime--;
		startingGameText.text = "";
		if (gameTime<=0)
		{
			gameTime = 0;
		}
		minutes = TimeSpan.FromSeconds(gameTime).Minutes;
		seconds = TimeSpan.FromSeconds(gameTime).Seconds;

		gameTimeText.text = minutes + " : " + seconds;

		if (PhotonNetwork.IsMasterClient)
		{
			if (gameTime > 0)
			{
				StartCoroutine(CountDownGaming());

				StopCoroutine(FakeCountdown());
			}
		}
		else
		{
			StartCoroutine(FakeCountdown());
		}
		
	}

	public void ReturnToMainScreen() 
	{
		PhotonNetwork.LeaveRoom();
	}

	public IEnumerator ReturnToMainMenu()
	{
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene(2);
	}
}
