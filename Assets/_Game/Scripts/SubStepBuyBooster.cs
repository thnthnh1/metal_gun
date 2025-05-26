using System;
using UnityEngine;

public class SubStepBuyBooster : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepBuyBooster, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
