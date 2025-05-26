using System;
using UnityEngine;

public class GunPreviewM4 : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewM4 bulletPreviewM = Singleton<PoolingPreviewController>.Instance.m4.New();
		if (bulletPreviewM == null)
		{
			bulletPreviewM = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewM4);
		}
		bulletPreviewM.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		Vector3 vector = bulletPreviewM.transform.position;
		vector += this.firePoint.up * UnityEngine.Random.Range(-0.15f, 0.15f);
		bulletPreviewM.transform.position = vector;
		this.ActiveMuzzle();
	}
}
