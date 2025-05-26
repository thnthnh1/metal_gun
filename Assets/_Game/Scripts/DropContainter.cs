using System;
using UnityEngine;

public class DropContainter : MonoBehaviour
{
	private Rigidbody2D rigid;

	private bool isGrounded;

	private void Awake()
	{
		this.rigid = base.GetComponent<Rigidbody2D>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			this.Drop();
		}
		else if (other.transform.root.CompareTag("Map") && !this.isGrounded)
		{
			this.isGrounded = true;
			this.SetDefault();
			Singleton<CameraFollow>.Instance.AddShake(0.5f, 0.3f);
			SoundManager.Instance.PlaySfx("sfx_trigger_water", 0f);
		}
	}

	private void Drop()
	{
		this.rigid.bodyType = RigidbodyType2D.Dynamic;
		this.rigid.useAutoMass = true;
	}

	private void SetDefault()
	{
		this.rigid.bodyType = RigidbodyType2D.Kinematic;
	}
}
