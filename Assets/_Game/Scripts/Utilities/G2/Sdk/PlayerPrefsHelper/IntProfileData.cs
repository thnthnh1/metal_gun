using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class IntProfileData : BaseProfileDataType<int>
	{
		private SecuredInt _data_k__BackingField;

		public SecuredInt data
		{
			get;
			private set;
		}

		public IntProfileData(string tag, int defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator int(IntProfileData intProfileData)
		{
			return intProfileData.data.Value;
		}

		public override void Set(int value)
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

		protected override void Load(int defaultValue)
		{
			this.data.Value = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(int value)
		{
			this.data.Value = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(int defaultValue)
		{
			this.data = new SecuredInt(defaultValue);
			base.InitData(defaultValue);
		}

		protected override int LoadFromPlayerPrefs(int defaultValue)
		{
			int result;
			try
			{
				result = int.Parse(this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag)));
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(int value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(value.ToString()));
		}
	}
}
