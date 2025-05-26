using G2.Sdk.SecurityHelper;
using System;

namespace G2.Sdk.PlayerPrefsHelper
{
	public class IntBitMaskProfileData : IntProfileData
	{
		public IntBitMaskProfileData(string tag, int defaultValue, DataEncryption dataEncryption) : base(tag, defaultValue, dataEncryption, true)
		{
		}

		public void turnOn(int bit)
		{
			if (bit < 32)
			{
				this.Save(base.data | 1 << bit);
			}
		}

		public void turnOff(int bit)
		{
			if (bit < 32)
			{
				this.Save(base.data & ~(1 << bit));
			}
		}

		public bool isOn(int bit)
		{
			return bit < 32 && (base.data >> bit & 1) != 0;
		}
	}
}
