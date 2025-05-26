using System;

public class BulletBossMegatron : BaseBullet
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletBossMegatron.Store(this);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, base.transform.position);
		Singleton<CameraFollow>.Instance.AddShake(0.3f, 0.5f);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}
}
