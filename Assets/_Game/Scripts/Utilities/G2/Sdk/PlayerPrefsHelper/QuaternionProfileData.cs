using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class QuaternionProfileData : BaseProfileDataType<Quaternion>
	{
		private Quaternion _data_k__BackingField;

		public Quaternion data
		{
			get;
			private set;
		}

		public QuaternionProfileData(string tag, Quaternion defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator Quaternion(QuaternionProfileData quaternionProfileData)
		{
			return quaternionProfileData.data;
		}

		public override void Set(Quaternion value)
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

		protected override void Load(Quaternion defaultValue)
		{
			this.data = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(Quaternion value)
		{
			this.data = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(Quaternion defaultValue)
		{
			this.data = defaultValue;
			base.InitData(defaultValue);
		}

		protected override Quaternion LoadFromPlayerPrefs(Quaternion defaultValue)
		{
			string text = this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag));
			Quaternion result;
			try
			{
				int num = 0;
				int num2 = text.IndexOf(',');
				float x = float.Parse(text.Substring(num, num2 - num));
				num = num2;
				num2 = text.IndexOf(',', num + 1);
				float y = float.Parse(text.Substring(num + 1, num2 - num - 1));
				num = num2;
				num2 = text.IndexOf(',', num + 1);
				float z = float.Parse(text.Substring(num + 1, num2 - num - 1));
				num = num2;
				num2 = text.Length;
				float w = float.Parse(text.Substring(num + 1, num2 - num - 1));
				result = new Quaternion(x, y, z, w);
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(Quaternion value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(string.Concat(new object[]
			{
				value.x,
				",",
				value.y,
				",",
				value.z,
				",",
				value.w
			})));
		}
	}
}
