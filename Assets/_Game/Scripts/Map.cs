using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Map : MonoBehaviour
{
	public string stageNameId;

	public ControllerType controllerType;

	public Zone[] mainZones;

	[Header("PLAYER")]
	public bool isRamboStartOnJet;

	public Transform jetStartPoint;

	public Transform jetDestination;

	public Transform playerSpawnPoint;

	[Header("ENEMY")]
	public bool isAutoSpawnEnemy = true;

	public float timeAutoSpawn = 5f;

	public int minLevelAutoSpawn = 1;

	public int maxLevelAutoSpawn = 3;

	public int enemyPerSpawn = 1;

	public BaseEnemy[] enemyPrefabs;

	public BaseEnemy[] enemyAutoSpawnPrefabs;

	private int previousZoneId = 1;

	private int remainingMainUnit;

	private Dictionary<int, int> mainEnemies = new Dictionary<int, int>();

	private List<int> passedZones = new List<int>();

	[Header("MAP DATA")]
	public MapData mapData;

	[Header("MARGIN")]
	public Transform marginLeft;

	public Transform marginTop;

	public Transform marginRight;

	public Transform marginBottom;

	public Transform mapEndPoint;

	public Transform cameraInitialPoint;

	private int _CurrentZoneId_k__BackingField;

	private int _CoinCompleteStage_k__BackingField;

	private static UnityAction __f__am_cache0;

	public int CurrentZoneId
	{
		get;
		set;
	}

	public int CoinCompleteStage
	{
		get;
		set;
	}

	public void Init()
	{
		this.SetDefaultMapMargin();
		this.LoadMapData();
		this.SetCoinDrop();
		this.CreateTriggerPoints();
		this.InitFirstZone();
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, new Action<Component, object>(this.OnUnitDie));
		EventDispatcher.Instance.RegisterListener(EventID.EnterZone, new Action<Component, object>(this.OnEnterZone));
	}

	public void LockCurrentZone()
	{
		for (int i = 0; i < this.mainZones.Length; i++)
		{
			Zone zone = this.mainZones[i];
			if (zone.id == this.CurrentZoneId)
			{
				if (this.remainingMainUnit <= 0)
				{
					this.SetDefaultMapMargin();
					zone.wallEnd.gameObject.SetActive(false);
				}
				else
				{
					zone.Lock();
				}
				return;
			}
		}
	}

	public void SetDefaultMapMargin()
	{
		Singleton<CameraFollow>.Instance.SetMarginTop(this.marginTop.position.y);
		Singleton<CameraFollow>.Instance.SetMarginLeft(this.marginLeft.position.x);
		Singleton<CameraFollow>.Instance.SetMarginRight(this.marginRight.position.x);
		Singleton<CameraFollow>.Instance.SetMarginBottom(this.marginBottom.position.y);
	}

	public void AddMainUnitToCurrentZone(BaseUnit unit)
	{
		if (this.mainEnemies.ContainsKey(this.CurrentZoneId))
		{
			Dictionary<int, int> dictionary;
			int currentZoneId;
			(dictionary = this.mainEnemies)[currentZoneId = this.CurrentZoneId] = dictionary[currentZoneId] + 1;
		}
		else
		{
			this.mainEnemies.Add(this.CurrentZoneId, 1);
		}
		this.remainingMainUnit++;
	}

	public Zone GetCurrentZone()
	{
		int num = this.mainZones.Length;
		for (int i = 0; i < this.mainZones.Length; i++)
		{
			if (this.mainZones[i].id == this.CurrentZoneId)
			{
				return this.mainZones[i];
			}
		}
		return null;
	}

	private int GetZoneIndex(int zoneId)
	{
		int result = this.mainZones.Length;
		for (int i = 0; i < this.mainZones.Length; i++)
		{
			if (this.mainZones[i].id == zoneId)
			{
				result = i;
				break;
			}
		}
		return result;
	}

	private void InitFirstZone()
	{
		this.CurrentZoneId = 1;
		this.CalculateMainUnits();
		this.LockCurrentZone();
	}

	private void CreateTriggerPoints()
	{
		TriggerPointBomber original = Resources.Load<TriggerPointBomber>("Objects/trigger_point_bomber");
		TriggerPointHelicopter original2 = Resources.Load<TriggerPointHelicopter>("Objects/trigger_point_helicopter");
		if (this.mapData.bomberData != null)
		{
			GameObject gameObject = new GameObject("Bomber Trigger Points");
			gameObject.transform.parent = base.transform;
			for (int i = 0; i < this.mapData.bomberData.Count; i++)
			{
				BomberPointData bomberPointData = this.mapData.bomberData[i];
				TriggerPointBomber triggerPointBomber = UnityEngine.Object.Instantiate<TriggerPointBomber>(original, gameObject.transform);
				triggerPointBomber.transform.position = bomberPointData.position;
				triggerPointBomber.levelInNormal = bomberPointData.levelInNormal;
				triggerPointBomber.isFromLeft = bomberPointData.isFromLeft;
			}
		}
		if (this.mapData.helicopterData != null)
		{
			GameObject gameObject2 = new GameObject("Helicopter Trigger Points");
			gameObject2.transform.parent = base.transform;
			for (int j = 0; j < this.mapData.helicopterData.Count; j++)
			{
				HelicopterPointData helicopterPointData = this.mapData.helicopterData[j];
				TriggerPointHelicopter triggerPointHelicopter = UnityEngine.Object.Instantiate<TriggerPointHelicopter>(original2, gameObject2.transform);
				triggerPointHelicopter.transform.position = helicopterPointData.position;
				triggerPointHelicopter.levelInNormal = helicopterPointData.levelInNormal;
				triggerPointHelicopter.isFinalBoss = helicopterPointData.isFinalBoss;
			}
		}
	}

	private void CalculateMainUnits()
	{
		if (this.mainEnemies.ContainsKey(this.CurrentZoneId))
		{
			this.remainingMainUnit = this.mainEnemies[this.CurrentZoneId];
		}
		else
		{
			this.remainingMainUnit = 0;
		}
	}

	private void LoadMapData()
	{
		this.mapData = MapUtils.GetMapData(this.stageNameId);
		if (this.mapData.enemyData == null)
		{
			return;
		}
		for (int i = 0; i < this.mapData.enemyData.Count; i++)
		{
			EnemySpawnData enemySpawnData = this.mapData.enemyData[i];
			if (enemySpawnData.isMainUnit)
			{
				if (this.mainEnemies.ContainsKey(enemySpawnData.zoneId))
				{
					Dictionary<int, int> dictionary;
					int zoneId;
					(dictionary = this.mainEnemies)[zoneId = enemySpawnData.zoneId] = dictionary[zoneId] + 1;
				}
				else
				{
					this.mainEnemies.Add(enemySpawnData.zoneId, 1);
				}
			}
		}
	}

	private void SetCoinDrop()
	{
		this.CoinCompleteStage = GameData.staticCampaignStageData.GetCoinCompleteStage(GameData.currentStage.id, GameData.currentStage.difficulty);
		if (this.mapData.enemyData == null)
		{
			return;
		}
		int coinDrop = GameData.staticCampaignStageData.GetCoinDrop(GameData.currentStage.id, GameData.currentStage.difficulty);
		int count = this.mapData.enemyData.Count;
		int i = Mathf.RoundToInt((float)count * 0.7f);
		int num = (i == 0) ? 0 : Mathf.RoundToInt((float)coinDrop / (float)i);
		List<int> list = new List<int>();
		for (int j = 0; j < count; j++)
		{
			list.Add(this.mapData.enemyData[j].index);
		}
		for (int k = 0; k < count; k++)
		{
			EnemySpawnData enemySpawnData = this.mapData.enemyData[k];
			if (enemySpawnData.isMainUnit)
			{
				int num2 = Mathf.RoundToInt((float)num * 1f);
				if (num2 <= 0)
				{
					num2 = 1;
				}
				enemySpawnData.bounty = num2;
				i--;
				list.Remove(enemySpawnData.index);
			}
		}
		while (i > 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			int num3 = num;
			if (num3 <= 0)
			{
				num3 = 1;
			}
			this.mapData.enemyData[list[index]].bounty = num3;
			list.Remove(list[index]);
			i--;
		}
	}

	private void OnEnterZone(Component senser, object param)
	{
		int currentZoneId = (int)param;
		this.previousZoneId = this.CurrentZoneId;
		this.CurrentZoneId = currentZoneId;
		this.CalculateMainUnits();
		if (this.remainingMainUnit <= 0)
		{
			this.OnCurrentZoneClear(null);
		}
		this.passedZones.Add(this.previousZoneId);
		this.ClearUnitPreviousZones();
	}

	private void OnUnitDie(Component senser, object param)
	{
		if (this.mainZones.Length <= 0)
		{
			return;
		}
		UnitDieData unitDieData = (UnitDieData)param;
		BaseEnemy component = unitDieData.unit.GetComponent<BaseEnemy>();
		if (component != null && component.isMainUnit)
		{
			if (this.mainEnemies.ContainsKey(component.zoneId))
			{
				Dictionary<int, int> dictionary;
				int zoneId;
				(dictionary = this.mainEnemies)[zoneId = component.zoneId] = dictionary[zoneId] - 1;
			}
			if (component.zoneId == this.CurrentZoneId)
			{
				this.remainingMainUnit--;
				if (this.remainingMainUnit <= 0)
				{
					this.OnCurrentZoneClear(component);
				}
			}
		}
	}

	public void OnCurrentZoneClear(BaseUnit lastEnemy = null)
	{
		this.remainingMainUnit = 0;
		Zone currentZone = this.GetCurrentZone();
		currentZone.ShowObjects(true);
		if (!currentZone.isLockWallEndWhenClear)
		{
			this.SetDefaultMapMargin();
		}
		if (currentZone.isFinalZone)
		{
			int zoneIndex = this.GetZoneIndex(this.previousZoneId);
			this.mainZones[zoneIndex].wallEnd.gameObject.SetActive(false);
			currentZone.Lock();
			Singleton<UIController>.Instance.ActiveIngameUI(false);
			if (lastEnemy != null)
			{
				Singleton<CameraFollow>.Instance.slowMotion.Show(3.5f, delegate
				{
					EventDispatcher.Instance.PostEvent(EventID.FinishStage, 0.5f);
				});
			}
			else
			{
				EventDispatcher.Instance.PostEvent(EventID.FinishStage, 0.5f);
			}
		}
		else if (!currentZone.isLockWallEndWhenClear)
		{
			currentZone.wallEnd.gameObject.SetActive(false);
			EventDispatcher.Instance.PostEvent(EventID.MoveCameraToNewZone, this.CurrentZoneId);
			if (currentZone.wallEndLockDir == CameraLockDirection.Right)
			{
				Singleton<UIController>.Instance.ShowArrowGo(true);
			}
			else if (currentZone.wallEndLockDir == CameraLockDirection.Left)
			{
				Singleton<UIController>.Instance.ShowArrowGo(false);
			}
		}
	}

	private void ClearUnitPreviousZones()
	{
		List<BaseUnit> list = new List<BaseUnit>();
		foreach (BaseUnit current in Singleton<GameController>.Instance.activeUnits.Values)
		{
			if (current is BaseEnemy && (this.passedZones.Contains(((BaseEnemy)current).zoneId) || ((BaseEnemy)current).zoneId == -1) && current.IsOutOfScreen())
			{
				list.Add(current);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].Deactive();
		}
	}
}
