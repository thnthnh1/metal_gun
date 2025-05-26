using System;
using UnityEngine;

public class TriggerPointBossProfessor : TriggerPointBoss
{
	public Transform minionSpawnLeftPoint;

	public Transform minionSpawnRightPoint;

	protected override void SpawnBoss()
	{
		if (base.gameObject.activeInHierarchy)
		{
			BossProfessor bossProfessor = UnityEngine.Object.Instantiate<BaseEnemy>(this.bossPrefab) as BossProfessor;
			bossProfessor.basePosition = this.basePosition.position;
			bossProfessor.SetPointSpawnMinion(this.minionSpawnLeftPoint.position, this.minionSpawnRightPoint.position);
			int level = base.GetLevel();
			bossProfessor.Active(this.bossPrefab.id, level, this.spawnPosition.position);
			bossProfessor.SetTarget(Singleton<GameController>.Instance.Player);
			Singleton<GameController>.Instance.AddUnit(bossProfessor.gameObject, bossProfessor);
			Singleton<UIController>.Instance.hudBoss.UpdateHP(1f);
			base.SwitchMusic();
		}
	}
}
