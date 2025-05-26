using System;
using UnityEngine;

namespace G2.Sdk.SecurityHelper
{
	[Serializable]
	public class SecuredInt
	{
		private static int XOR_KEY = UnityEngine.Random.Range(-1000000000, 1000000000);

		private int internalValue;

		public int Value
		{
			get
			{
				return this.internalValue ^ SecuredInt.XOR_KEY;
			}
			set
			{
				this.internalValue = (value ^ SecuredInt.XOR_KEY);
			}
		}

		public SecuredInt(int value)
		{
			this.Value = value;
		}

		public SecuredInt()
		{
		}

		public static implicit operator int(SecuredInt c)
		{
			return c.Value;
		}

		public static implicit operator string(SecuredInt c)
		{
			return c.Value.ToString();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
