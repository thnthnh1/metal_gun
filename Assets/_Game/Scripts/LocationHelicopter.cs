using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class LocationHelicopter : BaseSpawnLocation
{
	private sealed class _CoroutineSpawn_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _countSpawn___0;

		internal int _id___1;

		internal int _level___1;

		internal BaseEnemy _enemyPrefab___1;

		internal BaseEnemy _enemy___1;

		internal EnemyHelicopter _helicopter___1;

		internal Vector2 _position___1;

		internal LocationHelicopter _this;

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

		public _CoroutineSpawn_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.isSpawning = true;
				this._countSpawn___0 = 0;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._countSpawn___0 < this._this.spawnUnits.Count)
			{
				this._id___1 = (int)this._this.spawnUnits[this._countSpawn___0];
				this._level___1 = UnityEngine.Random.Range(this._this.minLevelUnit, this._this.maxLevelUnit + 1);
				this._enemyPrefab___1 = Singleton<GameController>.Instance.modeController.GetEnemyPrefab((int)this._this.spawnUnits[this._countSpawn___0]);
				this._enemy___1 = this._enemyPrefab___1.GetFromPool();
				this._helicopter___1 = (EnemyHelicopter)this._enemy___1;
				this._position___1 = Singleton<CameraFollow>.Instance.pointAirSpawnRight.position;
				this._helicopter___1.Active(this._id___1, this._level___1, this._position___1);
				this._helicopter___1.GetNextDestination();
				this._helicopter___1.SetTarget(Singleton<GameController>.Instance.Player);
				Singleton<GameController>.Instance.AddUnit(this._enemy___1.gameObject, this._enemy___1);
				((SurvivalModeController)Singleton<GameController>.Instance.modeController).AddUnit(this._enemy___1);
				this._countSpawn___0++;
				this._current = this._this.delay;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.isSpawning = false;
			this._this.Clear();
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

	private WaitForSeconds delay = new WaitForSeconds(2f);

	public override void Spawn()
	{
		if (this.coroutineSpawn == null)
		{
			this.coroutineSpawn = this.CoroutineSpawn();
			base.StartCoroutine(this.coroutineSpawn);
		}
	}

	private IEnumerator CoroutineSpawn()
	{
		LocationHelicopter._CoroutineSpawn_c__Iterator0 _CoroutineSpawn_c__Iterator = new LocationHelicopter._CoroutineSpawn_c__Iterator0();
		_CoroutineSpawn_c__Iterator._this = this;
		return _CoroutineSpawn_c__Iterator;
	}
}
