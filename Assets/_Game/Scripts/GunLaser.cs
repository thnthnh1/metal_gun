using System;
using UnityEngine;

public class GunLaser : BaseGun
{
	public Transform laserPoint;

	public LaserByGun laserPrefab;

	[HideInInspector]
	public BaseUnit shooter;

	private LaserByGun laser;

	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Laser/gun_laser_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunLaserStats>(path);
	}

	protected override void Awake()
	{
		BaseUnit component = base.transform.root.GetComponent<BaseUnit>();
		if (component is Vehicle)
		{
			this.shooter = ((Vehicle)component).Player;
		}
		else
		{
			this.shooter = component;
		}
		this.CreateLaser();
		EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, delegate(Component sender, object param)
		{
			this.ActiveLaser((bool)param);
		});
	}

	private void OnDisable()
	{
		if (this)
		{
			this.laser.Active(false);
		}
	}

	private void OnEnable()
	{
		if (this && ((Rambo)this.shooter).isFiring)
		{
			this.ActiveLaser(true);
		}
	}

	public override void Attack(AttackData attackData)
	{
	}

	private void ActiveLaser(bool isActive)
	{
		if (this && base.gameObject.activeInHierarchy)
		{
			if (this.muzzle == null)
			{
				this.muzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.muzzlePrefab, this.muzzlePoint.position, this.muzzlePoint.rotation, this.muzzlePoint.parent);
			}
			if (isActive)
			{
				if (this.ammo <= 0)
				{
					this.ammo = 0;
					EventDispatcher.Instance.PostEvent(EventID.OutOfAmmo);
					return;
				}
				this.laser.Active(true);
				this.muzzle.Active();
			}
			else
			{
				this.laser.Active(false);
				this.muzzle.Deactive();
			}
		}
	}

	private void CreateLaser()
	{
		this.laser = UnityEngine.Object.Instantiate<LaserByGun>(this.laserPrefab, this.laserPoint.position, this.laserPoint.rotation, this.firePoint);
		this.laser.gun = this;
		this.laser.gameObject.SetActive(false);
	}
}
