using DG.Tweening;
using System;
using UnityEngine;

public class DestructibleUnit : BaseUnit
{
	public bool isDealDamageAround;

	public LayerMask layerVictimExplode;

	public EffectObjectName destructEffect;

	public float yOffsetSpawnEffect;

	private SpriteRenderer render;

	private bool isBlinkingEffect;

	private Collider2D[] victims = new Collider2D[5];

	private void Start()
	{
		this.render = base.GetComponent<SpriteRenderer>();
		base.transform.parent = null;
		this.stats.Init(this.baseStats);
		Singleton<GameController>.Instance.AddUnit(base.gameObject, this);
	}

	protected override void EffectTakeDamage()
	{
		if (!this.isBlinkingEffect)
		{
			this.isBlinkingEffect = true;
			this.render.DOColor(Color.red, 0.1f).OnComplete(new TweenCallback(this.ChangeColorToDefault));
		}
	}

	protected override void Die()
	{
		base.Die();
		Vector3 position = base.transform.position;
		position.y += this.yOffsetSpawnEffect;
		EffectController.Instance.SpawnParticleEffect(this.destructEffect, position);
		if (this.isDealDamageAround)
		{
			int num = Physics2D.OverlapCircleNonAlloc(base.transform.position, 1f, this.victims, this.layerVictimExplode);
			for (int i = 0; i < num; i++)
			{
				BaseUnit unit = Singleton<GameController>.Instance.GetUnit(this.victims[i].transform.root.gameObject);
				if (unit != null && (unit.CompareTag("Enemy") || unit.CompareTag("Destructible Obstacle")))
				{
					AttackData curentAttackData = this.GetCurentAttackData();
					unit.TakeDamage(curentAttackData);
				}
			}
			Singleton<CameraFollow>.Instance.AddShake(0.15f, 0.3f);
		}
		base.gameObject.SetActive(false);
		Singleton<GameController>.Instance.RemoveUnit(base.gameObject);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}

	public override void TakeDamage(AttackData attackData)
	{
		if (this.isDead || attackData.attacker.isDead)
		{
			return;
		}
		this.stats.AdjustStats(-attackData.damage, StatsType.Hp);
		this.EffectTakeDamage();
		if (this.stats.HP <= 0f)
		{
			this.Die();
		}
	}

	private void ChangeColorToDefault()
	{
		this.render.color = Color.white;
		this.isBlinkingEffect = false;
	}
}
