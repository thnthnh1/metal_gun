using System;

public class BulletBossProfessor : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletBossProfessor.Store(this);
	}
}
