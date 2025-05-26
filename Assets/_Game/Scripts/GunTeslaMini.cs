using System;
using System.Collections.Generic;
using UnityEngine;

public class GunTeslaMini : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Tesla Mini/gun_tesla_mini_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunTeslaMiniStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletTeslaMini bulletTeslaMini = Singleton<PoolingController>.Instance.poolBulletTeslaMini.New();
		if (bulletTeslaMini == null)
		{
			bulletTeslaMini = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletTeslaMini);
		}
		float num = Mathf.Clamp01(((SO_GunTeslaMiniStats)this.baseStats).StunChance / 100f);
		bool flag = UnityEngine.Random.Range(0f, 1f) <= num;
		if (flag)
		{
			DebuffData item = new DebuffData(DebuffType.Stun, ((SO_GunTeslaMiniStats)this.baseStats).StunDuration, 0f);
			if (attackData.debuffs == null)
			{
				attackData.debuffs = new List<DebuffData>
				{
					item
				};
			}
			else
			{
				attackData.debuffs.Add(item);
			}
		}
		bulletTeslaMini.Active(attackData, this.firePoint, this.bulletSpeed, null);
		this.ActiveMuzzle();
	}
}
