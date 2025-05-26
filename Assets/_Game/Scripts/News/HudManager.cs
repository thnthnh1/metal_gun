using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class HudManager : MonoBehaviour
{
	public static HudManager instance;
	[Header("Health Bar")]
	public Slider healthBar;

	[Header("Jetpack Bar")]
	public Slider jetpackbar;

	[Header("Weapons status")]
	public Text ammoText;
	public Text granadeText;

	public Image weaponImage;

	[Header("Jump Button")]
	public bool jumpPushed;
	public bool jetpackPushed;
	public GameObject jumpButton;
	public GameObject jetpackButton;

	[Header("Change Weapon Button")]
	public Button changeWeaponButton;

	[Header("Fire Status")]
	public bool firing = false;

	public MP_Player localPlayerReference;
	public MP_Player_Demo localPlayerReference_Demo;

	public CnControls.SimpleJoystick joystick;
    // Start is called before the first frame update
    void Awake()
    {
		instance = this;

	}

	void Update()
	{
		firing = joystick.active;
	}


	public void RefreshHealthBar(float value, float maxValue)
	{
		healthBar.maxValue = maxValue;
		healthBar.value = value;
	}

	public void RefreshJetpackBar(float value, float maxValue)
	{
		jetpackbar.maxValue = maxValue;
		jetpackbar.value = value;
	}

	public void ReloadAction()
	{
		if (SceneManager.GetActiveScene().name == "Demo")
		{
			if (!localPlayerReference_Demo.currentWeapon.reloading)
			{
				StartCoroutine(localPlayerReference_Demo.currentWeapon.Reload());
				if (!Nik_Demo.instance.isWeaponReload)
				{
					Debug.Log("<color>Nik log is Tutorial Step Change 7</color>");
					Nik_Demo.instance.StartTutorialStep7();
				}
				
			}
		}
		else
		{
			if (!localPlayerReference.currentWeapon.reloading)
			{
				StartCoroutine(localPlayerReference.currentWeapon.Reload());
			}
		}
	}

	public void RefreshAmmo(float value, float maxValue)
	{
		ammoText.text = value + " / " + maxValue;

		if (value<=0)
		{
			ammoText.color = Color.red;
		}
		else
		{
			ammoText.color = Color.white;
		}
	}

	public void RefreshGranades(float value)
	{
		granadeText.text = value.ToString();

		if (value <= 0)
		{
			granadeText.color = Color.red;
		}
		else
		{
			granadeText.color = Color.white;
		}
	}
	public void ThrowGranadeAction()
	{
		if (SceneManager.GetActiveScene().name == "Demo")
		{
			localPlayerReference_Demo.ThrowGranade();
		}
		else
		{
			localPlayerReference.ThrowGranade();
		}
	}

	public void JumpOn()
	{
		jumpPushed = true;
		jetpackPushed = false;
	}
	public void JumpOff()
	{
		jumpPushed = false;
		jetpackPushed = false;
	}
	public void JetpackPressed()
	{
		jumpPushed = false;
		jetpackPushed = true;
	}

	public void ReleaseJetpack()
	{
		jumpPushed = false;
		jetpackPushed = false;
	}

	public void ResetJumpButtons()
	{
		jumpPushed = false;
		jetpackPushed = false;

		jumpButton.SetActive(true);
		jetpackButton.SetActive(false);
	}

	public void FireOn()
	{
		firing = true;
	}
	public void FireOff()
	{
		firing = false;
	}

	public void ChangeWeaponAction(int weaponID, string containerName)
	{
		if (SceneManager.GetActiveScene().name == "Demo")
		{
			Debug.Log("<color=blue>Change Weapon Is and Name : </color>" +weaponID + " : " + containerName);
			localPlayerReference_Demo.ChangeWeapon(weaponID, containerName);
			if (!Nik_Demo.instance.isReload)
			{
				Debug.Log("<color>Nik log is Tutorial Step Change 8</color>");
				Nik_Demo.instance.StartTutorialStep8();
			}
			
		}
		else
		{
			localPlayerReference.GetComponent<PhotonView>().RPC("ChangeWeapon", RpcTarget.All, weaponID, containerName);
		}
	}
}
