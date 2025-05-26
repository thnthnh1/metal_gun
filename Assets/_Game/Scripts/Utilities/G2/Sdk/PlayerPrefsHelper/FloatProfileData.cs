using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class FloatProfileData : BaseProfileDataType<float>
	{
		private SecuredFloat _data_k__BackingField;

		public SecuredFloat data
		{
			get;
			private set;
		}

		public FloatProfileData(string tag, float defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator float(FloatProfileData floatProfileData)
		{
			return floatProfileData.data;
		}

		public override void Set(float value)
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

		protected override void Load(float defaultValue)
		{
			this.data.Value = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(float value)
		{
			this.data.Value = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(float defaultValue)
		{
			this.data = new SecuredFloat(defaultValue);
			base.InitData(defaultValue);
		}

		protected override float LoadFromPlayerPrefs(float defaultValue)
		{
			float result;
			try
			{
				result = float.Parse(this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag)), CultureInfo.InvariantCulture);
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(float value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(value.ToString()));
		}
	}
}
