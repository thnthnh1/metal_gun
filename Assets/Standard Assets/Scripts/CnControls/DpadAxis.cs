using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CnControls
{
	public class DpadAxis : MonoBehaviour
	{
		public string AxisName;

		public float AxisMultiplier;

		private RectTransform _RectTransform_k__BackingField;

		private int _LastFingerId_k__BackingField;

		private VirtualAxis _virtualAxis;

		public RectTransform RectTransform
		{
			get;
			private set;
		}

		public int LastFingerId
		{
			get;
			set;
		}

		private void Awake()
		{
			this.RectTransform = base.GetComponent<RectTransform>();
		}

		private void OnEnable()
		{
			this._virtualAxis = (this._virtualAxis ?? new VirtualAxis(this.AxisName));
			this.LastFingerId = -1;
			CnInputManager.RegisterVirtualAxis(this._virtualAxis);
		}

		private void OnDisable()
		{
			CnInputManager.UnregisterVirtualAxis(this._virtualAxis);
		}

		public void Press(Vector2 screenPoint, Camera eventCamera, int pointerId)
		{
			this._virtualAxis.Value = Mathf.Clamp(this.AxisMultiplier, -1f, 1f);
			this.LastFingerId = pointerId;
		}

		public void TryRelease(int pointerId)
		{
			if (this.LastFingerId == pointerId)
			{
				this._virtualAxis.Value = 0f;
				this.LastFingerId = -1;
			}
		}
	}
}
