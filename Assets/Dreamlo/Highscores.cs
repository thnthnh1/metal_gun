/*using UnityEngine;
using System.Collections;

public class Highscores : MonoBehaviour {

	//Dreamlo Page http://dreamlo.com/lb/4xTwOMiW50SpYl-FjyN9fANk1QKGl9YUGIw6fhHwbrew

	const string privateCode = "4xTwOMiW50SpYl-FjyN9fANk1QKGl9YUGIw6fhHwbrew";
	const string publicCode = "6035c78d8f40bb299cd6cfda";
	const string webURL = "http://dreamlo.com/lb/";

	DisplayHighscores highscoreDisplay;
	public Highscore[] highscoresList;
	public static Highscores instance;
	public bool loaded;

	void Awake() {

		if (GetComponent<DisplayHighscores>())
		{
			highscoreDisplay = GetComponent<DisplayHighscores>();
		}
		
		instance = this;

		Invoke(nameof(DownloadHighscores), 10.5f);
	}

	public void AddNewHighscore(string username, int score) 
	{
		instance.StartCoroutine(instance.UploadNewHighscore(username,score));
	}

	IEnumerator UploadNewHighscore(string username, int score) 
	{
		WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
		string URL = webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score;

		// Debug.Log("URL Print : " + URL);
		yield return www;

		if (string.IsNullOrEmpty(www.error)) 
		{
			print ("Upload Successful");

			if (highscoreDisplay)
			{
				DownloadHighscores();
			}
			
		}
		else 
		{
			print ("Error uploading: " + www.error);
		}
	}

	public void DownloadHighscores() 
	{
		StartCoroutine("DownloadHighscoresFromDatabase");
	}

	IEnumerator DownloadHighscoresFromDatabase() 
	{
		WWW www = new WWW(webURL + publicCode + "/pipe/");
		// Debug.Log("Download Data url : " + webURL + publicCode + "/pipe/");

		yield return www;
		
		if (string.IsNullOrEmpty (www.error)) 
		{
			FormatHighscores (www.text);
			Debug.Log("Data : " + www.text);
			Debug.Log("<color=red>check Which Sign In</color>");
			highscoreDisplay.OnHighscoresDownloaded(highscoresList);
			loaded=true;
		}
		else 
		{
			print ("Error Downloading: " + www.error);
			loaded=false;
		}
	}

	public IEnumerator GetUserHighscore(string username)
	{
		//WWW www = new WWW("http://dreamlo.com/lb/6035c78d8f40bb299cd6cfda/pipe-get/" + username);

		WWW www = new WWW(webURL + publicCode + "/pipe-get/" + username);
		// string Url = webURL + publicCode + "/pipe-get/" + username;

		// Debug.Log("Get HS URL : " + Url);
		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			FormatUserHighscore(www.text);
		}
		else
		{
			print("Error Downloading: " + www.error);
		}
	}

	void FormatUserHighscore(string textStream)
	{
		Debug.Log(textStream);

		if (textStream != "")
		{
			string username = "";
			int score = 0;
			int label = 0;

			string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i <entries.Length; i ++) 
			{
				string[] entryInfo = entries[i].Split(new char[] {'|'});
				// Debug.Log("LB - DataEntry Name is : " + entryInfo[0] + PlayerPrefs.GetString("FacebookID"));
				if (entryInfo[0] == PlayerPrefs.GetString("FacebookID"))
				{
					Debug.Log("LB - DataEntry Name is : " + entryInfo[0]);
					username = entryInfo[0];
					score = int.Parse(entryInfo[1]);
					label = int.Parse(entryInfo[5]);
					Mp_RewardsMenu.instance.isMe = true;
				}
			}

			DisplayHighscores.instance.playerName = username;
			DisplayHighscores.instance.score = score;
			DisplayHighscores.instance.slotNumber = label++;

			DisplayHighscores.instance.currentRank.text = label.ToString();
			DisplayHighscores.instance.currentTrophy.text = Mp_playerSettings.instance.trophys.ToString();

			Mp_RewardsMenu.instance.posInLeaderBoard = label-1;
			Mp_RewardsMenu.instance.gettedPositionInLeader = true;
			Debug.Log("Nik log is the PlayerInfo 1 : " + PlayerPrefs.GetString("FbUserInfo"));
		}
	}

	void FormatHighscores(string textStream) 
	{
		string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
		highscoresList = new Highscore[entries.Length];

		for (int i = 0; i <entries.Length; i ++) 
		{
			string[] entryInfo = entries[i].Split(new char[] {'|'});
			string username = entryInfo[0];
			int score = int.Parse(entryInfo[1]);
			highscoresList[i] = new Highscore(username,score);
		}
		Debug.Log("Nik log is the PlayerInfo 2 : " + PlayerPrefs.GetString("FbUserInfo"));
	}
	public void DeleteUsername(string username)
	{
		Debug.Log("Deleting " + username);
		StartCoroutine(DeleteHighscore(username));
	}

	public IEnumerator DeleteHighscore(string username)
	{
		WWW www = new WWW(webURL + privateCode + "/delete/" + username);
		yield return www;

		if (string.IsNullOrEmpty(www.error))
		{
			print("Highscore Reward " + username + " , Deleting User Highscore");

			DisplayHighscores.instance.UpdateHighscores();
		}
		else
		{
			print("Error Downloading: " + www.error);
		}
	}

}

[System.Serializable]
public struct Highscore 
{
	public string username;
	public int score;

	public Highscore(string _username, int _score) 
	{
		username = _username;
		score = _score;
	}

}
*/