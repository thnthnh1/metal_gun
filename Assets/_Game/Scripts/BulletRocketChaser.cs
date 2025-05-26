using DG.Tweening;
using System;
using UnityEngine;

public class BulletRocketChaser : BaseBullet
{
	public float turnSpeed = 50f;

	public LayerMask layerVictim;

	private Vector2 readyPosition;

	private bool isReady;

	private bool isPreparing;

	private bool isFacingRight;

	protected Collider2D[] victims = new Collider2D[10];

	protected override void Move()
	{
		if (this.isReady)
		{
			if (this.target)
			{
				Vector3 vector = this.target.position - base.transform.position;
				base.transform.right = Vector3.MoveTowards(base.transform.right, vector.normalized, this.turnSpeed * Time.deltaTime);
			}
			base.transform.Translate(base.transform.right * this.moveSpeed * Time.deltaTime, Space.World);
		}
		else if (this.isPreparing)
		{
			this.isPreparing = false;
			base.transform.DOMove(this.readyPosition, 0.2f, false).OnComplete(delegate
			{
				this.isReady = true;
				BaseUnit nearestEnemy = this.GetNearestEnemy();
				if (nearestEnemy != null)
				{
					base.SetTarget(nearestEnemy.BodyCenterPoint);
				}
			});
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy Body Part"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.gameObject);
			if (unit != null)
			{
				unit.TakeDamage(this.attackData);
			}
		}
		else if (other.transform.root.CompareTag("Enemy"))
		{
			int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.attackData.radiusDealDamage, this.victims, this.layerVictim);
			for (int i = 0; i < num; i++)
			{
				if (!this.victims[i].CompareTag("Enemy Body Part"))
				{
					BaseUnit unit = Singleton<GameController>.Instance.GetUnit(this.victims[i].transform.root.gameObject);
					if (unit != null)
					{
						float num2 = Vector3.Distance(base.transform.position, unit.BodyCenterPoint.position);
						float num3 = Mathf.Clamp01((num2 - 0.5f) / (this.attackData.radiusDealDamage - 0.5f));
						float num4 = 1f - num3 * 0.4f;
						float damage = this.attackData.damage * num4;
						this.attackData.damage = damage;
						unit.TakeDamage(this.attackData);
					}
				}
			}
		}
		this.SpawnHitEffect();
		this.Deactive();
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, base.transform.position);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}

	public void Active(AttackData attackData, Transform releasePoint, Vector2 readyPosition, float moveSpeed, Transform parent = null)
	{
		this.target = null;
		this.attackData = attackData;
		this.moveSpeed = moveSpeed;
		this.readyPosition = readyPosition;
		base.transform.position = releasePoint.position;
		base.transform.rotation = releasePoint.rotation;
		base.transform.parent = parent;
		this.isReady = false;
		this.isPreparing = true;
		this.isFacingRight = (readyPosition.x - releasePoint.position.x > 0f);
		base.gameObject.SetActive(true);
	}

	public override void Deactive()
	{
		base.Deactive();
		this.target = null;
		Singleton<PoolingController>.Instance.poolBulletRocketChaser.Store(this);
	}

	private BaseUnit GetNearestEnemy()
	{
		BaseUnit result = null;
		float num = 15f;
		foreach (BaseUnit current in Singleton<GameController>.Instance.activeUnits.Values)
		{
			if (current.CompareTag("Enemy") && !current.isDead)
			{
				Vector2 a = current.BodyCenterPoint.position;
				float num2 = Vector2.Distance(a, base.transform.position);
				if (num2 <= num)
				{
					num = num2;
					result = current;
				}
			}
		}
		return result;
	}
}
