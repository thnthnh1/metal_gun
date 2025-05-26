using System;
using UnityEngine;

public class Shotgun : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Shotgun/shotgun_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletShotgun bulletShotgun = Singleton<PoolingController>.Instance.poolBulletShotgun.New();
		if (bulletShotgun == null)
		{
			bulletShotgun = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletShotgun);
		}
		bulletShotgun.Active(attackData, this.firePoint, this.bulletSpeed, null);
		this.ActiveMuzzle();
		Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.2f);
	}
}
