using System;

public class TutorialMenuController : Singleton<TutorialMenuController>
{
	public TutorialStep[] mainSteps;

	private void Awake()
	{
		this.Init();
	}

	public void ShowTutorial(TutorialType type)
	{
		for (int i = 0; i < this.mainSteps.Length; i++)
		{
			this.mainSteps[i].Active(this.mainSteps[i].type == type);
		}
	}

	private void Init()
	{
		for (int i = 0; i < this.mainSteps.Length; i++)
		{
			this.mainSteps[i].Init();
		}
	}
}
