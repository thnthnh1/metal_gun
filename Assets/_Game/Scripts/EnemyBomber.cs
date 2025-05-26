using Spine;
using System;
using UnityEngine;

public class EnemyBomber : BaseEnemy
{
	[Header("ENEMY BOMBER PROPERTIES")]
	public Bomb bombPrefab;

	public Transform bombReleasePoint;

	public bool isFromLeft;

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
		this.TrackOutOfScreen();
		if (!this.isDead)
		{
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
		string path = string.Format("Scriptable Object/Enemy/Enemy Bomber/enemy_bomber_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_EnemyHasProjectileStats>(path);
	}

	protected override void Attack()
	{
		Vector2 a = (!this.isFromLeft) ? Vector2.left : Vector2.right;
		base.transform.Translate(a * this.stats.MoveSpeed * Time.deltaTime);
		float time = Time.time;
		if (time - this.lastTimeAttack > this.stats.AttackRate)
		{
			this.lastTimeAttack = time;
			this.PlayAnimationThrow();
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
		if (string.Compare(entry.animation.name, this.throwGrenade) == 0)
		{
			this.PlaySound(this.soundAttack);
			this.ReleaseBomb();
		}
		if (this.dieAnimationNames.Contains(entry.animation.name))
		{
			this.rigid.bodyType = RigidbodyType2D.Dynamic;
			this.rigid.AddForce(UnityEngine.Random.onUnitSphere * 10000f);
			this.rigid.AddTorque(30000f);
		}
	}

	protected override void PlayAnimationThrow()
	{
		TrackEntry trackEntry = this.skeletonAnimation.AnimationState.SetAnimation(1, this.throwGrenade, false);
		trackEntry.TimeScale = 3f;
		trackEntry.AttachmentThreshold = 1f;
		trackEntry.MixDuration = 0f;
		TrackEntry trackEntry2 = this.skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
		trackEntry2.AttachmentThreshold = 1f;
	}

	public override void Renew()
	{
		base.Renew();
		this.rigid.bodyType = RigidbodyType2D.Kinematic;
		this.skeletonAnimation.Skeleton.FlipX = !this.isFromLeft;
		base.transform.rotation = Quaternion.identity;
		this.UpdateTransformPoints();
		this.EnableAudioMove(true);
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyBomber enemyBomber = Singleton<PoolingController>.Instance.poolEnemyBomber.New();
		if (enemyBomber == null)
		{
			enemyBomber = UnityEngine.Object.Instantiate<EnemyBomber>(this);
		}
		return enemyBomber;
	}

	public override void Deactive()
	{
		base.Deactive();
		this.EnableAudioMove(false);
		Singleton<PoolingController>.Instance.poolEnemyBomber.Store(this);
	}

	private void ReleaseBomb()
	{
		Bomb bomb = Singleton<PoolingController>.Instance.poolBulletBomb.New();
		if (bomb == null)
		{
			bomb = UnityEngine.Object.Instantiate<Bomb>(this.bombPrefab);
		}
		float projectileDamage = ((SO_EnemyHasProjectileStats)this.baseStats).ProjectileDamage;
		float projectileDamageRadius = ((SO_EnemyHasProjectileStats)this.baseStats).ProjectileDamageRadius;
		AttackData attackData = new AttackData(this, projectileDamage, projectileDamageRadius, false, WeaponType.NormalGun, -1, null);
		bomb.Active(attackData, this.bombReleasePoint, 0f, Singleton<PoolingController>.Instance.groupBullet);
	}

	private void TrackOutOfScreen()
	{
		bool flag;
		if (this.isFromLeft)
		{
			flag = (base.transform.position.x - 2f > Singleton<CameraFollow>.Instance.right.position.x);
		}
		else
		{
			flag = (base.transform.position.x + 2f < Singleton<CameraFollow>.Instance.left.position.x);
		}
		if (flag)
		{
			this.Deactive();
		}
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
