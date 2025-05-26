using System;
using UnityEngine;

public class Bomb : BaseBullet
{
	protected override void Move()
	{
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, base.transform.position);
		Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.35f);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}

	public override void Active(AttackData attackData, Transform releasePoint, float moveSpeed, Transform parent = null)
	{
		this.attackData = attackData;
		this.moveSpeed = moveSpeed;
		this.SetTagAndLayer();
		base.transform.position = releasePoint.position;
		base.transform.rotation = releasePoint.rotation;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
		this.rigid.AddForce(UnityEngine.Random.onUnitSphere * 1000f);
		this.rigid.AddTorque(250f);
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBulletBomb.Store(this);
	}
}
