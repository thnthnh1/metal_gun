using System;

public class BulletPistol : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletPistol.Store(this);
	}
}
