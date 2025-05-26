using System;
using UnityEngine;

public class SubStepEnterSkillTree : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepEnterSkillTree, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
