using System;
using UnityEngine;

public class TriggerPointBossMonkey : TriggerPointBoss
{
	public Transform minionSpawnPoint;

	public Transform minionMostLeftPoint;

	public Transform minionMostRightPoint;

	protected override void SpawnBoss()
	{
		if (base.gameObject.activeInHierarchy)
		{
			BossMonkey bossMonkey = UnityEngine.Object.Instantiate<BaseEnemy>(this.bossPrefab) as BossMonkey;
			bossMonkey.basePosition = this.basePosition.position;
			bossMonkey.SetPoints(this.minionSpawnPoint.position, this.minionMostLeftPoint.position, this.minionMostRightPoint.position);
			int level = base.GetLevel();
			bossMonkey.Active(this.bossPrefab.id, level, this.spawnPosition.position);
			bossMonkey.SetTarget(Singleton<GameController>.Instance.Player);
			Singleton<GameController>.Instance.AddUnit(bossMonkey.gameObject, bossMonkey);
			Singleton<UIController>.Instance.hudBoss.UpdateHP(1f);
			base.SwitchMusic();
		}
	}
}
