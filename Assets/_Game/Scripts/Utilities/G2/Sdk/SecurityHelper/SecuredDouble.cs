using System;
using UnityEngine;

namespace G2.Sdk.SecurityHelper
{
	[Serializable]
	public class SecuredDouble
	{
		private static long XOR_KEY = (long)UnityEngine.Random.Range(-1000000000, 1000000000);

		private long internalValue;

		public double Value
		{
			get
			{
				return TypeConverter.LongToDouble(this.internalValue ^ SecuredDouble.XOR_KEY);
			}
			set
			{
				this.internalValue = (TypeConverter.DoubleToLong(value) ^ SecuredDouble.XOR_KEY);
			}
		}

		public SecuredDouble(double value)
		{
			this.Value = value;
		}

		public SecuredDouble()
		{
		}

		public static implicit operator double(SecuredDouble c)
		{
			return c.Value;
		}

		public static implicit operator string(SecuredDouble c)
		{
			return c.Value.ToString();
		}

		public override string ToString()
		{
			return this.Value.ToString();
		}
	}
}
