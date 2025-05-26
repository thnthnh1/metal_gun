using System;
using UnityEngine;

public class SubStepSelectBooster : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectBooster, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
