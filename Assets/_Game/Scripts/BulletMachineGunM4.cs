using System;

public class BulletMachineGunM4 : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletMachineGunM4.Store(this);
	}
}
