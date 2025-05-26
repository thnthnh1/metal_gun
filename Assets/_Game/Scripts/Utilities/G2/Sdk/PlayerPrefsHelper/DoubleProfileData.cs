using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class DoubleProfileData : BaseProfileDataType<double>
	{
		private SecuredDouble _data_k__BackingField;

		public SecuredDouble data
		{
			get;
			private set;
		}

		public DoubleProfileData(string tag, double defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator double(DoubleProfileData doubleProfileData)
		{
			return doubleProfileData.data;
		}

		public override void Set(double value)
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

		protected override void Load(double defaultValue)
		{
			this.data.Value = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(double value)
		{
			this.data.Value = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(double defaultValue)
		{
			this.data = new SecuredDouble(defaultValue);
			base.InitData(defaultValue);
		}

		protected override double LoadFromPlayerPrefs(double defaultValue)
		{
			double result;
			try
			{
				result = double.Parse(this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag)), CultureInfo.InvariantCulture);
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(double value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(value.ToString()));
		}
	}
}
