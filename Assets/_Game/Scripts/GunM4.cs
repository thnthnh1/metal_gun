using System;
using UnityEngine;

public class GunM4 : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/M4CQB/gun_M4_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletMachineGunM4 bulletMachineGunM = Singleton<PoolingController>.Instance.poolBulletMachineGunM4.New();
		if (bulletMachineGunM == null)
		{
			bulletMachineGunM = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletMachineGunM4);
		}
		bulletMachineGunM.Active(attackData, this.firePoint, this.bulletSpeed, null);
		Vector3 vector = bulletMachineGunM.transform.position;
		vector += this.firePoint.up * UnityEngine.Random.Range(-0.15f, 0.15f);
		bulletMachineGunM.transform.position = vector;
		this.ActiveMuzzle();
	}

	public override void ReleaseCrossBullets(AttackData attackData, Transform crossAimPoint, bool isFacingRight)
	{
		base.ReleaseCrossBullets(attackData, crossAimPoint, isFacingRight);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		float num = 90f / (float)(this.numberCrossBullet + 1);
		for (int i = 0; i < this.numberCrossBullet; i++)
		{
			BulletMachineGunM4 bulletMachineGunM = Singleton<PoolingController>.Instance.poolBulletMachineGunM4.New();
			if (bulletMachineGunM == null)
			{
				bulletMachineGunM = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletMachineGunM4);
			}
			bulletMachineGunM.Active(attackData, crossAimPoint, this.bulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
			bulletMachineGunM.transform.Rotate(0f, 0f, (float)i * num);
			this.ActiveMuzzle();
		}
	}
}
