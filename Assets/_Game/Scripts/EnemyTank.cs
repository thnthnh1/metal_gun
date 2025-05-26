using System;
using UnityEngine;

public class EnemyTank : BaseEnemy
{
	[Header("ENEMY TANK PROPERTIES")]
	public BaseBullet bulletPrefab;

	public BaseMuzzle muzzlePrefab;

	public BaseMuzzle dustMuzzlePrefab;

	public Transform firePoint;

	public Transform muzzlePoint;

	protected BaseMuzzle muzzle;

	protected BaseMuzzle dustMuzzle;

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Enemy/Enemy Tank/enemy_tank_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BaseUnitStats>(path);
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
			this.CheckAllowAttackTarget();
			if (this.isAllowAttackTarget)
			{
				float time = Time.time;
				if (time - this.lastTimeAttack > this.stats.AttackRate)
				{
					this.lastTimeAttack = time;
					this.PlayAnimationShoot(1);
					this.PlaySound(this.soundAttack);
					this.ReleaseAttack();
				}
			}
		}
	}

	public override void TakeDamage(AttackData attackData)
	{
		base.TakeDamage(attackData);
		if (this.isDead && attackData.weapon == WeaponType.Grenade)
		{
			EventDispatcher.Instance.PostEvent(EventID.KillTankByGrenade);
		}
	}

	protected override void Die()
	{
		base.Die();
		EventDispatcher.Instance.PostEvent(EventID.KillEnemyTank);
	}

	protected override void ReleaseAttack()
	{
		BulletTank bulletTank = Singleton<PoolingController>.Instance.poolBulletTank.New();
		if (bulletTank == null)
		{
			bulletTank = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletTank);
		}
		AttackData attackData = new AttackData(this, this.baseStats.Damage, 0f, false, WeaponType.NormalGun, -1, null);
		bulletTank.Active(attackData, this.firePoint, 5f, Singleton<PoolingController>.Instance.groupBullet);
		this.ActiveMuzzle();
	}

	protected override void StartDie()
	{
		base.StartDie();
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, base.transform.position);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}

	protected override void SetColliderLayers(bool isActive)
	{
		if (this.bodyCollider != null)
		{
			this.bodyCollider.gameObject.layer = ((!isActive) ? StaticValue.LAYER_DEFAULT : StaticValue.LAYER_VEHICLE_ENEMY);
		}
		if (this.footCollider != null)
		{
			this.footCollider.gameObject.layer = ((!isActive) ? StaticValue.LAYER_DEFAULT : StaticValue.LAYER_VEHICLE_ENEMY);
		}
	}

	protected override void InitPatrolPoint()
	{
		Vector3 position = base.transform.position;
		position.x = ((!this.IsFacingRight) ? (position.x - 2f) : (position.x + 2f));
		base.SetDestinationMove(position);
	}

	protected override void SetDestinationPatrol(bool isMoveForward)
	{
		float num = UnityEngine.Random.Range(2f, 4.5f);
		Vector3 position = base.transform.position;
		if (isMoveForward)
		{
			if (this.IsFacingRight)
			{
				position.x += num;
			}
			else
			{
				position.x -= num;
			}
		}
		else if (this.IsFacingRight)
		{
			position.x -= num;
		}
		else
		{
			position.x += num;
		}
		base.SetDestinationMove(position);
	}

	public override void Renew()
	{
		base.Renew();
		this.isEffectMeleeWeapon = false;
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyTank enemyTank = Singleton<PoolingController>.Instance.poolEnemyTank.New();
		if (enemyTank == null)
		{
			enemyTank = UnityEngine.Object.Instantiate<EnemyTank>(this);
		}
		return enemyTank;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyTank.Store(this);
	}

	protected void ActiveMuzzle()
	{
		if (this.muzzle == null)
		{
			this.muzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.muzzlePrefab, this.muzzlePoint.position, this.muzzlePoint.rotation, this.muzzlePoint.parent);
		}
		this.muzzle.Active();
		if (this.dustMuzzle == null)
		{
			this.dustMuzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.dustMuzzlePrefab, this.muzzlePoint.position, this.muzzlePoint.rotation, this.muzzlePoint.parent);
		}
		this.dustMuzzle.Active();
	}
}
