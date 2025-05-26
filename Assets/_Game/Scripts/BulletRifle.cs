using System;

public class BulletRifle : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletRifle.Store(this);
	}
}
