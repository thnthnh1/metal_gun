using System;
using UnityEngine;

public class EnemyTankCannon : BaseEnemy
{
	[Header("ENEMY TANK PROPERTIES")]
	public BaseBullet bulletPrefab;

	public BaseMuzzle muzzlePrefab;

	public BaseMuzzle dustMuzzlePrefab;

	public Transform firePoint;

	public Transform muzzlePoint;

	public Vector2 fireDirection;

	protected BaseMuzzle muzzle;

	protected BaseMuzzle dustMuzzle;

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Enemy/Enemy Tank Cannon/enemy_tank_cannon_lv{0}", this.level);
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

	public override void TakeDamage(AttackData attackData)
	{
		base.TakeDamage(attackData);
		if (this.isDead && attackData.weapon == WeaponType.Grenade)
		{
			EventDispatcher.Instance.PostEvent(EventID.KillTankByGrenade);
		}
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyTankCannon enemyTankCannon = Singleton<PoolingController>.Instance.poolEnemyTankCannon.New();
		if (enemyTankCannon == null)
		{
			enemyTankCannon = UnityEngine.Object.Instantiate<EnemyTankCannon>(this);
		}
		return enemyTankCannon;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyTankCannon.Store(this);
	}

	protected override void Die()
	{
		base.Die();
		EventDispatcher.Instance.PostEvent(EventID.KillEnemyTank);
	}

	protected override void ReleaseAttack()
	{
		BulletTankCannon bulletTankCannon = Singleton<PoolingController>.Instance.poolBulletTankCannon.New();
		if (bulletTankCannon == null)
		{
			bulletTankCannon = (UnityEngine.Object.Instantiate<BaseBullet>(this.bulletPrefab) as BulletTankCannon);
		}
		AttackData attackData = new AttackData(this, this.baseStats.Damage, 1f, false, WeaponType.NormalGun, -1, null);
		Vector2 throwDirection = this.fireDirection;
		throwDirection.x = ((!this.IsFacingRight) ? (-throwDirection.x) : throwDirection.x);
		bulletTankCannon.Active(attackData, this.firePoint, this.target.transform, throwDirection);
		this.ActiveMuzzle();
	}

	protected void ActiveMuzzle()
	{
		if (this.muzzle == null)
		{
			this.muzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.muzzlePrefab, this.muzzlePoint.position, this.muzzlePoint.rotation, this.muzzlePoint.transform);
		}
		this.muzzle.Active();
		if (this.dustMuzzle == null)
		{
			this.dustMuzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.dustMuzzlePrefab, this.muzzlePoint.position, this.muzzlePoint.rotation, this.muzzlePoint.transform);
		}
		this.dustMuzzle.Active();
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
}
