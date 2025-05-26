using System;
using UnityEngine;

public class Guitar : BaseMeleeWeapon
{
	public override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Melee Weapon/Guitar/guitar_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_MeleeWeaponStats>(path);
	}
}
