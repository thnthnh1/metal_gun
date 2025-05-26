using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class EnemyMonkey : BaseEnemy
{
	[Header("ENEMY MONKEY PROPERTIES")]
	public Collider2D colliderArm;

	public Transform healthBarLeft;

	public Transform healthBarRight;

	[SpineAnimation("", "", true, false)]
	public string punch;

	[SpineAnimation("", "", true, false)]
	public string smash;

	[SpineAnimation("", "", true, false)]
	public string idleNormal;

	[SpineAnimation("", "", true, false)]
	public string idleRoar;

	[SpineEvent("", "", true, false)]
	public string eventPunch;

	[SpineEvent("", "", true, false)]
	public string eventSmash;

	[SerializeField]
	private bool flagAttack;

	public bool IsAttacking
	{
		get
		{
			return this.flagAttack;
		}
	}

	protected override void Update()
	{
		if (!this.isDead)
		{
			this.UpdateDirection();
			this.Idle();
			this.Patrol();
			this.Attack();
		}
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Enemy/Enemy Monkey/enemy_monkey_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BaseUnitStats>(path);
	}

	protected override void InitSortingLayerSpine()
	{
	}

	protected override void Attack()
	{
		if (this.state == EnemyState.Attack)
		{
			if (this.target == null || this.target.isDead)
			{
				this.CancelCombat();
				return;
			}
			if (this.flagAttack)
			{
				return;
			}
			this.GetCloseToTarget();
			if (!this.flagGetCloseToTarget)
			{
				float time = Time.time;
				if (time - this.lastTimeAttack > this.stats.AttackRate)
				{
					this.lastTimeAttack = time;
					this.flagAttack = true;
					this.colliderArm.enabled = true;
					this.PlayAnimationMeleeAttack();
				}
			}
		}
	}

	protected override void SetCloseRange()
	{
		if (this.nearSensor != null && this.nearSensor != null)
		{
			this.closeUpRange = UnityEngine.Random.Range(0.7f, 1.2f);
			this.nearSensor.col.radius = this.closeUpRange;
		}
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if ((string.Compare(e.Data.Name, this.eventPunch) == 0 || string.Compare(e.Data.Name, this.eventSmash) == 0) && !this.isDead)
		{
			this.PlaySound(this.soundAttack);
		}
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		base.HandleAnimationCompleted(entry);
		if (this.isDead)
		{
			return;
		}
		if (string.Compare(entry.animation.name, this.punch) == 0 || string.Compare(entry.animation.name, this.smash) == 0)
		{
			this.flagAttack = false;
			this.colliderArm.enabled = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
		}
	}

	protected override void PlayAnimationMeleeAttack()
	{
		string animationName = (UnityEngine.Random.Range(0, 2) != 0) ? this.smash : this.punch;
		this.skeletonAnimation.AnimationState.SetAnimation(1, animationName, false);
	}

	public override void PlayAnimationIdle()
	{
		TrackEntry current = this.skeletonAnimation.AnimationState.GetCurrent(0);
		if (current == null || (string.Compare(current.animation.name, this.idleNormal) != 0 && string.Compare(current.animation.name, this.idleRoar) != 0))
		{
			string animationName = this.idleNormal;
			if (this.target == null)
			{
				animationName = ((UnityEngine.Random.Range(0, 2) != 0) ? this.idleRoar : this.idleNormal);
			}
			this.skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
		}
	}

	protected override void UpdateTransformPoints()
	{
		base.UpdateTransformPoints();
		this.healthBar.transform.parent.position = ((!this.skeletonAnimation.Skeleton.flipX) ? this.healthBarRight.position : this.healthBarLeft.position);
	}

	public override void Renew()
	{
		base.Renew();
		this.colliderArm.enabled = false;
		this.flagAttack = false;
		this.canJump = true;
	}

	public override void Active(EnemySpawnData spawnData)
	{
		base.Active(spawnData);
		this.canMove = true;
		this.canJump = true;
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyMonkey enemyMonkey = Singleton<PoolingController>.Instance.poolEnemyMonkey.New();
		if (enemyMonkey == null)
		{
			enemyMonkey = UnityEngine.Object.Instantiate<EnemyMonkey>(this);
		}
		return enemyMonkey;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyMonkey.Store(this);
	}
}
