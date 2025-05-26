using System;

public class BulletBullpup : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletBullpup.Store(this);
	}
}
