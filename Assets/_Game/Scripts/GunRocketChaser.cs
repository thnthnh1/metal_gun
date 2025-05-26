using System;
using UnityEngine;

public class GunRocketChaser : BaseGun
{
	public Transform rocketReadyPosition;

	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Rocket Chaser/gun_rocket_chaser_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletRocketChaser bulletRocketChaser = Singleton<PoolingController>.Instance.poolBulletRocketChaser.New();
		if (bulletRocketChaser == null)
		{
			bulletRocketChaser = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletRocketChaser);
		}
		attackData.radiusDealDamage = ((SO_GunRocketChaserStats)this.baseStats).RadiusDealDamage;
		bulletRocketChaser.Active(attackData, this.firePoint, this.rocketReadyPosition.position, this.bulletSpeed, null);
		this.ActiveMuzzle();
	}
}
