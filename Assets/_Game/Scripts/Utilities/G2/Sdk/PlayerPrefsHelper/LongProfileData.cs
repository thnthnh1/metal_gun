using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class LongProfileData : BaseProfileDataType<long>
	{
		private SecuredLong _data_k__BackingField;

		public SecuredLong data
		{
			get;
			private set;
		}

		public LongProfileData(string tag, long defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator long(LongProfileData longProfileData)
		{
			return longProfileData.data.Value;
		}

		public override void Set(long value)
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

		protected override void Load(long defaultValue)
		{
			this.data.Value = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(long value)
		{
			this.data.Value = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(long defaultValue)
		{
			this.data = new SecuredLong(defaultValue);
			base.InitData(defaultValue);
		}

		protected override long LoadFromPlayerPrefs(long defaultValue)
		{
			long result;
			try
			{
				result = long.Parse(this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag)));
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(long value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(value.ToString()));
		}
	}
}
