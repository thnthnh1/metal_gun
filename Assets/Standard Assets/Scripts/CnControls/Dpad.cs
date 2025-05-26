using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CnControls
{
	public class Dpad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		public DpadAxis[] DpadAxis;

		private Camera _CurrentEventCamera_k__BackingField;

		public Camera CurrentEventCamera
		{
			get;
			set;
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			this.CurrentEventCamera = (eventData.pressEventCamera ?? this.CurrentEventCamera);
			DpadAxis[] dpadAxis = this.DpadAxis;
			for (int i = 0; i < dpadAxis.Length; i++)
			{
				DpadAxis dpadAxis2 = dpadAxis[i];
				if (RectTransformUtility.RectangleContainsScreenPoint(dpadAxis2.RectTransform, eventData.position, this.CurrentEventCamera))
				{
					dpadAxis2.Press(eventData.position, this.CurrentEventCamera, eventData.pointerId);
				}
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			DpadAxis[] dpadAxis = this.DpadAxis;
			for (int i = 0; i < dpadAxis.Length; i++)
			{
				DpadAxis dpadAxis2 = dpadAxis[i];
				dpadAxis2.TryRelease(eventData.pointerId);
			}
		}
	}
}
