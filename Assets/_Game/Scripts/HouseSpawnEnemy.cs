using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class HouseSpawnEnemy : MonoBehaviour
{
	private sealed class _CoroutineSpawnUnits_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		private sealed class _CoroutineSpawnUnits_c__AnonStorey1
		{
			internal BaseEnemy enemy;

			internal Vector2 v;

			internal HouseSpawnEnemy._CoroutineSpawnUnits_c__Iterator0 __f__ref_0;

			internal void __m__0()
			{
				if (!this.enemy.isDead)
				{
					this.enemy.isImmortal = false;
					this.enemy.Rigid.simulated = true;
					this.enemy.PlayAnimationIdle();
					this.enemy.ActiveSensor(true);
				}
			}

			internal void __m__1()
			{
				this.enemy.isImmortal = true;
				this.enemy.Rigid.simulated = false;
				this.enemy.skeletonAnimation.Skeleton.flipX = (this.v.x < this.enemy.transform.position.x);
				this.enemy.PlayAnimationMove();
			}
		}

		internal int _count___0;

		internal int _level___0;

		internal BaseEnemy _enemyPrefab___1;

		internal float _s___1;

		internal HouseSpawnEnemy _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		private HouseSpawnEnemy._CoroutineSpawnUnits_c__Iterator0._CoroutineSpawnUnits_c__AnonStorey1 _locvar0;

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

		public _CoroutineSpawnUnits_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.remainingUnits = this._this.totalUnits;
				this._count___0 = 0;
				this._level___0 = this._this.levelUnit;
				if (GameData.mode == GameMode.Campaign)
				{
					this._level___0 = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._count___0 < this._this.totalUnits)
			{
				this._locvar0 = new HouseSpawnEnemy._CoroutineSpawnUnits_c__Iterator0._CoroutineSpawnUnits_c__AnonStorey1();
				this._locvar0.__f__ref_0 = this;
				this._enemyPrefab___1 = this._this.enemyPrefabs[UnityEngine.Random.Range(0, this._this.enemyPrefabs.Length)];
				this._locvar0.enemy = this._enemyPrefab___1.GetFromPool();
				this._locvar0.enemy.isInvisibleWhenActive = true;
				this._locvar0.enemy.Active(this._enemyPrefab___1.id, this._level___0, this._this.spawnPoint.position);
				this._locvar0.enemy.zoneId = -1;
				this._locvar0.enemy.canMove = false;
				this._locvar0.enemy.ActiveSensor(false);
				this._locvar0.enemy.bounty = this._this.bountyPerUnit;
				this._locvar0.v = this._this.mostLeftPoint.position;
				this._locvar0.v.x = UnityEngine.Random.Range(this._this.mostLeftPoint.position.x, this._this.mostRightPoint.position.x);
				this._s___1 = Vector2.Distance(this._this.transform.position, this._locvar0.v);
				this._locvar0.enemy.transform.DOMove(this._locvar0.v, this._s___1 / this._locvar0.enemy.baseStats.MoveSpeed, false).SetEase(Ease.Linear).OnComplete(new TweenCallback(this._locvar0.__m__0)).OnStart(new TweenCallback(this._locvar0.__m__1));
				Singleton<GameController>.Instance.AddUnit(this._locvar0.enemy.gameObject, this._locvar0.enemy);
				this._this.activeUnits.Add(this._locvar0.enemy.gameObject, this._locvar0.enemy);
				this._count___0++;
				this._current = this._this.waitSpawn;
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

	public bool isFinalHouse;

	public Transform lockPointTop;

	public Transform spawnPoint;

	public Transform mostLeftPoint;

	public Transform mostRightPoint;

	public GameObject door;

	public int totalUnits = 5;

	public int levelUnit = 3;

	public float timeDelaySpawn = 1f;

	public BaseEnemy[] enemyPrefabs;

	private bool isActive;

	private int remainingUnits;

	private int bountyPerUnit;

	private WaitForSeconds waitSpawn;

	private Dictionary<GameObject, BaseUnit> activeUnits = new Dictionary<GameObject, BaseUnit>();

	private static UnityAction __f__am_cache0;

	private void Awake()
	{
		this.waitSpawn = new WaitForSeconds(this.timeDelaySpawn);
	}

	private void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, new Action<Component, object>(this.OnUnitDie));
		if (GameData.mode == GameMode.Campaign)
		{
			int coinDrop = GameData.staticCampaignStageData.GetCoinDrop(GameData.currentStage.id, GameData.currentStage.difficulty);
			this.bountyPerUnit = Mathf.RoundToInt((float)coinDrop * 0.1f / (float)this.totalUnits);
		}
	}

	public void Open()
	{
		Singleton<CameraFollow>.Instance.SetMarginTop(this.lockPointTop.position.y);
		float num = this.door.transform.position.y;
		num += 1.4f;
		this.door.transform.DOMoveY(num, 1.5f, false).OnComplete(delegate
		{
			this.isActive = true;
			base.StartCoroutine(this.CoroutineSpawnUnits());
		});
	}

	private void OnUnitDie(Component senser, object param)
	{
		if (!this.isActive)
		{
			return;
		}
		UnitDieData unitDieData = (UnitDieData)param;
		BaseEnemy component = unitDieData.unit.GetComponent<BaseEnemy>();
		if (this.activeUnits.ContainsKey(component.gameObject))
		{
			this.remainingUnits--;
			this.activeUnits.Remove(component.gameObject);
		}
		if (this.remainingUnits <= 0)
		{
			float y = Singleton<GameController>.Instance.CampaignMap.marginTop.position.y;
			Singleton<CameraFollow>.Instance.SetMarginTop(y);
			this.isActive = false;
			if (this.isFinalHouse)
			{
				Singleton<CameraFollow>.Instance.slowMotion.Show(3.5f, delegate
				{
					EventDispatcher.Instance.PostEvent(EventID.FinishStage, 0.5f);
				});
			}
		}
	}

	private IEnumerator CoroutineSpawnUnits()
	{
		HouseSpawnEnemy._CoroutineSpawnUnits_c__Iterator0 _CoroutineSpawnUnits_c__Iterator = new HouseSpawnEnemy._CoroutineSpawnUnits_c__Iterator0();
		_CoroutineSpawnUnits_c__Iterator._this = this;
		return _CoroutineSpawnUnits_c__Iterator;
	}
}
