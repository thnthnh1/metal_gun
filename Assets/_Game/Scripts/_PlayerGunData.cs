using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerGunData : Dictionary<int, PlayerGunData>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerGunData.Set(value);
		ProfileManager.SaveAll();
	}

	public void ReceiveNewGun(int id)
	{
		if (GameData.staticGunData.ContainsKey(id))
		{
			if (base.ContainsKey(id))
			{
				if (GameData.gunValueGem.ContainsKey(id))
				{
					int value = GameData.gunValueGem[id];
					GameData.playerResources.ReceiveGem(value);
					SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
				}
				else
				{
					PlayerGunData playerGunData = base[id];
					playerGunData.level += 2;
					playerGunData.level = Mathf.Clamp(playerGunData.level, 1, GameData.staticGunData[id].upgradeInfo.Length);
				}
			}
			else
			{
				StaticGunData data = GameData.staticGunData.GetData(id);
				PlayerGunData playerGunData2 = new PlayerGunData();
				playerGunData2.id = id;
				playerGunData2.level = 1;
				playerGunData2.isNew = true;
				if (data.isSpecialGun)
				{
					SO_GunStats baseStats = GameData.staticGunData.GetBaseStats(id, playerGunData2.level);
					playerGunData2.ammo = baseStats.Ammo;
				}
				else
				{
					playerGunData2.ammo = 0;
				}
				base.Add(id, playerGunData2);
			}
			this.Save();
		}
	}

	public int GetGunAmmo(int id)
	{
		if (base.ContainsKey(id))
		{
			return base[id].ammo;
		}
		return 0;
	}

	public void SetGunAmmo(int id, int ammo)
	{
		if (base.ContainsKey(id))
		{
			base[id].ammo = ammo;
			this.Save();
		}
	}

	public int GetGunLevel(int id)
	{
		int result = 1;
		if (base.ContainsKey(id))
		{
			result = base[id].level;
		}
		return result;
	}

	public void IncreaseGunLevel(int id)
	{
		if (base.ContainsKey(id))
		{
			int num = base[id].level;
			num++;
			base[id].level = num;
			this.Save();
		}
	}

	public void SetNew(int id, bool isNew)
	{
		if (base.ContainsKey(id))
		{
			base[id].isNew = isNew;
			this.Save();
		}
	}

	public int GetNumberOfNormalGun()
	{
		int num = 0;
		foreach (PlayerGunData current in base.Values)
		{
			StaticGunData data = GameData.staticGunData.GetData(current.id);
			if (data != null && !data.isSpecialGun)
			{
				num++;
			}
		}
		return num;
	}

	public int GetNumberOfSpecialGun()
	{
		int num = 0;
		foreach (PlayerGunData current in base.Values)
		{
			StaticGunData data = GameData.staticGunData.GetData(current.id);
			if (data != null && data.isSpecialGun)
			{
				num++;
			}
		}
		return num;
	}
}
