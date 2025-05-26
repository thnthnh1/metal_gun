using System;
using UnityEngine;

public class GunSplit : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Split/gun_split_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunSplitStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletSplitGun bulletSplitGun = Singleton<PoolingController>.Instance.poolBulletSplitGun.New();
		if (bulletSplitGun == null)
		{
			bulletSplitGun = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletSplitGun);
		}
		bulletSplitGun.Active(attackData, this.firePoint, this.baseStats.BulletSpeed, true, ((SO_GunSplitStats)this.baseStats).DamageSplit, null, Singleton<PoolingController>.Instance.groupBullet);
		this.ActiveMuzzle();
	}
}
