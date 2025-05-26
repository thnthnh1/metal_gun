using System;
using UnityEngine;

namespace G2.Sdk.SecurityHelper
{
	[Serializable]
	public class SecuredFloat
	{
		private static int XOR_KEY = UnityEngine.Random.Range(-1000000000, 1000000000);

		private int internalValue;

		public float Value
		{
			get
			{
				return TypeConverter.IntToFloat(this.internalValue ^ SecuredFloat.XOR_KEY);
			}
			set
			{
				this.internalValue = (TypeConverter.FloatToInt(value) ^ SecuredFloat.XOR_KEY);
			}
		}

		public SecuredFloat(float value)
		{
			this.Value = value;
		}

		public SecuredFloat()
		{
		}

		public static implicit operator float(SecuredFloat c)
		{
			return c.Value;
		}

		public static implicit operator string(SecuredFloat c)
		{
			return c.Value.ToString();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
