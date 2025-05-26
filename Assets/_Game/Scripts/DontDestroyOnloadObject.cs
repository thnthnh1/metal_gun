using System;
using UnityEngine;

public class DontDestroyOnloadObject : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}
}
