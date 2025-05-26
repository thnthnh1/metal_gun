using System;
using UnityEngine;

public class BulletSniperRifle : BaseBullet
{
	private int hitEnemies;

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletSniperRifle.Store(this);
	}

	public override void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
	{
		base.Active(attackData, releasePoint, moveSpeed, parent);
		this.hitEnemies = 0;
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
			this.attackData.damage *= 1f - (float)this.hitEnemies * 0.15f;
			baseUnit.TakeDamage(this.attackData);
			this.hitEnemies++;
			if (this.hitEnemies >= 3)
			{
				this.Deactive();
			}
		}
		this.SpawnHitEffect();
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeSmall, base.transform.position);
	}
}
