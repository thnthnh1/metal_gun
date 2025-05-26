using System;
using UnityEngine;

public class BulletSplitGun : BaseBullet
{
	public Transform splitPoint;

	public SpriteRenderer sprRenderer;

	public Sprite sprBulletSub;

	public Animator animator;

	private BaseUnit firstHitUnit;

	private float splitDamage;

	private bool isSplit;

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletSplitGun.Store(this);
	}

	public void Active(AttackData attackData, Transform releasePoint, float moveSpeed, bool isSplit, float splitDamage = 0f, BaseUnit firstHitUnit = null, Transform parent = null)
	{
		this.attackData = attackData;
		this.moveSpeed = moveSpeed;
		this.isSplit = isSplit;
		this.splitDamage = splitDamage;
		this.firstHitUnit = firstHitUnit;
		if (isSplit)
		{
			this.animator.enabled = true;
			this.sprRenderer.transform.localScale = Vector3.one * 0.75f;
		}
		else
		{
			this.animator.enabled = false;
			this.sprRenderer.sprite = this.sprBulletSub;
			this.sprRenderer.transform.localScale = Vector3.one;
		}
		this.SetTagAndLayer();
		base.transform.position = releasePoint.position;
		base.transform.rotation = releasePoint.rotation;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
	}

	protected override void OnTriggerEnter2D(Collider2D other)
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
			if (this.isSplit)
			{
				baseUnit.TakeDamage(this.attackData);
				for (int i = -2; i < 3; i++)
				{
					BulletSplitGun bulletSplitGun = Singleton<PoolingController>.Instance.poolBulletSplitGun.New();
					if (bulletSplitGun == null)
					{
						bulletSplitGun = UnityEngine.Object.Instantiate<BulletSplitGun>(this);
					}
					this.attackData.damage = this.splitDamage;
					BulletSplitGun arg_102_0 = bulletSplitGun;
					AttackData attackData = this.attackData;
					Transform releasePoint = this.splitPoint;
					float moveSpeed = this.moveSpeed;
					bool flag = false;
					BaseUnit baseUnit2 = baseUnit;
					arg_102_0.Active(attackData, releasePoint, moveSpeed, flag, 0f, baseUnit2, Singleton<PoolingController>.Instance.groupBullet);
					bulletSplitGun.transform.Rotate(0f, 0f, (float)i * 45f);
				}
				this.SpawnHitEffect();
				this.Deactive();
			}
			else if (!object.ReferenceEquals(this.firstHitUnit.gameObject, baseUnit.gameObject))
			{
				baseUnit.TakeDamage(this.attackData);
				this.SpawnHitEffect();
				this.Deactive();
			}
		}
		else
		{
			this.SpawnHitEffect();
			this.Deactive();
		}
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactSplitGun, base.transform.position);
	}
}
