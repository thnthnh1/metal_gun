using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseUnit : MonoBehaviour
{
	private sealed class _DelayAction_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal WaitForSeconds delayTime;

		internal UnityAction callback;

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

		public _DelayAction_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = this.delayTime;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				if (this.callback != null)
				{
					this.callback();
				}
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

	[Header("STATS")]
	public SO_BaseUnitStats baseStats;

	public UnitStats stats = new UnitStats();

	[Header("BASE UNIT PROPERTIES")]
	public int id;

	public int level;

	public bool isDead;

	public bool isImmortal;

	public bool isStun;

	public bool isKnockBack;

	public bool isOnVehicle;

	public SpriteRenderer healthBar;

	public Transform bodyCenterPoint;

	public Transform aimPoint;

	public Transform showDamagePoint;

	public AudioClip[] soundDie;

	[SerializeField]
	public List<ItemDropData> itemDropList;

	protected float healthBarSizeX;

	protected Rigidbody2D rigid;

	protected AudioSource audioSource;

	private bool _IsMoving_k__BackingField;

	public virtual float HpPercent
	{
		get
		{
			return Mathf.Clamp01(this.stats.HP / this.stats.MaxHp);
		}
	}

	public virtual bool IsMoving
	{
		get;
		set;
	}

	public Rigidbody2D Rigid
	{
		get
		{
			return this.rigid;
		}
	}

	public bool IsDisableAction
	{
		get
		{
			return this.isStun || this.isKnockBack;
		}
	}

	public virtual bool IsFacingRight
	{
		get
		{
			return false;
		}
	}

	public Transform BodyCenterPoint
	{
		get
		{
			if (this.bodyCenterPoint != null)
			{
				return this.bodyCenterPoint;
			}
			return base.transform;
		}
	}

	protected virtual void Awake()
	{
		this.rigid = base.GetComponent<Rigidbody2D>();
		this.audioSource = base.GetComponent<AudioSource>();
		if (this.healthBar)
		{
			this.healthBarSizeX = this.healthBar.size.x;
		}
	}

	protected virtual void EffectTakeDamage()
	{
	}

	protected virtual void LoadScriptableObject()
	{
	}

	protected virtual void StopMoving()
	{
		if (this.rigid != null)
		{
			Vector3 v = this.rigid.velocity;
			v.x = 0f;
			this.rigid.velocity = v;
			this.rigid.angularVelocity = 0f;
		}
	}

	protected virtual void Idle()
	{
	}

	protected virtual void Move()
	{
	}

	protected virtual void UpdateDirection()
	{
	}

	protected virtual void Jump()
	{
	}

	protected virtual void Attack()
	{
	}

	protected virtual void Die()
	{
		this.isDead = true;
		this.PlaySoundDie();
	}

	protected virtual void ApplyDebuffs(AttackData attackData)
	{
	}

	protected virtual void PlaySound(AudioClip clip)
	{
		if (this.audioSource)
		{
			if (clip)
			{
				this.audioSource.PlayOneShot(clip);
			}
		}
	}

	protected virtual void PlaySoundDie()
	{
		if (this.soundDie.Length > 0)
		{
			int num = UnityEngine.Random.Range(0, this.soundDie.Length);
			if (this.audioSource)
			{
				this.audioSource.PlayOneShot(this.soundDie[num]);
			}
			else
			{
				SoundManager.Instance.PlaySfx(this.soundDie[num], 0f);
			}
		}
	}

	protected virtual void ShowTextDamageTaken(AttackData attackData)
	{
		if (this.showDamagePoint != null)
		{
			Vector2 position = this.showDamagePoint.position;
			position.x += UnityEngine.Random.Range(-0.5f, 0.5f);
			position.y += UnityEngine.Random.Range(0f, 0.5f);
			EffectController.Instance.SpawnTextDamageTMP(position, attackData, Singleton<PoolingController>.Instance.groupText);
		}
	}

	protected virtual void ShowTextDamageTaken(float damage)
	{
		if (this.showDamagePoint != null)
		{
			Vector2 vector = this.showDamagePoint.position;
			vector.x += UnityEngine.Random.Range(-0.25f, 0.25f);
			vector.y += UnityEngine.Random.Range(0f, 0.5f);
			int num = Mathf.RoundToInt(damage * 10f);
			EffectController arg_9C_0 = EffectController.Instance;
			Vector2 position = vector;
			Color red = Color.red;
			string content = num.ToString();
			Transform groupText = Singleton<PoolingController>.Instance.groupText;
			arg_9C_0.SpawnTextTMP(position, red, content, 3.5f, groupText);
		}
	}

	protected IEnumerator DelayAction(UnityAction callback, WaitForSeconds delayTime)
	{
		BaseUnit._DelayAction_c__Iterator0 _DelayAction_c__Iterator = new BaseUnit._DelayAction_c__Iterator0();
		_DelayAction_c__Iterator.delayTime = delayTime;
		_DelayAction_c__Iterator.callback = callback;
		return _DelayAction_c__Iterator;
	}

	protected virtual void HandleAnimationStart(TrackEntry entry)
	{
	}

	protected virtual void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
	}

	protected virtual void HandleAnimationCompleted(TrackEntry entry)
	{
	}

	protected virtual void AvoidSlideOnInclinedPlane()
	{
	}

	public virtual void AddModifier(ModifierData data)
	{
	}

	public virtual void RemoveModifier(ModifierData data)
	{
	}

	public virtual void ApplyModifier()
	{
	}

	public virtual void ReloadStats()
	{
	}

	public virtual void TakeDamage(AttackData attackData)
	{
	}

	public virtual void TakeDamage(float damage)
	{
	}

	public virtual void Renew()
	{
		this.isDead = false;
		this.isStun = false;
		this.isKnockBack = false;
		this.LoadScriptableObject();
		this.stats.Init(this.baseStats);
	}

	public virtual void AddForce(Vector3 dir, float force, ForceMode2D forceMode = ForceMode2D.Impulse)
	{
		this.rigid.AddForce(dir * force, forceMode);
	}

	public virtual void Active(int id, int level, Vector2 position)
	{
		this.id = id;
		this.level = level;
		base.transform.position = position;
		this.Renew();
		base.enabled = true;
		base.gameObject.SetActive(true);
	}

	public virtual void Deactive()
	{
		base.StopAllCoroutines();
		base.CancelInvoke();
	}

	public virtual void UpdateHealthBar(bool isAutoHide)
	{
	}

	public virtual void ActiveHealthBar(bool isActive)
	{
		if (this.healthBar != null)
		{
			this.healthBar.transform.parent.gameObject.SetActive(isActive);
		}
	}

	public virtual void SetTarget(BaseUnit unit)
	{
	}

	public virtual AttackData GetCurentAttackData()
	{
		return new AttackData(this, this.stats.Damage, 0f, false, WeaponType.NormalGun, -1, null);
	}

	public virtual bool IsOutOfScreen()
	{
		bool flag = base.transform.position.x < Singleton<CameraFollow>.Instance.left.position.x - 0.5f || (double)base.transform.position.x > (double)Singleton<CameraFollow>.Instance.right.position.x + 0.5;
		bool flag2 = base.transform.position.y < Singleton<CameraFollow>.Instance.bottom.position.y - 0.5f || base.transform.position.y > Singleton<CameraFollow>.Instance.top.position.y + 0.5f;
		return flag || flag2;
	}

	public virtual void GetStun(float duration)
	{
	}

	public virtual void FallBackward(float duration)
	{
	}
}
