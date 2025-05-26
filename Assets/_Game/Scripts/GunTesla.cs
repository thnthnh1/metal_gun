using System;
using UnityEngine;

public class GunTesla : BaseGun
{
	public Transform teslaPoint;

	public Tesla teslaPrefab;

	[HideInInspector]
	public BaseUnit shooter;

	private Tesla tesla;

	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Tesla/gun_tesla_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_GunTeslaStats>(path);
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
		this.CreateTesla();
		EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, delegate(Component sender, object param)
		{
			this.ActiveTesla((bool)param);
		});
	}

	private void OnDisable()
	{
		if (this)
		{
			this.tesla.Active(false);
		}
	}

	private void OnEnable()
	{
		if (this && ((Rambo)this.shooter).isFiring)
		{
			this.ActiveTesla(true);
		}
	}

	public override void Attack(AttackData attackData)
	{
	}

	private void ActiveTesla(bool isActive)
	{
		if (this && base.gameObject.activeInHierarchy)
		{
			if (isActive)
			{
				if (this.ammo <= 0)
				{
					this.ammo = 0;
					EventDispatcher.Instance.PostEvent(EventID.OutOfAmmo);
					return;
				}
				this.tesla.Active(true);
			}
			else
			{
				this.tesla.Active(false);
			}
		}
	}

	private void CreateTesla()
	{
		this.tesla = UnityEngine.Object.Instantiate<Tesla>(this.teslaPrefab, this.teslaPoint.position, this.teslaPoint.rotation, this.firePoint);
		this.tesla.gun = this;
		this.tesla.gameObject.SetActive(false);
	}
}
