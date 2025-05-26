using System;
using System.Security.Cryptography;
using System.Text;

namespace G2.Sdk.SecurityHelper
{
	public class HashHelper
	{
		private const string stringFormat = "x2";

		public static string HashMD5(string input)
		{
			if (input != null)
			{
				MD5 mD = MD5.Create();
				byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(input));
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				return stringBuilder.ToString();
			}
			return null;
		}

		public static string HashSHA256(string input)
		{
			if (input != null)
			{
				SHA256Managed sHA256Managed = new SHA256Managed();
				byte[] array = sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(input), 0, Encoding.UTF8.GetByteCount(input));
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				return stringBuilder.ToString().ToLower();
			}
			return null;
		}
	}
}
