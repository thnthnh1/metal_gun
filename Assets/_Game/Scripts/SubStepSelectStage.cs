using System;
using UnityEngine;

public class SubStepSelectStage : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectStage, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
