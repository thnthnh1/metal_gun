using DG.Tweening;
using DG.Tweening.Core;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BaseEnemy : BaseUnit
{
	private sealed class _CoroutineHideHealthBar_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal BaseEnemy _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineHideHealthBar_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = StaticValue.waitTwoSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._this.ActiveHealthBar(false);
				this._PC = -1;
				break;
			}
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}

	[Header("BASE ENEMY PROPERTIES")]
	public bool canMove = true;

	public bool canJump;

	public bool isMainUnit;

	public bool isRunPassArea = true;

	[HideInInspector]
	public bool isMiniBoss;

	[HideInInspector]
	public bool isFinalBoss;

	[HideInInspector]
	public bool isEffectMeleeWeapon = true;

	[HideInInspector]
	public bool isInvisibleWhenActive;

	public int bounty;

	public EnemyState state;

	public Vector2 moveForce;

	public Vector2 jumpForce;

	public Transform groupTransformPoints;

	public Transform frontCheckPoint;

	public Transform groundCheckPoint;

	[HideInInspector]
	public Vector2 basePosition;

	public FarSensor farSensor;

	public NearSensor nearSensor;

	public Collider2D bodyCollider;

	public Collider2D footCollider;

	public AudioClip soundAttack;

	public LayerMask layerMaskCheckJump;

	public LayerMask layerMaskCheckObstacle;

	[HideInInspector]
	public int zoneId;

	[HideInInspector]
	public int packId;

	[Header("SPINE")]
	public SkeletonAnimation skeletonAnimation;

	public SkeletonUtilityBone aimBone;

	[SpineBone("", "", true, false)]
	public string gunBone;

	[SpineBone("", "", true, false)]
	public string knifeBone;

	[SpineBone("", "", true, false)]
	public string firePointBone;

	[SpineAnimation("", "", true, false)]
	public string idle;

	[SpineAnimation("", "", true, false)]
	public string move;

	[SpineAnimation("", "", true, false)]
	public string moveFast;

	[SpineAnimation("", "", true, false)]
	public string shoot;

	[SpineAnimation("", "", true, false)]
	public string throwGrenade;

	[SpineAnimation("", "", true, false)]
	public string meleeAttack;

	[SpineAnimation("", "", true, false)]
	public string aim;

	[SpineAnimation("", "", true, false, startsWith = "die")]
	public List<string> dieAnimationNames;

	[SpineEvent("", "", true, false)]
	public string eventMeleeAttack;

	[SpineEvent("", "", true, false)]
	public string eventThrowGrenade;

	[SpineEvent("", "", true, false)]
	public string eventShoot;

	[SpineSkin("", "", true, false)]
	public string defaultSkin;

	[SpineSkin("", "", true, false)]
	public string skinMap1;

	[SpineSkin("", "", true, false)]
	public string skinMap2;

	[SpineSkin("", "", true, false)]
	public string skinMap3;

	[Header("EFFECT")]
	public GameObject effectStun;

	[Space(20f)]
	public BaseUnit target;

	protected bool isNotAllowMoveForward;

	protected bool isRunning;

	[SerializeField]
	protected bool isReadyAttack;

	protected bool isAllowAttackTarget;

	[SerializeField]
	protected bool flagGetCloseToTarget;

	[SerializeField]
	protected bool flagMove = true;

	protected bool flagJumpPassObstacle;

	protected float timeChangePatrolPoint = 3f;

	protected float closeUpRange = 2f;

	protected float lastTimeAttack;

	protected float timeCountPatrol;

	protected Vector3 destinationMove;

	protected List<BaseUnit> nearbyVictims = new List<BaseUnit>();

	private bool isBlinkingEffect;

	private float minTimeMove = 1f;

	private float maxTimeMove = 2f;

	private IEnumerator coroutineHideHealthBar;

	private static UnityAction __f__am_cache0;

	public override bool IsFacingRight
	{
		get
		{
			return !this.skeletonAnimation.Skeleton.flipX;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		this.InitSkin();
		this.InitWeapon();
		this.InitSortingLayerSpine();
	}

	protected virtual void Start()
	{
		this.skeletonAnimation.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.HandleAnimationStart);
		this.skeletonAnimation.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.HandleAnimationCompleted);
		this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationEvent);
	}

	protected virtual void Update()
	{
		if (!this.isDead)
		{
			this.UpdateDirection();
			this.Idle();
			this.Patrol();
			this.Attack();
		}
	}

	protected void SwitchState(EnemyState newState)
	{
		if (this.state == newState)
		{
			return;
		}
		this.state = newState;
		switch (this.state)
		{
		case EnemyState.Idle:
			this.StartIdle();
			break;
		case EnemyState.Patrol:
			this.StartPatrol();
			break;
		case EnemyState.Attack:
			this.StartAttack();
			break;
		case EnemyState.Die:
			this.StartDie();
			break;
		}
	}

	protected virtual void StartIdle()
	{
		this.PlayAnimationIdle();
		this.StopMoving();
		this.isRunning = false;
	}

	protected virtual void StartPatrol()
	{
		this.GetRandomPatrolPoint();
		if (this.isRunning)
		{
			this.PlayAnimationMoveFast();
		}
		else
		{
			this.PlayAnimationMove();
		}
	}

	protected virtual void StartAttack()
	{
		this.timeCountPatrol = 0f;
		this.StopMoving();
	}

	protected virtual void StartDie()
	{
		this.target = null;
		this.ResetAim();
		this.SetColliderLayers(false);
		this.ActiveSensor(false);
		this.PlayAnimationDie();
		List<ItemDropData> itemDrop = this.GetItemDrop();
		Singleton<GameController>.Instance.itemDropController.Spawn(itemDrop, base.BodyCenterPoint.position, this);
		if (this.isMiniBoss || this.isFinalBoss)
		{
			if (GameData.mode == GameMode.Campaign)
			{
				Singleton<UIController>.Instance.ActiveIngameUI(false);
			}
			EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionMultiple, base.transform.position);
			SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
			Singleton<CameraFollow>.Instance.slowMotion.Show(3.5f, delegate
			{
				if (GameData.mode == GameMode.Campaign)
				{
					EventDispatcher.Instance.PostEvent(EventID.FinishStage, 0.5f);
				}
				else if (GameData.mode == GameMode.Survival)
				{
					EventDispatcher.Instance.PostEvent(EventID.BossSurvivalDie);
				}
			});
			if (this.isFinalBoss)
			{
				EventDispatcher.Instance.PostEvent(EventID.FinalBossDie, this.id);
			}
		}
	}

	protected override void Idle()
	{
		if (this.state == EnemyState.Idle && this.canMove)
		{
			this.timeCountPatrol += Time.deltaTime;
			if (this.timeCountPatrol >= this.timeChangePatrolPoint)
			{
				this.timeCountPatrol = 0f;
				this.SwitchState(EnemyState.Patrol);
			}
		}
	}

	protected override void Attack()
	{
	}

	protected override void Die()
	{
		base.transform.DOKill(false);
		this.rigid.DOKill(false);
		base.Die();
		base.StopAllCoroutines();
		this.skeletonAnimation.ClearState();
		this.SwitchState(EnemyState.Die);
	}

	protected override void Move()
	{
		float num = (!this.isRunning) ? this.stats.MoveSpeed : (this.stats.MoveSpeed * 1.5f);
		if (this.rigid.velocity.x < num || this.rigid.velocity.x > -num)
		{
			Vector2 force = (!this.IsFacingRight) ? (-this.moveForce) : this.moveForce;
			this.rigid.AddForce(force, ForceMode2D.Impulse);
		}
		if (this.rigid.velocity.x > num || this.rigid.velocity.x < -num)
		{
			Vector2 velocity = this.rigid.velocity;
			velocity.x = ((!this.IsFacingRight) ? (-this.stats.MoveSpeed) : this.stats.MoveSpeed);
			this.rigid.velocity = velocity;
		}
	}

	protected override void UpdateDirection()
	{
		if (this.target != null)
		{
			this.skeletonAnimation.Skeleton.flipX = (this.target.transform.position.x < base.transform.position.x);
		}
		else if (base.transform.position != this.destinationMove && this.canMove)
		{
			this.skeletonAnimation.Skeleton.flipX = (this.destinationMove.x < base.transform.position.x);
		}
		this.UpdateTransformPoints();
	}

	protected override void EffectTakeDamage()
	{
		if (!this.isBlinkingEffect)
		{
			this.isBlinkingEffect = true;
			DOTween.To(new DOSetter<float>(this.ColorSetter), 1f, 0f, 0.1f).OnComplete(new TweenCallback(this.ChangeColorToDefault));
		}
	}

	protected override void ApplyDebuffs(AttackData attackData)
	{
		if (attackData.debuffs == null || attackData.debuffs.Count <= 0)
		{
			return;
		}
		base.ApplyDebuffs(attackData);
		for (int i = 0; i < attackData.debuffs.Count; i++)
		{
			DebuffData debuffData = attackData.debuffs[i];
			DebuffType type = debuffData.type;
			if (type != DebuffType.Stun)
			{
				if (type != DebuffType.TakeMoreDamageWhenHighHP)
				{
					if (type == DebuffType.Reflect)
					{
						if (this.isFinalBoss)
						{
							attackData.damage = 0f;
						}
						else
						{
							EffectController arg_F5_0 = EffectController.Instance;
							Vector2 position = base.BodyCenterPoint.position;
							Color cyan = Color.cyan;
							string content = "REFLECT";
							Transform groupText = Singleton<PoolingController>.Instance.groupText;
							arg_F5_0.SpawnTextTMP(position, cyan, content, 3.5f, groupText);
						}
					}
				}
				else if (this.HpPercent >= 0.5f)
				{
					attackData.damage *= 1f + debuffData.damage / 100f;
				}
			}
			else
			{
				this.GetStun(debuffData.duration);
			}
		}
	}

	public override void SetTarget(BaseUnit unit)
	{
		if (this.target == null)
		{
			this.target = unit;
			this.SwitchState(EnemyState.Attack);
		}
	}

	public override void TakeDamage(AttackData attackData)
	{
		if (this.isImmortal || this.isDead || attackData.attacker.isDead)
		{
			return;
		}
		if (attackData.isCritical)
		{
			Vector2 position = base.BodyCenterPoint.position;
			position.y += 1f;
			EffectController.Instance.SpawnTextCRIT(position, null);
			EventDispatcher.Instance.PostEvent(EventID.GetCriticalHit);
		}
		this.ApplyDebuffs(attackData);
		if (attackData.damage > 0f)
		{
			this.EffectTakeDamage();
			this.ShowTextDamageTaken(attackData);
			this.stats.AdjustStats(-attackData.damage, StatsType.Hp);
		}
		this.UpdateHealthBar(true);
		if (this.stats.HP <= 0f)
		{
			this.Die();
			EventDispatcher.Instance.PostEvent(EventID.UnitDie, new UnitDieData(this, attackData));
		}
	}

	public override void TakeDamage(float damage)
	{
		if (this.isImmortal || this.isDead)
		{
			return;
		}
		if (damage > 0f)
		{
			this.EffectTakeDamage();
			this.ShowTextDamageTaken(damage);
			this.stats.AdjustStats(-damage, StatsType.Hp);
		}
		this.UpdateHealthBar(true);
		if (this.stats.HP <= 0f)
		{
			this.Die();
			UnitDieData param = new UnitDieData(this, null);
			EventDispatcher.Instance.PostEvent(EventID.UnitDie, param);
		}
	}

	public override void Active(int id, int level, Vector2 position)
	{
		base.Active(id, level, position);
		if (this.isInvisibleWhenActive)
		{
			this.FadeIn();
		}
	}

	public virtual void Active(EnemySpawnData spawnData)
	{
		if (GameData.mode == GameMode.Campaign)
		{
			int levelEnemy = GameData.staticCampaignStageData.GetLevelEnemy(GameData.currentStage.id, GameData.currentStage.difficulty);
			this.Active(spawnData.id, levelEnemy, spawnData.position);
			this.zoneId = spawnData.zoneId;
			this.packId = spawnData.packId;
			this.isMainUnit = spawnData.isMainUnit;
			this.isRunPassArea = spawnData.isRunPassArea;
			this.canMove = spawnData.isCanMove;
			this.canJump = spawnData.isCanJump;
			this.bounty = spawnData.bounty;
			this.itemDropList = new List<ItemDropData>();
			for (int i = 0; i < spawnData.items.Count; i++)
			{
				ItemDropData itemDropData = spawnData.items[i];
				ItemDropData item = new ItemDropData(itemDropData.type, itemDropData.value, itemDropData.dropRate);
				this.itemDropList.Add(item);
			}
		}
	}

	public override void Renew()
	{
		base.Renew();
		this.zoneId = 0;
		this.packId = 0;
		this.isMainUnit = false;
		this.canMove = true;
		this.canJump = false;
		this.isRunPassArea = false;
		this.isImmortal = false;
		this.flagGetCloseToTarget = false;
		this.flagJumpPassObstacle = false;
		this.flagMove = true;
		this.isEffectMeleeWeapon = true;
		if (this.effectStun)
		{
			this.effectStun.SetActive(false);
		}
		this.bounty = 0;
		this.target = null;
		base.transform.parent = null;
		this.timeChangePatrolPoint = UnityEngine.Random.Range(this.minTimeMove, this.maxTimeMove);
		this.SetCloseRange();
		this.InitPatrolPoint();
		this.skeletonAnimation.initialFlipX = true;
		this.UpdateTransformPoints();
		this.SetColliderLayers(true);
		this.ActiveSensor(true);
		this.UpdateHealthBar(false);
		this.ActiveHealthBar(false);
		this.skeletonAnimation.ClearState();
		this.nearbyVictims.Clear();
		this.itemDropList.Clear();
		this.SwitchState(EnemyState.Idle);
	}

	public override void UpdateHealthBar(bool isAutoHide = false)
	{
		if (this.healthBar != null)
		{
			Vector2 size = this.healthBar.size;
			size.x = this.healthBarSizeX * this.HpPercent;
			this.healthBar.size = size;
			this.ActiveHealthBar(this.HpPercent > 0f);
			if (isAutoHide)
			{
				if (this.coroutineHideHealthBar != null)
				{
					base.StopCoroutine(this.coroutineHideHealthBar);
					this.coroutineHideHealthBar = null;
				}
				this.coroutineHideHealthBar = this.CoroutineHideHealthBar();
				base.StartCoroutine(this.coroutineHideHealthBar);
			}
		}
	}

	public override void Deactive()
	{
		base.Deactive();
		if (this.coroutineHideHealthBar != null)
		{
			base.StopCoroutine(this.coroutineHideHealthBar);
			this.coroutineHideHealthBar = null;
		}
		Singleton<GameController>.Instance.RemoveUnit(base.gameObject);
		this.isInvisibleWhenActive = false;
		base.gameObject.SetActive(false);
	}

	public override void GetStun(float duration)
	{
		if (this.isFinalBoss || this.isMiniBoss || this.isStun || !this.isEffectMeleeWeapon)
		{
			return;
		}
		this.isStun = true;
		if (this.effectStun)
		{
			this.effectStun.SetActive(true);
		}
		this.StopMoving();
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.idle, false);
		this.rigid.angularDrag = 0f;
		this.rigid.velocity = Vector3.zero;
		base.enabled = false;
		this.StartDelayAction(delegate
		{
			this.isStun = false;
			this.rigid.angularDrag = 0.05f;
			base.enabled = true;
			if (this.effectStun)
			{
				this.effectStun.SetActive(false);
			}
		}, duration);
	}

	public override bool IsOutOfScreen()
	{
		return base.transform.position.x < Singleton<CameraFollow>.Instance.left.position.x - 0.5f || base.transform.position.x > Singleton<CameraFollow>.Instance.right.position.x + 0.5f;
	}

	protected virtual void InitSkin()
	{
		string skin = this.defaultSkin;
		if (GameData.mode == GameMode.Campaign)
		{
			int num = int.Parse(Singleton<GameController>.Instance.CampaignMap.stageNameId.Split(new char[]
			{
				'.'
			}).First<string>());
			MapType mapType = (MapType)num;
			if (mapType != MapType.Map_1_Desert)
			{
				if (mapType != MapType.Map_2_Lab)
				{
					if (mapType == MapType.Map_3_Jungle)
					{
						if (!string.IsNullOrEmpty(this.skinMap3))
						{
							skin = this.skinMap3;
						}
					}
				}
				else if (!string.IsNullOrEmpty(this.skinMap2))
				{
					skin = this.skinMap2;
				}
			}
			else if (!string.IsNullOrEmpty(this.skinMap1))
			{
				skin = this.skinMap1;
			}
		}
		else if (GameData.mode == GameMode.Survival)
		{
			int num2 = UnityEngine.Random.Range(0, 3);
			if (num2 == 0)
			{
				if (!string.IsNullOrEmpty(this.skinMap1))
				{
					skin = this.skinMap1;
				}
			}
			else if (num2 == 1)
			{
				if (!string.IsNullOrEmpty(this.skinMap2))
				{
					skin = this.skinMap2;
				}
			}
			else if (num2 == 2 && !string.IsNullOrEmpty(this.skinMap3))
			{
				skin = this.skinMap3;
			}
		}
		this.skeletonAnimation.Skeleton.SetSkin(skin);
	}

	protected virtual void InitWeapon()
	{
	}

	protected virtual void InitSortingLayerSpine()
	{
	}

	protected virtual void ReleaseAttack()
	{
		this.PlaySound(this.soundAttack);
	}

	protected virtual void Patrol()
	{
		if (this.state == EnemyState.Patrol)
		{
			this.CheckAllowMovePatrol();
			if (!this.isNotAllowMoveForward)
			{
				if (Mathf.Abs(base.transform.position.x - this.destinationMove.x) > 0.1f)
				{
					this.Move();
				}
				else
				{
					this.SwitchState(EnemyState.Idle);
				}
			}
			else
			{
				this.isRunning = false;
				this.SetDestinationPatrol(false);
			}
		}
	}

	protected virtual void CheckAllowAttackTarget()
	{
		if (this.target != null)
		{
			Vector3 position = base.BodyCenterPoint.position;
			Vector3 position2 = this.target.BodyCenterPoint.position;
			position2.y = position.y;
			this.isAllowAttackTarget = !Physics2D.Linecast(position, position2, this.layerMaskCheckObstacle);
		}
	}

	protected virtual void CheckAllowMoveForwardToTarget()
	{
		Vector2 end = this.frontCheckPoint.position + this.frontCheckPoint.right * 0.15f;
		bool flag = Physics2D.Linecast(this.frontCheckPoint.position, end, StaticValue.LAYER_OBSTACLE);
		bool flag2 = Physics2D.Linecast(this.frontCheckPoint.position, this.groundCheckPoint.position, this.layerMaskCheckObstacle);
		if (this.isRunPassArea)
		{
			this.isNotAllowMoveForward = flag;
		}
		else
		{
			this.isNotAllowMoveForward = (flag || !flag2);
		}
	}

	protected virtual void CheckAllowMovePatrol()
	{
		Vector2 vector = this.frontCheckPoint.position + this.frontCheckPoint.right * 0.15f;
		bool flag = Physics2D.Linecast(this.frontCheckPoint.position, vector, this.layerMaskCheckObstacle);
		UnityEngine.Debug.DrawLine(this.frontCheckPoint.position, vector, Color.red);
		bool flag2 = Physics2D.Linecast(this.frontCheckPoint.position, this.groundCheckPoint.position, this.layerMaskCheckObstacle);
		UnityEngine.Debug.DrawLine(this.frontCheckPoint.position, this.groundCheckPoint.position, Color.green);
		this.isNotAllowMoveForward = (flag || !flag2);
	}

	protected virtual bool IsTargetInCloseRange()
	{
		if (this.target != null)
		{
			float num = Mathf.Abs(base.transform.position.x - this.target.transform.position.x);
			return num <= this.closeUpRange;
		}
		return false;
	}

	protected virtual void CancelCombat()
	{
		this.target = null;
		this.SwitchState(EnemyState.Idle);
	}

	protected virtual void StartChasingTarget()
	{
		this.target = null;
		Vector3 position = base.transform.position;
		position.x = ((!this.IsFacingRight) ? (position.x - 5f) : (position.x + 5f));
		this.SetDestinationMove(position);
		this.isRunning = true;
		this.SwitchState(EnemyState.Patrol);
	}

	protected virtual void GetCloseToTarget()
	{
		if (this.canMove && this.target != null)
		{
			if (!this.flagGetCloseToTarget)
			{
				return;
			}
			Vector2 end = this.frontCheckPoint.position + this.frontCheckPoint.right * 0.15f;
			bool flag = Physics2D.Linecast(this.frontCheckPoint.position, end, this.layerMaskCheckJump);
			bool flag2 = Physics2D.Linecast(this.frontCheckPoint.position, this.groundCheckPoint.position, this.layerMaskCheckObstacle);
			if (flag)
			{
				if (this.canJump)
				{
					if (!this.flagJumpPassObstacle && flag2)
					{
						this.flagJumpPassObstacle = true;
						this.rigid.AddForce(this.jumpForce, ForceMode2D.Impulse);
						base.StartCoroutine(base.DelayAction(delegate
						{
							this.flagJumpPassObstacle = false;
						}, StaticValue.waitHalfSec));
					}
					this.Move();
				}
				else if (this.flagMove)
				{
					this.flagMove = false;
					this.PlayAnimationIdle();
				}
			}
			else if (!this.flagJumpPassObstacle)
			{
				bool flag3 = Mathf.Abs(this.target.transform.position.x - base.transform.position.x) < 0.3f;
				if (this.isNotAllowMoveForward || flag3)
				{
					if (this.flagMove)
					{
						this.flagMove = false;
						this.PlayAnimationIdle();
					}
				}
				else
				{
					if (!this.flagMove)
					{
						this.flagMove = true;
						this.PlayAnimationMoveFast();
					}
					this.Move();
				}
			}
		}
	}

	protected virtual void ReadyToAttack()
	{
		this.isReadyAttack = true;
	}

	protected virtual void SetColliderLayers(bool isActive)
	{
		if (this.bodyCollider != null)
		{
			this.bodyCollider.gameObject.layer = ((!isActive) ? StaticValue.LAYER_DEFAULT : StaticValue.LAYER_BODY_ENEMY);
		}
		if (this.footCollider != null)
		{
			this.footCollider.gameObject.layer = ((!isActive) ? StaticValue.LAYER_DEFAULT : StaticValue.LAYER_FOOT_ENEMY);
		}
	}

	protected virtual void SetCloseRange()
	{
		if (this.nearSensor != null)
		{
			this.closeUpRange = UnityEngine.Random.Range(1.5f, this.nearSensor.col.radius);
			this.nearSensor.col.radius = this.closeUpRange;
		}
	}

	protected virtual void InitPatrolPoint()
	{
		Vector3 position = base.transform.position;
		position.x = ((!this.IsFacingRight) ? (position.x - 0.5f) : (position.x + 0.5f));
		this.SetDestinationMove(position);
	}

	protected virtual void SetDestinationPatrol(bool isMoveForward)
	{
		float num = UnityEngine.Random.Range(0.5f, 3f);
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
		this.SetDestinationMove(position);
	}

	protected virtual List<ItemDropData> GetItemDrop()
	{
		List<ItemDropData> list = new List<ItemDropData>();
		int num = this.itemDropList.Count - 1;
		float num2 = 100f;
		bool flag = false;
		while (num >= 0 && !flag)
		{
			ItemDropData itemDropData = this.itemDropList[num];
			float num3 = Mathf.Clamp01(itemDropData.dropRate / num2);
			int num4 = UnityEngine.Random.Range(1, 101);
			if (Mathf.RoundToInt(num3 * 100f) >= num4)
			{
				flag = true;
				list.Add(itemDropData);
			}
			num2 -= itemDropData.dropRate;
			num2 = Mathf.Clamp(num2, 0f, 100f);
			num--;
		}
		if (this.bounty > 0)
		{
			ItemDropData item = new ItemDropData(ItemDropType.Coin, (float)this.bounty, 100f);
			list.Add(item);
		}
		return list;
	}

	protected virtual void FadeIn()
	{
		DOTween.To(new DOSetter<float>(this.AlphaSetter), 0f, 1f, 2.5f).OnComplete(new TweenCallback(this.FadeInDone));
	}

	protected virtual void FadeInDone()
	{
	}

	protected virtual void ResetAim()
	{
		if (this.aimPoint != null)
		{
			this.aimPoint.parent.localRotation = Quaternion.identity;
		}
		if (this.aimBone != null)
		{
			this.aimBone.transform.position = this.aimPoint.position;
		}
	}

	public virtual BaseEnemy GetFromPool()
	{
		return null;
	}

	public virtual void ActiveSensor(bool isActive)
	{
		if (this.farSensor != null)
		{
			this.farSensor.gameObject.SetActive(isActive);
		}
		if (this.nearSensor != null)
		{
			this.nearSensor.gameObject.SetActive(isActive);
		}
	}

	public virtual void OnUnitGetInFarSensor(BaseUnit unit)
	{
		this.SetTarget(unit);
		if (Vector2.Distance(this.target.transform.position, base.BodyCenterPoint.position) > this.nearSensor.col.radius)
		{
			if (this.canMove)
			{
				this.flagGetCloseToTarget = true;
				this.PlayAnimationMoveFast();
			}
			else
			{
				this.PlayAnimationIdle();
			}
		}
	}

	public virtual void OnUnitGetOutFarSensor(BaseUnit unit)
	{
		if (this.canMove)
		{
			this.farSensor.gameObject.SetActive(false);
			base.StartCoroutine(base.DelayAction(delegate
			{
				this.farSensor.gameObject.SetActive(true);
				this.flagGetCloseToTarget = false;
				this.StartChasingTarget();
			}, StaticValue.waitHalfSec));
		}
		else
		{
			this.CancelCombat();
		}
	}

	public virtual void OnUnitGetInNearSensor(BaseUnit unit)
	{
		if (this.canMove)
		{
			this.flagGetCloseToTarget = false;
			this.PlayAnimationIdle();
			this.StopMoving();
		}
		if (!this.nearbyVictims.Contains(unit))
		{
			this.nearbyVictims.Add(unit);
		}
	}

	public virtual void OnUnitGetOutNearSensor(BaseUnit unit)
	{
		if (this.canMove)
		{
			this.nearSensor.gameObject.SetActive(false);
			base.StartCoroutine(base.DelayAction(delegate
			{
				this.nearSensor.gameObject.SetActive(true);
				this.flagGetCloseToTarget = true;
				this.flagMove = true;
				this.PlayAnimationMoveFast();
			}, StaticValue.waitHalfSec));
		}
		if (this.nearbyVictims.Contains(unit))
		{
			this.nearbyVictims.Remove(unit);
		}
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventShoot) == 0)
		{
			this.ReleaseAttack();
		}
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		if (this.dieAnimationNames.Contains(entry.animation.name))
		{
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(0, 0f);
			this.skeletonAnimation.ClearState();
			this.Deactive();
		}
	}

	protected virtual void PlayAnimationShoot(int trackIndex = 1)
	{
		TrackEntry trackEntry;
		if (this.flagGetCloseToTarget)
		{
			trackEntry = this.skeletonAnimation.AnimationState.SetAnimation(1, this.shoot, false);
		}
		else
		{
			trackEntry = this.skeletonAnimation.AnimationState.SetAnimation(0, this.shoot, false);
		}
		trackEntry.AttachmentThreshold = 1f;
		trackEntry.MixDuration = 0f;
		TrackEntry trackEntry2 = this.skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
		trackEntry2.AttachmentThreshold = 1f;
	}

	protected virtual void PlayAnimationMeleeAttack()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(1, this.meleeAttack, false);
	}

	protected virtual void PlayAnimationThrow()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(1, this.throwGrenade, false);
	}

	protected virtual void PlayAnimationDie()
	{
		int index = UnityEngine.Random.Range(0, this.dieAnimationNames.Count);
		string animationName = this.dieAnimationNames[index];
		this.skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
	}

	public virtual void PlayAnimationIdle()
	{
		TrackEntry current = this.skeletonAnimation.AnimationState.GetCurrent(0);
		if (current == null || string.Compare(current.animation.name, this.idle) != 0)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.idle, true);
		}
	}

	public virtual void PlayAnimationMove()
	{
		TrackEntry current = this.skeletonAnimation.AnimationState.GetCurrent(0);
		if (current == null || string.Compare(current.animation.name, this.move) != 0)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.move, true);
		}
	}

	public virtual void PlayAnimationMoveFast()
	{
		TrackEntry current = this.skeletonAnimation.AnimationState.GetCurrent(0);
		if (string.IsNullOrEmpty(this.moveFast))
		{
			if (current == null || string.Compare(current.animation.name, this.move) != 0)
			{
				this.skeletonAnimation.AnimationState.SetAnimation(0, this.move, true);
			}
		}
		else if (current == null || string.Compare(current.animation.name, this.moveFast) != 0)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.moveFast, true);
		}
	}

	protected void GetRandomPatrolPoint()
	{
		int num = UnityEngine.Random.Range(0, 2);
		bool destinationPatrol = num < 1;
		this.SetDestinationPatrol(destinationPatrol);
	}

	protected virtual void TrackAimPoint()
	{
		if (this.target)
		{
			this.ActiveAim(true);
		}
		else
		{
			this.ActiveAim(false);
		}
	}

	protected virtual void ActiveAim(bool isActive)
	{
		if (isActive)
		{
			if (this.skeletonAnimation.AnimationState.GetCurrent(2) != null && string.Compare(this.skeletonAnimation.AnimationState.GetCurrent(2).animation.name, this.aim) == 0)
			{
				return;
			}
			this.skeletonAnimation.AnimationState.SetAnimation(2, this.aim, false);
		}
		else
		{
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(2, 0f);
			this.isReadyAttack = false;
			this.ResetAim();
		}
	}

	protected virtual void UpdateTransformPoints()
	{
		Vector3 localEulerAngles = this.groupTransformPoints.localEulerAngles;
		localEulerAngles.y = ((!this.skeletonAnimation.Skeleton.flipX) ? 0f : 180f);
		this.groupTransformPoints.localEulerAngles = localEulerAngles;
	}

	protected void AlphaSetter(float newValue)
	{
		SkeletonAnimation[] componentsInChildren = base.GetComponentsInChildren<SkeletonAnimation>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].skeleton.a = newValue;
		}
		SpriteRenderer[] componentsInChildren2 = base.GetComponentsInChildren<SpriteRenderer>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			Color color = componentsInChildren2[j].color;
			color.a = newValue;
			componentsInChildren2[j].color = color;
		}
	}

	private void ColorSetter(float pNewValue)
	{
		this.skeletonAnimation.skeleton.g = pNewValue;
		this.skeletonAnimation.skeleton.b = pNewValue;
	}

	private void ChangeColorToDefault()
	{
		this.skeletonAnimation.skeleton.SetColor(Color.white);
		this.isBlinkingEffect = false;
	}

	private IEnumerator CoroutineHideHealthBar()
	{
		BaseEnemy._CoroutineHideHealthBar_c__Iterator0 _CoroutineHideHealthBar_c__Iterator = new BaseEnemy._CoroutineHideHealthBar_c__Iterator0();
		_CoroutineHideHealthBar_c__Iterator._this = this;
		return _CoroutineHideHealthBar_c__Iterator;
	}

	public void DelayTargetPlayer()
	{
		this.StartDelayAction(delegate
		{
			this.SetTarget(Singleton<GameController>.Instance.Player);
			this.flagGetCloseToTarget = true;
			this.PlayAnimationMoveFast();
		}, 0.5f);
	}

	public void SetDestinationMove(Vector2 destination)
	{
		this.destinationMove = destination;
	}
}
