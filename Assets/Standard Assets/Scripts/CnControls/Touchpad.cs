using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CnControls
{
	public class Touchpad : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
	{
		private Camera _CurrentEventCamera_k__BackingField;

		public string HorizontalAxisName = "Horizontal";

		public string VerticalAxisName = "Vertical";

		public bool PreserveInertia = true;

		public float Friction = 3f;

		private VirtualAxis _horizintalAxis;

		private VirtualAxis _verticalAxis;

		private int _lastDragFrameNumber;

		public bool _isCurrentlyTweaking;

		[Tooltip("Constraints on the joystick movement axis")]
		public ControlMovementDirection ControlMoveAxis = ControlMovementDirection.Both;

		public bool IsTouching
		{
			get
			{
				return this._isCurrentlyTweaking;
			}
		}

		public Camera CurrentEventCamera
		{
			get;
			set;
		}

		private void OnEnable()
		{
			this._horizintalAxis = (this._horizintalAxis ?? new VirtualAxis(this.HorizontalAxisName));
			this._verticalAxis = (this._verticalAxis ?? new VirtualAxis(this.VerticalAxisName));
			CnInputManager.RegisterVirtualAxis(this._horizintalAxis);
			CnInputManager.RegisterVirtualAxis(this._verticalAxis);
		}

		private void OnDisable()
		{
			CnInputManager.UnregisterVirtualAxis(this._horizintalAxis);
			CnInputManager.UnregisterVirtualAxis(this._verticalAxis);
		}

		public virtual void OnDrag(PointerEventData eventData)
		{
			if ((this.ControlMoveAxis & ControlMovementDirection.Horizontal) != (ControlMovementDirection)0)
			{
				this._horizintalAxis.Value = eventData.delta.x;
			}
			if ((this.ControlMoveAxis & ControlMovementDirection.Vertical) != (ControlMovementDirection)0)
			{
				this._verticalAxis.Value = eventData.delta.y;
			}
			this._lastDragFrameNumber = Time.renderedFrameCount;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			this._isCurrentlyTweaking = false;
			if (!this.PreserveInertia)
			{
				this._horizintalAxis.Value = 0f;
				this._verticalAxis.Value = 0f;
			}
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			this._isCurrentlyTweaking = true;
			this.OnDrag(eventData);
		}

		private void Update()
		{
			if (this._isCurrentlyTweaking && this._lastDragFrameNumber < Time.renderedFrameCount - 2)
			{
				this._horizintalAxis.Value = 0f;
				this._verticalAxis.Value = 0f;
			}
			if (this.PreserveInertia && !this._isCurrentlyTweaking)
			{
				this._horizintalAxis.Value = Mathf.Lerp(this._horizintalAxis.Value, 0f, this.Friction * Time.deltaTime);
				this._verticalAxis.Value = Mathf.Lerp(this._verticalAxis.Value, 0f, this.Friction * Time.deltaTime);
			}
		}
	}
}
