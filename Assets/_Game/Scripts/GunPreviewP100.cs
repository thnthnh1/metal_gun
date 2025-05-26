using System;
using UnityEngine;

public class GunPreviewP100 : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewP100 bulletPreviewP = Singleton<PoolingPreviewController>.Instance.p100.New();
		if (bulletPreviewP == null)
		{
			bulletPreviewP = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewP100);
		}
		bulletPreviewP.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
