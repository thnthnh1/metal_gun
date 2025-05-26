using System;
using UnityEngine;

public class GunFlame : BaseGun
{
	[HideInInspector]
	public BaseUnit shooter;

	public Flame flame;

	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Gun/Flame/gun_flame_lv{0}", this.level); ///original path
		// string path = string.Format("scriptableobject/gun/flame/gun_flame_lv{0}", this.level);
		Debug.Log("path " + path);
		this.baseStats = Resources.Load<SO_GunFlameStats>(path);
	}

	protected override void Awake()
	{
		this.shooter = base.transform.root.GetComponent<BaseUnit>();
		this.ActiveFlame(false);
		EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, delegate(Component sender, object param)
		{
			this.ActiveFlame((bool)param);
		});
	}

	public override void Attack(AttackData attackData)
	{
	}

	private void ActiveFlame(bool isActive)
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
				this.flame.Active();
			}
			else
			{
				this.flame.Deactive();
			}
		}
	}
}
