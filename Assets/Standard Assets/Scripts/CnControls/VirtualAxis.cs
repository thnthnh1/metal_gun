using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CnControls
{
	public class VirtualAxis
	{
		private string _Name_k__BackingField;

		private float _Value_k__BackingField;

		public string Name
		{
			get;
			set;
		}

		public float Value
		{
			get;
			set;
		}

		public VirtualAxis(string name)
		{
			this.Name = name;
		}
	}
}
