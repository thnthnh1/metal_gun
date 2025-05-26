using System;
using UnityEngine;

public class StarAnimationEvent : MonoBehaviour
{
	public void OneStarAnimationFinish()
	{
		SoundManager.Instance.PlaySfx("sfx_get_1_star", 0f);
		Singleton<CameraFollow>.Instance.AddShake(0.3f, 0.15f);
	}

	public void TwoStarAnimationFinish()
	{
		SoundManager.Instance.PlaySfx("sfx_get_2_star", 0f);
		Singleton<CameraFollow>.Instance.AddShake(0.3f, 0.15f);
	}

	public void ThreeStarAnimationFinish()
	{
		SoundManager.Instance.PlaySfx("sfx_get_3_star", 0f);
		Singleton<CameraFollow>.Instance.AddShake(0.3f, 0.15f);
	}
}
