using System;

public class BulletPoisonBossVenom : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletPoisonBossVenom.Store(this);
	}
}
