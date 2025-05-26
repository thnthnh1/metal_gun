using System;

public class BulletPreviewBullpup : BaseBulletPreview
{
	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.bullpup.Store(this);
	}
}
