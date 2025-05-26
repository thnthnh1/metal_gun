using System;
using UnityEngine;

public class BulletShotgun : BaseBullet
{
	public float distance;

	private Vector2 startPoint;

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletShotgun.Store(this);
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
		}
	}

	public override void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
	{
		base.Active(attackData, releasePoint, moveSpeed, parent);
		this.startPoint = base.transform.position;
	}

	protected override void TrackingDeactive()
	{
		if (Vector2.Distance(base.transform.position, this.startPoint) >= this.distance)
		{
			this.Deactive();
			return;
		}
	}

	protected override void SpawnHitEffect()
	{
	}
}
