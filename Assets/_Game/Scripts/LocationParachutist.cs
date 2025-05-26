using System;
using UnityEngine;

public class LocationParachutist : BaseSpawnLocation
{
	public Transform mostLeftPoint;

	public Transform mostRightPoint;

	public override void Spawn()
	{
		for (int i = 0; i < this.spawnUnits.Count; i++)
		{
			int id = (int)this.spawnUnits[i];
			int level = UnityEngine.Random.Range(this.minLevelUnit, this.maxLevelUnit + 1);
			BaseEnemy enemyPrefab = Singleton<GameController>.Instance.modeController.GetEnemyPrefab((int)this.spawnUnits[i]);
			BaseEnemy fromPool = enemyPrefab.GetFromPool();
			Vector2 position = this.mostLeftPoint.position;
			position.x = UnityEngine.Random.Range(this.mostLeftPoint.position.x, this.mostRightPoint.position.x);
			fromPool.farSensor.col.radius = 30f;
			fromPool.Active(id, level, position);
			fromPool.canMove = (UnityEngine.Random.Range(1, 101) > 70);
			fromPool.canJump = false;
			fromPool.isRunPassArea = true;
			fromPool.ActiveSensor(true);
			fromPool.isImmortal = false;
			Singleton<GameController>.Instance.AddUnit(fromPool.gameObject, fromPool);
			((SurvivalModeController)Singleton<GameController>.Instance.modeController).AddUnit(fromPool);
		}
		base.Clear();
	}
}
