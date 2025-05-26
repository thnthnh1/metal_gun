using System;
using UnityEngine;

public class Knife : BaseMeleeWeapon
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Melee Weapon/Knife/knife_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_MeleeWeaponStats>(path);
	}
}
