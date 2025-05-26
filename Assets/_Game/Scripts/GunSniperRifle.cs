using System;
using UnityEngine;

public class GunSniperRifle : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Sniper Rifle/gun_sniper_rifle_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletSniperRifle bulletSniperRifle = Singleton<PoolingController>.Instance.poolBulletSniperRifle.New();
		if (bulletSniperRifle == null)
		{
			bulletSniperRifle = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletSniperRifle);
		}
		bulletSniperRifle.Active(attackData, this.firePoint, this.bulletSpeed, null);
		this.ActiveMuzzle();
	}
}
