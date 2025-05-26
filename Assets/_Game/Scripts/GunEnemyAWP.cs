using System;
using UnityEngine;

public class GunEnemyAWP : BaseGunEnemy
{
	public SniperLaser laserAim;

	public void ActiveLaserAim(bool isActive)
	{
		this.laserAim.gameObject.SetActive(isActive);
	}

	public override void Attack(BaseEnemy attacker)
	{
		base.Attack(attacker);
		BulletSniper bulletSniper = Singleton<PoolingController>.Instance.poolBulletSniper.New();
		if (bulletSniper == null)
		{
			bulletSniper = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletSniper);
		}
		bulletSniper.Active(attacker.GetCurentAttackData(), this.firePoint, attacker.baseStats.BulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
	}
}
