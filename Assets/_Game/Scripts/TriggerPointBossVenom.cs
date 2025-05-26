using System;
using UnityEngine;

public class TriggerPointBossVenom : TriggerPointBoss
{
	public Transform furthestLaserPoint;

	public Transform nearestLaserPoint;

	protected override void SpawnBoss()
	{
		if (base.gameObject.activeInHierarchy)
		{
			BossVenom bossVenom = UnityEngine.Object.Instantiate<BaseEnemy>(this.bossPrefab) as BossVenom;
			bossVenom.isInvisibleWhenActive = true;
			bossVenom.FurthestLaserPoint = this.furthestLaserPoint;
			bossVenom.NearestLaserPoint = this.nearestLaserPoint;
			int level = base.GetLevel();
			bossVenom.Active(this.bossPrefab.id, level, this.spawnPosition.position);
			bossVenom.SetTarget(Singleton<GameController>.Instance.Player);
			Singleton<GameController>.Instance.AddUnit(bossVenom.gameObject, bossVenom);
			Singleton<UIController>.Instance.hudBoss.UpdateHP(1f);
			base.SwitchMusic();
		}
	}
}
