using System;

public class TutorialGamePlayController : Singleton<TutorialGamePlayController>
{
	public TutorialStep[] mainSteps;

	private void Awake()
	{
		this.Init();
	}

	public void ShowTutorialBooster()
	{
		for (int i = 0; i < this.mainSteps.Length; i++)
		{
			this.mainSteps[i].Active(this.mainSteps[i].type == TutorialType.Booster);
		}
	}

	public void ShowTutorialActionIngame()
	{
		for (int i = 0; i < this.mainSteps.Length; i++)
		{
			this.mainSteps[i].Active(this.mainSteps[i].type == TutorialType.ActionInGame);
		}
	}

	public void ShowTutorialRecommendUpgradeWeapon()
	{
		for (int i = 0; i < this.mainSteps.Length; i++)
		{
			this.mainSteps[i].Active(this.mainSteps[i].type == TutorialType.RecommendUpgradeWeapon);
		}
	}

	public void ShowTutorialRecommendUpgradeCharacter()
	{
		for (int i = 0; i < this.mainSteps.Length; i++)
		{
			this.mainSteps[i].Active(this.mainSteps[i].type == TutorialType.RecommendUpgradeCharacter);
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
