using System;

public class BulletFamasGun : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletFamasGun.Store(this);
	}
}
