using Newtonsoft.Json;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
	private void Awake()
	{
		//ResetData();
	}
	private sealed class _AutoBackupData_c__AnonStorey0
	{
		internal string fbId;

		private static UnityAction<bool> __f__am_cache0;

		internal void __m__0(UserInfo authUser)
		{
			if (authUser != null)
			{
				/*
				Singleton<FireBaseDatabase>.Instance.SaveUserData(this.fbId, delegate(bool complete)
				{
				});
				*/
			}
		}

		private static void __m__1(bool complete)
		{
		}
	}

	private sealed class _ProcessBackupData_c__AnonStorey1
	{
		internal string fbId;

		private static UnityAction<bool> __f__am_cache0;

		internal void __m__0(UserInfo authUser)
		{
			if (authUser != null)
			{
				/*
				Singleton<FireBaseDatabase>.Instance.SaveUserData(this.fbId, delegate(bool complete)
				{
					if (complete)
					{
						Singleton<Popup>.Instance.ShowToastMessage("Save game data successfully", ToastLength.Long);
					}
					else
					{
						Singleton<Popup>.Instance.ShowToastMessage("Save game data error, please try again later. Sorry for this inconvinient", ToastLength.Long);
					}
				});
				*/
			}
			//else
			{
				Singleton<Popup>.Instance.ShowToastMessage("Authentication failed", ToastLength.Long);
			}
		}

		private static void __m__1(bool complete)
		{
			if (complete)
			{
				Singleton<Popup>.Instance.ShowToastMessage("Save game data successfully", ToastLength.Long);
			}
			else
			{
				Singleton<Popup>.Instance.ShowToastMessage("Save game data error, please try again later. Sorry for this inconvinient", ToastLength.Long);
			}
		}
	}

	private sealed class _ProcessRestoreData_c__AnonStorey2
	{
		internal string fbId;

		private static UnityAction<RawUserProfile> __f__am_cache0;

		internal void __m__0(UserInfo authUser)
		{
			if (authUser != null)
			{
				/*
				Singleton<FireBaseDatabase>.Instance.GetUserData(this.fbId, delegate(RawUserProfile inventory)
				{
					Singleton<Popup>.Instance.HideInstantLoading();
					if (inventory != null)
					{
						ProfileManager.Load(inventory);
						ProfileManager.SaveAll();
						SceneManager.LoadScene("Login");
					}
				});
				*/
			}
			else
			{
				Singleton<Popup>.Instance.HideInstantLoading();
				Singleton<Popup>.Instance.ShowToastMessage("Authentication failed", ToastLength.Long);
			}
		}

		private static void __m__1(RawUserProfile inventory)
		{
			Singleton<Popup>.Instance.HideInstantLoading();
			if (inventory != null)
			{
				ProfileManager.Load(inventory);
				ProfileManager.SaveAll();
				SceneManager.LoadScene("Login");
			}
		}
	}

	public Text textVersion;

	public Slider sound;

	public Slider music;

	private void OnEnable()
	{
		/*
		if (FB.IsLoggedIn)
		{
			this.textVersion.text = "ID: " + AccessToken.CurrentAccessToken.UserId;
		}
		*/
		this.textVersion.text = $"v{UtilityUnity.GameVersion()}";
		this.sound.value = ProfileManager.UserProfile.soundVolume;
		this.music.value = ProfileManager.UserProfile.musicVolume;
	}

	private void OnDisable()
	{
		ProfileManager.UserProfile.soundVolume.Set(this.sound.value);
		ProfileManager.UserProfile.musicVolume.Set(this.music.value);
		ProfileManager.SaveAll();
	}

	public void Show()
	{
		base.gameObject.SetActive(true);
	}

	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	public void OnAdjustSoundVolume()
	{
		SoundManager.Instance.AdjustSoundVolume(this.sound.value);
	}

	public void OnAdjustMusicVolume()
	{
		SoundManager.Instance.AdjustMusicVolume(this.music.value);
	}

	public void BackUpData()
	{
		this.Hide();
		Singleton<Popup>.Instance.ShowRateUs();
		// Singleton<Popup>.Instance.Show("Do you want to save current game data?", "CONFIRMATION", PopupType.YesNo, delegate
		// {
		/*
		if (FB.IsLoggedIn)
		{
			this.ProcessBackupData();
		}
		else
		{
			FbController.Instance.LoginWithReadPermission(delegate(bool success)
			{
				if (success)
				{
					this.ProcessBackupData();
				}
				else
				{
					Singleton<Popup>.Instance.ShowToastMessage("Login Facebook failed", ToastLength.Long);
				}
			});
		}
		*/
		// }, null);
	}

	public void AutoBackupData()
	{
		/*
		if (FB.IsLoggedIn)
		{
			string fbId = AccessToken.CurrentAccessToken.UserId;
			Singleton<FireBaseDatabase>.Instance.AuthenWithFacebook(fbId, AccessToken.CurrentAccessToken.TokenString, delegate(UserInfo authUser)
			{
				if (authUser != null)
				{
					Singleton<FireBaseDatabase>.Instance.SaveUserData(fbId, delegate(bool complete)
					{
					});
				}
			});
		}
		*/
	}

	public void RestoreData()
	{
		this.Hide();
		Singleton<Popup>.Instance.Show("Do you want to replace current game data with previous one?", PopupTitleID.Confirmation, PopupType.YesNo, delegate
		{
			Singleton<Popup>.Instance.ShowInstantLoading(15);
			/*
			if (FB.IsLoggedIn)
			{
				this.ProcessRestoreData();
			}
			else
			{
				FbController.Instance.LoginWithReadPermission(delegate(bool success)
				{
					if (success)
					{
						this.ProcessRestoreData();
					}
					else
					{
						Singleton<Popup>.Instance.HideInstantLoading();
						Singleton<Popup>.Instance.ShowToastMessage("Login Facebook failed", ToastLength.Long);
					}
				});
			}
			*/
		}, delegate
		{
			this.Show();
		});
	}

	private void ProcessBackupData()
	{
		/*
		string fbId = AccessToken.CurrentAccessToken.UserId;
		Singleton<FireBaseDatabase>.Instance.AuthenWithFacebook(fbId, AccessToken.CurrentAccessToken.TokenString, delegate(UserInfo authUser)
		{
			if (authUser != null)
			{
				Singleton<FireBaseDatabase>.Instance.SaveUserData(fbId, delegate(bool complete)
				{
					if (complete)
					{
						Singleton<Popup>.Instance.ShowToastMessage("Save game data successfully", ToastLength.Long);
					}
					else
					{
						Singleton<Popup>.Instance.ShowToastMessage("Save game data error, please try again later. Sorry for this inconvinient", ToastLength.Long);
					}
				});
			}
			else
			{
				Singleton<Popup>.Instance.ShowToastMessage("Authentication failed", ToastLength.Long);
			}
		});
		*/
	}

	private void ProcessRestoreData()
	{
		/*
		string fbId = AccessToken.CurrentAccessToken.UserId;
		Singleton<FireBaseDatabase>.Instance.AuthenWithFacebook(fbId, AccessToken.CurrentAccessToken.TokenString, delegate(UserInfo authUser)
		{
			if (authUser != null)
			{
				Singleton<FireBaseDatabase>.Instance.GetUserData(fbId, delegate(RawUserProfile inventory)
				{
					Singleton<Popup>.Instance.HideInstantLoading();
					if (inventory != null)
					{
						ProfileManager.Load(inventory);
						ProfileManager.SaveAll();
						SceneManager.LoadScene("Login");
					}
				});
			}
			else
			{
				Singleton<Popup>.Instance.HideInstantLoading();
				Singleton<Popup>.Instance.ShowToastMessage("Authentication failed", ToastLength.Long);
			}
		});
		*/
	}

	public void MaxData()
	{
		string text = Resources.Load<TextAsset>("JSON/TMP/max_campaign_progress").text;
		ProfileManager.UserProfile.playerCampaignProgessData.Set(string.Empty);
		ProfileManager.UserProfile.playerCampaignStageProgessData.Set(text);
		GameData.playerCampaignStageProgress = JsonConvert.DeserializeObject<_PlayerCampaignStageProgressData>(text);
		text = Resources.Load<TextAsset>("JSON/TMP/max_campaign_reward_progress").text;
		ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(text);
		GameData.playerCampaignRewardProgress = JsonConvert.DeserializeObject<_PlayerCampaignRewardProgressData>(text);
		text = Resources.Load<TextAsset>("JSON/TMP/max_grenade_data").text;
		ProfileManager.UserProfile.playerGrenadeData.Set(text);
		GameData.playerGrenades = JsonConvert.DeserializeObject<_PlayerGrenadeData>(text);
		text = Resources.Load<TextAsset>("JSON/TMP/max_gun_data").text;
		ProfileManager.UserProfile.playerGunData.Set(text);
		GameData.playerGuns = JsonConvert.DeserializeObject<_PlayerGunData>(text);
		text = Resources.Load<TextAsset>("JSON/TMP/max_melee_weapon_data").text;
		ProfileManager.UserProfile.playerMeleeWeaponData.Set(text);
		GameData.playerMeleeWeapons = JsonConvert.DeserializeObject<_PlayerMeleeWeaponData>(text);
		text = Resources.Load<TextAsset>("JSON/TMP/max_resources_data").text;
		ProfileManager.UserProfile.playerResourcesData.Set(text);
		GameData.playerResources = JsonConvert.DeserializeObject<_PlayerResourcesData>(text);
		text = Resources.Load<TextAsset>("JSON/TMP/max_rambo_data").text;
		ProfileManager.UserProfile.playerRamboData.Set(text);
		GameData.playerRambos = JsonConvert.DeserializeObject<_PlayerRamboData>(text);
		text = Resources.Load<TextAsset>("JSON/TMP/max_booster_data").text;
		ProfileManager.UserProfile.playerBoosterData.Set(text);
		GameData.playerBoosters = JsonConvert.DeserializeObject<_PlayerBoosterData>(text);
		ProfileManager.SaveAll();
		base.gameObject.SetActive(false);
		SceneFading.Instance.FadeOutAndLoadScene("Menu", false, 2f);
	}

	public void ResetData()
	{
		ProfileManager.DeleteAll();
		string text = Resources.Load<TextAsset>("JSON/New Player Data/new_player_resources_data").text;
		ProfileManager.UserProfile.playerResourcesData.Set(text);
		GameData.playerResources = JsonConvert.DeserializeObject<_PlayerResourcesData>(text);
		text = Resources.Load<TextAsset>("JSON/New Player Data/new_player_rambo_data").text;
		ProfileManager.UserProfile.playerRamboData.Set(text);
		ProfileManager.UserProfile.ramboId.Set(0);
		GameData.playerRambos = JsonConvert.DeserializeObject<_PlayerRamboData>(text);
		text = Resources.Load<TextAsset>("JSON/New Player Data/new_player_gun_data").text;
		ProfileManager.UserProfile.playerGunData.Set(text);
		ProfileManager.UserProfile.gunNormalId.Set(0);
		ProfileManager.UserProfile.gunSpecialId.Set(-1);
		ProfileManager.UserProfile.getDailyGiftDay.Set(1);
		GameData.playerGuns = JsonConvert.DeserializeObject<_PlayerGunData>(text);
		text = Resources.Load<TextAsset>("JSON/New Player Data/new_player_grenades_data").text;
		ProfileManager.UserProfile.playerGrenadeData.Set(text);
		ProfileManager.UserProfile.grenadeId.Set(500);
		GameData.playerGrenades = JsonConvert.DeserializeObject<_PlayerGrenadeData>(text);
		text = Resources.Load<TextAsset>("JSON/New Player Data/new_player_melee_weapon_data").text;
		ProfileManager.UserProfile.playerMeleeWeaponData.Set(text);
		ProfileManager.UserProfile.meleeWeaponId.Set(600);
		GameData.playerMeleeWeapons = JsonConvert.DeserializeObject<_PlayerMeleeWeaponData>(text);
		text = Resources.Load<TextAsset>("JSON/New Player Data/new_player_campaign_stage_progress_data").text;
		ProfileManager.UserProfile.playerCampaignStageProgessData.Set(text);
		ProfileManager.UserProfile.playerCampaignProgessData.Set(string.Empty);
		GameData.playerCampaignStageProgress = JsonConvert.DeserializeObject<_PlayerCampaignStageProgressData>(text);
		text = Resources.Load<TextAsset>("JSON/New Player Data/new_player_campaign_reward_progress_data").text;
		ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(text);
		GameData.playerCampaignRewardProgress = JsonConvert.DeserializeObject<_PlayerCampaignRewardProgressData>(text);
		text = Resources.Load<TextAsset>("JSON/New Player Data/new_player_booster_data").text;
		ProfileManager.UserProfile.playerBoosterData.Set(text);
		GameData.playerBoosters = JsonConvert.DeserializeObject<_PlayerBoosterData>(text);
		text = Resources.Load<TextAsset>("JSON/New Player Data/new_player_selecting_booster").text;
		ProfileManager.UserProfile.playerSelectingBooster.Set(text);
		GameData.selectingBoosters = JsonConvert.DeserializeObject<_PlayerSelectingBooster>(text);
		ProfileManager.SaveAll();
		S.Instance.CleanData();

		base.gameObject.SetActive(false);
		SceneFading.Instance.FadeOutAndLoadScene("Menu", false, 2f);


	}
}
