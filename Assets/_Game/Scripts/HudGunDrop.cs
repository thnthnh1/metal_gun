using System;
using UnityEngine;
using UnityEngine.UI;

public class HudGunDrop : MonoBehaviour
{
	public GameObject popup;

	public Text gunName;

	public Image gunImage;

	public void Init()
	{
	}

	public void Open(int gunId)
	{
		this.gunImage.sprite = GameResourcesUtils.GetGunImage(gunId);
		this.gunImage.SetNativeSize();
		if (GameData.staticGunData.ContainsKey(gunId))
		{
			this.gunName.text = GameData.staticGunData[gunId].gunName.ToUpper();
		}
		else
		{
			this.gunName.text = string.Empty;
		}
		Singleton<GameController>.Instance.Player.enabled = false;
		Singleton<GameController>.Instance.SetActiveAllUnits(false);
		this.popup.SetActive(true);
		SoundManager.Instance.PlaySfx("sfx_show_dialog", 0f);
	}

	public void Close()
	{
		this.popup.SetActive(false);
		Singleton<GameController>.Instance.Player.enabled = true;
		Singleton<GameController>.Instance.SetActiveAllUnits(true);
	}
}
