using System;
using UnityEngine;

public class BulletPreviewP100 : BaseBulletPreview
{
	public SpriteRenderer sprRenderer;

	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.p100.Store(this);
	}

	protected override void Move()
	{
		base.Move();
		this.sprRenderer.transform.Rotate(0f, 0f, 1000f * Time.deltaTime);
	}
}
