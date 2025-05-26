using System;
using UnityEngine;

public class Spike : BaseBullet
{
	private bool isReady;

	protected override void Move()
	{
		if (this.isReady)
		{
			base.transform.Translate(-base.transform.up * this.moveSpeed * Time.deltaTime);
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null)
			{
				unit.TakeDamage(this.attackData);
				EventDispatcher.Instance.PostEvent(EventID.BossMonkeySpikeHitPlayer);
			}
		}
		this.SpawnHitEffect();
		this.Deactive();
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.StoneRainExplosion, base.transform.position);
		Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.35f);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}

	public void Active(AttackData attackData, Vector2 position, float moveSpeed, Transform parent = null)
	{
		this.attackData = attackData;
		this.moveSpeed = moveSpeed;
		this.isReady = false;
		this.SetTagAndLayer();
		base.transform.position = position;
		base.gameObject.SetActive(true);
		base.Invoke("ReadyDrop", 1f);
	}

	public override void Deactive()
	{
		base.Deactive();
		base.CancelInvoke();
		EventDispatcher.Instance.PostEvent(EventID.BossMonkeySpikeDeactive);
		Singleton<PoolingController>.Instance.poolSpike.Store(this);
	}

	private void ReadyDrop()
	{
		this.isReady = true;
	}
}
