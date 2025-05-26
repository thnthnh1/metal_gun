using System;
using UnityEngine;

public class GunPreviewTeslaMini : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewTeslaMini bulletPreviewTeslaMini = Singleton<PoolingPreviewController>.Instance.teslaMini.New();
		if (bulletPreviewTeslaMini == null)
		{
			bulletPreviewTeslaMini = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewTeslaMini);
		}
		bulletPreviewTeslaMini.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
