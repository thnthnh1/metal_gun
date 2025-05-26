/*
using System;
using UnityEngine;

public class PreciseLocale
{
	private interface PlatformBridge
	{
		string GetRegion();

		string GetLanguage();

		string GetLanguageID();

		string GetCurrencyCode();

		string GetCurrencySymbol();
	}

	private class EditorBridge : PreciseLocale.PlatformBridge
	{
		public string GetRegion()
		{
			return "US";
		}

		public string GetLanguage()
		{
			return "en";
		}

		public string GetLanguageID()
		{
			return "en_US";
		}

		public string GetCurrencyCode()
		{
			return "USD";
		}

		public string GetCurrencySymbol()
		{
			return "$";
		}
	}

	private class PreciseLocaleAndroid : PreciseLocale.PlatformBridge
	{
		private static AndroidJavaClass _preciseLocale = new AndroidJavaClass("com.kokosoft.preciselocale.PreciseLocale");

		public string GetRegion()
		{
			return PreciseLocale.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getRegion", new object[0]);
		}

		public string GetLanguage()
		{
			return PreciseLocale.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getLanguage", new object[0]);
		}

		public string GetLanguageID()
		{
			return PreciseLocale.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getLanguageID", new object[0]);
		}

		public string GetCurrencyCode()
		{
			return PreciseLocale.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getCurrencyCode", new object[0]);
		}

		public string GetCurrencySymbol()
		{
			return PreciseLocale.PreciseLocaleAndroid._preciseLocale.CallStatic<string>("getCurrencySymbol", new object[0]);
		}
	}

	private static PreciseLocale.PlatformBridge _platform;

	private static PreciseLocale.PlatformBridge platform
	{
		get
		{
			if (PreciseLocale._platform == null)
			{
				PreciseLocale._platform = new PreciseLocale.PreciseLocaleAndroid();
			}
			return PreciseLocale._platform;
		}
	}

	public static string GetRegion()
	{
		return PreciseLocale.platform.GetRegion();
	}

	public static string GetLanguageID()
	{
		return PreciseLocale.platform.GetLanguageID();
	}

	public static string GetLanguage()
	{
		return PreciseLocale.platform.GetLanguage();
	}

	public static string GetCurrencyCode()
	{
		return PreciseLocale.platform.GetCurrencyCode();
	}

	public static string GetCurrencySymbol()
	{
		return PreciseLocale.platform.GetCurrencySymbol();
	}
}
*/