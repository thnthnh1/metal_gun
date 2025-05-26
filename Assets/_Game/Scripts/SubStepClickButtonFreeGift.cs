using System;
using UnityEngine;

public class SubStepClickButtonFreeGift : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepClickButtonFreeGift, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
