using System;

public class BulletTeslaMini : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletTeslaMini.Store(this);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactTeslaMini, base.transform.position);
	}
}
