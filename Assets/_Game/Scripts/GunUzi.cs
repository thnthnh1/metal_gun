using System;
using UnityEngine;

public class GunUzi : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Uzi/gun_uzi_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletUzi bulletUzi = Singleton<PoolingController>.Instance.poolBulletUzi.New();
		if (bulletUzi == null)
		{
			bulletUzi = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletUzi);
		}
		bulletUzi.Active(attackData, this.firePoint, this.bulletSpeed, null);
		Vector3 vector = bulletUzi.transform.position;
		vector += this.firePoint.up * UnityEngine.Random.Range(-0.15f, 0.15f);
		bulletUzi.transform.position = vector;
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
			BulletUzi bulletUzi = Singleton<PoolingController>.Instance.poolBulletUzi.New();
			if (bulletUzi == null)
			{
				bulletUzi = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletUzi);
			}
			bulletUzi.Active(attackData, crossAimPoint, this.bulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
			bulletUzi.transform.Rotate(0f, 0f, (float)i * num);
			this.ActiveMuzzle();
		}
	}
}
