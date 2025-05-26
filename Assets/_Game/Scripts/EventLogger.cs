using System;
using System.Collections.Generic;
using UnityEngine;

public class EventLogger
{
	public static void LogScreen(string screen)
	{

	}

	public static void LogEvent(string eventName, params object[] args)
	{
		Debug.Log(eventName);
	}
}
