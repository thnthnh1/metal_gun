using System;
using UnityEngine;

public class RocketBossSubmarine : BaseBullet
{
	public float turnSpeed = 4f;

	public AudioClip soundExplode;

	public AudioClip soundMoving;

	private Vector3 destination;

	protected override void Move()
	{
		Vector3 vector = this.destination - base.transform.position;
		base.transform.right = Vector3.MoveTowards(base.transform.right, vector.normalized, this.turnSpeed * Time.deltaTime);
		base.transform.Translate(base.transform.right * this.moveSpeed * Time.deltaTime, Space.World);
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolRocketBossSubmarine.Store(this);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, base.transform.position);
		Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.35f);
		SoundManager.Instance.PlaySfx(this.soundExplode, 0f);
	}

	public void Active(AttackData attackData, Transform releasePoint, Vector3 destination, float moveSpeed, Transform parent = null)
	{
		this.attackData = attackData;
		this.moveSpeed = moveSpeed;
		this.SetTagAndLayer();
		base.transform.position = releasePoint.position;
		base.transform.rotation = releasePoint.rotation;
		base.transform.parent = parent;
		this.destination = destination;
		base.gameObject.SetActive(true);
		SoundManager.Instance.PlaySfx(this.soundMoving, -15f);
	}
}
