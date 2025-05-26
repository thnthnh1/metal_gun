using G2.Sdk.SecurityHelper;
using System;
using UnityEngine;

public class ProfileManager
{
	private static UserProfile userProfile;

	private static DataEncryption dataEncryption;

	public static UserProfile UserProfile
	{
		get
		{
			return ProfileManager.userProfile;
		}
	}

	public static void Init(string password = "nzt", string saltKey = "N7x9QZt2")
	{
		if (ProfileManager.userProfile == null)
		{
			ProfileManager.dataEncryption = new DataEncryption(password, saltKey);
		}
		ProfileManager.Load();
	}

	public static void Load()
	{
		if (ProfileManager.userProfile == null)
		{
			ProfileManager.userProfile = new UserProfile(ProfileManager.dataEncryption);
		}
	}

	public static void Load(RawUserProfile newData)
	{
		ProfileManager.userProfile.ResetTo(newData);
	}

	public static void SaveAll()
	{
		PlayerPrefs.Save();
	}

	public static void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}
}
