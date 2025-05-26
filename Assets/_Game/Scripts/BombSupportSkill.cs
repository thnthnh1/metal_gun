using System;
using UnityEngine;

public class BombSupportSkill : BaseBullet
{
	public LayerMask layerVictim;

	private float damage;

	private Collider2D[] victims = new Collider2D[10];

	protected override void Move()
	{
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.CompareTag("Ground Upstairs"))
		{
			this.Explode();
			this.SpawnHitEffect();
			this.Deactive();
		}
	}

	private void Explode()
	{
		int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, 2.5f, this.victims, this.layerVictim);
		for (int i = 0; i < num; i++)
		{
			BaseUnit baseUnit = null;
			if (this.victims[i].CompareTag("Enemy Body Part") || this.victims[i].CompareTag("Destructible Obstacle"))
			{
				baseUnit = Singleton<GameController>.Instance.GetUnit(this.victims[i].gameObject);
			}
			else if (this.victims[i].transform.root.CompareTag("Enemy"))
			{
				baseUnit = Singleton<GameController>.Instance.GetUnit(this.victims[i].transform.root.gameObject);
			}
			if (baseUnit)
			{
				baseUnit.TakeDamage(this.damage);
			}
		}
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, base.transform.position);
		Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.35f);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}

	public void Active(Vector2 position, float damage, Transform parent = null)
	{
		this.damage = damage;
		base.transform.position = position;
		base.transform.parent = parent;
		base.gameObject.SetActive(true);
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBombSupportSkill.Store(this);
	}
}
