using System;
using UnityEngine;

public class SubStepUnlockSkillPhoenixDown : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepUnlockSkillPhoenixDown, delegate(Component sender, object param)
		{
			this.Next();
			GameData.playerTutorials.SetComplete(TutorialType.Character);
		});
	}
}
