using System;
using UnityEngine;

public class GunPreviewFamas : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewFamas bulletPreviewFamas = Singleton<PoolingPreviewController>.Instance.famas.New();
		if (bulletPreviewFamas == null)
		{
			bulletPreviewFamas = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewFamas);
		}
		bulletPreviewFamas.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
