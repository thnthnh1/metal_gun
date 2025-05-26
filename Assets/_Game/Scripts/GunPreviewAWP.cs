using System;
using UnityEngine;

public class GunPreviewAWP : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewAWP bulletPreviewAWP = Singleton<PoolingPreviewController>.Instance.awp.New();
		if (bulletPreviewAWP == null)
		{
			bulletPreviewAWP = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewAWP);
		}
		bulletPreviewAWP.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
