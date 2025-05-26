using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class BossMonkeyMinion : BaseEnemy
{
	[Header("MONKEY MINION PROPERTIES")]
	public BaseBullet stonePrefab;

	public Transform stoneStartPoint;

	public Vector2 stoneDirection;

	public Transform healthBarLeft;

	public Transform healthBarRight;

	[SpineAnimation("", "", true, false)]
	public string throwStone;

	[SpineEvent("", "", true, false)]
	public string eventThrowStone;

	public AudioClip soundAppear;

	private bool flagThrow;

	private bool flagEntrance;

	private Vector2 mostLeftPoint;

	private Vector2 mostRightPoint;

	private Vector2 standPosition;

	protected override void Start()
	{
		base.Start();
		EventDispatcher.Instance.RegisterListener(EventID.BossMonkeyDie, delegate(Component sender, object param)
		{
			this.Deactive();
		});
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Boss/Boss Monkey Minion/boss_monkey_minion_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BaseUnitStats>(path);
	}

	protected override void Update()
	{
		if (!this.isDead)
		{
			this.Entrance();
			if (this.isReadyAttack)
			{
				this.UpdateDirection();
				this.Attack();
			}
		}
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
			if (this.flagThrow)
			{
				return;
			}
			float time = Time.time;
			if (time - this.lastTimeAttack > this.stats.AttackRate)
			{
				this.lastTimeAttack = time;
				this.flagThrow = true;
				this.PlayAnimationThrow();
			}
		}
	}

	protected override void UpdateDirection()
	{
		if (this.target)
		{
			this.skeletonAnimation.Skeleton.flipX = (this.target.transform.position.x < base.transform.position.x);
		}
		this.UpdateTransformPoints();
	}

	protected override void UpdateTransformPoints()
	{
		base.UpdateTransformPoints();
		this.healthBar.transform.parent.position = ((!this.skeletonAnimation.Skeleton.flipX) ? this.healthBarRight.position : this.healthBarLeft.position);
	}

	public void SetPoints(Vector2 mostLeftPoint, Vector2 mostRightPoint)
	{
		this.mostLeftPoint = mostLeftPoint;
		this.mostRightPoint = mostRightPoint;
		Vector2 vector = mostLeftPoint;
		vector.x = UnityEngine.Random.Range(mostLeftPoint.x, mostRightPoint.x);
		this.standPosition = vector;
	}

	public override void Renew()
	{
		base.Renew();
		this.ActiveSensor(false);
		this.isReadyAttack = false;
		this.isImmortal = true;
		this.flagEntrance = true;
		this.flagThrow = false;
		this.PlaySound(this.soundAppear);
	}

	public override BaseEnemy GetFromPool()
	{
		BossMonkeyMinion bossMonkeyMinion = Singleton<PoolingController>.Instance.poolBossMonkeyMinion.New();
		if (bossMonkeyMinion == null)
		{
			bossMonkeyMinion = UnityEngine.Object.Instantiate<BossMonkeyMinion>(this);
		}
		return bossMonkeyMinion;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolBossMonkeyMinion.Store(this);
	}

	private void Entrance()
	{
		if (this.flagEntrance)
		{
			this.flagEntrance = false;
			this.PlayAnimationMove();
			this.skeletonAnimation.Skeleton.flipX = (this.standPosition.x < base.transform.position.x);
			float num = Mathf.Abs(this.standPosition.x - base.transform.position.x);
			float moveSpeed = this.baseStats.MoveSpeed;
			float duration = num / moveSpeed;
			base.transform.DOMove(this.standPosition, duration, false).SetEase(Ease.Linear).OnComplete(delegate
			{
				this.isImmortal = false;
				this.isReadyAttack = true;
			});
		}
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		base.HandleAnimationCompleted(entry);
		if (this.isDead)
		{
			return;
		}
		if (string.Compare(entry.animation.name, this.throwStone) == 0)
		{
			this.flagThrow = false;
			this.PlayAnimationIdle();
		}
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventThrowStone) == 0)
		{
			StoneBossMonkeyMinion stoneBossMonkeyMinion = Singleton<PoolingController>.Instance.poolStoneBossMonkeyMinion.New();
			if (stoneBossMonkeyMinion == null)
			{
				stoneBossMonkeyMinion = (UnityEngine.Object.Instantiate<BaseBullet>(this.stonePrefab) as StoneBossMonkeyMinion);
			}
			AttackData attackData = new AttackData(this, this.baseStats.Damage, 0f, false, WeaponType.NormalGun, -1, null);
			stoneBossMonkeyMinion.Active(attackData, this.stoneStartPoint, this.target.BodyCenterPoint, this.stoneDirection);
		}
	}

	protected override void PlayAnimationThrow()
	{
		this.PlaySound(this.soundAttack);
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.throwStone, false);
	}
}
