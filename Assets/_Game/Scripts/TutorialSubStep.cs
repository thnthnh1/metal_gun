using System;
using UnityEngine;

public class TutorialSubStep : MonoBehaviour
{
	public TutorialType type;

	public int stepIndex;

	public virtual void Init()
	{
	}

	public virtual void StartSubStep()
	{
		base.gameObject.SetActive(true);
	}

	public virtual void Next()
	{
		SoundManager.Instance.PlaySfxClick();
		TutorialSubStepData param = new TutorialSubStepData(this.type, this.stepIndex);
		EventDispatcher.Instance.PostEvent(EventID.CompleteSubStep, param);
		base.gameObject.SetActive(false);
	}
}
