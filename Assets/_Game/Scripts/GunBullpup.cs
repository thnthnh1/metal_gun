using System;
using UnityEngine;

public class GunBullpup : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Bullpup/gun_bullpup_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletBullpup bulletBullpup = Singleton<PoolingController>.Instance.poolBulletBullpup.New();
		if (bulletBullpup == null)
		{
			bulletBullpup = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletBullpup);
		}
		bulletBullpup.Active(attackData, this.firePoint, this.bulletSpeed, null);
		Vector3 vector = bulletBullpup.transform.position;
		vector += this.firePoint.up * UnityEngine.Random.Range(-0.15f, 0.15f);
		bulletBullpup.transform.position = vector;
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
			BulletBullpup bulletBullpup = Singleton<PoolingController>.Instance.poolBulletBullpup.New();
			if (bulletBullpup == null)
			{
				bulletBullpup = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletBullpup);
			}
			bulletBullpup.Active(attackData, crossAimPoint, this.bulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
			bulletBullpup.transform.Rotate(0f, 0f, (float)i * num);
			this.ActiveMuzzle();
		}
	}
}
