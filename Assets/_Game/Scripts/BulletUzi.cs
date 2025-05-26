using System;

public class BulletUzi : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletUzi.Store(this);
	}
}
