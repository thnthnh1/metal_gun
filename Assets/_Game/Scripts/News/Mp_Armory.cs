/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mp_Armory : MonoBehaviour
{
	public static Mp_Armory instance;
	public string currentWeapon;

	public ProfileManager profile;

	[Header("Character Screen LevelUp")]
	public GameObject[] charactersButtons;

	[Header("Characters Upgrades costs")]
	public int[] upgradeCosts;

	public GameObject UpgradeButton;
	public GameObject LockedMessage;

	[Header("Preview Char")]
	public string currentCharacter;
	public int currentCharacterlevel;
	public GameObject[] characters;
	public GameObject[] charactersHeads;
	public GameObject previewCam;
	public GameObject previewTexture;
	public Text attributes;

	[Header("Stars Level")]
	public GameObject[] stars;

	[Header("Name Preview")]
	public Text namePreview;

	[Header("CharactersToUnlock")]
	public UnlockeablesCharacters[] unlokeablesChars;
	public Button[] unlockButtons;
	public Button purchaseCharButton;
	public Text charPrice;
	public int currentCharUnlockedList;

	[Header("Preview Weapons in Multiplayer")]
	public GameObject[] previewWeaponInUI;

	[Header("Weapons and Weapons Selector")]
	private int currentCharacterSelectedUI;
	private int currentWeaponSelectedUI;
	public Text characterUnlockText;
	public Text characterUnlockText2;
	public Text weaponUnlockText;

	[Header("Weapons and Characters Selector")]
	public Button [] selectorsButtons;

	[Header("Tour Tickets")]
	public Text tourTicket;
	public GameObject tourTicketButton;

	[Header("Tutorial Item")]
	public GameObject Tutorial;
	public List<GameObject> AfterTutorial = new List<GameObject>();

	[Header("Notify Tutorial Item")]
	public GameObject NotifyTutorial;
	public GameObject OtherTutorial;


	private Image[] images;
	// Start is called before the first frame update
	void Awake()
	{
		instance = this;

		currentWeapon = ProfileManager.UserProfile.gunNormalId.ToString();

		Invoke("RefreshTickets", 1);
		Invoke("RefreshCharactersButtons", 1);
		Invoke("RefreshRifleWeaponMP", 1);
		Invoke("LoadUnlockStatus", 1);

		SoundManager.Instance.GetComponent<AudioListener>().enabled = true;

		//GameData.playerResources.ReceiveGem(999999);
		//GameData.playerResources.ReceiveCoin(999999);

		SoundManager.Instance.PlayMusic("music_menu");

		for (int i = 0; i < AfterTutorial.Count; i++)
		{
			AfterTutorial[i].SetActive(false);
		}
		Tutorial.SetActive(false);
	}

	void Start() 
	{
		if (PlayerPrefs.GetInt("NotifyTutorial") == 0)
		{
			NotifyTutorial.SetActive(true);
			OtherTutorial.SetActive(false);
		}
	}

	public void AfterTutorial01()
	{
		AfterTutorial[0].SetActive(false);
		AfterTutorial[1].SetActive(true);
	}
	public void AfterTutorial02()
	{
		AfterTutorial[1].SetActive(false);
		AfterTutorial[2].SetActive(true);
	}
	public void AfterTutorial03()
	{
		AfterTutorial[2].SetActive(false);
		PlayerPrefs.SetInt("AfterTutotial", 1);
		PlayerPrefs.Save();
	}

	public void RefreshTickets()
	{
		if (GameData.playerResources.tournamentTicket >= 5)
		{
			//GameData.playerResources.tournamentTicket = 5;
			tourTicketButton.SetActive(false);
		}
		else
		{
			tourTicketButton.SetActive(true);
		}

		tourTicket.text = GameData.playerResources.tournamentTicket + " / " + 5;
	}

	public void RefreshRifleWeaponMP()
	{
		currentWeapon = ProfileManager.UserProfile.gunNormalId.ToString();

		SelectWeaponInUI(ProfileManager.UserProfile.gunNormalId);

		weaponUnlockText.text = "UNLOCKED";

		//Send weapon Selected to plater settings
		Mp_playerSettings.instance.weaponSelected = ProfileManager.UserProfile.gunNormalId;

		PreviewCharacter(Mp_playerSettings.instance.characterSelected);
	}

	public void ExitCharacterUpgrade()
	{
		UpgradeButton.SetActive(false);
		previewTexture.SetActive(false);

		attributes.text = "";
	}

	public void PreviewCharacter(string charName)
	{
		UpgradeButton.SetActive(true);
		LockedMessage.SetActive(false);
		previewTexture.SetActive(true);

		Launcher.launcher.characterReady = true;

		characterUnlockText.text = "UNLOCKED";
		characterUnlockText2.text = "UNLOCKED";

		currentCharacter = charName;
		currentCharacterlevel = Mp_playerSettings.instance.GetCharacterLevel(charName);

		//Set char name
		namePreview.text = charName;

		attributes.text = "" + Mp_playerSettings.instance.GetCharacterHealth(charName);

		//Reset char purchaseButton
		purchaseCharButton.enabled = false;
		charPrice.text = "UNLOCKED";



		//Get CharLevel
		int charLevel = Mp_playerSettings.instance.GetCharacterLevel(charName);
		for (int i = 0; i < stars.Length; i++)
		{
			stars[i].SetActive(false);
		}
		for (int i = 0; i < charLevel; i++)
		{
			stars[i].SetActive(true);
		}

		//Set price
		if (currentCharacterlevel < 10)
		{
			UpgradeButton.transform.Find("Price Upgrade").GetComponent<Text>().text = upgradeCosts[currentCharacterlevel].ToString();
		}
		else
		{
			UpgradeButton.transform.Find("Price Upgrade").GetComponent<Text>().text = "MAXED!";
		}

		for (int i = 0; i < characters.Length; i++)
		{
			if (characters[i].name == charName)
			{
				for (int e = 0; e < unlokeablesChars.Length; e++) //Comprobate if player is Unlock
				{
					if (i == unlokeablesChars[e].id)
					{
						string unlockStatus = PlayerPrefs.GetString("CharUnlockStatus" + e);
						if (unlockStatus == "Locked")
						{
							Debug.Log("Locked Char!");
							UpgradeButton.SetActive(false);
							LockedMessage.SetActive(true);

							characterUnlockText.text = "LOCKED";
							characterUnlockText2.text = "LOCKED";

							characters[i].SetActive(true);

							Launcher.launcher.characterReady = false;

							//Enable for purchase
							currentCharUnlockedList = e;
							purchaseCharButton.enabled = true;
							charPrice.text = "" + unlokeablesChars[e].cost;

							break;
						}
						else if (PlayerPrefs.GetString("CharUnlockStatus" + e) == "Unlocked")
						{
							Debug.Log("Unlocked Char!");
							Mp_playerSettings.instance.characterSelected = charName;
							PlayerPrefs.SetString("CharacterSelected", charName);

							

							characters[i].SetActive(true);

							break;
						}
					}
				}

				characters[i].SetActive(true);
			}
			else
			{
				characters[i].SetActive(false);
			}
		}

		ShowHead(charName);

		Mp_playerSettings.instance.characterSelected = charName;
		PlayerPrefs.SetString("CharacterSelected", charName);

		if (charName == "")
		{
			characters[0].SetActive(true);
		}
	}

	public void ShowHead(string name) 
	{
		for (int i = 0; i < charactersHeads.Length; i++)
		{
			if (charactersHeads[i].name == name)
			{
				charactersHeads[i].SetActive(true);
			}
			else
			{
				charactersHeads[i].SetActive(false);
			}
		}
	}

	public void UpgradeCharacter()
	{
		if (currentCharacterlevel < 10)
		{
			if (GameData.playerResources.coin < upgradeCosts[currentCharacterlevel])
			{
				Singleton<Popup>.Instance.ShowToastMessage("not enough coins", ToastLength.Normal);
				ShopIAP.Instance.enoughCoinScreen.SetActive(true);
				ShopIAP.Instance.ChangeStatusObjects(false);
				SoundManager.Instance.PlaySfxClick();
			}
			else
			{
				GameData.playerResources.ConsumeCoin(upgradeCosts[currentCharacterlevel]);
				SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);

				//UpgradeCharacter
				for (int i = 0; i < Mp_playerSettings.instance.characters.Length; i++)
				{
					if (Mp_playerSettings.instance.characters[i].characterName == currentCharacter)
					{
						Mp_playerSettings.instance.characters[i].level++;

						Mp_playerSettings.instance.characters[i].health += Mp_playerSettings.instance.characters[i].healthPerLevel;

						//RefreshCharactersButtons();
						PreviewCharacter(currentCharacter);

						Mp_playerSettings.instance.SaveMpCharacterMpData(currentCharacter, Mp_playerSettings.instance.characters[i].level);
						break;
					}
				}
			}
		}
		
	}

	public void PurchaseCharWithGems()
	{
		if (GameData.playerResources.gem < unlokeablesChars[currentCharUnlockedList].cost)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough gems", ToastLength.Normal);
			SoundManager.Instance.PlaySfxClick();
		}
		else
		{
			GameData.playerResources.ConsumeGem(5000);
			SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);

			//unlockButtons[currentCharUnlockedList].interactable = false;

			SaveUnlockStatus(currentCharUnlockedList, "Unlocked");

			//Reset char purchaseButton
			purchaseCharButton.enabled = false;
			charPrice.text = "UNLOCKED";

			UpgradeButton.SetActive(true);
			LockedMessage.SetActive(false);
		}
	}
	void LoadUnlockStatus()
	{
		if (PlayerPrefs.GetString("CharUnlockStatus0") == "")
		{
			Debug.Log("No Unlock Save!****");

			for (int i = 0; i < unlokeablesChars.Length; i++)
			{
				SaveUnlockStatus(i, "Locked");
			}
			return;
		}

		for (int i = 0; i < unlokeablesChars.Length; i++)
		{
			if (PlayerPrefs.GetString("CharUnlockStatus" + i) == "Locked")
			{
				unlockButtons[i].interactable = true;
			}
			else
			{
				unlockButtons[i].interactable = false;
			}
		}
	}

	void SaveUnlockStatus(int id, string status)
	{
		PlayerPrefs.SetString("CharUnlockStatus" + id, status);

		Debug.Log("Saving char unlock " + id + " " + status);
	}

	public void RightCharSelector()
	{
		currentCharacterSelectedUI++;

		if (currentCharacterSelectedUI == characters.Length)
		{
			currentCharacterSelectedUI = 0;
		}

		PreviewCharacter(characters[currentCharacterSelectedUI].name);
	}
	public void LeftCharSelector()
	{
		currentCharacterSelectedUI--;

		if (currentCharacterSelectedUI < 0)
		{
			currentCharacterSelectedUI = characters.Length - 1;
		}

		PreviewCharacter(characters[currentCharacterSelectedUI].name);
	}
	public void RightWeaponSelector()
	{
		currentWeaponSelectedUI++;

		if (currentWeaponSelectedUI == previewWeaponInUI.Length)
		{
			currentWeaponSelectedUI = 0;
		}

		SelectWeaponInUI(currentWeaponSelectedUI);
	}
	public void LeftWeaponSelector()
	{
		currentWeaponSelectedUI--;

		if (currentWeaponSelectedUI < 0)
		{
			currentWeaponSelectedUI = previewWeaponInUI.Length - 1;
		}

		SelectWeaponInUI(currentWeaponSelectedUI);
	}

	public void SelectWeaponInUI(int weapon)
	{
		for (int i = 0; i < previewWeaponInUI.Length; i++)
		{
			if (i == weapon)
			{
				previewWeaponInUI[i].SetActive(true);
			}
			else
			{
				previewWeaponInUI[i].SetActive(false);
			}
		}

		if (GameData.playerGuns.ContainsKey(weapon))
		{
			weaponUnlockText.text = "UNLOCKED";
			Mp_playerSettings.instance.weaponSelected = weapon;

			Launcher.launcher.weaponReady = true;
		}
		else
		{
			weaponUnlockText.text = "LOCKED";

			Launcher.launcher.weaponReady = false;
		}
	}

	public void DissableSelectorsButtons() 
	{
		for (int i = 0; i < selectorsButtons.Length; i++)
		{
			selectorsButtons[i].enabled = false;
		}
	}
	public void EnableSelectorsButtons() 
	{
		for (int i = 0; i < selectorsButtons.Length; i++)
		{
			selectorsButtons[i].enabled = true;
		}
	}

	[System.Serializable]
	public class UnlockeablesCharacters
	{
		public string name;
		public int id;
		public int cost;
	}
}
*/