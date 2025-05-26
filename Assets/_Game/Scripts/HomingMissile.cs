using System;
using UnityEngine;

public class HomingMissile : BaseBullet
{
	public float turnSpeed;

	public float delayBeforeChase;

	public float delaySeek = 0.01f;

	public float timeOutChase = 2.5f;

	private float timer;

	private float timerSeek;

	private float countTimeOutChase;

	protected override void Move()
	{
		if (this.timer < this.delayBeforeChase)
		{
			this.timer += Time.deltaTime;
		}
		else if (this.target && this.countTimeOutChase < this.timeOutChase)
		{
			this.countTimeOutChase += Time.deltaTime;
			this.timerSeek += Time.deltaTime;
			if (this.timerSeek > this.delaySeek)
			{
				this.timerSeek = 0f;
				Vector3 vector = this.target.position - base.transform.position;
				base.transform.right = Vector3.MoveTowards(base.transform.right, vector.normalized, this.turnSpeed * Time.deltaTime);
			}
		}
		base.transform.Translate(base.transform.right * this.moveSpeed * Time.deltaTime, Space.World);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, base.transform.position);
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
	}

	public override void Deactive()
	{
		base.Deactive();
		this.timer = 0f;
		this.countTimeOutChase = 0f;
		Singleton<PoolingController>.Instance.poolHomingMissile.Store(this);
	}

	public void Rotate(int index)
	{
		float zAngle = (float)index * 45f;
		base.transform.Rotate(0f, 0f, zAngle);
	}
}
