using System;
using System.Collections.Generic;
using UnityEngine;

namespace CnControls
{
	public class CnInputManager
	{
		private static CnInputManager _instance;

		private Dictionary<string, List<VirtualAxis>> _virtualAxisDictionary = new Dictionary<string, List<VirtualAxis>>();

		private Dictionary<string, List<VirtualButton>> _virtualButtonsDictionary = new Dictionary<string, List<VirtualButton>>();

		private static CnInputManager Instance
		{
			get
			{
				CnInputManager arg_17_0;
				if ((arg_17_0 = CnInputManager._instance) == null)
				{
					arg_17_0 = (CnInputManager._instance = new CnInputManager());
				}
				return arg_17_0;
			}
		}

		public static int TouchCount
		{
			get
			{
				return UnityEngine.Input.touchCount;
			}
		}

		private CnInputManager()
		{
		}

		public static Touch GetTouch(int touchIndex)
		{
			return UnityEngine.Input.GetTouch(touchIndex);
		}

		public static float GetAxis(string axisName)
		{
			return CnInputManager.GetAxis(axisName, false);
		}

		public static float GetAxisRaw(string axisName)
		{
			return CnInputManager.GetAxis(axisName, true);
		}

		private static float GetAxis(string axisName, bool isRaw)
		{
			if (CnInputManager.AxisExists(axisName))
			{
				return CnInputManager.GetVirtualAxisValue(CnInputManager.Instance._virtualAxisDictionary[axisName], axisName, isRaw);
			}
			if (CnInputManager.ButtonExists(axisName))
			{
				bool anyVirtualButton = CnInputManager.GetAnyVirtualButton(CnInputManager.Instance._virtualButtonsDictionary[axisName]);
				if (anyVirtualButton)
				{
					return 1f;
				}
			}
			return (!isRaw) ? UnityEngine.Input.GetAxis(axisName) : UnityEngine.Input.GetAxisRaw(axisName);
		}

		public static bool GetButton(string buttonName)
		{
			bool button = Input.GetButton(buttonName);
			return button || (CnInputManager.ButtonExists(buttonName) && CnInputManager.GetAnyVirtualButton(CnInputManager.Instance._virtualButtonsDictionary[buttonName]));
		}

		public static bool GetButtonDown(string buttonName)
		{
			bool buttonDown = Input.GetButtonDown(buttonName);
			return buttonDown || (CnInputManager.ButtonExists(buttonName) && CnInputManager.GetAnyVirtualButtonDown(CnInputManager.Instance._virtualButtonsDictionary[buttonName]));
		}

		public static bool GetButtonUp(string buttonName)
		{
			bool buttonUp = Input.GetButtonUp(buttonName);
			return buttonUp || (CnInputManager.ButtonExists(buttonName) && CnInputManager.GetAnyVirtualButtonUp(CnInputManager.Instance._virtualButtonsDictionary[buttonName]));
		}

		public static bool AxisExists(string axisName)
		{
			return CnInputManager.Instance._virtualAxisDictionary.ContainsKey(axisName);
		}

		public static bool ButtonExists(string buttonName)
		{
			return CnInputManager.Instance._virtualButtonsDictionary.ContainsKey(buttonName);
		}

		public static void RegisterVirtualAxis(VirtualAxis virtualAxis)
		{
			if (!CnInputManager.Instance._virtualAxisDictionary.ContainsKey(virtualAxis.Name))
			{
				CnInputManager.Instance._virtualAxisDictionary[virtualAxis.Name] = new List<VirtualAxis>();
			}
			CnInputManager.Instance._virtualAxisDictionary[virtualAxis.Name].Add(virtualAxis);
		}

		public static void UnregisterVirtualAxis(VirtualAxis virtualAxis)
		{
			if (CnInputManager.Instance._virtualAxisDictionary.ContainsKey(virtualAxis.Name))
			{
				if (!CnInputManager.Instance._virtualAxisDictionary[virtualAxis.Name].Remove(virtualAxis))
				{
					UnityEngine.Debug.LogError("Requested axis " + virtualAxis.Name + " exists, but there's no such virtual axis that you're trying to unregister");
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Trying to unregister an axis " + virtualAxis.Name + " that was never registered");
			}
		}

		public static void RegisterVirtualButton(VirtualButton virtualButton)
		{
			if (!CnInputManager.Instance._virtualButtonsDictionary.ContainsKey(virtualButton.Name))
			{
				CnInputManager.Instance._virtualButtonsDictionary[virtualButton.Name] = new List<VirtualButton>();
			}
			CnInputManager.Instance._virtualButtonsDictionary[virtualButton.Name].Add(virtualButton);
		}

		public static void UnregisterVirtualButton(VirtualButton virtualButton)
		{
			if (CnInputManager.Instance._virtualButtonsDictionary.ContainsKey(virtualButton.Name))
			{
				if (!CnInputManager.Instance._virtualButtonsDictionary[virtualButton.Name].Remove(virtualButton))
				{
					UnityEngine.Debug.LogError("Requested button axis exists, but there's no such virtual button that you're trying to unregister");
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Trying to unregister a button that was never registered");
			}
		}

		private static float GetVirtualAxisValue(List<VirtualAxis> virtualAxisList, string axisName, bool isRaw)
		{
			float num = (!isRaw) ? UnityEngine.Input.GetAxis(axisName) : UnityEngine.Input.GetAxisRaw(axisName);
			if (!Mathf.Approximately(num, 0f))
			{
				return num;
			}
			for (int i = 0; i < virtualAxisList.Count; i++)
			{
				float value = virtualAxisList[i].Value;
				if (!Mathf.Approximately(value, 0f))
				{
					return value;
				}
			}
			return 0f;
		}

		private static bool GetAnyVirtualButtonDown(List<VirtualButton> virtualButtons)
		{
			for (int i = 0; i < virtualButtons.Count; i++)
			{
				if (virtualButtons[i].GetButtonDown)
				{
					return true;
				}
			}
			return false;
		}

		private static bool GetAnyVirtualButtonUp(List<VirtualButton> virtualButtons)
		{
			for (int i = 0; i < virtualButtons.Count; i++)
			{
				if (virtualButtons[i].GetButtonUp)
				{
					return true;
				}
			}
			return false;
		}

		private static bool GetAnyVirtualButton(List<VirtualButton> virtualButtons)
		{
			for (int i = 0; i < virtualButtons.Count; i++)
			{
				if (virtualButtons[i].GetButton)
				{
					return true;
				}
			}
			return false;
		}
	}
}
