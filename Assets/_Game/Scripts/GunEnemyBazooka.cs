using System;
using UnityEngine;

public class GunEnemyBazooka : BaseGunEnemy
{
	public override void Attack(BaseEnemy attacker)
	{
		base.Attack(attacker);
		BulletBazooka bulletBazooka = Singleton<PoolingController>.Instance.poolBulletBazooka.New();
		if (bulletBazooka == null)
		{
			bulletBazooka = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletBazooka);
		}
		bulletBazooka.Active(attacker.GetCurentAttackData(), this.firePoint, attacker.baseStats.BulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
		bulletBazooka.SetTarget(attacker.target.BodyCenterPoint);
	}
}
