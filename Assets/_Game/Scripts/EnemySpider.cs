using Spine;
using System;
using UnityEngine;

public class EnemySpider : BaseEnemy
{
	[Header("ENEMY SPIDER PROPERTIES")]
	public BaseBullet bulletPrefab;

	private bool flagShoot;

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
		string path = string.Format("Scriptable Object/Enemy/Enemy Spider/enemy_spider_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BaseUnitStats>(path);
	}

	protected override void InitWeapon()
	{
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
			this.GetCloseToTarget();
			this.CheckAllowAttackTarget();
			if (this.isAllowAttackTarget)
			{
				float time = Time.time;
				if (time - this.lastTimeAttack > this.stats.AttackRate)
				{
					this.lastTimeAttack = time;
					this.PlayAnimationShoot(1);
				}
			}
		}
	}

	protected override void ReleaseAttack()
	{
		BulletSpider bulletSpider = Singleton<PoolingController>.Instance.poolBulletSpider.New();
		if (bulletSpider == null)
		{
			bulletSpider = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletSpider);
		}
		AttackData curentAttackData = this.GetCurentAttackData();
		float bulletSpeed = this.baseStats.BulletSpeed;
		bulletSpider.Active(curentAttackData, this.aimPoint, bulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
	}

	protected override void SetCloseRange()
	{
		if (this.nearSensor != null)
		{
			this.closeUpRange = this.nearSensor.col.radius;
		}
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		base.HandleAnimationCompleted(entry);
		if (this.isDead)
		{
			return;
		}
	}

	public override BaseEnemy GetFromPool()
	{
		EnemySpider enemySpider = Singleton<PoolingController>.Instance.poolEnemySpider.New();
		if (enemySpider == null)
		{
			enemySpider = UnityEngine.Object.Instantiate<EnemySpider>(this);
		}
		return enemySpider;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemySpider.Store(this);
	}
}
