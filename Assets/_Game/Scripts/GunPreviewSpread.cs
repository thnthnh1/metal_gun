using System;
using UnityEngine;

public class GunPreviewSpread : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		for (int i = -2; i < 3; i++)
		{
			BulletPreviewSpread bulletPreviewSpread = Singleton<PoolingPreviewController>.Instance.spread.New();
			if (bulletPreviewSpread == null)
			{
				bulletPreviewSpread = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewSpread);
			}
			bulletPreviewSpread.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
			bulletPreviewSpread.transform.Rotate(0f, 0f, (float)i * 10f);
			this.ActiveMuzzle();
		}
	}
}
