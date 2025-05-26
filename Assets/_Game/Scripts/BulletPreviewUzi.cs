using System;

public class BulletPreviewUzi : BaseBulletPreview
{
	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.uzi.Store(this);
	}
}
