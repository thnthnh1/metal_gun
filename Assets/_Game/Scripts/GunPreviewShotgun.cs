using System;
using UnityEngine;

public class GunPreviewShotgun : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewShotgun bulletPreviewShotgun = Singleton<PoolingPreviewController>.Instance.shotgun.New();
		if (bulletPreviewShotgun == null)
		{
			bulletPreviewShotgun = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewShotgun);
		}
		bulletPreviewShotgun.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
