using System;
using UnityEngine;

public class TriggerPointBomber : MonoBehaviour
{
	public EnemyBomber bomberPrefab;

	public int levelInNormal = 1;

	public bool isFromLeft;

	private BoxCollider2D sensor;

	private void Awake()
	{
		this.sensor = base.GetComponent<BoxCollider2D>();
		if (this.sensor != null)
		{
			this.sensor.enabled = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			this.SpawnBombardier();
			this.sensor.enabled = false;
		}
	}

	private void SpawnBombardier()
	{
		BaseEnemy fromPool = this.bomberPrefab.GetFromPool();
		Vector2 position = (!this.isFromLeft) ? Singleton<CameraFollow>.Instance.pointAirSpawnRight.position : Singleton<CameraFollow>.Instance.pointAirSpawnLeft.position;
		int levelEnemy = this.levelInNormal;
		if (GameData.mode == GameMode.Campaign)
		{
			levelEnemy = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);
		}
		((EnemyBomber)fromPool).isFromLeft = this.isFromLeft;
		((EnemyBomber)fromPool).Active(this.bomberPrefab.id, levelEnemy, position);
		Singleton<GameController>.Instance.AddUnit(fromPool.gameObject, fromPool);
	}
}
