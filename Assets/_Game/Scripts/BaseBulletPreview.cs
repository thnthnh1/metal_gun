using System;
using UnityEngine;

public class BaseBulletPreview : MonoBehaviour
{
	protected float moveSpeed;

	protected virtual void Update()
	{
		this.Move();
	}

	protected virtual void Move()
	{
		base.transform.Translate(Vector3.right * Time.deltaTime * this.moveSpeed);
	}

	public virtual void Active(Transform firePoint, float moveSpeed, Transform parent = null)
	{
		this.moveSpeed = moveSpeed;
		base.transform.position = firePoint.position;
		base.transform.rotation = firePoint.rotation;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
	}

	protected virtual void Deactive()
	{
		base.gameObject.SetActive(false);
	}

	protected virtual void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
		}
		this.SpawnHitEffect();
		this.Deactive();
	}

	protected virtual void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactNormal, base.transform.position);
	}
}
