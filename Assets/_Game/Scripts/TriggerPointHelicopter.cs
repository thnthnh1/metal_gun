using System;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPointHelicopter : MonoBehaviour
{
	public EnemyHelicopter helicopterPrefabs;

	public Collider2D wallStart;

	public Collider2D wallEnd;

	public int levelInNormal = 1;

	public bool isFinalBoss;

	private Collider2D sensor;

	private Dictionary<GameObject, EnemyHelicopter> activeUnits = new Dictionary<GameObject, EnemyHelicopter>();

	private void Awake()
	{
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, new Action<Component, object>(this.OnUnitDie));
		this.sensor = base.GetComponent<BoxCollider2D>();
		if (this.sensor != null)
		{
			this.sensor.enabled = true;
		}
		this.wallStart.gameObject.SetActive(false);
		this.wallEnd.gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			this.SpawnHelicopter();
			this.LockArea(true);
			this.sensor.enabled = false;
		}
	}

	private void LockArea(bool isLock)
	{
		this.wallStart.gameObject.SetActive(isLock);
		this.wallEnd.gameObject.SetActive(isLock);
		if (isLock)
		{
			Singleton<CameraFollow>.Instance.SetMarginLeft(this.wallStart.transform.position.x);
			Singleton<CameraFollow>.Instance.SetMarginRight(this.wallEnd.transform.position.x);
		}
		else
		{
			Singleton<CameraFollow>.Instance.SetMarginRight(this.wallEnd.transform.position.x);
		}
	}

	private void SpawnHelicopter()
	{
		BaseEnemy fromPool = this.helicopterPrefabs.GetFromPool();
		EnemyHelicopter enemyHelicopter = (EnemyHelicopter)fromPool;
		Vector2 position = Singleton<CameraFollow>.Instance.pointAirSpawnRight.position;
		int num = this.levelInNormal;
		if (GameData.mode == GameMode.Campaign)
		{
			if (GameData.currentStage.difficulty == Difficulty.Hard)
			{
				num += 2;
			}
			else if (GameData.currentStage.difficulty == Difficulty.Crazy)
			{
				num += 7;
			}
			num = Mathf.Clamp(num, 1, 20);
		}
		enemyHelicopter.Active(this.helicopterPrefabs.id, num, position);
		enemyHelicopter.GetNextDestination();
		enemyHelicopter.isMainUnit = true;
		enemyHelicopter.zoneId = Singleton<GameController>.Instance.CampaignMap.CurrentZoneId;
		enemyHelicopter.isMiniBoss = this.isFinalBoss;
		enemyHelicopter.SetTarget(Singleton<GameController>.Instance.Player);
		this.activeUnits.Add(enemyHelicopter.gameObject, enemyHelicopter);
		Singleton<GameController>.Instance.AddUnit(enemyHelicopter.gameObject, enemyHelicopter);
		Singleton<GameController>.Instance.CampaignMap.AddMainUnitToCurrentZone(enemyHelicopter);
	}

	private void OnUnitDie(Component senser, object param)
	{
		UnitDieData unitDieData = (UnitDieData)param;
		BaseEnemy component = unitDieData.unit.GetComponent<BaseEnemy>();
		if (this.activeUnits.ContainsKey(component.gameObject))
		{
			base.CancelInvoke();
			this.activeUnits.Remove(component.gameObject);
			if (this.activeUnits.Count <= 0)
			{
				this.LockArea(false);
				Singleton<GameController>.Instance.CampaignMap.SetDefaultMapMargin();
			}
		}
	}
}
