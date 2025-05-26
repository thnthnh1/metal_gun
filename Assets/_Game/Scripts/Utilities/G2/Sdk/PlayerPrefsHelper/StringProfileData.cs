using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class StringProfileData : BaseProfileDataType<string>
	{
		private string _data_k__BackingField;

		public string data
		{
			get;
			private set;
		}

		public StringProfileData(string tag, string defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator string(StringProfileData stringProfileData)
		{
			return stringProfileData.data;
		}

		public override void Set(string value)
		{
			if (this.data != value)
			{
				this.Save(value);
			}
		}

		public override string ToString()
		{
			return this.data.ToString();
		}

		protected override void Load(string defaultValue)
		{
			this.data = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(string value)
		{
			this.data = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override string LoadFromPlayerPrefs(string defaultValue)
		{
			string result;
			try
			{
				result = this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag));
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(string value)
		{
			if (value != null)
			{
				PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(value));
			}
		}
	}
}
