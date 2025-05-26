using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon;

public class Mp_Granade : MonoBehaviour
{
	public string playerFired;

	[Header("Time to explode")]
	public float explodeTime;

	[Header("radius")]
	public float explotionRadius;

	[Header("explotion Effect")]
	public ParticleSystem explotionEffect;

	[Header("Enable to Explode Time")]
	public float activationTime;
	public bool activated = false;

	public RaycastHit2D[] hits;

	// Start is called before the first frame update
	void Start()
	{
		if (SceneManager.GetActiveScene().name == "Demo")
		{
			Invoke("explode", explodeTime);
			Invoke("ActivateGranade", activationTime);
			GetComponent<Rigidbody2D>().angularVelocity = Random.Range(0f, 1f);
		}
		else
		{
			if (PhotonNetwork.IsMasterClient)
			{
				Debug.Log("Dev || - GRANADE Is start");
				Invoke("explode", explodeTime);
				// Invoke("ActivateGranade", activationTime);
				GetComponent<Rigidbody2D>().angularVelocity = Random.Range(0f, 1f);
			}
		}
	}

	void ActivateGranade() 
	{
		Debug.Log("Dev || - GRANADE Is ActivateGranade");
		activated = true;
	}

	void explode()
	{
		Debug.Log("Dev || - GRANADE Is explode");
		Debug.Log("Nik log explode in activated " + activated);
		if (activated)
		{
			if (SceneManager.GetActiveScene().name == "Demo")
			{
				SendExplode_Local();
			}
			else
			{
				GetComponent<PhotonView>().RPC("SendExplode", RpcTarget.All);
				// // GetComponent<PhotonView>().RPC("Explode", PhotonTargets.All);
			}
			activated = false;
		}
	}

	[PunRPC]
	public void SendExplode() 
	{
		Debug.Log("Dev || - GRANADE Is SendExplode");
		GetComponent<AudioSource>().Play();

		GetComponent<Rigidbody2D>().simulated = false;
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<CircleCollider2D>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;

		if (PhotonNetwork.IsMasterClient)
		{
			hits = Physics2D.CircleCastAll(transform.position, explotionRadius, Vector2.zero);

			for (int i = 0; i < hits.Length; i++)
			{
				if (hits[i].transform.gameObject.tag == "Player")
				{
					hits[i].transform.GetComponent<MP_Player>().ApplyDamage(100, playerFired);
				}
			}
		}

		explotionEffect.Play();

		Destroy(gameObject, 3);
		// PhotonNetwork.Destroy(GetComponent<PhotonView>());
	}
	public void SendExplode_Local() 
	{
		Debug.Log("Dev || - GRANADE Is SendExplode_Local");
		GetComponent<AudioSource>().Play();

		GetComponent<Rigidbody2D>().simulated = false;
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<CircleCollider2D>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;

		hits = Physics2D.CircleCastAll(transform.position, explotionRadius, Vector2.zero);

		for (int i = 0; i < hits.Length; i++)
		{
			if (hits[i].transform.gameObject.tag == "Player")
			{
				hits[i].transform.GetComponent<MP_Player_Demo>().ApplyDamage(100, playerFired);
			}
		}

		explotionEffect.Play();

		Destroy(gameObject, 3);
		if (Nik_Demo.instance.isGrenade)
		{
			Nik_Demo.instance.WeaponJS.IsAcitve = false;
			Debug.Log("<color>Nik log is Tutorial Step Change 6</color>");
			Nik_Demo.instance.StartTutorialStep6();
		}
		
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			Debug.Log("<color=red>Nik Hit the player</color>");
			explode();
		}
	}
}
