using System;
using UnityEngine;

public class SubStepSelectKame : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectKame, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
