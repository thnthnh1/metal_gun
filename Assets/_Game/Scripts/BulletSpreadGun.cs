using System;

public class BulletSpreadGun : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletSpreadGun.Store(this);
	}
}
