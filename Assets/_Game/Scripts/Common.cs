using System;
using System.Diagnostics;
using UnityEngine;

public class Common
{
	[Conditional("DEBUG_ENABLE")]
	public static void Log(object message)
	{
		UnityEngine.Debug.Log(message);
	}

	[Conditional("DEBUG_ENABLE")]
	public static void Log(string format, params object[] args)
	{
		UnityEngine.Debug.Log(string.Format(format, args));
	}

	[Conditional("DEBUG_ENABLE")]
	public static void LogWarning(object message, UnityEngine.Object context)
	{
		UnityEngine.Debug.LogWarning(message, context);
	}

	[Conditional("DEBUG_ENABLE")]
	public static void LogWarning(UnityEngine.Object context, string format, params object[] args)
	{
		UnityEngine.Debug.LogWarning(string.Format(format, args), context);
	}

	[Conditional("DEBUG_ENABLE")]
	public static void Warning(bool condition, object message)
	{
		if (!condition)
		{
			UnityEngine.Debug.LogWarning(message);
		}
	}

	[Conditional("DEBUG_ENABLE")]
	public static void Warning(bool condition, object message, UnityEngine.Object context)
	{
		if (!condition)
		{
			UnityEngine.Debug.LogWarning(message, context);
		}
	}

	[Conditional("DEBUG_ENABLE")]
	public static void Warning(bool condition, UnityEngine.Object context, string format, params object[] args)
	{
		if (!condition)
		{
			UnityEngine.Debug.LogWarning(string.Format(format, args), context);
		}
	}

	[Conditional("ASSERT")]
	public static void Assert(bool condition)
	{
		if (!condition)
		{
			throw new UnityException();
		}
	}

	[Conditional("ASSERT")]
	public static void Assert(bool condition, string message)
	{
		if (!condition)
		{
			throw new UnityException(message);
		}
	}

	[Conditional("ASSERT")]
	public static void Assert(bool condition, string format, params object[] args)
	{
		if (!condition)
		{
			throw new UnityException(string.Format(format, args));
		}
	}
}
