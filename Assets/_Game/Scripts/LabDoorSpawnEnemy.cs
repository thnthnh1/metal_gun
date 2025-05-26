using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LabDoorSpawnEnemy : MonoBehaviour
{
	private sealed class _CoroutineAlarm_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal LabDoorSpawnEnemy _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineAlarm_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				SoundManager.Instance.PlaySfx(this._this.soundAlarm, 0f);
				break;
			case 1u:
				SoundManager.Instance.PlaySfx(this._this.soundAlarm, 0f);
				break;
			default:
				return false;
			}
			if (this._this.isAlarm)
			{
				this._current = this._this.delayAlarm;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	public int totalUnits = 5;

	public int minLevel = 1;

	public int maxLevel = 3;

	public float timeBetweenSpawn = 3f;

	public float doorMoveSpeed = 2f;

	public Transform[] spawnPoints;

	public BaseEnemy[] enemyPrefabs;

	private CircleCollider2D sensor;

	private AudioClip soundAlarm;

	private WaitForSeconds delayAlarm;

	private Vector2 destination;

	private string methodNameSpawn = "SpawnUnit";

	private int unitSpawned;

	private int bountyPerUnit;

	private bool isAlarm;

	private bool isOpeningDoor;

	private Dictionary<GameObject, BaseUnit> activeUnits = new Dictionary<GameObject, BaseUnit>();

	private void Awake()
	{
		this.sensor = base.GetComponent<CircleCollider2D>();
		Vector2 vector = base.transform.position;
		vector.y += 1.36f;
		this.destination = vector;
	}

	private void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, new Action<Component, object>(this.OnUnitDie));
		this.InitAlarm();
		if (GameData.mode == GameMode.Campaign)
		{
			int coinDrop = GameData.staticCampaignStageData.GetCoinDrop(GameData.currentStage.id, GameData.currentStage.difficulty);
			this.bountyPerUnit = Mathf.RoundToInt((float)coinDrop * 0.1f / (float)this.totalUnits);
		}
	}

	private void Update()
	{
		if (this.isOpeningDoor)
		{
			if (Mathf.Abs(this.destination.y - base.transform.position.y) > 0.1f)
			{
				base.transform.position = Vector2.MoveTowards(base.transform.position, this.destination, this.doorMoveSpeed * Time.deltaTime);
			}
			else
			{
				base.transform.position = this.destination;
				this.isOpeningDoor = false;
				this.SpawnUnit();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.root.CompareTag("Player"))
		{
			this.OnAlarm();
		}
	}

	public void OnAlarm()
	{
		this.isOpeningDoor = true;
		this.isAlarm = true;
		base.StartCoroutine(this.CoroutineAlarm());
		this.sensor.enabled = false;
		SoundManager.Instance.PlaySfx("sfx_door_open", 0f);
	}

	private void InitAlarm()
	{
		this.soundAlarm = SoundManager.Instance.GetAudioClip("sfx_military_alarm");
		if (this.soundAlarm != null)
		{
			this.delayAlarm = new WaitForSeconds(this.soundAlarm.length + 1f);
		}
	}

	private void SpawnUnit()
	{
		if (this.unitSpawned >= this.totalUnits)
		{
			base.enabled = false;
			base.StopAllCoroutines();
			return;
		}
		Vector2 position = this.spawnPoints[UnityEngine.Random.Range(0, this.spawnPoints.Length)].position;
		if (GameData.mode == GameMode.Campaign)
		{
			int levelEnemy = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);
			int level = UnityEngine.Random.Range(1, levelEnemy + 1);
			BaseEnemy baseEnemy = this.enemyPrefabs[UnityEngine.Random.Range(0, this.enemyPrefabs.Length)];
			BaseEnemy fromPool = baseEnemy.GetFromPool();
			fromPool.isInvisibleWhenActive = true;
			fromPool.Active(baseEnemy.id, level, position);
			fromPool.canMove = true;
			fromPool.canJump = true;
			fromPool.isRunPassArea = true;
			fromPool.bounty = this.bountyPerUnit;
			Singleton<GameController>.Instance.AddUnit(fromPool.gameObject, fromPool);
			this.activeUnits.Add(fromPool.gameObject, fromPool);
			this.unitSpawned++;
			base.Invoke(this.methodNameSpawn, this.timeBetweenSpawn);
		}
	}

	private void OnUnitDie(Component senser, object param)
	{
		UnitDieData unitDieData = (UnitDieData)param;
		BaseEnemy component = unitDieData.unit.GetComponent<BaseEnemy>();
		if (this.activeUnits.ContainsKey(component.gameObject))
		{
			base.CancelInvoke();
			this.activeUnits.Remove(component.gameObject);
			base.Invoke(this.methodNameSpawn, 1f);
		}
	}

	private IEnumerator CoroutineAlarm()
	{
		LabDoorSpawnEnemy._CoroutineAlarm_c__Iterator0 _CoroutineAlarm_c__Iterator = new LabDoorSpawnEnemy._CoroutineAlarm_c__Iterator0();
		_CoroutineAlarm_c__Iterator._this = this;
		return _CoroutineAlarm_c__Iterator;
	}
}
