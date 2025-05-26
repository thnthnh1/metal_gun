using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MP_Bullet : MonoBehaviour
{
	public float damage;
	public string playerFired;

	[Header("Bullet Rotation")]
	public Transform rotator;
	public float rotationSpeed;

	public float destroyTime = 2;

	private void Start()
	{
		if (rotator)
		{
			StartCoroutine(Rotate());
		}
	}
	private void OnCollisionEnter2D(Collision2D collision)
	{
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<CapsuleCollider2D>().enabled = false;
		GetComponentInChildren<SpriteRenderer>().enabled = false;

		GetComponentInChildren<ParticleSystem>().Play();

		Destroy(gameObject, destroyTime);
	}
	IEnumerator Rotate()
	{
		yield return new WaitForSeconds(0.01f);

		rotator.Rotate(new Vector3(0, 0, 1), rotationSpeed);

		StartCoroutine(Rotate());
	}
}
