using System;
using UnityEngine;

public class GunPreviewRocketChaser : BaseGunPreview
{
	public Transform rocketReadyPosition;

	public override void Fire()
	{
		base.Fire();
		BulletPreviewRocketChaser bulletPreviewRocketChaser = Singleton<PoolingPreviewController>.Instance.rocketChaser.New();
		if (bulletPreviewRocketChaser == null)
		{
			bulletPreviewRocketChaser = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewRocketChaser);
		}
		bulletPreviewRocketChaser.Active(this.firePoint, this.rocketReadyPosition.position, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}
