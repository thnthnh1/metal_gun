using System;

public class TutorialSubStepData
{
	public TutorialType type;

	public int stepIndex;

	public TutorialSubStepData(TutorialType type, int stepIndex)
	{
		this.type = type;
		this.stepIndex = stepIndex;
	}
}
