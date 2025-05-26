using System;
using UnityEngine;

public class GunFireBall : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Fire Ball/gun_fire_ball_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunFireBallStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		FireBall fireBall = Singleton<PoolingController>.Instance.poolBulletFireBall.New();
		if (fireBall == null)
		{
			fireBall = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as FireBall);
		}
		float timeApplyDamage = ((SO_GunFireBallStats)this.baseStats).TimeApplyDamage;
		float distance = ((SO_GunFireBallStats)this.baseStats).Distance;
		fireBall.Active(attackData, this.firePoint, this.bulletSpeed, timeApplyDamage, distance, null);
		this.ActiveMuzzle();
	}
}
