using System;
using UnityEngine;

public class GunPreviewScarH : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewScarH bulletPreviewScarH = Singleton<PoolingPreviewController>.Instance.scarH.New();
		if (bulletPreviewScarH == null)
		{
			bulletPreviewScarH = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewScarH);
		}
		bulletPreviewScarH.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
