using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootHandler : MonoBehaviour
{
	public static ShootHandler instance;

	// public CnControls.SimpleJoystick joystick;

	public bool pressed;
    // Start is called before the first frame update
    void Awake()
    {
		instance = this;
	}

	private void Update()
	{
		// pressed = joystick.active;
	}

	//public void OnPointerEnter(PointerEventData eventData)
	//{
	//	pressed = true;
	//}

	//public void OnPointerExit(PointerEventData eventData)
	//{
	//	pressed = false;
	//}
}
