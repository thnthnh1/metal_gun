using System;
using UnityEngine;

public class RocketBossMegatank : BaseBullet
{
	public AudioClip soundExplode;

	private bool isHitPlayer;

	private Vector3 destinationRocket;

	protected override void Move()
	{
		Vector3 vector = this.destinationRocket - base.transform.position;
		base.transform.right = Vector3.MoveTowards(base.transform.right, vector.normalized, 4f * Time.deltaTime);
		base.transform.Translate(base.transform.right * this.moveSpeed * Time.deltaTime, Space.World);
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolRocketBossMegatank.Store(this);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, base.transform.position);
		Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.35f);
		SoundManager.Instance.PlaySfx(this.soundExplode, 0f);
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		string tag = base.tag;
		if (tag != null)
		{
			if (tag == "Bullet Enemy")
			{
				if (other.transform.root.CompareTag("Player"))
				{
					BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
					if (unit != null)
					{
						unit.TakeDamage(this.attackData);
						this.isHitPlayer = true;
					}
				}
			}
		}
		this.SpawnHitEffect();
		if (this.isHitPlayer)
		{
			EventDispatcher.Instance.PostEvent(EventID.RocketMegatankHitPlayer);
		}
		else
		{
			EventDispatcher.Instance.PostEvent(EventID.RocketMegatankMissPlayer);
		}
		this.Deactive();
	}

	public void Active(AttackData attackData, Transform startPoint, Transform endPoint, Vector2 throwDirection)
	{
		this.attackData = attackData;
		base.transform.position = startPoint.position;
		base.transform.rotation = startPoint.rotation;
		this.destinationRocket = endPoint.position;
		this.isHitPlayer = false;
		this.SetTagAndLayer();
		base.gameObject.SetActive(true);
		Vector3 vector = endPoint.position - startPoint.position;
		vector = MathUtils.ProjectVectorOnPlane(Vector3.up, vector);
		float num = Vector2.Angle((throwDirection.x <= 0f) ? Vector3.left : Vector3.right, throwDirection);
		float yOffset = -vector.y;
		float d = MathUtils.CalculateLaunchSpeed(vector.magnitude, yOffset, Physics2D.gravity.magnitude, num * 0.0174532924f);
		this.rigid.velocity = throwDirection * d;
	}
}
