using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class BoolProfileData : BaseProfileDataType<bool>
	{
		private bool _data_k__BackingField;

		public bool data
		{
			get;
			private set;
		}

		public BoolProfileData(string tag, bool defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator bool(BoolProfileData boolProfileData)
		{
			return boolProfileData.data;
		}

		public override void Set(bool value)
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

		protected override void Load(bool defaultValue)
		{
			this.data = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(bool value)
		{
			this.data = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override bool LoadFromPlayerPrefs(bool value)
		{
			bool result;
			try
			{
				result = bool.Parse(this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag)));
			}
			catch
			{
				return value;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(bool value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(value.ToString()));
		}
	}
}
