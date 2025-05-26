using System;
using UnityEngine;

public class BulletPreviewShotgun : BaseBulletPreview
{
	public float distance;

	private Vector2 startPoint;

	protected override void Move()
	{
		if (Vector2.Distance(base.transform.position, this.startPoint) >= this.distance)
		{
			this.Deactive();
			return;
		}
		base.Move();
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
		}
	}

	public override void Active(Transform firePoint, float moveSpeed, Transform parent = null)
	{
		base.Active(firePoint, moveSpeed, parent);
		this.startPoint = base.transform.position;
	}

	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.shotgun.Store(this);
	}
}
