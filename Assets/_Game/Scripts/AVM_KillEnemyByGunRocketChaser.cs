using System;
using UnityEngine;

public class AVM_KillEnemyByGunRocketChaser : BaseAchievement
{
	public override void Init()
	{
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, delegate(Component sender, object param)
		{
			UnitDieData unitDieData = (UnitDieData)param;
			if (unitDieData.attackData == null || unitDieData.attackData.weaponId == -1)
			{
				return;
			}
			StaticGunData data = GameData.staticGunData.GetData(unitDieData.attackData.weaponId);
			if (data != null && data.id == 101)
			{
				this.IncreaseProgress();
			}
		});
	}
}
