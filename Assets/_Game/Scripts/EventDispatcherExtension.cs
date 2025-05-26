using System;
using UnityEngine;

public static class EventDispatcherExtension
{
	public static void RegisterListener(this MonoBehaviour sender, EventID eventID, Action<Component, object> callback)
	{
		EventDispatcher.Instance.RegisterListener(eventID, callback);
	}

	public static void PostEvent(this MonoBehaviour sender, EventID eventID, object param)
	{
		EventDispatcher.Instance.PostEvent(eventID, sender, param);
	}

	public static void PostEvent(this MonoBehaviour sender, EventID eventID)
	{
		EventDispatcher.Instance.PostEvent(eventID, sender, null);
	}
}
