using System;
using UnityEngine;

#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

public class UtilityUnity
{
#if UNITY_IOS
	[DllImport("__Internal")] private static extern bool _showRating();
	[DllImport("__Internal")] private static extern string _getBuildVersion();
#endif

	public static string GetDeviceId()
	{
		string empty = string.Empty;
		AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getContentResolver", new object[0]);
		AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.provider.Settings$Secure");
		return androidJavaClass2.CallStatic<string>("getString", new object[]
		{
			androidJavaObject,
			"android_id"
		});
	}

	public static void OpenStore()
	{
#if UNITY_IOS
		int timeRating = PlayerPrefs.GetInt("TIME_RATING_GAME", 0);
		if (timeRating < 3)
		{
			PlayerPrefs.SetInt("TIME_RATING_GAME", timeRating + 1);
			PlayerPrefs.Save();
			if (!_showRating())
			{
				Application.OpenURL(StaticValue.storeUrl);
			}
		}
		else
		{
			Application.OpenURL(StaticValue.storeUrl);
		}
#else
		Application.OpenURL(StaticValue.storeUrl);
#endif
	}

	public static string GameVersion()
	{
#if UNITY_IOS && !UNITY_EDITOR
        return $"{Application.version} ({_getBuildVersion()})";
#elif UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        var pInfo = packageManager.Call<AndroidJavaObject>("getPackageInfo", Application.identifier, 0);
        return $"{Application.version} ({pInfo.Get<int>("versionCode")})";
#else
		return $"{Application.version}";
#endif
	}
}
