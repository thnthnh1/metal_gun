using System;
using UnityEngine;

public class BulletTankCannon : BaseBullet
{
	public AudioClip soundExplode;

	public LayerMask layerVictim;

	private Vector3 destinationRocket;

	private Collider2D[] victims = new Collider2D[5];

	protected override void Move()
	{
		Vector3 vector = this.destinationRocket - base.transform.position;
		base.transform.right = Vector3.MoveTowards(base.transform.right, vector.normalized, 4f * Time.deltaTime);
		base.transform.Translate(base.transform.right * this.moveSpeed * Time.deltaTime, Space.World);
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletTankCannon.Store(this);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, base.transform.position);
		Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.35f);
		SoundManager.Instance.PlaySfx(this.soundExplode, 0f);
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, this.attackData.radiusDealDamage, this.victims, this.layerVictim);
		for (int i = 0; i < num; i++)
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(this.victims[i].transform.root.gameObject);
			if (unit != null && unit.CompareTag("Player"))
			{
				float num2 = Vector3.Distance(base.transform.position, unit.BodyCenterPoint.position);
				float num3 = Mathf.Clamp01((num2 - 0.5f) / (this.attackData.radiusDealDamage - 0.5f));
				float num4 = 1f - num3 * 0.4f;
				this.attackData.damage *= num4;
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
		this.destinationRocket = endPoint.position;
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
