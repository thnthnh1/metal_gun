using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class SlowMotion : MonoBehaviour
{
	private UnityAction endSlowMotionCallback;

	private float duration;

	private float timer;

	private bool _IsShowing_k__BackingField;

	public bool IsShowing
	{
		get;
		set;
	}

	private void LateUpdate()
	{
		if (this.IsShowing)
		{
			Time.fixedDeltaTime = 0.02f * Time.timeScale;
			this.timer += Time.deltaTime * 5f;
			if (this.timer >= this.duration)
			{
				Time.timeScale = 1f;
				Time.fixedDeltaTime = 0.02f;
				this.IsShowing = false;
				if (this.endSlowMotionCallback != null)
				{
					this.endSlowMotionCallback();
					this.endSlowMotionCallback = null;
				}
			}
		}
	}

	public void Show(float duration = 3.5f, UnityAction callback = null)
	{
		this.IsShowing = true;
		this.endSlowMotionCallback = callback;
		Singleton<CameraFollow>.Instance.SetSlowMotion();
		this.duration = duration;
		this.timer = 0f;
		Time.timeScale = 0.2f;
	}

	private void Reset()
	{
		Singleton<CameraFollow>.Instance.ResetCameraToPlayer();
	}
}
