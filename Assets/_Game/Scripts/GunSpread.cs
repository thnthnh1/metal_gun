using System;
using UnityEngine;

public class GunSpread : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Spread/gun_spread_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		for (int i = -2; i < 3; i++)
		{
			BulletSpreadGun bulletSpreadGun = Singleton<PoolingController>.Instance.poolBulletSpreadGun.New();
			if (bulletSpreadGun == null)
			{
				bulletSpreadGun = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletSpreadGun);
			}
			bulletSpreadGun.Active(attackData, this.firePoint, this.bulletSpeed, null);
			bulletSpreadGun.transform.Rotate(0f, 0f, (float)i * 10f);
		}
		this.ActiveMuzzle();
	}
}
