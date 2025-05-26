using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Mp_Weapon : MonoBehaviour
{
	public MP_Player localPlayerReference;
	public MP_Player_Demo localPlayerReference_Demo;

	[Header("Attack values")]
	public float damage;
	public float attackRate;
	public float bulletForce;
	public Transform cannonPos;
	public GameObject muzzle;
	public GameObject bulletPrefab;
	public ParticleSystem trashParticles;

	[Header("Dispersion Weapon")]
	public float dispersion;

	[Header("Ammo values")]
	public float ammo;
	private float ammoReference;
	public float maxAmmo;
	private float maxAmmoReference;
	public float reloadTime;

	[Header("Zoom Values")]
	public float zoomValue = 6;

	[Header("Weapon Image for UI")]
	public Sprite weaponImage;

	public GameObject line;

	[Header("Audio values")]
	public List <AudioSource> soundsPool;

	[Header("States")]
	public bool reloading;

	private PhotonView pv;
	private MP_Player playerScript;
	private MP_Player_Demo playerScript_Demo;


	private void Awake()
	{
		ammoReference = ammo;
		maxAmmoReference = maxAmmo;

		
		if (SceneManager.GetActiveScene().name == "Demo")
		{
			playerScript_Demo = transform.parent.parent.GetComponent<MP_Player_Demo>();
		}
		else
		{
			pv = GetComponent<PhotonView>();
			playerScript = transform.parent.parent.GetComponent<MP_Player>();
		}
	}

	public void Attack(string playerFired)
    {
		//Add dispersion
		if (dispersion != 0)
		{
			cannonPos.localRotation = Quaternion.Euler(0, 0, Random.Range(-dispersion, dispersion));
		}

		//localPlayerReference.inReverse
		Vector3 finalDirection = Vector3.zero;

		if (SceneManager.GetActiveScene().name == "Demo")
		{
			if (localPlayerReference_Demo.inReverse)
			{
				finalDirection = cannonPos.right * -bulletForce;
			}
			else
			{
				finalDirection = cannonPos.right * bulletForce;
			}
			Debug.Log("Nik Log Attack Call");
			AttackRPC_Local(playerFired, cannonPos.position, cannonPos.rotation, finalDirection, localPlayerReference_Demo.inReverse);
		}
		else
		{
			if (localPlayerReference.inReverse)
			{
				finalDirection = cannonPos.right * -bulletForce;
			}
			else
			{
				finalDirection = cannonPos.right * bulletForce;
			}
			if (pv.IsMine)
			{
				pv.RPC("AttackRPC", RpcTarget.All, playerFired, cannonPos.position, cannonPos.rotation, finalDirection, localPlayerReference.inReverse);
			}
		}
	}
	[PunRPC]
	public void AttackRPC(string playerFired, Vector3 bulletPos, Quaternion bulletRotation , Vector3 forceDirection, bool inverseDirection)
	{
		if (ammo <= 0)
		{
			StartCoroutine(Reload());
			return;
		}

		muzzle.SetActive(true);

		

		GameObject newBullet = Instantiate(bulletPrefab, bulletPos, bulletRotation);
		//GameObject newBullet = PhotonNetwork.InstantiateRoomObject(bulletPrefab.name, cannonPos.position, cannonPos.rotation);

		newBullet.GetComponent<MP_Bullet>().damage = damage;
		newBullet.GetComponent<MP_Bullet>().playerFired = playerFired;

		if (inverseDirection)
		{
			//newBullet.GetComponent<Rigidbody2D>().AddForce(cannonPos.right * -bulletForce);

			newBullet.transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			//newBullet.GetComponent<Rigidbody2D>().AddForce(cannonPos.right * bulletForce);
		}

		newBullet.GetComponent<Rigidbody2D>().AddForce(forceDirection);

		if (trashParticles)
		{
			trashParticles.Play();
		}

		if (gameObject.activeSelf)
		{
			StartCoroutine(DesactivateMuzzle());
		}

		for (int i = 0; i < soundsPool.Count; i++)
		{
			if (!soundsPool[i].isPlaying)
			{
				soundsPool[i].Play();
				break;
			}
		}

		ammo--;

		//Refresh ammo in UI
		if (pv.IsMine && !playerScript.isBot)
		{
			HudManager.instance.RefreshAmmo(ammo, maxAmmo);
		}

	}
	public void AttackRPC_Local(string playerFired, Vector3 bulletPos, Quaternion bulletRotation , Vector3 forceDirection, bool inverseDirection)
	{
		Debug.Log("Nik Log AttackRPC_Local Call Enter");
		if (ammo <= 0)
		{
			StartCoroutine(Reload());
			return;
		}

		muzzle.SetActive(true);

		

		GameObject newBullet = Instantiate(bulletPrefab, bulletPos, bulletRotation);
		//GameObject newBullet = PhotonNetwork.InstantiateRoomObject(bulletPrefab.name, cannonPos.position, cannonPos.rotation);

		newBullet.GetComponent<MP_Bullet>().damage = damage;
		newBullet.GetComponent<MP_Bullet>().playerFired = playerFired;

		if (inverseDirection)
		{
			//newBullet.GetComponent<Rigidbody2D>().AddForce(cannonPos.right * -bulletForce);

			newBullet.transform.localScale = new Vector3(-1, 1, 1);
		}
		else
		{
			//newBullet.GetComponent<Rigidbody2D>().AddForce(cannonPos.right * bulletForce);
		}

		newBullet.GetComponent<Rigidbody2D>().AddForce(forceDirection);

		if (trashParticles)
		{
			trashParticles.Play();
		}

		if (gameObject.activeSelf)
		{
			StartCoroutine(DesactivateMuzzle());
		}

		for (int i = 0; i < soundsPool.Count; i++)
		{
			if (!soundsPool[i].isPlaying)
			{
				soundsPool[i].Play();
				break;
			}
		}

		ammo--;

		//Refresh ammo in UI
		if (!playerScript_Demo.isBot)
		{
			HudManager.instance.RefreshAmmo(ammo, maxAmmo);
		}
		if (Nik_Demo.instance.WeaponJS.Active())
		{
			if (!Nik_Demo.instance.isweaponMove)
			{
				Debug.Log("<color>Nik log is Tutorial Step Change 2</color>");
				Nik_Demo.instance.StartTutorialStep2();
			}
		}

	}

	public IEnumerator Reload()
	{
		reloading = true;
		yield return new WaitForSeconds(reloadTime);

		float oldAmmo = ammo;
		maxAmmo += oldAmmo;

		if (maxAmmo > ammoReference)
		{

			ammo = ammoReference;

			maxAmmo -= ammoReference;
		}
		else
		{
			ammo = maxAmmo;
			maxAmmo = 0;
		}

		if (SceneManager.GetActiveScene().name == "Demo")
		{
			if (!playerScript_Demo.isBot)
			{
				HudManager.instance.RefreshAmmo(ammo, maxAmmo);
			}
			else if (playerScript_Demo.isBot)
			{
				ammo = ammoReference;
			}
		}
		else
		{
			if (pv.IsMine && !playerScript.isBot)
			{
				HudManager.instance.RefreshAmmo(ammo, maxAmmo);
			}
			else if (pv.IsMine && playerScript.isBot)
			{
				ammo = ammoReference;
			}
		}
		
		
		reloading = false;
	}
	IEnumerator DesactivateMuzzle()
	{
		yield return new WaitForSeconds(0.2f);
		muzzle.SetActive(false);
	}

	public void RefillWeapon()
	{
		ammo = ammoReference;
		maxAmmo = maxAmmoReference;

		if (SceneManager.GetActiveScene().name == "Demo")
		{
			if (!playerScript_Demo.isBot)
			{
				HudManager.instance.RefreshAmmo(ammo, maxAmmo);
			}
		}
		else
		{
			if (pv.IsMine && !playerScript.isBot)
			{
				HudManager.instance.RefreshAmmo(ammo, maxAmmo);
			}
		}
	}
}
