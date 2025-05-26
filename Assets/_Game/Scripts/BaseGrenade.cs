using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaseGrenade : BaseWeapon
{
	private sealed class _DelayExplode_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal BaseGrenade _this;

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
				this._current = StaticValue.waitHalfSec;
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

	[Header("BASE STATS")]
	public SO_GrenadeStats baseStats;

	[Header("BASE GRENADE PROPERTIES")]
	public BaseEffect effectTextPrefab;

	public LayerMask layerVictim;

	protected bool isExploding;

	protected AttackData attackData;

	protected Collider2D[] victims = new Collider2D[10];

	private Rigidbody2D rigid;

	private void Awake()
	{
		this.rigid = base.GetComponent<Rigidbody2D>();
	}

	protected virtual void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.root.CompareTag("Enemy"))
		{
			if (!this.isExploding)
			{
				this.isExploding = true;
				this.Explode();
			}
		}
		else if (!other.transform.root.CompareTag("Player") && !this.isExploding)
		{
			this.isExploding = true;
			base.StartCoroutine(this.DelayExplode());
		}
	}

	private IEnumerator DelayExplode()
	{
		BaseGrenade._DelayExplode_c__Iterator0 _DelayExplode_c__Iterator = new BaseGrenade._DelayExplode_c__Iterator0();
		_DelayExplode_c__Iterator._this = this;
		return _DelayExplode_c__Iterator;
	}

	private void SpawnEffectText()
	{
		if (this.effectTextPrefab == null)
		{
			return;
		}
		EffectTextBANG effectTextBANG = Singleton<PoolingController>.Instance.poolTextBANG.New();
		if (effectTextBANG == null)
		{
			effectTextBANG = (UnityEngine.Object.Instantiate<BaseEffect>(this.effectTextPrefab) as EffectTextBANG);
		}
		Vector2 v = base.transform.position;
		v.y += 1.5f;
		effectTextBANG.Active(v, null);
	}

	private void TrackOutOfScreen()
	{
		bool flag = base.transform.position.x < Singleton<CameraFollow>.Instance.left.position.x || base.transform.position.x > Singleton<CameraFollow>.Instance.right.position.x;
		bool flag2 = base.transform.position.y < Singleton<CameraFollow>.Instance.bottom.position.y || base.transform.position.y > Singleton<CameraFollow>.Instance.top.position.y;
		if (flag || flag2)
		{
			this.Deactive();
		}
	}

	public virtual BaseGrenade Create()
	{
		BaseGrenade baseGrenade = Singleton<PoolingController>.Instance.poolBaseGrenade.New();
		if (baseGrenade == null)
		{
			baseGrenade = UnityEngine.Object.Instantiate<BaseGrenade>(this);
		}
		return baseGrenade;
	}

	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Grenade/Grenade Base/grenade_base_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GrenadeStats>(path);
	}

	public override void Init(int level)
	{
		base.Init(level);
	}

	public override void ApplyOptions(BaseUnit unit)
	{
	}

	public override void Attack(AttackData attackData)
	{
	}

	public virtual void Active(AttackData attackData, Vector3 startPoint, Vector2 throwForce, Transform parent = null)
	{
		this.attackData = attackData;
		this.isExploding = false;
		base.transform.position = startPoint;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
		this.rigid.AddForce(throwForce, ForceMode2D.Impulse);
	}

	public virtual void Deactive()
	{
		base.CancelInvoke();
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
		Singleton<PoolingController>.Instance.poolBaseGrenade.Store(this);
	}

	public virtual void Explode()
	{
		int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.attackData.radiusDealDamage, this.victims, this.layerVictim);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			BaseUnit baseUnit = null;
			if (this.victims[i].CompareTag("Enemy Body Part") || this.victims[i].CompareTag("Destructible Obstacle"))
			{
				baseUnit = Singleton<GameController>.Instance.GetUnit(this.victims[i].gameObject);
			}
			else if (this.victims[i].transform.root.CompareTag("Enemy"))
			{
				baseUnit = Singleton<GameController>.Instance.GetUnit(this.victims[i].transform.root.gameObject);
			}
			if (baseUnit)
			{
				float num3 = Vector3.Distance(base.transform.position, baseUnit.BodyCenterPoint.position);
				float num4 = Mathf.Clamp01((num3 - 0.5f) / (this.attackData.radiusDealDamage - 0.5f));
				float num5 = 1f - num4 * 0.4f;
				this.attackData.damage *= num5;
				baseUnit.TakeDamage(this.attackData);
				if (baseUnit.CompareTag("Enemy") && baseUnit.isDead)
				{
					num2++;
				}
			}
		}
		EventDispatcher.Instance.PostEvent(EventID.GrenadeKillEnemyAtOnce, num2);
		this.Deactive();
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, base.transform.position);
		this.SpawnEffectText();
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
		Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.35f);
	}
}
