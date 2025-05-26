using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class Vector2ProfileData : BaseProfileDataType<Vector2>
	{
		private Vector2 _data_k__BackingField;

		public Vector2 data
		{
			get;
			private set;
		}

		public Vector2ProfileData(string tag, Vector2 defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator Vector2(Vector2ProfileData vector2ProfileData)
		{
			return vector2ProfileData.data;
		}

		public override void Set(Vector2 value)
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

		protected override void Load(Vector2 defaultValue)
		{
			this.data = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(Vector2 value)
		{
			this.data = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(Vector2 defaultValue)
		{
			this.data = defaultValue;
			base.InitData(defaultValue);
		}

		protected override Vector2 LoadFromPlayerPrefs(Vector2 defaultValue)
		{
			string text = this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag));
			Vector2 result;
			try
			{
				int num = 0;
				int num2 = text.IndexOf(',');
				float x = float.Parse(text.Substring(num, num2 - num));
				num = num2;
				num2 = text.Length;
				float y = float.Parse(text.Substring(num + 1, num2 - num - 1));
				result = new Vector2(x, y);
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(Vector2 value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(value.x + "," + value.y));
		}
	}
}
