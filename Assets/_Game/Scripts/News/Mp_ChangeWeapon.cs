using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Mp_ChangeWeapon : MonoBehaviour
{
	public int weaponID;
	public bool swapWeapon;

	[Header("Weapon Identity")]
	public SpriteRenderer sprite;

    void OnTriggerEnter2D(Collider2D collider)
    {
		if (collider.gameObject.tag == "Player")
		{
			if (collider.gameObject.GetComponent<PhotonView>())
			{
				if (collider.gameObject.GetComponent<PhotonView>().IsMine)
				{
					if (!collider.gameObject.GetComponent<MP_Player>().isBot)
					{
						HudManager.instance.changeWeaponButton.gameObject.SetActive(true);
						HudManager.instance.changeWeaponButton.onClick.AddListener(delegate { HudManager.instance.ChangeWeaponAction(weaponID, gameObject.name); });
					}
				}
			}
			if (SceneManager.GetActiveScene().name == "Demo")
			{
				if (!collider.gameObject.GetComponent<MP_Player_Demo>().isBot)
				{
					HudManager.instance.changeWeaponButton.gameObject.SetActive(true);
					HudManager.instance.changeWeaponButton.onClick.AddListener(delegate { HudManager.instance.ChangeWeaponAction(weaponID, gameObject.name); });
					Nik_Demo.instance.WeaponItem.SetActive(false);
					Nik_Demo.instance.Arrow.SetActive(false);
					Debug.Log("<color=blue>Change Weapon Is and Name : </color>" +weaponID + " : " + gameObject.name);
				}
			}
		}
    }

	private void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			if (collider.gameObject.GetComponent<PhotonView>())
			{
				if (collider.gameObject.GetComponent<PhotonView>().IsMine)
				{
					if (!collider.gameObject.GetComponent<MP_Player>().isBot)
					{
						HudManager.instance.changeWeaponButton.gameObject.SetActive(false);
						HudManager.instance.changeWeaponButton.onClick.RemoveAllListeners();
					}
				}
			}
			if (SceneManager.GetActiveScene().name == "Demo")
			{
				if (!collider.gameObject.GetComponent<MP_Player_Demo>().isBot)
				{
					HudManager.instance.changeWeaponButton.gameObject.SetActive(false);
					HudManager.instance.changeWeaponButton.onClick.RemoveAllListeners();
					Nik_Demo.instance.WeaponItem.SetActive(true);
					Nik_Demo.instance.Arrow.SetActive(true);
					Debug.Log("<color=red>Change Weapon Exit</color>");
				}
			}
		}
	}

	public void ChangeSprite(Sprite newIcon)
	{
		sprite.sprite = newIcon;
	}
}
