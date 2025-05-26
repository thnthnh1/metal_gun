using System;
using UnityEngine;

public class GunEnemyPistol : BaseGunEnemy
{
	public override void Attack(BaseEnemy attacker)
	{
		base.Attack(attacker);
		BulletPistol bulletPistol = Singleton<PoolingController>.Instance.poolBulletPistol.New();
		if (bulletPistol == null)
		{
			bulletPistol = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletPistol);
		}
		bulletPistol.Active(attacker.GetCurentAttackData(), this.firePoint, attacker.baseStats.BulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
	}
}
