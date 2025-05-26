using System;
using UnityEngine;

public class SubStepUpgradeRamboToLevel2 : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepUpgradeRamboToLevel2, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
