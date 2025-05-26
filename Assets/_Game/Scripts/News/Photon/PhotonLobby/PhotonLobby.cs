using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon;

public class PhotonLobby : MonoBehaviour 
{
	[Header ("Enable custom loading Screen")]
	public bool enableCustomLoadingScreen = false;
	public bool [] slots;
	public Text [] playerSlots;

	[Header("Players in this lobby")]
	public int playersInLobby;
	public int playersToStartLobby = 2;
	[HideInInspector]
	public PhotonView pv;

	[Header ("Time to AutoStart Game")]
	public Text timeToStartGameText;
	public int timeToStartGame = 30;
	private int timeToStart;

	[Header ("Start Game Button")]
	public GameObject startGameButton;
	public GameObject minPlayersWarning;

	[Header ("Messaging in Lobby Room")]
	public InputField writeFieldMessage;
	public Text bodyContentText;
	public RectTransform messageContentWindow;
	public float messageSpacing;
	private float spacing;
	public AudioSource newMessageSound;

	//Values to Reset CHat Lobby ViewPort
	private RectTransform MessageContentReference;

	public bool startingGame = false;

	[Header("IA Bots Names")]
	public string[] botsNames;
	public List <string> botsInMatch;



	void Awake () 
	{
		pv = GetComponent<PhotonView>();
		System.Array.Resize(ref slots, playerSlots.Length);

		//Dissable start button
		//startGameButton.SetActive(false);

		//Get Message Viewport reference
		MessageContentReference = messageContentWindow;

		if (enableCustomLoadingScreen)
		{
			PhotonNetwork.AutomaticallySyncScene = false;
		}
		else
		{
			PhotonNetwork.AutomaticallySyncScene = true;
		}
	}
	
	public void ModifyStatusSlot (int slot) 
	{
		if (playerSlots[slot].text == PhotonNetwork.NickName)
		{
			pv.RPC("RefreshPlayerStatus", RpcTarget.AllBuffered, slot);
		}
		else
		{
			//playerSlots[slot].isOn = false;
		}
	}

	[PunRPC]
	public void RefreshPlayerStatus(int slot)
	{
		//playerSlots[slot].isOn = playerStatus;
	}

	public void StartRegister()
	{
		//If no have a nickname change it
		if (PhotonNetwork.NickName == "")
		{
			//If username is set, take it
			//if (ControllerFirebase.controllerFirebase.savedUser.DisplayName != "")
			//{
			//	PhotonNetwork.NickName = ControllerFirebase.controllerFirebase.savedUser.DisplayName;
			//}
			//else//If usermane is not set, Set random anonimous
			//{
			//	PhotonNetwork.NickName = "Anonimous" + Random.Range(100,999);
			//}

			PhotonNetwork.NickName = Mp_playerSettings.instance.playerName;
		}

		//Send player register with delay
		Invoke("SendRegister", 1);
	}

	void SendRegister()
	{
		pv.RPC("RegisterPlayer", RpcTarget.AllBuffered, PhotonNetwork.NickName, Mp_playerSettings.instance.facebookPhotoURL);
  
			//if (PhotonNetwork.IsMasterClient)
			//{
				//startGameButton.SetActive(true);
			//}
			//else
			//{
				//startGameButton.SetActive(false);
			//}
	}

