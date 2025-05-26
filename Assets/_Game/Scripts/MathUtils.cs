using System;
using UnityEngine;

public class MathUtils
{
	private const float DOUBLE_PI = 6.28318548f;

	private const float HALF_PI = 1.57079637f;

	public static float FastSin(float angle)
	{
		if (angle < -3.14159274f)
		{
			angle += 6.28318548f;
		}
		else if (angle > 3.14159274f)
		{
			angle -= 6.28318548f;
		}
		float num;
		if (angle < 0f)
		{
			num = angle * (1.27323949f + 0.405284733f * angle);
			if (num < 0f)
			{
				num = 0.225f * (num * -num - num) + num;
			}
			else
			{
				num = 0.225f * (num * num - num) + num;
			}
		}
		else
		{
			num = angle * (1.27323949f - 0.405284733f * angle);
			if (num < 0f)
			{
				num = 0.225f * (num * -num - num) + num;
			}
			else
			{
				num = 0.225f * (num * num - num) + num;
			}
		}
		return num;
	}

	public static float FastCos(float angle)
	{
		if (angle < -3.14159274f)
		{
			angle += 6.28318548f;
		}
		else if (angle > 3.14159274f)
		{
			angle -= 6.28318548f;
		}
		angle += 1.57079637f;
		if (angle > 3.14159274f)
		{
			angle -= 6.28318548f;
		}
		float num;
		if (angle < 0f)
		{
			num = angle * (1.27323949f + 0.405284733f * angle);
			if (num < 0f)
			{
				num = 0.225f * (num * -num - num) + num;
			}
			else
			{
				num = 0.225f * (num * num - num) + num;
			}
		}
		else
		{
			num = angle * (1.27323949f - 0.405284733f * angle);
			if (num < 0f)
			{
				num = 0.225f * (num * -num - num) + num;
			}
			else
			{
				num = 0.225f * (num * num - num) + num;
			}
		}
		return num;
	}

	public static float CalculateLaunchSpeed(float distance, float yOffset, float gravity, float angle)
	{
		return distance * Mathf.Sqrt(gravity) * Mathf.Sqrt(1f / Mathf.Cos(angle)) / Mathf.Sqrt(2f * distance * Mathf.Sin(angle) + 2f * yOffset * Mathf.Cos(angle));
	}

	public static Vector3 ProjectVectorOnPlane(Vector3 planeNormal, Vector3 vector)
	{
		return vector - Vector3.Dot(vector, planeNormal) * planeNormal;
	}
}
