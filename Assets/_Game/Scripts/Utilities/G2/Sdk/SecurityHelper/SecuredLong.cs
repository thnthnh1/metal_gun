using System;
using UnityEngine;

namespace G2.Sdk.SecurityHelper
{
	[Serializable]
	public class SecuredLong
	{
		private static long XOR_KEY = (long)UnityEngine.Random.Range(-1000000000, 1000000000);

		private long internalValue;

		public long Value
		{
			get
			{
				return this.internalValue ^ SecuredLong.XOR_KEY;
			}
			set
			{
				this.internalValue = (value ^ SecuredLong.XOR_KEY);
			}
		}

		public SecuredLong(long value)
		{
			this.Value = value;
		}

		public SecuredLong()
		{
		}

		public static implicit operator long(SecuredLong c)
		{
			return c.Value;
		}

		public static implicit operator string(SecuredLong c)
		{
			return c.Value.ToString();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
