using System;

public class BulletPreviewSpread : BaseBulletPreview
{
	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.spread.Store(this);
	}
}
