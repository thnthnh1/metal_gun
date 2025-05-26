using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class Vector4ProfileData : BaseProfileDataType<Vector4>
	{
		private Vector4 _data_k__BackingField;

		public Vector4 data
		{
			get;
			private set;
		}

		public Vector4ProfileData(string tag, Vector4 defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator Vector4(Vector4ProfileData vector4ProfileData)
		{
			return vector4ProfileData.data;
		}

		public override void Set(Vector4 value)
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

		protected override void Load(Vector4 defaultValue)
		{
			this.data = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(Vector4 value)
		{
			this.data = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(Vector4 defaultValue)
		{
			this.data = defaultValue;
			base.InitData(defaultValue);
		}

		protected override Vector4 LoadFromPlayerPrefs(Vector4 defaultValue)
		{
			string text = this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag));
			Vector4 result;
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
				result = new Vector4(x, y, z, w);
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(Vector4 value)
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
