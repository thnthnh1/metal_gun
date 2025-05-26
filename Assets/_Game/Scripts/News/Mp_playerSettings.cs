using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mp_playerSettings : MonoBehaviour
{
	public static Mp_playerSettings instance;

	[Header("Player Info")]
	public string playerName;
	public int playerLevel;
	public float playerExp;
	public int trophys;

	[Header("Facebook Info")]
	public string facebookID;
	public string facebookName;
	public string facebookEmail;
	public string facebookPhotoURL;

	[Header("Google Info")]
	public string googleID;
	public string googleName;
	public string googleEmail;
	public string googlePhotoURL;

	[Header("Characters Info")]
	public Character [] characters;

	[Header("Selections")]
	public string characterSelected;
	public int weaponSelected;

	[Header("Name of Bots")]
	public List<string> botsNames;

	[Header("Player have kicked")]
	public bool kicked;
	public string cause;

	[Header("Battle reward")]
	public int trophy;
	public int coins;

	public bool startMainMenuInMultiplayerScreen;
	// Start is called before the first frame update
	void Awake()
    {
		if (instance)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;

			DontDestroyOnLoad(gameObject);
		}

		LoadAllMpCharacters();

		StartCoroutine(ComprobateScene());
	}

    public void LoadPlayerData()
    {
		playerName = PlayerPrefs.GetString("PlayerName");
		playerLevel = PlayerPrefs.GetInt("PlayerLevel");
		playerExp = PlayerPrefs.GetFloat("PlayerExp");
		trophys = PlayerPrefs.GetInt("Trophys");

		characterSelected = PlayerPrefs.GetString("CharacterSelected");

		facebookID = PlayerPrefs.GetString("FacebookID");
		facebookName = PlayerPrefs.GetString("FacebookName");
		facebookEmail = PlayerPrefs.GetString("FacebookEmail");
		facebookPhotoURL = PlayerPrefs.GetString("FacebookPhotoURL");
	}

	public void SavePlayerData(string name, int level, float exp)
	{
		Debug.Log("Saving Player Data");
		PlayerPrefs.SetString("PlayerName", name);
		PlayerPrefs.SetInt("PlayerLevel", level);
		PlayerPrefs.SetFloat("PlayerExp", exp);

		Invoke(nameof(LoadPlayerData), 0.25f);
	}

	public void SaveFacebookInfo() 
	{
		Debug.Log("Saving Facebook Player Data");
		PlayerPrefs.SetString("FacebookID", facebookID);
		PlayerPrefs.SetString("FacebookName", facebookName);
		PlayerPrefs.SetString("FacebookEmail", facebookEmail);
		PlayerPrefs.SetString("FacebookPhotoURL", facebookPhotoURL);

		Mp_playerSettings.instance.SavePlayerData(facebookName, 1, 0);
	}

	public void LoadAllMpCharacters()
	{
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].level = PlayerPrefs.GetInt(characters[i].characterName);

			for (int e = 0; e < characters[i].level; e++)
			{
				characters[i].health += characters[i].healthPerLevel;
			}
		}
	}

	public void SaveMpCharacterMpData(string charName, int level)
	{
		PlayerPrefs.SetInt(charName, level);
	}

	public int GetCharacterLevel(string charName)
	{
		for (int i = 0; i < characters.Length; i++)
		{
			if (charName == characters[i].characterName)
			{
				return characters[i].level;
			}
		}

		return 0;
	}

	public int GetCharacterHealth(string charName)
	{
		for (int i = 0; i < characters.Length; i++)
		{
			if (charName == characters[i].characterName)
			{
				return characters[i].health;
			}
		}

		return 0;
	}
// Bad Ping Code
	IEnumerator ComprobateScene() 
	{
		yield return new WaitForSeconds(1);

		if (SceneManager.GetActiveScene().name == "Menu")
		{
			if (kicked)
			{
				//Singleton<Popup>.Instance.ShowToastMessage("You are kicked!, Cause : ", ToastLength.Normal);
// Bad Ping Code call
				Singleton<Popup>.Instance.Show(cause, "Bad ping!", PopupType.Ok, null, null);

				kicked = false;
			}
		}

		StartCoroutine(ComprobateScene());
	}

	[System.Serializable]
	public class Character
	{
		public string characterName;
		public int level;
		public int health;
		public int healthPerLevel;
	}
}
