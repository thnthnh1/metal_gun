using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CnControls
{
	public class SimpleButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IEventSystemHandler
	{
		public string ButtonName = "Jump";

		private VirtualButton _virtualButton;

		private void OnEnable()
		{
			this._virtualButton = (this._virtualButton ?? new VirtualButton(this.ButtonName));
			CnInputManager.RegisterVirtualButton(this._virtualButton);
		}

		private void OnDisable()
		{
			CnInputManager.UnregisterVirtualButton(this._virtualButton);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			this._virtualButton.Release();
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			this._virtualButton.Press();
		}
	}
}
