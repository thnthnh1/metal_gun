using System;
using UnityEngine;

public class TutorialStep : MonoBehaviour
{
	public TutorialType type;

	public TutorialSubStep[] subSteps;

	private int curSubStepIndex;

	public virtual void Init()
	{
		this.curSubStepIndex = 0;
		for (int i = 0; i < this.subSteps.Length; i++)
		{
			this.subSteps[i].type = this.type;
			this.subSteps[i].stepIndex = i;
			this.subSteps[i].Init();
		}
		EventDispatcher.Instance.RegisterListener(EventID.CompleteSubStep, delegate(Component sender, object param)
		{
			this.OnSubStepComplete((TutorialSubStepData)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.CompleteStep, delegate(Component sender, object param)
		{
			if ((TutorialType)param == this.type)
			{
				this.Complete();
			}
		});
	}

	public virtual void Active(bool isActive)
	{
		if (isActive)
		{
			GameData.isShowingTutorial = true;
			base.gameObject.SetActive(true);
			this.StartCurrentSubStep();
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	public virtual void Skip()
	{
		this.Complete();
		EventLogger.LogEvent("SkipTut", new object[]
		{
			this.type.ToString()
		});
	}

	public virtual void Complete()
	{
		GameData.isShowingTutorial = false;
		GameData.playerTutorials.SetComplete(this.type);
		this.Active(false);
	}

	public virtual void OnSubStepComplete(TutorialSubStepData data)
	{
		if (data.type == this.type)
		{
			if (this.curSubStepIndex == this.subSteps.Length - 1)
			{
				this.Complete();
				EventLogger.LogEvent("CompleteTut", new object[]
				{
					this.type.ToString()
				});
			}
			else
			{
				this.curSubStepIndex++;
				this.StartCurrentSubStep();
			}
		}
	}

	private void StartCurrentSubStep()
	{
		for (int i = 0; i < this.subSteps.Length; i++)
		{
			if (this.curSubStepIndex == i)
			{
				this.subSteps[i].StartSubStep();
			}
			else
			{
				this.subSteps[i].gameObject.SetActive(false);
			}
		}
	}
}
