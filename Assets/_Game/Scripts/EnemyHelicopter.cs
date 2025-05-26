using Spine;
using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHelicopter : BaseEnemy
{
	[HideInInspector]
	public int indexMove;

	public HomingMissile bombPrefab;

	public BaseMuzzle dustMuzzlePrefab;

	public Transform missileReleasePoint;

	private int indexRotate = -1;

	private bool isMovingToDestination;

	private float lastTimeIdle;

	private float timeIdle;

	private BaseMuzzle dustMuzzle;

	private AudioSource audioMove;

	private AudioClip soundMove;

	protected override void Awake()
	{
		base.Awake();
		this.audioMove = base.GetComponent<AudioSource>();
		this.soundMove = SoundManager.Instance.GetAudioClip("sfx_plane_move");
	}

	protected override void Update()
	{
		if (!this.isDead)
		{
			this.UpdateDirection();
			this.Attack();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.layer == StaticValue.LAYER_GROUND && this.isDead)
		{
			EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, base.transform.position);
			SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
			this.Deactive();
		}
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Enemy/Enemy Helicopter/enemy_helicopter_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_EnemyHelicopterStats>(path);
	}

	protected override void Attack()
	{
		if (this.state == EnemyState.Attack && (this.target == null || this.target.isDead))
		{
			return;
		}
		if (this.isMovingToDestination)
		{
			if (Mathf.Abs(this.destinationMove.x - base.transform.position.x) > 0.05f)
			{
				base.transform.position = Vector2.MoveTowards(base.transform.position, this.destinationMove, this.stats.MoveSpeed * Time.deltaTime);
			}
			else
			{
				base.transform.position = this.destinationMove;
				this.isMovingToDestination = false;
				this.EnableAudioMove(false);
				this.skeletonAnimation.transform.rotation = Quaternion.identity;
				base.StartCoroutine(base.DelayAction(new UnityAction(this.ReadyToAttack), StaticValue.waitOneSec));
			}
		}
		else if (this.isReadyAttack)
		{
			float time = Time.time;
			if (time - this.lastTimeIdle > this.timeIdle)
			{
				this.GetNextDestination();
				this.skeletonAnimation.transform.rotation = Quaternion.Euler(0f, 0f, (!this.IsFacingRight) ? 15f : -15f);
				this.isReadyAttack = false;
				this.isMovingToDestination = true;
				this.EnableAudioMove(true);
				this.PlayAnimationIdle();
				return;
			}
			if (time - this.lastTimeAttack > this.stats.AttackRate)
			{
				this.lastTimeAttack = time;
				this.PlayAnimationShoot(1);
				this.PlaySound(this.soundAttack);
				this.ReleaseMissile();
			}
		}
	}

	protected override void Die()
	{
		base.Die();
		EventDispatcher.Instance.PostEvent(EventID.KillEnemyFlying);
	}

	protected override void StartDie()
	{
		base.StartDie();
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, base.transform.position);
		SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		if (this.dieAnimationNames.Contains(entry.animation.name))
		{
			this.rigid.bodyType = RigidbodyType2D.Dynamic;
			this.rigid.AddForce(UnityEngine.Random.onUnitSphere * 10000f);
			this.rigid.AddTorque(30000f);
		}
	}

	protected override void UpdateDirection()
	{
		if (this.isMovingToDestination)
		{
			this.skeletonAnimation.Skeleton.flipX = (this.destinationMove.x < base.transform.position.x);
		}
		else
		{
			this.skeletonAnimation.Skeleton.flipX = (this.target.transform.position.x < base.transform.position.x);
		}
		this.UpdateTransformPoints();
	}

	public override void Renew()
	{
		base.Renew();
		this.isEffectMeleeWeapon = false;
		this.indexMove = 0;
		this.indexRotate = -1;
		this.timeIdle = UnityEngine.Random.Range(5f, 7f);
		this.isMovingToDestination = true;
		this.EnableAudioMove(true);
		base.transform.rotation = Quaternion.identity;
		this.rigid.bodyType = RigidbodyType2D.Kinematic;
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyHelicopter enemyHelicopter = Singleton<PoolingController>.Instance.poolEnemyHelicopter.New();
		if (enemyHelicopter == null)
		{
			enemyHelicopter = UnityEngine.Object.Instantiate<EnemyHelicopter>(this);
		}
		return enemyHelicopter;
	}

	public override void Deactive()
	{
		base.Deactive();
		this.EnableAudioMove(false);
		Singleton<PoolingController>.Instance.poolEnemyHelicopter.Store(this);
	}

	protected override void ReadyToAttack()
	{
		base.ReadyToAttack();
		this.lastTimeIdle = Time.time;
	}

	public void GetNextDestination()
	{
		Vector2 nextDestination = Singleton<CameraFollow>.Instance.GetNextDestination(this);
		base.SetDestinationMove(nextDestination);
	}

	private void ReleaseMissile()
	{
		SO_EnemyHelicopterStats sO_EnemyHelicopterStats = (SO_EnemyHelicopterStats)this.baseStats;
		for (int i = 0; i < sO_EnemyHelicopterStats.NumberOfProjectilePerShot; i++)
		{
			HomingMissile homingMissile = Singleton<PoolingController>.Instance.poolHomingMissile.New();
			if (homingMissile == null)
			{
				homingMissile = UnityEngine.Object.Instantiate<HomingMissile>(this.bombPrefab);
			}
			float projectileDamage = ((SO_EnemyHelicopterStats)this.baseStats).ProjectileDamage;
			float projectileDamageRadius = ((SO_EnemyHelicopterStats)this.baseStats).ProjectileDamageRadius;
			AttackData attackData = new AttackData(this, projectileDamage, projectileDamageRadius, false, WeaponType.NormalGun, -1, null);
			homingMissile.Active(attackData, this.missileReleasePoint, sO_EnemyHelicopterStats.ProjectileSpeed, Singleton<PoolingController>.Instance.groupBullet);
			homingMissile.Rotate(this.indexRotate);
			homingMissile.SetTarget(this.target.transform);
			this.indexRotate++;
			if (this.indexRotate >= 2)
			{
				this.indexRotate = -1;
			}
		}
		this.ActiveMuzzle();
	}

	private void ActiveMuzzle()
	{
		if (this.dustMuzzle == null)
		{
			this.dustMuzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.dustMuzzlePrefab, this.missileReleasePoint.position, this.missileReleasePoint.rotation, this.missileReleasePoint);
		}
		this.dustMuzzle.Active();
	}

	private void EnableAudioMove(bool isEnable)
	{
		if (isEnable)
		{
			if (this.audioMove.clip == null)
			{
				this.audioMove.clip = this.soundMove;
				this.audioMove.loop = true;
			}
			this.audioMove.Play();
		}
		else
		{
			AudioClip audioClip = this.soundMove;
			this.audioMove.clip = audioClip;
			if (audioClip)
			{
				this.audioMove.clip = null;
			}
			this.audioMove.Stop();
		}
	}
}
