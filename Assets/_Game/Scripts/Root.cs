using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Root : MonoBehaviour
{
	[SerializeField]
	private string nextScene;

	private List<string> EU = new List<string>
	{
		"BE",
		"BG",
		"CZ",
		"DK",
		"DE",
		"EE",
		"IE",
		"EL",
		"ES",
		"FR",
		"HR",
		"IT",
		"CY",
		"LV",
		"LT",
		"LU",
		"HU",
		"MT",
		"NL",
		"AT",
		"PL",
		"PT",
		"RO",
		"SI",
		"SK",
		"FI",
		"SE",
		"UK"
	};

	private bool isInEU;

	private void Awake()
	{
		ProfileManager.Init("nzt", "N7x9QZt2");
		Screen.sleepTimeout = -1;
		Application.targetFrameRate = 60;
		Application.runInBackground = true;
		//string region = PreciseLocale.GetRegion();
		//UnityEngine.Debug.Log("VANTV-" + region);
		//this.isInEU = this.EU.Contains(region);
	}

	private void Start()
	{
		string @string = PlayerPrefs.GetString("user_consent", string.Empty);
		if (this.isInEU && string.IsNullOrEmpty(@string))
		{
			Singleton<Popup>.Instance.ShowPrivacy(delegate
			{
				SceneManager.LoadScene(this.nextScene);
			});
		}
		else
		{
			SceneManager.LoadScene(this.nextScene);
		}
	}
}
