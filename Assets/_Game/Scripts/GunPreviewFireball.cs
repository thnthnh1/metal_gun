using System;
using UnityEngine;

public class GunPreviewFireball : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewFireball bulletPreviewFireball = Singleton<PoolingPreviewController>.Instance.fireball.New();
		if (bulletPreviewFireball == null)
		{
			bulletPreviewFireball = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewFireball);
		}
		bulletPreviewFireball.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
