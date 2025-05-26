using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mp_PlayerInfoScreen : MonoBehaviour
{
	public static Mp_PlayerInfoScreen instance;
	[Header ("Player Info")]
	public Text playerNameText;
	public Text levelText;
	public Slider levelSlider;
	public Text playerExpText;

	[Header("Firts Name Screen")]
	public GameObject loginScreen;
	public InputField firtsNameInput;
	// Start is called before the first frame update

	[Header("Tournament Screen")]
	public GameObject tournamentScreen;

	void Start()
    {
		instance = this;
		Mp_playerSettings.instance.LoadPlayerData();
		Invoke("LoadData", 1);
    }

    // Update is called once per frame
    public void LoadData()
    {
		if (Mp_playerSettings.instance.playerName != "")
		{
			playerNameText.text = Mp_playerSettings.instance.playerName;
			levelText.text = ""+ Mp_playerSettings.instance.playerLevel;
			levelSlider.maxValue = 200;
			levelSlider.value = Mp_playerSettings.instance.playerExp;
			playerExpText.text = Mp_playerSettings.instance.playerExp + " / " + "200";

			//Load Fb Image
			StartCoroutine(LoadImage(Mp_playerSettings.instance.facebookPhotoURL));
		}
    }

	public IEnumerator LoadImage(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		HudTournamentRanking.instance.imgPlayerAvatar.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	}

	public void TryOpenTornamentScreen() 
	{
		if (Mp_playerSettings.instance.playerName != "")
		{
			//tournamentScreen.SetActive(true);
			Launcher.launcher.OpenMultiplayerMenu();

			SoundManager.Instance.ChangeMultiplayerMenuMusic(0);
		}
		else
		{
			loginScreen.SetActive(true);
		}
	}

	public void ResetMainMenuMusic() 
	{
		SoundManager.Instance.ChangeMultiplayerMenuMusic(1);
	}

	//public void SetFirtsData()
	//{
	//	Mp_playerSettings.instance.SavePlayerData(firtsNameInput.text, 1, 0);

	//	Invoke("LoadData", 1);
	//}
}
