using System;
using UnityEngine;

public class BulletKamePower : BaseBullet
{
	private float percentCharge;

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletKamePower.Store(this);
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
			baseUnit.TakeDamage(this.attackData);
			this.SpawnHitEffect();
			this.Deactive();
		}
	}

	public virtual void Active(AttackData attackData, Transform releasePoint, float moveSpeed, float percentCharge, Transform parent = null)
	{
		this.attackData = attackData;
		this.moveSpeed = moveSpeed;
		this.percentCharge = percentCharge;
		this.SetTagAndLayer();
		base.transform.position = releasePoint.position;
		base.transform.rotation = releasePoint.rotation;
		base.transform.localScale = Vector3.one * percentCharge;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
	}

	protected override void SpawnHitEffect()
	{
		if (this.percentCharge >= 0.95f)
		{
			Singleton<CameraFollow>.Instance.AddShake(0.2f, 0.2f);
			EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, base.transform.position);
		}
		else if (this.percentCharge >= 0.7f)
		{
			EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, base.transform.position);
		}
		else
		{
			EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactNormal, base.transform.position);
		}
	}
}
