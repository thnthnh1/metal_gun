using System;
using System.Collections.Generic;

[Serializable]
public class StaticGunData
{
	public int id;

	public int index;

	public bool isSpecialGun;

	public string gunName;

	public string statsPath;

	public int coinUnlock;

	public int gemUnlock;

	public int medalUnlock;

	public int ammoPrice;

	public int[] upgradeInfo;

	public List<WayToObtain> otherWayObtain;
}
