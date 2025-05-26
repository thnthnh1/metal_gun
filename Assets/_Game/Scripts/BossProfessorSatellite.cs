using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class BossProfessorSatellite : BaseEnemy
{
	[Header("SATELLITE PROPERTIES")]
	public float hp;

	[SpineAnimation("", "", true, false)]
	public string die;

	[SpineAnimation("", "", true, false)]
	public string preShoot;

	private BossProfessor boss;

	private bool isShooting;

	protected override void Awake()
	{
		this.bodyCollider = base.GetComponent<CircleCollider2D>();
		this.bodyCollider.enabled = false;
		this.boss = base.transform.root.GetComponent<BossProfessor>();
		Singleton<GameController>.Instance.AddUnit(base.gameObject, this);
	}

	protected override void Update()
	{
	}

	public void Init()
	{
		this.bodyCollider.enabled = true;
		this.hp = ((SO_BossProfessorStats)this.boss.baseStats).SatelliteHp;
	}

	public void Shoot()
	{
		if (!this.isDead && !this.isShooting)
		{
			this.isShooting = true;
			this.skeletonAnimation.AnimationState.SetAnimation(1, this.preShoot, false);
		}
	}

	public override void Deactive()
	{
		this.PlaySoundDie();
		this.bodyCollider.enabled = false;
		this.skeletonAnimation.ClearState();
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.die, false);
		Singleton<GameController>.Instance.RemoveUnit(base.gameObject);
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, base.transform.position);
	}

	protected override void ReleaseAttack()
	{
		BulletBossProfessor bulletBossProfessor = Singleton<PoolingController>.Instance.poolBulletBossProfessor.New();
		if (bulletBossProfessor == null)
		{
			bulletBossProfessor = (UnityEngine.Object.Instantiate<BaseBullet>(this.boss.bulletSatellitePrefab) as BulletBossProfessor);
		}
		float damage = ((SO_BossProfessorStats)this.boss.baseStats).Damage;
		float bulletSpeed = ((SO_BossProfessorStats)this.boss.baseStats).BulletSpeed;
		AttackData attackData = new AttackData(this.boss, damage, 0f, false, WeaponType.NormalGun, -1, null);
		bulletBossProfessor.Active(attackData, base.transform, bulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
	}

	protected override void Die()
	{
		this.isDead = true;
		this.Deactive();
		EventDispatcher.Instance.PostEvent(EventID.BossProfessorSatelliteDie);
	}

	public override void TakeDamage(AttackData attackData)
	{
		if (this.isDead || attackData.attacker.isDead)
		{
			return;
		}
		this.EffectTakeDamage();
		this.hp -= attackData.damage;
		if (this.hp <= 0f)
		{
			this.hp = 0f;
			this.Die();
		}
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventShoot) == 0)
		{
			this.ReleaseAttack();
		}
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		if (this.isDead)
		{
			return;
		}
		if (string.Compare(entry.animation.name, this.shoot) == 0)
		{
			this.isShooting = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
		}
		if (string.Compare(entry.animation.name, this.preShoot) == 0)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(1, this.shoot, false);
		}
	}
}
