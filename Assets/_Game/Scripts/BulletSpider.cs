using System;
using UnityEngine;

public class BulletSpider : BaseBullet
{
	public float amplitude = 0.5f;

	public float rate = 0.5f;

	private Vector3 startPos;

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletSpider.Store(this);
	}

	protected override void Move()
	{
		float f = Time.timeSinceLevelLoad / this.rate;
		float d = this.amplitude * Mathf.Sin(f);
		this.startPos += base.transform.right * this.moveSpeed * Time.deltaTime;
		base.transform.position = this.startPos + base.transform.up * d;
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null)
			{
				unit.TakeDamage(this.attackData);
				this.SpawnHitEffect();
				this.Deactive();
			}
		}
	}

	public override void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
	{
		base.Active(attackData, releasePoint, moveSpeed, parent);
		this.startPos = releasePoint.position;
	}
}
