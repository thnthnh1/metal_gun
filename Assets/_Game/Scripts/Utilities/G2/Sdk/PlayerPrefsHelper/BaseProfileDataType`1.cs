using G2.Sdk.SecurityHelper;
using System;
using UnityEngine;

namespace G2.Sdk.PlayerPrefsHelper
{
	public abstract class BaseProfileDataType<T>
	{
		protected string encryptedTag;

		protected DataEncryption dataEncryption;

		public BaseProfileDataType(string tag, T defaultValue, DataEncryption dataEncryption, bool isAutoInit)
		{
			this.dataEncryption = dataEncryption;
			this.encryptedTag = dataEncryption.Encrypt(tag);
			if (isAutoInit)
			{
				this.InitData(defaultValue);
			}
		}

		protected bool IsHasValue()
		{
			return PlayerPrefs.HasKey(this.encryptedTag);
		}

		public abstract void Set(T value);

		protected abstract void Load(T defaultValue);

		protected abstract void Save(T value);

		protected virtual void InitData(T defaultValue)
		{
			if (!this.IsHasValue())
			{
				this.Save(defaultValue);
			}
			else
			{
				this.Load(defaultValue);
			}
		}

		protected abstract T LoadFromPlayerPrefs(T defaultValue);

		protected abstract void SaveToPlayerPrefs(T value);
	}
}
