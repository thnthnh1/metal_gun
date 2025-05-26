using System;

public class BulletPreviewKamePower : BaseBulletPreview
{
	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.kamePower.Store(this);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeLarge, base.transform.position);
	}
}
