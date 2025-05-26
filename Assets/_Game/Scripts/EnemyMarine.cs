using DG.Tweening;
using DG.Tweening.Core;
using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class EnemyMarine : BaseEnemy
{
	[SpineAnimation("", "", true, false), Header("ENEMY MARINE PROPERTIES")]
	public string jumpForward;

	private float underWaterY;

	private bool isAppearDone;

	private bool flagAttack;

	protected override void Start()
	{
		base.Start();
		EventDispatcher.Instance.RegisterListener(EventID.BossSubmarineDie, delegate(Component sender, object param)
		{
			this.Deactive();
		});
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Enemy/Enemy Marine/enemy_marine_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BaseUnitStats>(path);
	}

	protected override void Update()
	{
		if (!this.isDead && this.isReadyAttack)
		{
			this.Attack();
		}
	}

	protected override void Attack()
	{
		if (this.state == EnemyState.Attack && this.isAppearDone)
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
			this.UpdateDirection();
			if (this.IsTargetInCloseRange())
			{
				float time = Time.time;
				if (time - this.lastTimeAttack > this.stats.AttackRate)
				{
					this.lastTimeAttack = time;
					this.PlayAnimationIdle();
					this.StopMoving();
					this.flagAttack = true;
					this.skeletonAnimation.AnimationState.SetAnimation(1, this.meleeAttack, false);
				}
			}
			else
			{
				this.PlayAnimationMove();
				this.Move();
			}
		}
	}

	protected override void CheckAllowMoveForwardToTarget()
	{
	}

	protected override bool IsTargetInCloseRange()
	{
		if (this.target != null)
		{
			float sqrMagnitude = (this.target.transform.position - base.BodyCenterPoint.position).sqrMagnitude;
			bool flag = Mathf.Abs(this.target.transform.position.y - base.BodyCenterPoint.position.y) < 1.2f;
			return sqrMagnitude < 1.3f && flag;
		}
		return false;
	}

	public void Active(int level, Vector2 position, float underWaterY)
	{
		base.Active(this.id, level, position);
		this.underWaterY = underWaterY;
	}

	public override void Renew()
	{
		base.Renew();
		this.ActiveSensor(false);
		this.flagAttack = false;
		this.isAppearDone = false;
		this.isImmortal = true;
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyMarine enemyMarine = Singleton<PoolingController>.Instance.poolEnemyMarine.New();
		if (enemyMarine == null)
		{
			enemyMarine = UnityEngine.Object.Instantiate<EnemyMarine>(this);
		}
		return enemyMarine;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyMarine.Store(this);
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		base.HandleAnimationCompleted(entry);
		if (this.isDead)
		{
			return;
		}
		if (string.Compare(entry.animation.name, this.meleeAttack) == 0)
		{
			this.flagAttack = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
		}
		if (string.Compare(entry.animation.name, this.jumpForward) == 0)
		{
			this.PlayAnimationIdle();
			this.rigid.bodyType = RigidbodyType2D.Kinematic;
			this.bodyCollider.gameObject.SetActive(false);
			this.footCollider.gameObject.SetActive(false);
			base.transform.DOMoveY(this.underWaterY, 1f, false).OnComplete(delegate
			{
				this.rigid.bodyType = RigidbodyType2D.Dynamic;
				this.bodyCollider.gameObject.SetActive(true);
				this.footCollider.gameObject.SetActive(true);
				this.isAppearDone = true;
				this.isReadyAttack = true;
				this.isImmortal = false;
			});
		}
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventMeleeAttack) == 0 && this.IsTargetInCloseRange() && this.target.transform.root.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(this.target.transform.root.gameObject);
			if (unit != null)
			{
				unit.TakeDamage(this.GetCurentAttackData());
			}
		}
	}

	protected override void FadeIn()
	{
		DOTween.To(new DOSetter<float>(base.AlphaSetter), 0f, 1f, 0.5f).OnComplete(new TweenCallback(this.FadeInDone));
	}

	protected override void FadeInDone()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.jumpForward, false);
	}
}
