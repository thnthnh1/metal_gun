using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseGrenadePreview : MonoBehaviour
{
	private sealed class _DelayExplode_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal BaseGrenadePreview _this;

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

		public _DelayExplode_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._this.Explode();
				this._PC = -1;
				break;
			}
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

	public int id;

	public SO_GrenadeStats baseStats;

	public LayerMask layerVictim;

	protected bool isExploding;

	protected Collider2D[] victims = new Collider2D[2];

	private Rigidbody2D rigid;

	private void Awake()
	{
		this.rigid = base.GetComponent<Rigidbody2D>();
	}

	protected virtual void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Enemy"))
		{
			if (!this.isExploding)
			{
				this.isExploding = true;
				this.Explode();
			}
		}
		else if (!other.gameObject.CompareTag("Player") && !this.isExploding)
		{
			this.isExploding = true;
			base.StartCoroutine(this.DelayExplode());
		}
	}

	public virtual void Deactive()
	{
		base.CancelInvoke();
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
	}

	public virtual void Active(Vector3 startPoint, Vector2 throwForce, Transform parent = null)
	{
		this.isExploding = false;
		base.transform.position = startPoint;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
		this.rigid.AddForce(throwForce, ForceMode2D.Impulse);
	}

	protected virtual void Explode()
	{
		int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.baseStats.Radius, this.victims, this.layerVictim);
		if (num > 0)
		{
			EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
		}
		this.Deactive();
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, base.transform.position);
	}

	protected IEnumerator DelayExplode()
	{
		BaseGrenadePreview._DelayExplode_c__Iterator0 _DelayExplode_c__Iterator = new BaseGrenadePreview._DelayExplode_c__Iterator0();
		_DelayExplode_c__Iterator._this = this;
		return _DelayExplode_c__Iterator;
	}
}
