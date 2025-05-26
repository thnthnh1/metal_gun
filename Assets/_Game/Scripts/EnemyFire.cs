using System;
using UnityEngine;

public class EnemyFire : BaseEnemy
{
	[Header("ENEMY FIRE PROPERTIES")]
	public Fire fire;

	protected override void Start()
	{
		base.Start();
		EventDispatcher.Instance.RegisterListener(EventID.PlayerDie, delegate(Component sender, object param)
		{
			this.fire.Deactive();
		});
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
		string path = string.Format("Scriptable Object/Enemy/Enemy Fire/enemy_fire_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_EnemyFireStats>(path);
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
			this.GetCloseToTarget();
		}
	}

	protected override void Die()
	{
		base.Die();
		EventDispatcher.Instance.PostEvent(EventID.KillEnemyFire);
	}

	protected override void SetCloseRange()
	{
		if (this.nearSensor != null)
		{
			this.closeUpRange = UnityEngine.Random.Range(2f, 3f);
			this.nearSensor.col.radius = this.closeUpRange;
		}
	}

	protected override void PlayAnimationShoot(int trackIndex = 1)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(trackIndex, this.shoot, true);
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyFire enemyFire = Singleton<PoolingController>.Instance.poolEnemyFire.New();
		if (enemyFire == null)
		{
			enemyFire = UnityEngine.Object.Instantiate<EnemyFire>(this);
		}
		return enemyFire;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyFire.Store(this);
	}

	public override void OnUnitGetInFarSensor(BaseUnit unit)
	{
		this.SetTarget(unit);
		this.fire.Active();
		if (Vector2.Distance(this.target.transform.position, base.BodyCenterPoint.position) > this.nearSensor.col.radius)
		{
			if (this.canMove)
			{
				this.flagGetCloseToTarget = true;
				this.PlayAnimationMoveFast();
				this.PlayAnimationShoot(1);
			}
			else
			{
				this.PlayAnimationShoot(0);
			}
		}
		else
		{
			this.PlayAnimationShoot(0);
		}
	}

	public override void OnUnitGetOutFarSensor(BaseUnit unit)
	{
		this.fire.Deactive();
		if (this.canMove)
		{
			this.farSensor.gameObject.SetActive(false);
			base.StartCoroutine(base.DelayAction(delegate
			{
				this.farSensor.gameObject.SetActive(true);
				this.flagGetCloseToTarget = false;
				this.StartChasingTarget();
				this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
			}, StaticValue.waitHalfSec));
		}
		else
		{
			this.CancelCombat();
		}
	}

	public override void OnUnitGetInNearSensor(BaseUnit unit)
	{
		if (this.canMove)
		{
			this.flagGetCloseToTarget = false;
			this.PlayAnimationIdle();
			this.StopMoving();
			this.PlayAnimationShoot(0);
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
		}
		if (!this.nearbyVictims.Contains(unit))
		{
			this.nearbyVictims.Add(unit);
		}
	}

	public override void OnUnitGetOutNearSensor(BaseUnit unit)
	{
		if (this.canMove)
		{
			this.nearSensor.gameObject.SetActive(false);
			base.StartCoroutine(base.DelayAction(delegate
			{
				this.nearSensor.gameObject.SetActive(true);
				this.PlayAnimationMoveFast();
				this.PlayAnimationShoot(1);
				this.flagGetCloseToTarget = true;
			}, StaticValue.waitHalfSec));
		}
		if (this.nearbyVictims.Contains(unit))
		{
			this.nearbyVictims.Remove(unit);
		}
	}

	protected override void UpdateTransformPoints()
	{
		base.UpdateTransformPoints();
		Vector3 localScale = this.fire.fireEffect.localScale;
		localScale.x = (float)((!this.IsFacingRight) ? (-1) : 1);
		this.fire.fireEffect.localScale = localScale;
	}
}
