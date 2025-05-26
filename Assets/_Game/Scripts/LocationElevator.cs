using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LocationElevator : BaseSpawnLocation
{
	private Vector2 startPosition;

	private Vector2 endPosition;

	private List<BaseEnemy> units = new List<BaseEnemy>();

	private void Awake()
	{
		this.startPosition = base.transform.position;
		Vector2 vector = this.startPosition;
		vector.y -= 7.5f;
		this.endPosition = vector;
	}

	public override void Spawn()
	{
		this.SpawnUnitOnElevator();
		this.MoveDown();
	}

	private void SpawnUnitOnElevator()
	{
		for (int i = 0; i < this.spawnUnits.Count; i++)
		{
			int id = (int)this.spawnUnits[i];
			int level = UnityEngine.Random.Range(this.minLevelUnit, this.maxLevelUnit + 1);
			BaseEnemy enemyPrefab = Singleton<GameController>.Instance.modeController.GetEnemyPrefab((int)this.spawnUnits[i]);
			BaseEnemy fromPool = enemyPrefab.GetFromPool();
			Vector2 position = this.spawnPoint.position;
			position.x += UnityEngine.Random.Range(-1.5f, 1.5f);
			fromPool.farSensor.col.radius = 30f;
			fromPool.Active(id, level, position);
			fromPool.transform.parent = base.transform;
			fromPool.canMove = true;
			fromPool.canJump = false;
			fromPool.isRunPassArea = true;
			fromPool.ActiveSensor(false);
			fromPool.isImmortal = true;
			fromPool.Rigid.simulated = false;
			fromPool.skeletonAnimation.Skeleton.FlipX = (UnityEngine.Random.Range(0, 2) == 0);
			fromPool.PlayAnimationIdle();
			fromPool.enabled = false;
			this.units.Add(fromPool);
			Singleton<GameController>.Instance.AddUnit(fromPool.gameObject, fromPool);
			((SurvivalModeController)Singleton<GameController>.Instance.modeController).AddUnit(fromPool);
		}
		base.Clear();
	}

	private void MoveDown()
	{
		this.isSpawning = true;
		float y = this.endPosition.y;
		base.transform.DOMoveY(y, 3f, false).OnComplete(delegate
		{
			for (int i = 0; i < this.units.Count; i++)
			{
				BaseEnemy baseEnemy = this.units[i];
				baseEnemy.transform.parent = null;
				baseEnemy.isImmortal = false;
				baseEnemy.Rigid.simulated = true;
				baseEnemy.ActiveSensor(true);
				baseEnemy.enabled = true;
			}
			this.units.Clear();
			this.MoveUp(2f);
		});
	}

	private void MoveUp(float delay)
	{
		float y = this.startPosition.y;
		base.transform.DOMoveY(y, 2f, false).SetDelay(2f).OnComplete(delegate
		{
			this.isSpawning = false;
		});
	}
}
