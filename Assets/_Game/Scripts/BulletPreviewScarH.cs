using System;

public class BulletPreviewScarH : BaseBulletPreview
{
	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.scarH.Store(this);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeSmall, base.transform.position);
	}
}