	[PunRPC]
	public void RegisterPlayer(string playerName, string fbPhotonURL)
	{
		Debug.Log("New Player : " + playerName);
		for (int i = 0; i < slots.Length; i++)
		{
			if (!slots[i])
			{
				slots[i]=true;
				playerSlots[i].text = playerName;

				if (fbPhotonURL != "")
				{
					StartCoroutine(LoadImage(fbPhotonURL, i));
				}
				

				break;
			}
		}

		//Add Player to Lobby
		playersInLobby++;

		//Start Lobby screen
		if (playersInLobby>=playersToStartLobby)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				//minPlayersWarning.SetActive(false);

				if (!startingGame)
				{
					//Register all bots names
					for (int i = 0; i < botsNames.Length; i++)
					{
						botsInMatch.Add(botsNames[i]);
					}
					pv.RPC("InitializeStartingGame", RpcTarget.All);

					Mp_playerSettings.instance.botsNames.Clear();

					startingGame = true;
				}
				
			}
			
		}
	}

	public IEnumerator LoadImage(string url, int slot)
	{
		WWW www = new WWW(url);
		yield return www;
		playerSlots[slot].transform.parent.Find("FbPic").GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	}

	public void StartingGame()
	{
		if (playersInLobby<playersToStartLobby)
		{
			minPlayersWarning.SetActive(true);
			Invoke("DesactivateMessage", 3);
		}
		else
		{
			pv.RPC("InitializeStartingGame", RpcTarget.All);
		}
		
	}
	public void DesactivateMessage()
	{
		minPlayersWarning.SetActive(false);
	}
	[PunRPC]
	public void InitializeStartingGame()
	{
		Launcher.launcher.lobbyScreen.SetActive(true);

		//startGameButton.SetActive(false);
		
		//Close Room
		if (PhotonNetwork.IsMasterClient)
		{
			//PhotonNetwork.CurrentRoom.IsOpen = false;
			Debug.Log("Closing room");
		}

		//Start AutoStart Game
		timeToStart = timeToStartGame;

		//Refresh time in the screen
		timeToStartGameText.text = "Game will start in " + timeToStart.ToString();

		StartCoroutine(StartGameCountDown());
	}

	//Start CountDown to Start Game when players is inside in the lobby
	IEnumerator StartGameCountDown()
	{
		yield return new WaitForSeconds(1);

		timeToStart--;

		//Start Register bots
		if (timeToStart < 6 && PhotonNetwork.IsMasterClient)
		{
			if (playersInLobby<5)
			{
				//Check bot name
				int randomNumber = Random.Range(0, botsInMatch.Count);

				Mp_playerSettings.instance.botsNames.Add(botsInMatch[randomNumber]);

				pv.RPC("RegisterPlayer", RpcTarget.AllBuffered, botsInMatch[randomNumber], "");

				botsInMatch.Remove(botsInMatch[randomNumber]);
			}
		}

		//Refresh time in the screen
		if (timeToStart<0)
		{
			timeToStart = 0;
		}
		timeToStartGameText.text = "Match begins in " + timeToStart.ToString();

		if (timeToStart <= 0)
		{
			StartGame();
		}
		else
		{
			StartCoroutine(StartGameCountDown());
		}

		if (PhotonNetwork.IsMasterClient)
		{
			pv.RPC("SyncCountDownTime", RpcTarget.All, timeToStart);
		}
	}

	[PunRPC]
	public void SyncCountDownTime(int time) 
	{
		if (!PhotonNetwork.IsMasterClient)
		{
			timeToStartGameText.text = "Match begins in " + time.ToString();
		}
	}

	public void StartGame()
	{
		if (!enableCustomLoadingScreen)
		{
			if (PhotonNetwork.IsMasterClient)
			{
				PhotonNetwork.CurrentRoom.IsOpen = false;
				Debug.Log("Closing room");
				PhotonNetwork.LoadLevel(4);
			
			}
		}
		else
		{
			//PanelManagerScript.instance.LoadingScreenManager.GetComponent<LoadingScreenManager>().LoadScene("GameScene_DayMP");
		}
	}

	[PunRPC]
	public void UnregisterPlayer(string playerName)
	{
		for (int i = 0; i < slots.Length; i++)
		{
			if (playerSlots[i].text == playerName)
			{
				slots[i]=false;
				playerSlots[i].text = "Empty";

				//Decrease Player to Lobby
				playersInLobby--;
				break;
			}
		}

		if (!PhotonNetwork.IsMasterClient)
		{
			startGameButton.SetActive(false);
		}
	}
	public void LeaveRoom()
	{
		pv.RPC("UnregisterPlayer", RpcTarget.All, PhotonNetwork.NickName);

		PhotonNetwork.LeaveRoom();

		ResetLobby();
	}

	public void ResetLobby()
	{
		Debug.Log("Reseting Lobby");
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i] = false;
			playerSlots[i].text = "Empty";
		}

		//Desactivate Lobby screen
		if (Launcher.launcher.lobbyScreen)
		{
			Launcher.launcher.lobbyScreen.SetActive(false);
		}

		//Reset Time To start Battle
		timeToStartGameText.text = "";

		//Reset Messages in lobby
		bodyContentText.text = "";

		//Reset Player to Lobby
		playersInLobby = 0;

		//Stop Courutines
		StopAllCoroutines();

		//Refresh time in the screen
		Launcher.launcher.timeSearchingGame.text = "";

		Launcher.launcher.canSearchGame = true;

		Launcher.launcher.time = Launcher.launcher.timeSaved;

		//Get Message Viewport reference
		messageContentWindow = MessageContentReference;

		messageContentWindow.sizeDelta = new Vector2(0,30);

		startingGame = false;
	}

	public void KickPlayer(string playerName)
	{
		for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
		{
			if (PhotonNetwork.PlayerList[i].NickName == playerName)
			{
				Debug.Log("Kick " + playerName);
				PhotonNetwork.CloseConnection(PhotonNetwork.PlayerList[i]); //Send kick for the player

				//Renew Slots
				slots[i] = false;
				playerSlots[i].text = "Empty";
			}
		}
	}

	public void SendMessage()
	{
		if (writeFieldMessage.text != "")
		{
			//Assembly message to send in string
			string messageBody = PhotonNetwork.NickName + " : " + writeFieldMessage.text;

			pv.RPC("SendMessageRPC", RpcTarget.All, messageBody);

			//Erase Message from write field
			writeFieldMessage.text = "";

			//Playe Messaging Sound
			newMessageSound.Play();
		}
	}

	[PunRPC]
	public void SendMessageRPC(string messageBody)
	{
		spacing -= messageSpacing; //Set spacing for messages bodies

		//Setting new position to rectTransform Message Content
		messageContentWindow.GetComponent<RectTransform>().offsetMax = 
		new Vector2(0,messageContentWindow.GetComponent<RectTransform>().offsetMax.y + messageSpacing);

		bodyContentText.GetComponent<Text>().text += "\n \n" +  messageBody;

		newMessageSound.Play();
	}
}
