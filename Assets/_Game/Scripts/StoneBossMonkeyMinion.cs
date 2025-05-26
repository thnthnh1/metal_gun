using System;
using UnityEngine;

public class StoneBossMonkeyMinion : BaseBullet
{
	public AudioClip soundMoving;

	private Vector3 destination;

	protected override void Move()
	{
		Vector3 vector = this.destination - base.transform.position;
		base.transform.right = Vector3.MoveTowards(base.transform.right, vector.normalized, 4f * Time.deltaTime);
		base.transform.Translate(base.transform.right * this.moveSpeed * Time.deltaTime, Space.World);
	}

	public override void Deactive()
	{
		base.Deactive();
		this.audioSource.Stop();
		Singleton<PoolingController>.Instance.poolStoneBossMonkeyMinion.Store(this);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.StoneBrokenSmall, base.transform.position);
		Singleton<CameraFollow>.Instance.AddShake(0.1f, 0.2f);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null)
			{
				unit.TakeDamage(this.attackData);
			}
		}
		this.SpawnHitEffect();
		this.Deactive();
	}

	public void Active(AttackData attackData, Transform startPoint, Transform endPoint, Vector2 throwDirection)
	{
		this.attackData = attackData;
		base.transform.position = startPoint.position;
		base.transform.rotation = startPoint.rotation;
		this.destination = endPoint.position;
		this.SetTagAndLayer();
		base.gameObject.SetActive(true);
		Vector3 vector = endPoint.position - startPoint.position;
		vector = MathUtils.ProjectVectorOnPlane(Vector3.up, vector);
		float num = Vector2.Angle((throwDirection.x <= 0f) ? Vector3.left : Vector3.right, throwDirection);
		float yOffset = -vector.y;
		float d = MathUtils.CalculateLaunchSpeed(vector.magnitude, yOffset, Physics2D.gravity.magnitude, num * 0.0174532924f);
		this.rigid.velocity = throwDirection * d;
		if (this.soundMoving)
		{
			this.audioSource.loop = true;
			this.audioSource.PlayOneShot(this.soundMoving);
		}
	}
}
