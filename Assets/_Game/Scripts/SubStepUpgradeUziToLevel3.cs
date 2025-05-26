using System;
using UnityEngine;

public class SubStepUpgradeUziToLevel3 : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepUpgradeUziTolevel3, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
