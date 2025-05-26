using System;
using UnityEngine;

public class GunPreviewLaser : BaseGunPreview
{
	public Transform laserPoint;

	public BulletPreviewLaser laserPrefab;

	private BulletPreviewLaser laser;

	private bool isFiring;

	private void Awake()
	{
		this.CreateLaser();
	}

	private void OnEnable()
	{
		this.isFiring = false;
	}

	public override void Fire()
	{
		if (!this.isFiring)
		{
			this.ActiveLaser(true);
		}
	}

	private void ActiveLaser(bool isActive)
	{
		this.laser.Active(isActive);
		if (this.muzzle == null)
		{
			this.muzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.muzzlePrefab, this.muzzlePoint.position, this.muzzlePoint.rotation, this.muzzlePoint.parent);
		}
		if (isActive)
		{
			this.muzzle.Active();
		}
		else
		{
			this.muzzle.Deactive();
		}
	}

	private void CreateLaser()
	{
		this.laser = UnityEngine.Object.Instantiate<BulletPreviewLaser>(this.laserPrefab, this.laserPoint.position, this.laserPoint.rotation, this.firePoint);
		this.laser.gun = this;
	}
}
