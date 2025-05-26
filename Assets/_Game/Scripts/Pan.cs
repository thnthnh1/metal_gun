using System;
using UnityEngine;

public class Pan : BaseMeleeWeapon
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Melee Weapon/Pan/pan_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_MeleeWeaponStats>(path);
	}
}
