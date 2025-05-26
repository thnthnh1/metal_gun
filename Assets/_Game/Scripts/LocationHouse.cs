using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LocationHouse : BaseSpawnLocation
{
	private sealed class _CorountineSpawn_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		private sealed class _CorountineSpawn_c__AnonStorey1
		{
			internal BaseEnemy enemy;

			internal Vector2 v;

			internal LocationHouse._CorountineSpawn_c__Iterator0 __f__ref_0;

			internal void __m__0()
			{
				this.enemy.Rigid.simulated = true;
				this.enemy.PlayAnimationIdle();
				this.enemy.ActiveSensor(true);
				this.enemy.enabled = true;
			}

			internal void __m__1()
			{
				this.enemy.Rigid.simulated = false;
				this.enemy.SetDestinationMove(this.v);
				this.enemy.skeletonAnimation.Skeleton.flipX = (this.v.x < this.__f__ref_0._this.transform.position.x);
				this.enemy.PlayAnimationMove();
				this.enemy.enabled = false;
			}
		}

		internal int _countSpawn___0;

		internal int _id___1;

		internal int _level___1;

		internal BaseEnemy _enemyPrefab___1;

		internal float _s___1;

		internal LocationHouse _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		private LocationHouse._CorountineSpawn_c__Iterator0._CorountineSpawn_c__AnonStorey1 _locvar0;

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

		public _CorountineSpawn_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._countSpawn___0 = 0;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._countSpawn___0 < this._this.spawnUnits.Count)
			{
				this._locvar0 = new LocationHouse._CorountineSpawn_c__Iterator0._CorountineSpawn_c__AnonStorey1();
				this._locvar0.__f__ref_0 = this;
				this._id___1 = (int)this._this.spawnUnits[this._countSpawn___0];
				this._level___1 = UnityEngine.Random.Range(this._this.minLevelUnit, this._this.maxLevelUnit + 1);
				this._enemyPrefab___1 = Singleton<GameController>.Instance.modeController.GetEnemyPrefab((int)this._this.spawnUnits[this._countSpawn___0]);
				this._locvar0.enemy = this._enemyPrefab___1.GetFromPool();
				this._locvar0.enemy.isInvisibleWhenActive = true;
				this._locvar0.enemy.farSensor.col.radius = 30f;
				this._locvar0.enemy.Active(this._id___1, this._level___1, this._this.spawnPoint.position);
				if (this._locvar0.enemy is EnemyKnife || this._locvar0.enemy is EnemyMonkey || this._locvar0.enemy is EnemyFire)
				{
					this._locvar0.enemy.canMove = true;
				}
				else
				{
					this._locvar0.enemy.canMove = (UnityEngine.Random.Range(1, 101) > 70);
				}
				this._locvar0.enemy.canJump = false;
				this._locvar0.enemy.isRunPassArea = (UnityEngine.Random.Range(1, 11) <= 5);
				this._locvar0.enemy.ActiveSensor(false);
				this._locvar0.v = this._this.spawnPoint.position;
				LocationHouse._CorountineSpawn_c__Iterator0._CorountineSpawn_c__AnonStorey1 expr_201_cp_0 = this._locvar0;
				expr_201_cp_0.v.x = expr_201_cp_0.v.x + UnityEngine.Random.Range(-2f, 2f);
				this._s___1 = Vector2.Distance(this._this.transform.position, this._locvar0.v);
				this._locvar0.enemy.transform.DOMove(this._locvar0.v, this._s___1 / this._locvar0.enemy.baseStats.MoveSpeed, false).SetEase(Ease.Linear).OnComplete(new TweenCallback(this._locvar0.__m__0)).OnStart(new TweenCallback(this._locvar0.__m__1));
				Singleton<GameController>.Instance.AddUnit(this._locvar0.enemy.gameObject, this._locvar0.enemy);
				((SurvivalModeController)Singleton<GameController>.Instance.modeController).AddUnit(this._locvar0.enemy);
				this._countSpawn___0++;
				this._current = this._this.delaySpawn;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.Clear();
			this._this.CloseDoor();
			this._this.coroutineSpawn = null;
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

	public GameObject door;

	private Vector2 doorClosePosition;

	private void Awake()
	{
		this.doorClosePosition = this.door.transform.position;
	}

	public override void Spawn()
	{
		this.OpenDoor();
	}

	private IEnumerator CorountineSpawn()
	{
		LocationHouse._CorountineSpawn_c__Iterator0 _CorountineSpawn_c__Iterator = new LocationHouse._CorountineSpawn_c__Iterator0();
		_CorountineSpawn_c__Iterator._this = this;
		return _CorountineSpawn_c__Iterator;
	}

	private void OpenDoor()
	{
		this.isSpawning = true;
		float num = this.door.transform.position.y;
		num += 1.4f;
		this.door.transform.DOMoveY(num, 1.5f, false).OnComplete(delegate
		{
			if (this.coroutineSpawn == null)
			{
				this.coroutineSpawn = this.CorountineSpawn();
				base.StartCoroutine(this.coroutineSpawn);
			}
		});
	}

	private void CloseDoor()
	{
		this.door.transform.DOMove(this.doorClosePosition, 1.5f, false).OnComplete(delegate
		{
			this.isSpawning = false;
		});
	}
}
