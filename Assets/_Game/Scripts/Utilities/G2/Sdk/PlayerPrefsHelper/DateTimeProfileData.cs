using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class DateTimeProfileData : BaseProfileDataType<DateTime>
	{
		private DateTime _data_k__BackingField;

		public DateTime data
		{
			get;
			private set;
		}

		public DateTimeProfileData(string tag, DateTime defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator DateTime(DateTimeProfileData dateTimeProfileData)
		{
			return dateTimeProfileData.data;
		}

		public override void Set(DateTime value)
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

		protected override void Load(DateTime defaultValue)
		{
			this.data = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(DateTime value)
		{
			this.data = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(DateTime defaultValue)
		{
			this.data = defaultValue;
			base.InitData(defaultValue);
		}

		protected override DateTime LoadFromPlayerPrefs(DateTime defaultValue)
		{
			DateTime result;
			try
			{
				result = DateTime.Parse(this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag)));
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(DateTime value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(value.ToString()));
		}
	}
}
