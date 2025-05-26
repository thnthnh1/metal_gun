using System;
using UnityEngine;

public class SubStepClickMission : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepClickMission, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
