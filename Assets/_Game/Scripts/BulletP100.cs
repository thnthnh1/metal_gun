using System;
using UnityEngine;

public class BulletP100 : BaseBullet
{
	public SpriteRenderer sprRenderer;

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletP100.Store(this);
	}

	protected override void Move()
	{
		base.Move();
		this.sprRenderer.transform.Rotate(0f, 0f, 1000f * Time.deltaTime);
	}
}
