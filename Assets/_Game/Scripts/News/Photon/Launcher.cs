using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Launcher :  MonoBehaviourPunCallbacks
{
    public int botSpawned = 0;

    public static Launcher launcher;
    [Header("Use this Launcher for Main Screen and Gaming Screen")]
    public bool isGamingScene;

    public Transform [] startPosition;

	Transform finalStartPos;

	public int numberToSpawn;

	[Header("Show photon status")]
    public Text statusText;

    [Header("Change player nickname in photon")]
    public InputField nameInputField;

    [Header("Region")]
    public Text regionConnectedText;

    [Header("Time Searching Game")]
    public Text timeSearchingGame;

    [HideInInspector]
    public float time;
    [HideInInspector]
    public float timeSaved;

    [Header("Lobby Screen")]
    public PhotonLobby lobby;
    public GameObject lobbyScreen;
    public GameObject startSearchButton;
    public GameObject stopSearchButton;
    public bool canSearchGame = true;

    [Header("Custom Room Created")]
    public string customRoomName;
    public int photonLevelNumber = 0;

    [Header("Message Screen")]
    public GameObject messageScreen;
    public Text messageText;
    public AudioSource notifySound;

    [Header("ArmoryStatus")]
    public bool weaponReady;
    public bool characterReady;

    [Header("Multiplayer Main Menu")]
    public GameObject multiplayerMainMenu;
    public GameObject [] multiplayerMainMenues;

    public bool connectedToPhoton;
    public GameObject noInternetMessage;

    private Dictionary<string, RoomInfo> cachedRoomList;

    [Header("Ping Text")]
    public Text pingText;

    public bool disconnectedFaul;

    [Header("Demo_Scene")]
    public Button TutorialBTN;

    public void T_Btn_Click()
    {
        // Mp_Armory.instance.AfterTutorial[0].SetActive(true);
        SceneManager.LoadScene("Demo");
    }



    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all the clients in the same room to sync their info or level automatically
        PhotonNetwork.AutomaticallySyncScene = false;

        launcher = this;

        timeSaved = time;

		if (!isGamingScene && Mp_playerSettings.instance.startMainMenuInMultiplayerScreen)
		{
            multiplayerMainMenu.SetActive(true);

        }
    }
	void Start() 
	{
        //Clamp FPS (tick rate improvements)
        Application.targetFrameRate = 60;

        //If we are not connected, reconnect.
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = "1.6";
            PhotonNetwork.ConnectUsingSettings();

            //PhotonNetwork.ConnectToRegion("us");

            //If status text exists, then display info
            if (statusText)
            {
                statusText.text = "Connecting";
            }

            connectedToPhoton = false;
        }
        //If we're connected and inside GamingScene, let's instantiate the player
        else if (isGamingScene)
        {
            StartCoroutine(GamingScene());
        }

        if (lobby)
        {
            //regionConnectedText.text = "Connecting";
        }

        StartCoroutine(ShowPing());
    }

    public IEnumerator ShowPing() 
    {
        yield return new WaitForSeconds(1);

		if (pingText)
		{
			if (PhotonNetwork.IsConnected)
			{
                pingText.text = "Ping : " + PhotonNetwork.GetPing();
            }
		}

        StartCoroutine(ShowPing());
    }
    public IEnumerator GamingScene()
    {
        yield return new WaitForSeconds(0.5f);

         //Get Player Team
        SpawnPlayer();
    }

    public void SortingPositions() 
    {

    }
    [PunRPC]
    public void SortPositions(int value) 
    {

    }
    //Is connected in photon, selectiong team
    public void SpawnPlayer()
    {
		//Comprobate where to spawn player 
		if (numberToSpawn < startPosition.Length)
		{
			finalStartPos = startPosition[Random.Range(0, startPosition.Length)];
		}
		else
		{
			finalStartPos = startPosition[numberToSpawn];
		}
		PhotonNetwork.Instantiate("Player MP", finalStartPos.position, Quaternion.identity, 0);

		GetComponent<PhotonView>().RPC("PopulateSpawnSite", RpcTarget.AllBuffered);

		if (PhotonNetwork.IsMasterClient && Mp_playerSettings.instance.botsNames.Count > 0)
		{
			StartCoroutine(SpawningBots());
		}
	}

	IEnumerator SpawningBots()
	{
        // Comprobate where to spawn player
        if (numberToSpawn >= startPosition.Length)
		{
			finalStartPos = startPosition[Random.Range(0, startPosition.Length)];
		}
		else
		{
			finalStartPos = startPosition[numberToSpawn];
		}
        PhotonNetwork.InstantiateRoomObject("Bot MP", finalStartPos.position, Quaternion.identity, 0);

        GetComponent<PhotonView>().RPC("PopulateSpawnSite", RpcTarget.AllBuffered);

        yield return new WaitForSeconds(0.5f);

		if (botSpawned <= Mp_playerSettings.instance.botsNames.Count-2)
		{
            StartCoroutine(SpawningBots());
		}

        botSpawned++;
    }
	[PunRPC]
	public void PopulateSpawnSite()
	{
		numberToSpawn++;
	}

    public void ChangeRegion(int regionSelected)
    {
        Debug.Log("Region " + regionSelected + " Selected");
        //regionSelector.
        PhotonNetwork.Disconnect();
       regionConnectedText.text = "Changing region";
        StartCoroutine(ConnectToRegion(regionSelected));
    }
    IEnumerator ConnectToRegion(int regionSelected)
    {
        yield return new WaitForSeconds(0.5f);
        if (regionSelected == 0)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        if (regionSelected == 1)
        {
            PhotonNetwork.ConnectToRegion("us");
        }
        if (regionSelected == 2)
        {
            PhotonNetwork.ConnectToRegion("usw");
        }
        if (regionSelected == 3)
        {
            PhotonNetwork.ConnectToRegion("au");
        }
        if (regionSelected == 4)
        {
            PhotonNetwork.ConnectToRegion("eu");
        }
        if (regionSelected == 5)
        {
            PhotonNetwork.ConnectToRegion("sa");
        }
    }

	public void Connect()
    {
        // Check if we are connected or not, join if we are, else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first connect to the Photon Online Server.
            PhotonNetwork.GameVersion = "0";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    //Start and Finish Search Game
    public void StartSearchGame()
    {
        if (GameData.playerResources.tournamentTicket<=0)
        {
            Singleton<Popup>.Instance.ShowToastMessage("not enough Tickets!", ToastLength.Normal);
            return;
        }
		if (!characterReady)
		{
            Singleton<Popup>.Instance.ShowToastMessage("character selected is locked!", ToastLength.Normal);
            return;
        }
		if (!weaponReady)
		{
            Singleton<Popup>.Instance.ShowToastMessage("weapon selected is locked!", ToastLength.Normal);
            return;
        }
        if (!PhotonNetwork.IsConnected)
        {
            //LocalUserMessage("Photon server is Offline or yput connection is offline");
            Singleton<Popup>.Instance.ShowToastMessage("You no have a connection", ToastLength.Normal);
        }
        else if (PhotonNetwork.IsConnected)
        {
            if (!canSearchGame)
            {
                //LocalUserMessage("Wait a little Moment for join in another room");
                Singleton<Popup>.Instance.ShowToastMessage("Wait a little Moment for join in another room", ToastLength.Normal);
            }
            else
            {
                startSearchButton.SetActive(false);
                stopSearchButton.SetActive(false);
                time = 10;
                StartCoroutine(SearchingGame());
            }
        } 
    }
    public void StopSearchGame()
    {
        startSearchButton.SetActive(true);
        stopSearchButton.SetActive(false);

        StopAllCoroutines();
        time = 10;

        //Refresh time UI
        timeSearchingGame.text = "";
    }
    IEnumerator SearchingGame()
    {
        yield return new WaitForSeconds(1);

        time--;

        //Refresh time UI
        timeSearchingGame.text = "Time : "+time+" Seconds";

        //Try join a room
        JoinRandomRoom();
    }

    public void JoinRandomTestRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom(null, 0);
            //PhotonNetwork.JoinRoom("roomName");

             //If status text exists, then display info
            if (statusText)
            {
                statusText.text = "Try join a random Room";
            }
        }
    }

    public void JoinRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
            //PhotonNetwork.JoinRandomRoom(null, 0);
            //PhotonNetwork.JoinRoom("roomName");

             //If status text exists, then display info
            if (statusText)
            {
                statusText.text = "Try join a random Room";
            }
        }
    }
    public void JoinCustomRoom(string roomName)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(roomName);

             //If status text exists, then display info
            if (statusText)
            {
                statusText.text = "Try join a random Room";
            }
        }
    }
    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {

            //Erase custom room
            //customRoomName = ControllerFirebase.controllerFirebase.savedUser.DisplayName;

            //GameObject.FindGameObjectWithTag("Progress").GetComponent<Progress>().customRoom = customRoomName;

            //RoomOptions newOptions = new RoomOptions();
            //newOptions.IsOpen = true;
            //newOptions.IsVisible = true;

            //PhotonNetwork.CreateRoom(null, newOptions);

            string roomName = "Room " + Random.Range(1000, 10000);

            RoomOptions options = new RoomOptions { MaxPlayers = 4, PlayerTtl = 60,  };

            PhotonNetwork.CreateRoom(roomName, options, null);

            //Erase custom room
            customRoomName = "";

            //If status text exists, then display info
            if (statusText)
            {
                statusText.text = "Creating Room";
            }

            lobbyScreen.SetActive(true);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("****Master Client changed!****");
        //Comprobate bots authority again
        for (int i = 0; i < Mp_GameController.instance.bots.Count; i++)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Mp_GameController.instance.bots[i].GetComponent<Rigidbody2D>().isKinematic = false;
            }
            else
            {
                Mp_GameController.instance.bots[i].GetComponent<Rigidbody2D>().isKinematic = true;
            }
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (isGamingScene)
        {
			//GameOptions.instance.EndLevel(false);

			
        }
        if (lobby)
        {
            if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
            {
                StartCoroutine(PlayerLeftLobby(otherPlayer.NickName));
            }
        }
    }
    IEnumerator PlayerLeftLobby(string playerName)
    {
        yield return new WaitForSeconds(3);

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
            {
                lobby.pv.RPC("UnregisterPlayer", RpcTarget.All, playerName);

                //Enable to start 
                lobby.startGameButton.SetActive(true);
            }
        }
    }

    public void CreateCustomRoom(int levelNumber)
    {
        if (PhotonNetwork.IsConnected)
        {
            //customRoomName = ControllerFirebase.controllerFirebase.savedUser.DisplayName +" Room";

            //GameObject.FindGameObjectWithTag("Progress").GetComponent<Progress>().customRoom = customRoomName;

            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 4;

            PhotonNetwork.CreateRoom(customRoomName, options);

            photonLevelNumber = levelNumber;

            //If status text exists, then display info
            if (statusText)
            {
                statusText.text = "Creating Room";
            }
        }
        else
        {
            Debug.Log("Disconnected, unable to create room");
            //Photon is not connect message
            //LocalUserMessage("Photon is Disconnected, please check your internet conection.");
            Singleton<Popup>.Instance.ShowToastMessage("Disconnected, unable to create room", ToastLength.Normal);
        }
    }

    void LocalUserMessage(string message)
    {

		if (messageScreen)
		{
            //Play Notification audio
            notifySound.Play();

            messageScreen.GetComponent<Animator>().SetTrigger("SendMessage");
            messageText.text = message;
        }
       
    }

    public void CloseCurrentRoom() 
    {
        //PhotonNetwork.CurrentRoom.IsOpen = false;
        Debug.Log("Closing room");
    }

    //Callbacks
	public void JoinRoomOrCreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            //PhotonNetwork.LocalPlayer.NickName = "playerName"; //1
            Debug.Log("PhotonNetwork.IsConnected! | Trying to Create/Join Room ");
            RoomOptions roomOptions = new RoomOptions(); //2
            TypedLobby typedLobby = new TypedLobby("roomName", LobbyType.Default); //3
            PhotonNetwork.JoinOrCreateRoom("roomName", roomOptions, typedLobby); //4
        }
    }

    #region MonoBehaviourPunCallbacks Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("PUN Launcher: OnConnectedToMaster() was called by PUN");

        //If status text exists, then display info
        if (statusText)
        {
            statusText.text = "Conected to Photon";
        }

        if (startSearchButton)
		    startSearchButton.SetActive(true);

        //Delete extra chars
        // string value = PhotonNetwork.CloudRegion;
        // value = value.Substring(0, value.Length - 2);

        if (regionConnectedText)
            regionConnectedText.text = PhotonNetwork.CloudRegion +" Region";
        if (timeSearchingGame)
            timeSearchingGame.text = "";

        if (noInternetMessage)
        {
            noInternetMessage.SetActive(false);
        }

        //ChangeRegion(4);

        //if (!PhotonNetwork.InLobby)
        //{
        //    PhotonNetwork.JoinLobby();
        //}

        connectedToPhoton = true;
    }

    public override void OnLeftLobby()
    {
        Debug.Log("On Left Lobby");

        //Reseting Lobby if exist
        if (lobby)
        {
            lobby.ResetLobby();
        }
    }
    public override void OnLeftRoom()
    {
        Debug.Log("On Left Room");

		if (disconnectedFaul)
        {
            disconnectedFaul = false;
            return;
		}
		//Wait time for join in another room
		if (SceneManager.GetActiveScene().buildIndex != 2)
		{
			Debug.Log("Returning to Main Menu");
			StartCoroutine(Mp_GameController.instance.ReturnToMainMenu()) ;
		}
		else
		{
			StartCoroutine(RenewJoinGame());
		}

    }
    public void PhotonReconection()
    {
        StartCoroutine(TryMasterReconection());
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("PUN Launcher: OnDisconnected() was called by PUN with reason :" + cause);
// Bad Ping Code
		if (isGamingScene)
		{
            Mp_playerSettings.instance.kicked = true;
            Mp_playerSettings.instance.cause = "Try connecting to Wi-Fi for better experience";
            disconnectedFaul = true;
        }

        //If status text exists, then display info
        if (statusText)
        {
            statusText.text = "Disconnected";
        }

        if (isGamingScene)
        {
            StartCoroutine(TryGameReconection());
            return;
        }
        else
        {
            StartCoroutine(TryMasterReconection());
        }


		for (int i = 0; i < multiplayerMainMenues.Length; i++)
		{
            multiplayerMainMenues[i].SetActive(false);

        }

		if (noInternetMessage)
		{
            noInternetMessage.SetActive(true);
        }
       

        connectedToPhoton = false;

        SoundManager.Instance.ChangeMultiplayerMenuMusic(1);
    }

    IEnumerator TryMasterReconection() 
    {
        //If status text exists, then display info
        if (statusText)
        {
            timeSearchingGame.text = "Try reconnecting photon";
        }

        //regionConnectedText.text = "Try reconnecting photon";

        yield return new WaitForSeconds(3);

        PhotonNetwork.Reconnect();
    }
    IEnumerator TryGameReconection()
    {
        //If status text exists, then display info
        if (statusText)
        {
            statusText.text = "Try reconnecting photon";
        }

        LocalUserMessage("Conection lost, try reconnecting...");

        yield return new WaitForSeconds(3);

        PhotonNetwork.ReconnectAndRejoin();
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        if (isGamingScene)
        {
            StartCoroutine(TryGameReconection());
        }
    }
    #endregion



    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Launcher: OnJoinRandomFailed() was called by PUN. No random room available");
        Debug.Log("Message : " + message);
        Debug.Log("ReturnCode : " + returnCode);

        if (time>=7)
        {
            stopSearchButton.SetActive(true);
            startSearchButton.SetActive(false);
        }
        if (time<=0)
        {
            CreateRoom();
            StopAllCoroutines();
            stopSearchButton.SetActive(false);
            startSearchButton.SetActive(true);

            timeSearchingGame.text = "";
        }
        else
        {
            StartCoroutine(SearchingGame());
        }

        //If status text exists, then display info
        if (statusText)
        {
            statusText.text = "SEARCHING FOR PLAYERS...";
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("PUN Launcher:  OnJoinedRoom() called by PUN. Now this client is in a room.");

        //If this user created a room change scene
        if (photonLevelNumber != 0)
        {
            PhotonNetwork.LoadLevel(photonLevelNumber);
        }

        //Reset buttons
        time = 10;

        //Refresh time UI
        timeSearchingGame.text = "Time : " + time + " Seconds";

        stopSearchButton.SetActive(false);
        startSearchButton.SetActive(true);

        //Dissable Rejoin in other Room
        canSearchGame = false;

        //If status text exists, then display info
        if (statusText)
        {
            statusText.text = "Joined a random room, changing scene...";
        }

        //Activate Lobby screen
		if (Launcher.launcher.lobbyScreen)
		{
			Launcher.launcher.lobbyScreen.SetActive(true);
		}

        //if have a lobby, register this player in lobby
        if (lobby)
        {
            lobby.StartRegister();

            if (PhotonNetwork.IsMasterClient)
            {
                //lobby.startGameButton.SetActive(true);
                
            }
            else
            {
                //Enable to start 
                //lobby.startGameButton.SetActive(false);
            }
        }

        //PhotonNetwork.LoadLevel(1);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined in lobby");
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
        
    }
    public void LeaveRoom() 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //GetComponent<GameManager>().KickAllPlayers();
        }
        PhotonNetwork.LeaveRoom();

        Invoke("ReturnToMain", 1);
    }
    void ReturnToMain() 
    {
        SceneManager.LoadScene(0);
    }
    //Change Name
    public void ChangeName()
    {
        PhotonNetwork.NickName = nameInputField.text;
    }

    IEnumerator RenewJoinGame()
    {
        yield return new WaitForSeconds(3);
        canSearchGame = true;        
    }

    public void OpenMultiplayerMenu() 
    {
		if (connectedToPhoton)
		{
            Debug.Log("Nik Photon Connected");
            multiplayerMainMenu.SetActive(true);
		}
		else
		{
            Debug.Log("Nik No Internet");
            noInternetMessage.SetActive(true);
        }
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    public void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            Debug.Log( "" + info.Name + "   " + (byte)info.PlayerCount + "   " + info.MaxPlayers);
        }
    }
}
