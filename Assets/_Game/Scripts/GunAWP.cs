using System;
using UnityEngine;

public class GunAWP : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/AWP/gun_awp_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletAWP bulletAWP = Singleton<PoolingController>.Instance.poolBulletAWP.New();
		if (bulletAWP == null)
		{
			bulletAWP = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletAWP);
		}
		bulletAWP.Active(attackData, this.firePoint, this.bulletSpeed, null);
		this.ActiveMuzzle();
	}
}
