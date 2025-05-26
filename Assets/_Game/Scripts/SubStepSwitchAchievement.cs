using System;
using UnityEngine;

public class SubStepSwitchAchievement : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepClickAchievement, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
