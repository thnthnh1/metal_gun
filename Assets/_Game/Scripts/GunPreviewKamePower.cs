using System;
using UnityEngine;

public class GunPreviewKamePower : BaseGunPreview
{
	public float chargeTime;

	public GameObject chargeEffect;

	private float timerCharge;

	private void Update()
	{
		this.timerCharge += Time.deltaTime;
		if (this.timerCharge >= this.chargeTime)
		{
			this.timerCharge = 0f;
			BulletPreviewKamePower bulletPreviewKamePower = Singleton<PoolingPreviewController>.Instance.kamePower.New();
			if (bulletPreviewKamePower == null)
			{
				bulletPreviewKamePower = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewKamePower);
			}
			float bulletSpeed = this.baseStats.BulletSpeed;
			bulletPreviewKamePower.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
			this.ActiveMuzzle();
		}
	}
}
