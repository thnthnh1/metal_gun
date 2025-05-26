using System;

public class BulletPreviewM4 : BaseBulletPreview
{
	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.m4.Store(this);
	}
}
