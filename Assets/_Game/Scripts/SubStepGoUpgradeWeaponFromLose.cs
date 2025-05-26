using System;
using UnityEngine;

public class SubStepGoUpgradeWeaponFromLose : TutorialSubStep
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.SubStepGoUpgradeWeaponFromLose, delegate(Component sender, object param)
		{
			this.Next();
		});
	}
}
