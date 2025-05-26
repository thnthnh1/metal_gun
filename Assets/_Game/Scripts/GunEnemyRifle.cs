using System;
using UnityEngine;

public class GunEnemyRifle : BaseGunEnemy
{
	public override void Attack(BaseEnemy attacker)
	{
		base.Attack(attacker);
		BulletRifle bulletRifle = Singleton<PoolingController>.Instance.poolBulletRifle.New();
		if (bulletRifle == null)
		{
			bulletRifle = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletRifle);
		}
		bulletRifle.Active(attacker.GetCurentAttackData(), this.firePoint, attacker.baseStats.BulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
	}
}
