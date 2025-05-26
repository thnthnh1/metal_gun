using System;
using UnityEngine;

public class GunFamas : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Famas/gun_famas_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletFamasGun bulletFamasGun = Singleton<PoolingController>.Instance.poolBulletFamasGun.New();
		if (bulletFamasGun == null)
		{
			bulletFamasGun = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletFamasGun);
		}
		bulletFamasGun.Active(attackData, this.firePoint, this.bulletSpeed, null);
		this.ActiveMuzzle();
	}

	public override void ReleaseCrossBullets(AttackData attackData, Transform crossAimPoint, bool isFacingRight)
	{
		base.ReleaseCrossBullets(attackData, crossAimPoint, isFacingRight);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		float num = 90f / (float)(this.numberCrossBullet + 1);
		for (int i = 0; i < this.numberCrossBullet; i++)
		{
			BulletFamasGun bulletFamasGun = Singleton<PoolingController>.Instance.poolBulletFamasGun.New();
			if (bulletFamasGun == null)
			{
				bulletFamasGun = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletFamasGun);
			}
			bulletFamasGun.Active(attackData, crossAimPoint, this.bulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
			bulletFamasGun.transform.Rotate(0f, 0f, (float)i * num);
			this.ActiveMuzzle();
		}
	}
}
