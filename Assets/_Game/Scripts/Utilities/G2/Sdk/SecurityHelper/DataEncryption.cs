using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace G2.Sdk.SecurityHelper
{
	public class DataEncryption
	{
		public enum HASH_MODE
		{
			MODE_0_NO_HASH,
			MODE_1_MD5,
			MODE_2_SHA256
		}

		private char[] delimiters = new char[]
		{
			'.'
		};

		private char delimiter = '.';

		private AesEncryption aesEncryption;

		private DataEncryption.HASH_MODE _encryptionHashMode_k__BackingField;

		private DataEncryption.HASH_MODE _decryptionHashMode_k__BackingField;

		public DataEncryption.HASH_MODE encryptionHashMode
		{
			get;
			set;
		}

		public DataEncryption.HASH_MODE decryptionHashMode
		{
			get;
			set;
		}

		public DataEncryption(string password, string saltKey)
		{
			this.aesEncryption = new AesEncryption(password, saltKey);
			this.encryptionHashMode = DataEncryption.HASH_MODE.MODE_1_MD5;
			this.decryptionHashMode = DataEncryption.HASH_MODE.MODE_1_MD5;
		}

		public string Encrypt(string plain)
		{
			string text = this.aesEncryption.Encrypt(plain);
			DataEncryption.HASH_MODE encryptionHashMode = this.encryptionHashMode;
			if (encryptionHashMode != DataEncryption.HASH_MODE.MODE_1_MD5)
			{
				if (encryptionHashMode == DataEncryption.HASH_MODE.MODE_2_SHA256)
				{
					text = HashHelper.HashSHA256(plain) + this.delimiter + text;
				}
			}
			else
			{
				text = HashHelper.HashMD5(plain) + this.delimiter + text;
			}
			return text;
		}

		public string Decrypt(string encrypted)
		{
			if (this.decryptionHashMode == DataEncryption.HASH_MODE.MODE_0_NO_HASH)
			{
				return this.aesEncryption.Decrypt(encrypted);
			}
			string[] array = encrypted.Split(this.delimiters, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length != 2)
			{
				return null;
			}
			string text = this.aesEncryption.Decrypt(array[1]);
			string b = array[0];
			if (this.Hash(text, this.decryptionHashMode) == b)
			{
				return text;
			}
			return null;
		}

		public string Hash(string plain, DataEncryption.HASH_MODE hashMode)
		{
			if (hashMode == DataEncryption.HASH_MODE.MODE_1_MD5)
			{
				return HashHelper.HashMD5(plain);
			}
			if (hashMode != DataEncryption.HASH_MODE.MODE_2_SHA256)
			{
				return string.Empty;
			}
			return HashHelper.HashSHA256(plain);
		}
	}
}
