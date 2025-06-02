/*using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// using Facebook.Unity;

public class DisplayHighscores : MonoBehaviour 
{
	public static DisplayHighscores instance;

	public GameObject[] highscoreFields; 
	Highscores highscoresManager;

	[Header("Get Current Player score")]
	public string playerName;
	public int slotNumber;
	public int score;

	public Text currentRank;
	public Text currentTrophy;
	public Text currentTrophy2;

	public int firebaseUserCount;

	void Start() 
	{
		instance = this;

		UpdateHighscores();
	}

	public void UpdateHighscores() 
	{
		for (int i = 0; i < highscoreFields.Length; i++)
		{
			highscoreFields[i].transform.Find("NameText").GetComponent<Text>().text = "Fetching...";

			highscoreFields[i].transform.Find("Mask").transform.Find("Trophy").transform.Find("Trophy").transform.Find("Text").GetComponent<Text>().text = "";

			if (i > 2)
			{
				int newNumber = i + 1;
				highscoreFields[i].transform.Find("NumberText").GetComponent<Text>().text = "" + newNumber;
			}

		}

		highscoresManager = GetComponent<Highscores>();
		// StartCoroutine("RefreshHighscores");

		Invoke(nameof(GetPlayerScore), 3);
	}
	
	public void OnHighscoresDownloaded(Highscore[] highscoreList) 
	{
		firebaseUserCount = 0;

		if (highscoreList.Length>0)
		{
			// if (!FB.IsLoggedIn)
			// {
			// 	// FacebookSDK Remove
			// 	FbController.Instance.GetFriendProfileForHighscore(highscoreList[firebaseUserCount].username);
			// }
			Debug.Log("<color=blue>Change The PhotoURl</color>");

			// FireBase Remove
			ControllerFirebase.controllerFirebase.RetrieveFaceBookInfoInDatabase(highscoreList[firebaseUserCount].username);
		}
	}

	public void GetFirebaseInfo() 
	{
		// FireBase Remove
		highscoreFields[firebaseUserCount].transform.Find("NameText").GetComponent<Text>().text = ControllerFirebase.controllerFirebase.remoteUser.facebookName;
		highscoreFields[firebaseUserCount].transform.Find("Mask").transform.Find("Trophy").transform.Find("Trophy").transform.Find("Text").GetComponent<Text>().text = "" + Highscores.instance.highscoresList[firebaseUserCount].score;

		// Debug.Log("Nik Check : " + ControllerFirebase.controllerFirebase.remoteUser.facebookName +  " : "+ Highscores.instance.highscoresList[firebaseUserCount].score);

		ControllerFirebase.controllerFirebase.remoteUser.facebookName = "";

		// FireBase Remove
		StartCoroutine(DownloadPhoto(ControllerFirebase.controllerFirebase.remoteUser.facebookPhoto));
		// StartCoroutine(DownloadPhoto(ControllerFirebase.controllerFirebase.remoteUser.facebookPhoto));


		// firebaseUserCount++;
		// Debug.Log("Nik check " + firebaseUserCount + " < " + Highscores.instance.highscoresList.Length);
		// if (firebaseUserCount < Highscores.instance.highscoresList.Length)
		// {
		// 	// FireBase Remove
			
		// 	Debug.Log("Going in RetrieveFaceBookInfoInDatabase " + firebaseUserCount);
		// 	ControllerFirebase.controllerFirebase.RetrieveFaceBookInfoInDatabase(Highscores.instance.highscoresList[firebaseUserCount].username);
		// }
		// else
		// {
		// 	// Loop Over

		// }
	}
	public IEnumerator DownloadPhoto(string url) 
	{
		if (url != "")
		{
			WWW www = new WWW(url);
			yield return www;
			highscoreFields[firebaseUserCount].transform.Find("FbPic").GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
		}
		// WWW www = new WWW(url);
		// yield return www;
		// highscoreFields[firebaseUserCount].transform.Find("FbPic").GetComponent<Image>().sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));

		firebaseUserCount++;

		// Debug.Log("Nik check " + firebaseUserCount + " < " + Highscores.instance.highscoresList.Length);
		if (firebaseUserCount < Highscores.instance.highscoresList.Length)
		{
			// FireBase Remove
			// Debug.Log("Going in RetrieveFaceBookInfoInDatabase");
			ControllerFirebase.controllerFirebase.RetrieveFaceBookInfoInDatabase(Highscores.instance.highscoresList[firebaseUserCount].username);
		}
	}

	IEnumerator RefreshHighscores()
	 {
		while (true) 
		{
			highscoresManager.DownloadHighscores();
			yield return new WaitForSeconds(30);
		}
	}

	public void GetPlayerScore()
	{
		if (Mp_playerSettings.instance)
		{
			playerName = Mp_playerSettings.instance.facebookID;

			currentTrophy.text = Mp_playerSettings.instance.trophys.ToString();
			currentTrophy2.text = Mp_playerSettings.instance.trophys.ToString();

			StartCoroutine(highscoresManager.GetUserHighscore(playerName));
		}
	}
}
*/