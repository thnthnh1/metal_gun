using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class Vector3ProfileData : BaseProfileDataType<Vector3>
	{
		private Vector3 _data_k__BackingField;

		public Vector3 data
		{
			get;
			private set;
		}

		public Vector3ProfileData(string tag, Vector3 defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator Vector3(Vector3ProfileData vector3ProfileData)
		{
			return vector3ProfileData.data;
		}

		public override void Set(Vector3 value)
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

		protected override void Load(Vector3 defaultValue)
		{
			this.data = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(Vector3 value)
		{
			this.data = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(Vector3 defaultValue)
		{
			this.data = defaultValue;
			base.InitData(defaultValue);
		}

		protected override Vector3 LoadFromPlayerPrefs(Vector3 defaultValue)
		{
			string text = this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag));
			Vector3 result;
			try
			{
				int num = 0;
				int num2 = text.IndexOf(',');
				float x = float.Parse(text.Substring(num, num2 - num));
				num = num2;
				num2 = text.IndexOf(',', num + 1);
				float y = float.Parse(text.Substring(num + 1, num2 - num - 1));
				num = num2;
				num2 = text.Length;
				float z = float.Parse(text.Substring(num + 1, num2 - num - 1));
				result = new Vector3(x, y, z);
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(Vector3 value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(string.Concat(new object[]
			{
				value.x,
				",",
				value.y,
				",",
				value.z
			})));
		}
	}
}
