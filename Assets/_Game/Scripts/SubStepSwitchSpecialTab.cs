using System;
using UnityEngine;

public class SubStepSwitchSpecialTab : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepSwitchSpecialTab, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
