using System;

public class BulletPreviewFamas : BaseBulletPreview
{
	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.famas.Store(this);
	}
}
