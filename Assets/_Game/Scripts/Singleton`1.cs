using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	public static T Instance
	{
		get
		{
			if (Singleton<T>.instance == null)
			{
				Singleton<T>.instance = (UnityEngine.Object.FindObjectOfType(typeof(T)) as T);
				if (Singleton<T>.instance == null)
				{
					Singleton<T>.instance = new GameObject().AddComponent<T>();
					Singleton<T>.instance.gameObject.name = Singleton<T>.instance.GetType().Name;
				}
			}
			return Singleton<T>.instance;
		}
	}

	public void Reset()
	{
		Singleton<T>.instance = (T)((object)null);
	}

	public static bool Exists()
	{
		return Singleton<T>.instance != null;
	}
}
