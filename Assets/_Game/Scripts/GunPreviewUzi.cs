using System;
using UnityEngine;

public class GunPreviewUzi : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewUzi bulletPreviewUzi = Singleton<PoolingPreviewController>.Instance.uzi.New();
		if (bulletPreviewUzi == null)
		{
			bulletPreviewUzi = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewUzi);
		}
		bulletPreviewUzi.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		Vector3 vector = bulletPreviewUzi.transform.position;
		vector += this.firePoint.up * UnityEngine.Random.Range(-0.15f, 0.15f);
		bulletPreviewUzi.transform.position = vector;
		this.ActiveMuzzle();
	}
}
