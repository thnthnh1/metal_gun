using G2.Sdk.SecurityHelper;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class RectProfileData : BaseProfileDataType<Rect>
	{
		private Rect _data_k__BackingField;

		public Rect data
		{
			get;
			private set;
		}

		public RectProfileData(string tag, Rect defaultValue, DataEncryption dataEncryption, bool isAutoInit = true) : base(tag, defaultValue, dataEncryption, isAutoInit)
		{
		}

		public static implicit operator Rect(RectProfileData rectProfileData)
		{
			return rectProfileData.data;
		}

		public override void Set(Rect value)
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

		protected override void Load(Rect defaultValue)
		{
			this.data = this.LoadFromPlayerPrefs(defaultValue);
		}

		protected override void Save(Rect value)
		{
			this.data = value;
			this.SaveToPlayerPrefs(value);
		}

		protected override void InitData(Rect defaultValue)
		{
			this.data = defaultValue;
			base.InitData(defaultValue);
		}

		protected override Rect LoadFromPlayerPrefs(Rect defaultValue)
		{
			string text = this.dataEncryption.Decrypt(PlayerPrefs.GetString(this.encryptedTag));
			Rect result;
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
				float width = float.Parse(text.Substring(num + 1, num2 - num - 1));
				num = num2;
				num2 = text.Length;
				float height = float.Parse(text.Substring(num + 1, num2 - num - 1));
				result = new Rect(x, y, width, height);
			}
			catch
			{
				return defaultValue;
			}
			return result;
		}

		protected override void SaveToPlayerPrefs(Rect value)
		{
			PlayerPrefs.SetString(this.encryptedTag, this.dataEncryption.Encrypt(string.Concat(new object[]
			{
				value.x,
				",",
				value.y,
				",",
				value.width,
				",",
				value.height
			})));
		}
	}
}
