public class GunPreviewPizzez : BaseGunPreview
{
	public override void Fire()
	{
		base.Fire();
		BulletPreviewBullpup bulletPreviewAWP = Singleton<PoolingPreviewController>.Instance.bullpup.New();
		if (bulletPreviewAWP == null)
		{
			bulletPreviewAWP = (UnityEngine.Object.Instantiate<BaseBulletPreview>(this.bulletPrefab) as BulletPreviewBullpup);
		}
		bulletPreviewAWP.Active(this.firePoint, this.baseStats.BulletSpeed, Singleton<PoolingPreviewController>.Instance.group);
		this.ActiveMuzzle();
	}
}