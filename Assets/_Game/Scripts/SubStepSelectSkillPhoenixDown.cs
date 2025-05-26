using System;
using UnityEngine;

public class SubStepSelectSkillPhoenixDown : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepSelectSkillPhoenixDown, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
