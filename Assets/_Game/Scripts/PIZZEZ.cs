using UnityEngine;

public class PIZZEZ : BaseGun
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/pizzez/gun_pizzez_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunStats>(path);
	}

	protected override void ReleaseBullet(AttackData attackData)
	{
		base.ReleaseBullet(attackData);
		if (!this.isInfinityAmmo && this.ammo <= 0)
		{
			return;
		}
		BulletBullpup bulletAWP = Singleton<PoolingController>.Instance.poolBulletBullpup.New();
		if (bulletAWP == null)
		{
			bulletAWP = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletBullpup);
		}
		bulletAWP.Active(attackData, this.firePoint, this.bulletSpeed, null);
		this.ActiveMuzzle();
	}
}
