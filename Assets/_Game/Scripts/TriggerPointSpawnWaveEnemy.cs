using System;
using UnityEngine;

public class TriggerPointSpawnWaveEnemy : MonoBehaviour
{
	public int totalEnemies = 10;

	public int minLevel = 1;

	public int maxLevel = 3;

	public Transform spawnPointCenter;

	public BaseEnemy[] enemyPrefabs;

	private BoxCollider2D col;

	private void Awake()
	{
		this.col = base.GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			this.Spawn();
			this.col.enabled = false;
		}
	}

	private void Spawn()
	{
		for (int i = 0; i < this.totalEnemies; i++)
		{
			int num = UnityEngine.Random.Range(0, this.enemyPrefabs.Length);
			int id = this.enemyPrefabs[num].id;
			int level = UnityEngine.Random.Range(this.minLevel, this.maxLevel + 1);
			if (GameData.mode == GameMode.Campaign)
			{
				level = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);
			}
			BaseEnemy enemyPrefab = this.GetEnemyPrefab(id);
			BaseEnemy fromPool = enemyPrefab.GetFromPool();
			Vector2 position = this.spawnPointCenter.position;
			position.x += UnityEngine.Random.Range(-5f, 5f);
			fromPool.Active(id, level, position);
			fromPool.SetTarget(Singleton<GameController>.Instance.Player);
			Singleton<GameController>.Instance.AddUnit(fromPool.gameObject, fromPool);
		}
	}

	private BaseEnemy GetEnemyPrefab(int id)
	{
		for (int i = 0; i < this.enemyPrefabs.Length; i++)
		{
			BaseEnemy baseEnemy = this.enemyPrefabs[i];
			if (baseEnemy.id == id)
			{
				return baseEnemy;
			}
		}
		return null;
	}
}
