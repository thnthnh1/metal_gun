using System;
using System.Diagnostics;
using UnityEngine;

public class DebugCustom
{
	[Conditional("DEBUG_ENABLE")]
	public static void Log(object content)
	{
		UnityEngine.Debug.Log(content.ToString());
	}

	[Conditional("DEBUG_ENABLE")]
	public static void LogError(object content)
	{
		UnityEngine.Debug.Log(content.ToString());
		// UnityEngine.Debug.LogError(content.ToString());
	}

	[Conditional("DEBUG_ENABLE")]
	public static void LogWarning(object content)
	{
		// UnityEngine.Debug.LogWarning(content.ToString());
		UnityEngine.Debug.Log(content.ToString());
	}

	[Conditional("DEBUG_ENABLE")]
	public static void LogException(Exception e)
	{
		UnityEngine.Debug.LogException(e);
	}
}
