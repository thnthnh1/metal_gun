using System;
using UnityEngine;

public class EventAnimSurvivalWaveCompleteLabel : MonoBehaviour
{
	public void OnComplete()
	{
		base.gameObject.SetActive(false);
		EventDispatcher.Instance.PostEvent(EventID.LabelWaveAnimateComplete);
	}

	public void ShakeCamera()
	{
		Singleton<CameraFollow>.Instance.AddShake(1.5f, 0.5f);
	}
}
