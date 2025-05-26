using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTab : MonoBehaviour
{
	public WeaponTab tab;

	public Image bg;

	public Image label;

	public GameObject notification;

	public Sprite bgSelect;

	public Sprite bgUnselect;

	public Sprite labelSelect;

	public Sprite labelUnselect;

	public void Highlight(bool isActive)
	{
		this.bg.sprite = ((!isActive) ? this.bgUnselect : this.bgSelect);
		this.bg.SetNativeSize();
		this.label.sprite = ((!isActive) ? this.labelUnselect : this.labelSelect);
		this.label.SetNativeSize();
	}

	public void UpdateNotification()
	{
		switch (this.tab)
		{
		case WeaponTab.Rifle:
		{
			bool active = false;
			foreach (KeyValuePair<int, PlayerGunData> current in GameData.playerGuns)
			{
				if (current.Value.isNew && !GameData.staticGunData[current.Key].isSpecialGun)
				{
					active = true;
					break;
				}
			}
			this.notification.SetActive(active);
			break;
		}
		case WeaponTab.Special:
		{
			bool active2 = false;
			foreach (KeyValuePair<int, PlayerGunData> current2 in GameData.playerGuns)
			{
				if (current2.Value.isNew && GameData.staticGunData[current2.Key].isSpecialGun)
				{
					active2 = true;
					break;
				}
			}
			this.notification.SetActive(active2);
			break;
		}
		case WeaponTab.Grenade:
		{
			bool active3 = false;
			foreach (KeyValuePair<int, PlayerGrenadeData> current3 in GameData.playerGrenades)
			{
				if (current3.Value.isNew)
				{
					active3 = true;
					break;
				}
			}
			this.notification.SetActive(active3);
			break;
		}
		case WeaponTab.MeleeWeapon:
		{
			bool active4 = false;
			foreach (KeyValuePair<int, PlayerMeleeWeaponData> current4 in GameData.playerMeleeWeapons)
			{
				if (current4.Value.isNew)
				{
					active4 = true;
					break;
				}
			}
			this.notification.SetActive(active4);
			break;
		}
		}
	}

	public void OnClick()
	{
		EventDispatcher.Instance.PostEvent(EventID.SwichTabUpgradeWeapon, this.tab);
	}
}
