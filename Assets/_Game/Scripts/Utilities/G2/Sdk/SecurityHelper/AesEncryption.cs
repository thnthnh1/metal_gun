using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace G2.Sdk.SecurityHelper
{
	public class AesEncryption
	{
		private AesManaged aes;

		public AesEncryption(string password, string saltKey)
		{
			int iterations = 100;
			Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(saltKey), iterations);
			this.aes = new AesManaged();
			this.aes.Key = rfc2898DeriveBytes.GetBytes(16);
			this.aes.IV = rfc2898DeriveBytes.GetBytes(16);
		}

		public string Encrypt(string plain)
		{
			if (plain != null)
			{
				try
				{
					string text = string.Empty;
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, this.aes.CreateEncryptor(), CryptoStreamMode.Write))
						{
							byte[] bytes = Encoding.UTF8.GetBytes(plain);
							cryptoStream.Write(bytes, 0, bytes.Length);
							cryptoStream.FlushFinalBlock();
							text = Convert.ToBase64String(memoryStream.ToArray());
						}
					}
					string result = text;
					return result;
				}
				catch (Exception var_5_7A)
				{
					string result = null;
					return result;
				}
			}
			return null;
		}

		public string Decrypt(string encrypted)
		{
			if (encrypted != null)
			{
				try
				{
					string text = string.Empty;
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (CryptoStream cryptoStream = new CryptoStream(memoryStream, this.aes.CreateDecryptor(), CryptoStreamMode.Write))
						{
							byte[] array = Convert.FromBase64String(encrypted);
							cryptoStream.Write(array, 0, array.Length);
							cryptoStream.FlushFinalBlock();
							byte[] array2 = memoryStream.ToArray();
							text = Encoding.UTF8.GetString(array2, 0, array2.Length);
						}
					}
					string result = text;
					return result;
				}
				catch (Exception var_6_83)
				{
					string result = null;
					return result;
				}
			}
			return null;
		}
	}
}
