using System;

public class BulletPreviewSniperRifle : BaseBulletPreview
{
	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.sniperRifle.Store(this);
	}
}
