using System;
using UnityEngine;

public class GunPreviewSniperRifle : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewSniperRifle bulletPreviewSniperRifle = Singleton<PoolingPreviewController>.Instance.sniperRifle.New();
		if (bulletPreviewSniperRifle == null)
		{
			bulletPreviewSniperRifle = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewSniperRifle);
		}
		bulletPreviewSniperRifle.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
