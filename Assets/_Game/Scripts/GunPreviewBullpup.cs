using System;
using UnityEngine;

public class GunPreviewBullpup : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewBullpup bulletPreviewBullpup = Singleton<PoolingPreviewController>.Instance.bullpup.New();
		if (bulletPreviewBullpup == null)
		{
			bulletPreviewBullpup = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewBullpup);
		}
		bulletPreviewBullpup.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
