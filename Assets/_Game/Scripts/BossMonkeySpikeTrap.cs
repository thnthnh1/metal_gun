using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossMonkeySpikeTrap : MonoBehaviour
{
	private sealed class _CoroutineDropSpikes_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _countSpike___0;

		internal BossMonkeySpikeTrap _this;

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

		public _CoroutineDropSpikes_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._countSpike___0 = 0;
				this._this.activeSpikes = this._this.totalSpikes;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._countSpike___0 < this._this.totalSpikes)
			{
				this._this.SpawnSpike();
				this._countSpike___0++;
				this._current = this._this.waitDelaySpike;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.coroutineDropSpikes = null;
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

	public Transform mostLeftPoint;

	public Transform mostRightPoint;

	public Spike spikePrefab;

	private BossMonkey boss;

	private int totalSpikes;

	private int activeSpikes;

	private float spikeDamage;

	private float spikeDropSpeed;

	private WaitForSeconds waitDelaySpike;

	private IEnumerator coroutineDropSpikes;

	private void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.BossMonkeySpikeTrapStart, delegate(Component sender, object param)
		{
			this.ActiveSpikeTrap((DropSpikeData)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.BossMonkeySpikeDeactive, delegate(Component sender, object param)
		{
			this.OnSpikeDeactive();
		});
	}

	private void ActiveSpikeTrap(DropSpikeData data)
	{
		this.boss = data.boss;
		this.totalSpikes = data.numberSpikes;
		this.spikeDamage = data.spikeDamage;
		this.spikeDropSpeed = data.spikeDropSpeed;
		this.waitDelaySpike = new WaitForSeconds(data.spikeDelay);
		if (this.coroutineDropSpikes != null)
		{
			base.StopCoroutine(this.coroutineDropSpikes);
			this.coroutineDropSpikes = null;
		}
		this.coroutineDropSpikes = this.CoroutineDropSpikes();
		base.StartCoroutine(this.coroutineDropSpikes);
	}

	private IEnumerator CoroutineDropSpikes()
	{
		BossMonkeySpikeTrap._CoroutineDropSpikes_c__Iterator0 _CoroutineDropSpikes_c__Iterator = new BossMonkeySpikeTrap._CoroutineDropSpikes_c__Iterator0();
		_CoroutineDropSpikes_c__Iterator._this = this;
		return _CoroutineDropSpikes_c__Iterator;
	}

	private void SpawnSpike()
	{
		Spike spike = Singleton<PoolingController>.Instance.poolSpike.New();
		if (spike == null)
		{
			spike = UnityEngine.Object.Instantiate<Spike>(this.spikePrefab);
		}
		AttackData attackData = new AttackData(this.boss, this.spikeDamage, 0f, false, WeaponType.NormalGun, -1, null);
		float x = UnityEngine.Random.Range(this.mostLeftPoint.position.x, this.mostRightPoint.position.x);
		Vector2 position = this.mostLeftPoint.position;
		position.x = x;
		spike.Active(attackData, position, this.spikeDropSpeed, null);
	}

	private void OnSpikeDeactive()
	{
		this.activeSpikes--;
		if (this.activeSpikes <= 0)
		{
			EventDispatcher.Instance.PostEvent(EventID.BossMonkeySpikeTrapEnd);
		}
	}
}
