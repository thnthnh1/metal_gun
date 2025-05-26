using System;
using UnityEngine;

public class GunPreviewSplit : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewSplit bulletPreviewSplit = Singleton<PoolingPreviewController>.Instance.split.New();
		if (bulletPreviewSplit == null)
		{
			bulletPreviewSplit = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewSplit);
		}
		bulletPreviewSplit.Active(this.firePoint, this.baseStats.BulletSpeed, true, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
