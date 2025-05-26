using System;
using UnityEngine;

public class SubStepSelectUzi : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectUzi, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
