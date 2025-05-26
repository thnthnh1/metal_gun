using System;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
	public bool isTrackingLifeTime;

	public bool isDeactiveWhenOutScreen = true;

	public float lifeTime = 2f;

	protected float timerLifeTime;

	protected float moveSpeed;

	protected Transform target;

	protected AttackData attackData;

	protected Rigidbody2D rigid;

	protected AudioSource audioSource;

	protected virtual void Awake()
	{
		this.rigid = base.GetComponent<Rigidbody2D>();
		this.audioSource = base.GetComponent<AudioSource>();
	}

	protected virtual void Update()
	{
		this.Move();
		this.TrackingDeactive();
	}

	protected virtual void Move()
	{
		base.transform.Translate(Vector3.right * Time.deltaTime * this.moveSpeed);
	}

	protected virtual void TrackingDeactive()
	{
		if (this.isTrackingLifeTime && GameData.mode == GameMode.Survival)
		{
			this.timerLifeTime += Time.deltaTime;
			if (this.timerLifeTime >= this.lifeTime)
			{
				this.timerLifeTime = 0f;
				this.Deactive();
				return;
			}
		}
		if (this.isDeactiveWhenOutScreen)
		{
			bool flag = base.transform.position.x < Singleton<CameraFollow>.Instance.left.position.x - 0.7f || base.transform.position.x > Singleton<CameraFollow>.Instance.right.position.x + 0.7f;
			bool flag2 = base.transform.position.y > Singleton<CameraFollow>.Instance.top.position.y + 0.7f;
			if (flag || flag2)
			{
				this.Deactive();
			}
		}
	}

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		string tag = base.tag;
		if (tag != null)
		{
			if (!(tag == "Bullet Rambo"))
			{
				if (tag == "Bullet Enemy")
				{
					if (other.transform.root.CompareTag("Player"))
					{
						BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
						if (unit != null)
						{
							unit.TakeDamage(this.attackData);
							EventDispatcher.Instance.PostEvent(EventID.EnemyShootHitPlayer, this.attackData);
						}
					}
				}
			}
			else
			{
				BaseUnit baseUnit = null;
				if (other.CompareTag("Enemy Body Part") || other.CompareTag("Destructible Obstacle"))
				{
					baseUnit = Singleton<GameController>.Instance.GetUnit(other.gameObject);
				}
				else if (other.transform.root.CompareTag("Enemy"))
				{
					baseUnit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
				}
				if (baseUnit != null)
				{
					baseUnit.TakeDamage(this.attackData);
				}
			}
		}
		this.SpawnHitEffect();
		this.Deactive();
	}

	protected virtual void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactNormal, base.transform.position);
	}

	protected virtual void SetTagAndLayer()
	{
		if (this.attackData == null)
		{
			return;
		}
		string tag = this.attackData.attacker.tag;
		if (tag != null)
		{
			if (tag == "Player")
			{
				base.tag = "Bullet Rambo";
				base.gameObject.layer = StaticValue.LAYER_BULLET_RAMBO;
				return;
			}
			if (tag == "Enemy")
			{
				base.tag = "Bullet Enemy";
				base.gameObject.layer = StaticValue.LAYER_BULLET_ENEMY;
				return;
			}
		}
		base.tag = "Untagged";
		base.gameObject.layer = StaticValue.LAYER_DEFAULT;
	}

	public virtual void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
	{
		this.attackData = attackData;
		this.moveSpeed = moveSpeed;
		this.SetTagAndLayer();
		base.transform.position = releasePoint.position;
		base.transform.rotation = releasePoint.rotation;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
	}

	public virtual void Deactive()
	{
		this.timerLifeTime = 0f;
		base.gameObject.SetActive(false);
	}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}
}
