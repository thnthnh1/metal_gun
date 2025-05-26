using System;
using UnityEngine;

public class SubStepUpgradeUziToLevel2 : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepUpgradeUziTolevel2, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
