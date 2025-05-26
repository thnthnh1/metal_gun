using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CnControls
{
	public class VirtualButton
	{
		private string _Name_k__BackingField;

		private bool _IsPressed_k__BackingField;

		private int _lastPressedFrame = -1;

		private int _lastReleasedFrame = -1;

		public string Name
		{
			get;
			set;
		}

		public bool IsPressed
		{
			get;
			private set;
		}

		public bool GetButton
		{
			get
			{
				return this.IsPressed;
			}
		}

		public bool GetButtonDown
		{
			get
			{
				return this._lastPressedFrame != -1 && this._lastPressedFrame - Time.frameCount == -1;
			}
		}

		public bool GetButtonUp
		{
			get
			{
				return this._lastReleasedFrame != -1 && this._lastReleasedFrame == Time.frameCount - 1;
			}
		}

		public VirtualButton(string name)
		{
			this.Name = name;
		}

		public void Press()
		{
			if (this.IsPressed)
			{
				return;
			}
			this.IsPressed = true;
			this._lastPressedFrame = Time.frameCount;
		}

		public void Release()
		{
			this.IsPressed = false;
			this._lastReleasedFrame = Time.frameCount;
		}
	}
}
