using System;
using UnityEngine;

public class SubStepGoUpgradeCharacterFromLose : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepGoUpgradeCharacterFromLose, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
