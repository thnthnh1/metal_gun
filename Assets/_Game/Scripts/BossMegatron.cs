using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BossMegatron : BaseEnemy
{
	[Header("BOSS MEGATRON PROPERTIES")]
	public Collider2D head;

	public Collider2D foot;

	public BossMegatronColliderGround colliderCheckGround;

	public BulletBossMegatron bulletPrefab;

	public BaseMuzzle muzzlePrefab;

	public Transform muzzlePoint;

	public ParticleSystem dustMove;

	public GameObject effectWarningPoint;

	public GameObject effectTrailFire;

	[SpineAnimation("", "", true, false)]
	public string idleToShoot;

	[SpineAnimation("", "", true, false)]
	public string jumpToIdle;

	[SpineAnimation("", "", true, false)]
	public string jumpAttack;

	[SpineAnimation("", "", true, false)]
	public string landing;

	[SpineSkin("", "", true, false)]
	public string skin25;

	[SpineSkin("", "", true, false)]
	public string skin50;

	[SpineSkin("", "", true, false)]
	public string skin100;

	[SpineEvent("", "", true, false)]
	public string eventFootStep;

	[SpineEvent("", "", true, false)]
	public string eventActiveTrail;

	[SpineEvent("", "", true, false)]
	public string eventFly;

	public AudioClip soundAppear;

	public AudioClip soundMove;

	public AudioClip soundShoot;

	public AudioClip soundPreSmash;

	public AudioClip soundSmash;

	public AudioClip soundJump;

	public AudioClip soundGrounded;

	private BaseMuzzle muzzle;

	private bool flagMovingEntrance = true;

	private bool flagSmash;

	private bool flagShoot;

	private bool flagJump;

	private int countShoot;

	private int totalBulletShoot;

	private int countSmash;

	private int totalSmash;

	private bool _IsSmashing_k__BackingField;

	private static UnityAction __f__am_cache0;

	public bool IsSmashing
	{
		get;
		set;
	}

	protected override void Awake()
	{
		base.Awake();
		EventDispatcher.Instance.RegisterListener(EventID.ShowInfoBossDone, delegate(Component sender, object param)
		{
			this.ViewInfoBossDone();
		});
	}

	protected override void Update()
	{
		if (!this.isDead)
		{
			this.UpdateDirection();
			this.Entrance();
			if (this.isReadyAttack)
			{
				this.Attack();
			}
		}
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Boss/Boss Megatron/boss_megatron_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BossMegatronStats>(path);
	}

	protected override void Attack()
	{
		if (this.flagJump)
		{
			this.flagJump = false;
			this.PlayAnimationJump();
		}
		else if (this.flagShoot)
		{
			this.flagShoot = false;
			this.PlayAnimationPrepareShoot();
		}
		else if (this.flagSmash)
		{
			if (Mathf.Abs(this.target.transform.position.x - base.transform.position.x) > 3.5f)
			{
				this.PlayAnimationMove();
				this.Move();
			}
			else
			{
				this.flagSmash = false;
				this.StopMoving();
				this.PlayAnimationMeleeAttack();
				SoundManager.Instance.PlaySfx(this.soundPreSmash, 0f);
			}
		}
	}

	protected override void UpdateDirection()
	{
		if (this.target != null && !this.flagSmash && !this.flagShoot && !this.flagJump)
		{
			this.skeletonAnimation.Skeleton.flipX = (this.target.transform.position.x < base.transform.position.x);
		}
		this.UpdateTransformPoints();
	}

	public override void Renew()
	{
		this.isDead = false;
		this.LoadScriptableObject();
		this.stats.Init(this.baseStats);
		this.isFinalBoss = true;
		this.head.enabled = false;
		this.foot.enabled = false;
		this.canMove = true;
		this.isEffectMeleeWeapon = false;
		this.isReadyAttack = false;
		this.target = null;
		base.transform.parent = null;
		this.effectWarningPoint.transform.parent = null;
		this.UpdateTransformPoints();
		this.UpdateHealthBar(false);
	}

	public override void UpdateHealthBar(bool isAutoHide = false)
	{
		Singleton<UIController>.Instance.hudBoss.SetIconBoss(this.id);
		Singleton<UIController>.Instance.hudBoss.UpdateHP(this.HpPercent);
	}

	public override void TakeDamage(AttackData attackData)
	{
		base.TakeDamage(attackData);
		string defaultSkin = this.defaultSkin;
		if (this.HpPercent < 0.5f && this.HpPercent > 0.25f)
		{
			defaultSkin = this.skin50;
		}
		else if (this.HpPercent < 0.25f)
		{
			defaultSkin = this.skin25;
		}
		if (string.Compare(this.defaultSkin, defaultSkin) != 0)
		{
			this.skeletonAnimation.Skeleton.SetSkin(defaultSkin);
		}
	}

	private void Entrance()
	{
		if (this.flagMovingEntrance)
		{
			this.flagMovingEntrance = false;
			base.transform.DOMove(this.basePosition, 0.4f, false).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(delegate
			{
				this.rigid.bodyType = RigidbodyType2D.Dynamic;
				this.skeletonAnimation.AnimationState.SetAnimation(1, this.jumpToIdle, false);
				Singleton<CameraFollow>.Instance.AddShake(1.2f, 1f);
				SoundManager.Instance.PlaySfx(this.soundGrounded, 10f);
				if (GameData.mode == GameMode.Campaign)
				{
					base.StartCoroutine(base.DelayAction(delegate
					{
						EventDispatcher.Instance.PostEvent(EventID.ShowInfoBossMegatron);
					}, StaticValue.waitOneSec));
				}
				else if (GameData.mode == GameMode.Survival)
				{
					this.ViewInfoBossDone();
				}
			}).OnStart(delegate
			{
				this.rigid.bodyType = RigidbodyType2D.Kinematic;
				this.skeletonAnimation.AnimationState.SetAnimation(1, this.landing, false);
			});
		}
	}

	private void ViewInfoBossDone()
	{
		this.head.enabled = true;
		this.foot.enabled = true;
		this.ActiveShoot();
		EventDispatcher.Instance.PostEvent(EventID.FinalBossStart);
		SoundManager.Instance.PlaySfx(this.soundAppear, 0f);
	}

	private void Landing()
	{
		if (this.target == null)
		{
			return;
		}
		Vector2 v = this.target.transform.position;
		v.y = this.basePosition.y;
		v.x += UnityEngine.Random.Range(-2f, 2f);
		v.x = Mathf.Clamp(v.x, Singleton<CameraFollow>.Instance.left.position.x + 2f, Singleton<CameraFollow>.Instance.right.position.x - 2f);
		Vector2 v2 = base.transform.position;
		v2.x = v.x;
		base.transform.position = v2;
		this.effectWarningPoint.transform.position = v;
		this.effectWarningPoint.SetActive(true);
		base.transform.DOMove(v, 0.4f, false).SetDelay(0.85f).SetEase(Ease.Linear).OnComplete(delegate
		{
			this.ActiveGroundCollider(false);
			this.rigid.bodyType = RigidbodyType2D.Dynamic;
			this.dustMove.Play();
			this.effectWarningPoint.SetActive(false);
			this.skeletonAnimation.AnimationState.SetAnimation(1, this.jumpToIdle, false);
			Singleton<CameraFollow>.Instance.AddShake(1.2f, 1f);
			SoundManager.Instance.PlaySfx(this.soundGrounded, 10f);
			if (UnityEngine.Random.Range(0, 2) == 0)
			{
				this.ActiveSmash();
			}
			else
			{
				this.ActiveShoot();
			}
		}).OnStart(delegate
		{
			this.skeletonAnimation.AnimationState.SetAnimation(1, this.landing, false);
		});
	}

	private void ReleaseBullet()
	{
		BulletBossMegatron bulletBossMegatron = Singleton<PoolingController>.Instance.poolBulletBossMegatron.New();
		if (bulletBossMegatron == null)
		{
			bulletBossMegatron = UnityEngine.Object.Instantiate<BulletBossMegatron>(this.bulletPrefab);
		}
		bulletBossMegatron.Active(this.GetCurentAttackData(), this.aimPoint, this.baseStats.BulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
		SoundManager.Instance.PlaySfx(this.soundShoot, 0f);
	}

	private void ActiveJump()
	{
		this.isReadyAttack = false;
		this.StopMoving();
		this.PlayAnimationIdle();
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagJump = true;
			this.flagSmash = false;
			this.flagShoot = false;
		}, StaticValue.waitOneSec));
	}

	private void ActiveSmash()
	{
		this.countSmash = 0;
		this.totalSmash = UnityEngine.Random.Range(1, 3);
		this.isReadyAttack = false;
		this.StopMoving();
		this.PlayAnimationIdle();
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagJump = false;
			this.flagSmash = true;
			this.flagShoot = false;
		}, StaticValue.waitOneSec));
	}

	private void ActiveShoot()
	{
		this.countShoot = 0;
		this.totalBulletShoot = UnityEngine.Random.Range(1, 3);
		this.isReadyAttack = false;
		this.StopMoving();
		this.PlayAnimationIdle();
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagJump = false;
			this.flagSmash = false;
			this.flagShoot = true;
		}, StaticValue.waitOneSec));
	}

	private void ActiveGroundCollider(bool isActive)
	{
		this.colliderCheckGround.gameObject.SetActive(isActive);
	}

	protected override void HandleAnimationStart(TrackEntry entry)
	{
		base.HandleAnimationStart(entry);
		if (string.Compare(entry.animation.name, this.shoot) == 0)
		{
			if (this.muzzle == null)
			{
				this.muzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.muzzlePrefab, this.muzzlePoint.position, this.muzzlePoint.rotation, this.muzzlePoint.parent);
			}
			this.muzzle.Active();
		}
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		if (this.dieAnimationNames.Contains(entry.animation.name))
		{
			this.Deactive();
		}
		if (this.isDead)
		{
			return;
		}
		if (string.Compare(entry.animation.name, this.idleToShoot) == 0)
		{
			this.PlayAnimationShoot(1);
		}
		if (string.Compare(entry.animation.name, this.shoot) == 0)
		{
			this.countShoot++;
			if (this.countShoot < this.totalBulletShoot)
			{
				this.flagShoot = true;
			}
			else if (UnityEngine.Random.Range(0, 2) == 0)
			{
				this.ActiveJump();
			}
			else
			{
				this.ActiveSmash();
			}
		}
		if (string.Compare(entry.animation.name, this.meleeAttack) == 0)
		{
			this.IsSmashing = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
			SoundManager.Instance.PlaySfx(this.soundSmash, 0f);
			this.countSmash++;
			if (this.countSmash < this.totalSmash)
			{
				this.flagSmash = true;
			}
			else if (UnityEngine.Random.Range(0, 2) == 0)
			{
				this.ActiveJump();
			}
			else
			{
				this.ActiveShoot();
			}
		}
		if (string.Compare(entry.animation.name, this.jumpAttack) == 0)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(1, this.jumpToIdle, false);
		}
		if (string.Compare(entry.animation.name, this.jumpToIdle) == 0)
		{
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
		}
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventFootStep) == 0)
		{
			Singleton<CameraFollow>.Instance.AddShake(0.3f, 0.5f);
			this.dustMove.Play();
			SoundManager.Instance.PlaySfx(this.soundMove, 0f);
		}
		if (string.Compare(e.Data.Name, this.eventMeleeAttack) == 0)
		{
			Singleton<CameraFollow>.Instance.AddShake(0.3f, 0.5f);
		}
		if (string.Compare(e.Data.Name, this.eventActiveTrail) == 0)
		{
			this.effectTrailFire.SetActive(true);
		}
		if (string.Compare(e.Data.Name, this.eventShoot) == 0)
		{
			this.ReleaseBullet();
		}
		if (string.Compare(e.Data.Name, this.eventFly) == 0)
		{
			this.rigid.bodyType = RigidbodyType2D.Kinematic;
			Singleton<CameraFollow>.Instance.AddShake(0.5f, 0.5f);
			float endValue = this.basePosition.y + 8f;
			base.transform.DOMoveY(endValue, 2f, false).OnComplete(delegate
			{
				this.ActiveGroundCollider(true);
				this.effectTrailFire.SetActive(false);
				this.Landing();
			});
			SoundManager.Instance.PlaySfx(this.soundJump, 0f);
		}
	}

	protected override void PlayAnimationMeleeAttack()
	{
		base.PlayAnimationMeleeAttack();
		this.IsSmashing = true;
	}

	private void PlayAnimationPrepareShoot()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.idleToShoot, false);
	}

	private void PlayAnimationJump()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(1, this.jumpAttack, false);
	}
}
