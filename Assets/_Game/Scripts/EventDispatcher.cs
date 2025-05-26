using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher : MonoBehaviour
{
	private static EventDispatcher s_instance;

	private Dictionary<EventID, List<Action<Component, object>>> _listenersDict = new Dictionary<EventID, List<Action<Component, object>>>();

	public static bool IsNull
	{
		get
		{
			return EventDispatcher.s_instance == null;
		}
	}

	public static EventDispatcher Instance
	{
		get
		{
			if (EventDispatcher.s_instance == null)
			{
				GameObject gameObject = new GameObject();
				EventDispatcher.s_instance = gameObject.AddComponent<EventDispatcher>();
				gameObject.name = "Singleton EventDispatcher";
			}
			return EventDispatcher.s_instance;
		}
		private set
		{
		}
	}

	private void Awake()
	{
		if (EventDispatcher.s_instance != null && EventDispatcher.s_instance.GetInstanceID() != base.GetInstanceID())
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			EventDispatcher.s_instance = this;
		}
	}

	private void OnDestroy()
	{
		if (EventDispatcher.s_instance == this)
		{
			EventDispatcher.s_instance = null;
		}
	}

	public void RegisterListener(EventID eventID, Action<Component, object> callback)
	{
		if (this._listenersDict.ContainsKey(eventID))
		{
			this._listenersDict[eventID].Add(callback);
		}
		else
		{
			List<Action<Component, object>> list = new List<Action<Component, object>>();
			list.Add(callback);
			this._listenersDict.Add(eventID, list);
		}
	}

	public void PostEvent(EventID eventID, Component sender, object param = null)
	{
		List<Action<Component, object>> list;
		if (this._listenersDict.TryGetValue(eventID, out list))
		{
			int i = 0;
			int num = list.Count;
			while (i < num)
			{
				try
				{
					list[i](sender, param);
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError(string.Format("Remove listener at {0} that cause the exception, message: {1}", eventID.ToString(), ex.Message));
					list.RemoveAt(i);
					if (list.Count == 0)
					{
						this._listenersDict.Remove(eventID);
					}
					num--;
					i--;
				}
				i++;
			}
		}
	}

	public void RemoveListener(EventID eventID, Action<Component, object> callback)
	{
		List<Action<Component, object>> list;
		if (this._listenersDict.TryGetValue(eventID, out list))
		{
			if (list.Contains(callback))
			{
				list.Remove(callback);
				if (list.Count == 0)
				{
					this._listenersDict.Remove(eventID);
				}
			}
		}
	}

	public void RemoveRedundancies()
	{
		foreach (KeyValuePair<EventID, List<Action<Component, object>>> current in this._listenersDict)
		{
			List<Action<Component, object>> value = current.Value;
			int count = value.Count;
			for (int i = count - 1; i >= 0; i--)
			{
				Action<Component, object> action = value[i];
				if (action == null || action.Target.Equals(null))
				{
					value.RemoveAt(i);
					if (value.Count == 0)
					{
						this._listenersDict.Remove(current.Key);
					}
					i--;
				}
			}
		}
	}

	public void ClearAllListener()
	{
		this._listenersDict.Clear();
	}

	public int ListenerCount()
	{
		return this._listenersDict.Count;
	}
}
