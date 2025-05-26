using CnControls;
using DG.Tweening;
using DG.Tweening.Core;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rambo : BaseUnit
{
	private sealed class _CoroutineImmortal_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timer___0;

		internal WaitForSeconds _colorChangeInterval___0;

		internal float duration;

		internal Color _c___1;

		internal Rambo _this;

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

		public _CoroutineImmortal_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timer___0 = 0f;
				this._colorChangeInterval___0 = new WaitForSeconds(0.2f);
				this._this.isImmortal = true;
				this._this.effectImmortal.SetActive(true);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timer___0 < this.duration)
			{
				this._timer___0 += 0.2f;
				this._c___1 = this._this.skeletonAnimation.skeleton.GetColor();
				this._c___1.a = ((this._c___1.a != 1f) ? 1f : 0.5f);
				this._this.skeletonAnimation.skeleton.SetColor(this._c___1);
				this._current = this._colorChangeInterval___0;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.isImmortal = false;
			this._this.effectImmortal.SetActive(false);
			this._this.ChangeColorToDefault();
			this._PC = -1;
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

	private sealed class _CoroutineCooldownGrenade_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _count___0;

		internal float cooldown;

		internal float _percentCooldown___1;

		internal Rambo _this;

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

		public _CoroutineCooldownGrenade_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.isCooldownGrenade = true;
				Singleton<UIController>.Instance.SetCooldownButtonGrenade(false);
				this._count___0 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._this.isCooldownGrenade)
			{
				this._count___0 += Time.deltaTime;
				this._this.isCooldownGrenade = (this._count___0 < this.cooldown);
				this._percentCooldown___1 = Mathf.Clamp01(this._count___0 / this.cooldown);
				Singleton<UIController>.Instance.imageCooldownGrenade.fillAmount = this._percentCooldown___1;
				Singleton<UIController>.Instance.textCooldownGrenade.text = string.Format("{0:f1}", this.cooldown - this._count___0);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.isCooldownGrenade = false;
			Singleton<UIController>.Instance.SetCooldownButtonGrenade(true);
			this._PC = -1;
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

	[Header("TRANSFORM")]
	public Transform spawnCrossBulletPoint;

	public Transform throwGrenadePoint;

	public Transform groundCheck;

	public Transform groupWeapon;

	public Transform groupTransformFlip;

	public BoxCollider2D colliderBody;

	public Vector2 moveForce;

	public Vector2 jumpForce;

	public Vector2 throwGrenadeDirection;

	public float throwForceValue;

	public float fallForceValue;

	[HideInInspector]
	public Vector2 lastDiePosition;

	[Header("AIM")]
	public Transform straightAimPoint;

	public Transform crossAimPoint;

	public Transform upAimPoint;

	public Transform downAimPoint;

	public Transform crouchAimPoint;

	[Header("EQUIPMENT")]
	public BaseWeapon currentWeapon;

	private WeaponType currentWeaponType;

	private BaseGun normalGun;

	private BaseGun specialGun;

	[SerializeField]
	private BaseGun dropGun;

	private BaseMeleeWeapon meleeWeapon;

	private BaseGrenade grenadePrefab;

	private int numberOfGrenade;

	private int grenadeLevel;

	private float meleeAttackRate;

	[Header("SPINE")]
	public SkeletonAnimation skeletonAnimation;

	public SkeletonRenderer skeletonRenderer;

	public SkeletonUtilityBone aimBone;

	[SpineAnimation("", "", true, false)]
	public string crouch;

	[SpineAnimation("", "", true, false)]
	public string move;

	[SpineAnimation("", "", true, false)]
	public string jump;

	[SpineAnimation("", "", true, false)]
	public string lookDown;

	[SpineAnimation("", "", true, false)]
	public string throwGrenade;

	[SpineAnimation("", "", true, false)]
	public string victory;

	[SpineAnimation("", "", true, false)]
	public string parachute;

	[SpineAnimation("", "", true, false)]
	public string fallBackward;

	[SpineAnimation("", "", true, false)]
	public string idle;

	[SpineAnimation("", "", true, false)]
	public string idleRifle;

	[SpineAnimation("", "", true, false)]
	public string idlePistol;

	[SpineAnimation("", "", true, false)]
	public string idleInJet;

	[SpineAnimation("", "", true, false)]
	public string shoot;

	[SpineAnimation("", "", true, false)]
	public string shootRifle;

	[SpineAnimation("", "", true, false)]
	public string shootPistol;

	[SpineAnimation("", "", true, false)]
	public string aim;

	[SpineAnimation("", "", true, false)]
	public string aimRifle;

	[SpineAnimation("", "", true, false)]
	public string aimPistol;

	[SpineAnimation("", "", true, false)]
	public string meleeAttack;

	[SpineAnimation("", "", true, false)]
	public string knife;

	[SpineAnimation("", "", true, false)]
	public string pan;

	[SpineAnimation("", "", true, false)]
	public string guitar;

	[SpineAnimation("", "", true, false)]
	public List<string> dieAnimationNames;

	[SpineBone("", "", true, false)]
	public string equipGunBoneName;

	[SpineBone("", "", true, false)]
	public string equipMeleeWeaponBoneName;

	[SpineBone("", "", true, false)]
	public string effectWindBoneName;

	[SpineEvent("", "", true, false)]
	public string eventFootstep;

	[SpineEvent("", "", true, false)]
	public string eventMeleeAttack;

	[SpineEvent("", "", true, false)]
	public string eventThrowGrenade;

	private Vector2 idleAimPointPosition;

	private Vector2 crouchAimPointPosition;

	[Header("ACTION CONFIG")]
	public bool enableAttack = true;

	public bool enableMoving = true;

	public bool enableJumping = true;

	public bool enableAiming = true;

	public bool enableCrouching = true;

	public bool enableFlipX = true;

	[Header("STATE")]
	public PlayerState state;

	public bool isGrounded;

	[Header("EFFECT")]
	public ParticleSystem effectDustGround;

	public ParticleSystem effectRestoreHP;

	public GameObject effectStun;

	public GameObject effectImmortal;

	[Space(20f)]
	public BaseSkillTree skillTreePrefab;

	public LayerMask layerMaskCheckObstacle;

	public AudioClip soundGetItemDrop;

	public AudioClip soundMoveUnderWater;

	public AudioClip soundRevive;

	public AudioClip soundChangeWeapon;

	public AudioClip soundLowHp;

	public AudioClip soundKnifeKill;

	public AudioClip soundGrenadeKill;

	protected BaseSkillTree skillTree;

	private float inputHorizontalValue;

	private float inputVerticalValue;

	private float inputAttackHorizontalValue;

	private float inputAttackVerticalValue;

	private float lastTimeShoot;

	private float lastTimeMeleeAttack;

	private float defaultReviveImmortalTime = 3f;

	private bool isUsingHorizontal;
	private bool isUsingAimHorizontal;
	private bool isUsingAimVertical;
	private bool isUsingAim;
	private bool isUsingMove;

	[HideInInspector]
	public bool isUsingVerticalUp;

	[HideInInspector]
	public bool isUsingVerticalDown;

	private bool isUsingVerticalCross;

	private bool flagJump;

	private bool flagLookDown;

	private bool flagLookUp;

	private bool flagLookCross;

	private bool flagSpawnCrossBullet;

	private bool flagMeleeAttack;

	private bool flagThrowGrenade;

	private bool flagAnimVictory;

	public bool isFiring;

	private bool isAutoFire;

	protected bool isCooldownGrenade;

	private bool isUsingNormalGun = true;

	private bool isBlinkingEffect;

	private bool isDamageBuffed;

	private bool isCriticalBuffed;

	private bool isSpeedBuffed;

	private bool isRenew;

	private int countRevive;

	private int countComboKill;

	private int countComboKillBySpecialGun;

	private List<BaseUnit> nearbyEnemies = new List<BaseUnit>();

	private List<BaseUnit> meleeWeaponVictims = new List<BaseUnit>();

	private List<ModifierData> listModifier = new List<ModifierData>();

	private static Action __f__am_cache0;

	public Transform aimParent;
	public Transform aimReference;

	public float xAttackDirection;
	public float yAttackDirection;

	public override bool IsFacingRight
	{
		get
		{
			return !this.skeletonAnimation.Skeleton.flipX;
		}
	}

	public override bool IsMoving
	{
		get
		{
			return this.isUsingHorizontal && this.rigid.velocity.sqrMagnitude != 0f;
		}
	}

	public void SetLookDir(bool isFacingRight)
	{
		if (isFacingRight)
		{
			this.skeletonAnimation.Skeleton.flipX = false;
			Vector3 localEulerAngles = this.groupTransformFlip.localEulerAngles;
			localEulerAngles.y = 0f;
			this.groupTransformFlip.localEulerAngles = localEulerAngles;
		}
		else
		{
			this.skeletonAnimation.Skeleton.flipX = true;
			Vector3 localEulerAngles2 = this.groupTransformFlip.localEulerAngles;
			localEulerAngles2.y = 180f;
			this.groupTransformFlip.localEulerAngles = localEulerAngles2;
		}
	}

	protected override void Awake()
	{
		this.level = GameData.playerRambos.GetRamboLevel(ProfileManager.UserProfile.ramboId);
		this.grenadeLevel = GameData.playerGrenades.GetGrenadeLevel(ProfileManager.UserProfile.grenadeId);
		this.InitSkills();
		base.Awake();
	}

	protected virtual void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ClickButtonJump, delegate(Component sender, object param)
		{
			this.TryJump();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ClickButtonShoot, delegate(Component sender, object param)
		{
			this.TryAttack((bool)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ClickButtonThrowGrenade, delegate(Component sender, object param)
		{
			this.TryThrowGrenade();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ToggleSwitchGun, delegate(Component sender, object param)
		{
			this.TrySwitchGun();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ToggleAutoFire, delegate(Component sender, object param)
		{
			this.OnToggleAutoFire();
		});
		EventDispatcher.Instance.RegisterListener(EventID.OutOfAmmo, delegate(Component sender, object param)
		{
			this.OnSpecialGunOutOfAmmo();
		});
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, delegate(Component sender, object param)
		{
			this.OnKillUnit((UnitDieData)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.TimeOutComboKill, delegate(Component sender, object param)
		{
			this.OnTimeOutComboKill();
		});
		EventDispatcher.Instance.RegisterListener(EventID.GetItemDrop, delegate(Component sender, object param)
		{
			this.OnGetItemDrop((ItemDropData)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.GetGunDrop, delegate(Component sender, object param)
		{
			this.GetGunDrop((int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReviveByGem, delegate(Component sender, object param)
		{
			this.OnReviveByGem();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReviveByAds, delegate(Component sender, object param)
		{
			this.OnReviveByAds();
		});
		EventDispatcher.Instance.RegisterListener(EventID.FinishStage, delegate(Component sender, object param)
		{
			this.OnFinishStage((float)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.RamboActiveSkill, delegate(Component sender, object param)
		{
			this.OnActiveSkill();
		});
		EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemHP, delegate(Component sender, object param)
		{
			this.OnUseSupportHP();
		});
		EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemGrenade, delegate(Component sender, object param)
		{
			this.OnUseSupportGrenade((int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemBooster, delegate(Component sender, object param)
		{
			this.OnUseSupportBooster((BoosterType)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.UseSupportItemBomb, delegate(Component sender, object param)
		{
			this.OnUseSupportBomb();
		});
		EventDispatcher.Instance.RegisterListener(EventID.CompleteWave, delegate(Component sender, object param)
		{
			this.OnCompleteSurvivalWave();
		});
		EventDispatcher.Instance.RegisterListener(EventID.UseBoosterHP, delegate(Component sender, object param)
		{
			float value = this.stats.MaxHp * 0.4f;
			this.RestoreHP(value, false);
		});
		this.InitWeapons();
		this.skeletonAnimation.AnimationState.Start += new Spine.AnimationState.TrackEntryDelegate(this.HandleAnimationStart);
		this.skeletonAnimation.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.HandleAnimationCompleted);
		this.skeletonAnimation.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleAnimationEvent);
	}

	private void Update()
	{
		if (this.isDead || base.IsDisableAction)
		{
			return;
		}

		// if (ShootHandler.instance)
		// {
		// 	if (xAttackDirection > 0.7f || xAttackDirection < -0.7f || yAttackDirection > 0.7f || yAttackDirection < -0.7f)
		// 	{
		// 		// if (ShootHandler.instance.pressed)
		// 		// {
		// 			UIController.Instance.Shoot(true);
		// 		// }
		// 		// else if(this.isFiring)
		// 		// {
		// 		// 	UIController.Instance.Shoot(false);
		// 		// }
		// 	}
		// 	else if (this.isFiring)
		// 	{
		// 		UIController.Instance.Shoot(false);
		// 	}
		// }

		// xAttackDirection = CnControls.CnInputManager.GetAxis("AttackHorizontal");
		// yAttackDirection = CnControls.CnInputManager.GetAxis("AttackVertical");

		this.isGrounded = Physics2D.Linecast(base.transform.position, this.groundCheck.position, this.layerMaskCheckObstacle);
		this.AvoidSlideOnInclinedPlane();
		this.ProcessInput();
		this.Attack();
		this.ActiveAim(!this.flagMeleeAttack && !this.flagThrowGrenade);
		this.UpdateDirection();
		this.TrackAimPoint();
	}

	private void FixedUpdate()
	{
		if (this.isDead || base.IsDisableAction)
		{
			return;
		}
		this.Move();
		this.Jump();
	}

	private void OnDisable()
	{
		this.rigid.angularDrag = 0f;
		this.rigid.velocity = Vector3.zero;
	}

	private void OnDestroy()
	{
		this.OnOutGamePlay();
	}

	private void OnApplicationPause(bool pause)
	{
		this.isFiring = false;
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Rambo/{0}/rambo_{0}_lv{1}", this.id, this.level);
		this.baseStats = Resources.Load<SO_BaseUnitStats>(path);
	}

	protected override void Move()
	{
		if (GameController.Instance.joystick.isTouch == false)
		{
			return;
		}
		if (this.enableMoving && (this.state == PlayerState.Move || this.state == PlayerState.Jump))
		{
			if (this.isUsingHorizontal && this.inputHorizontalValue * this.rigid.velocity.x < this.stats.MoveSpeed)
			{
				this.rigid.AddForce(this.moveForce * this.inputHorizontalValue, ForceMode2D.Impulse);
			}
			if (this.rigid.velocity.x > this.stats.MoveSpeed || this.rigid.velocity.x < -this.stats.MoveSpeed)
			{
				Vector2 velocity = this.rigid.velocity;
				velocity.x = ((this.inputHorizontalValue >= 0f) ? this.stats.MoveSpeed : (-this.stats.MoveSpeed));
				this.rigid.velocity = velocity;
			}
		}
	}

	protected override void Jump()
	{
		if (this.flagJump && this.enableJumping)
		{
			this.flagJump = false;
			this.rigid.AddForce(this.jumpForce, ForceMode2D.Impulse);
		}
	}

	protected override void Die()
	{
		base.Die();
		Singleton<CameraFollow>.Instance.slowMotion.Show(3.5f, null);
		Singleton<CameraFollow>.Instance.SetGrayScaleEffect(true);
		Singleton<GameController>.Instance.IsPaused = true;
		Singleton<GameController>.Instance.SetActiveAllUnits(false);
		this.lastDiePosition = base.transform.position;
		this.SwitchState(PlayerState.Die);
		EventDispatcher.Instance.PostEvent(EventID.PlayerDie);
	}

	protected override void Attack()
	{
		if (!this.enableAttack || this.flagThrowGrenade || this.flagMeleeAttack)
		{
			return;
		}
		if (this.isAutoFire && this.currentWeaponType == WeaponType.NormalGun)
		{
			this.isFiring = true;
		}
		if (this.isFiring)
		{
			float time = Time.time;
			if (this.nearbyEnemies.Count > 0 && time - this.lastTimeMeleeAttack > this.meleeAttackRate)
			{
				this.SwitchWeapon(WeaponType.MeleeWeapon);
				this.LookStraight();
			}
			float num = (this.currentWeaponType != WeaponType.MeleeWeapon) ? this.lastTimeShoot : this.lastTimeMeleeAttack;
			if (time - num > this.stats.AttackRate)
			{
				this.PlayAnimationAttack();
				if (this.currentWeaponType == WeaponType.MeleeWeapon)
				{
					this.lastTimeMeleeAttack = time;
				}
				else
				{
					this.lastTimeShoot = time;
					AttackData gunAttackData = this.GetGunAttackData();
					this.currentWeapon.Attack(gunAttackData);
					if (this.currentWeaponType == WeaponType.SpecialGun)
					{
						Singleton<UIController>.Instance.UpdateGunTypeText(this.isUsingNormalGun, this.specialGun.ammo);
					}
					else if (this.currentWeaponType == WeaponType.DropGun)
					{
						Singleton<UIController>.Instance.UpdateGunTypeText(this.isUsingNormalGun, this.dropGun.ammo);
					}
				}
			}
		}
	}

	protected override void UpdateDirection()
	{
		if (!this.enableFlipX)
		{
			return;
		}
		if (isUsingAim)
        {
			UpdaateSingleDirection(this.inputAttackHorizontalValue);

		}
		else if (isUsingMove)
        {

			UpdaateSingleDirection(this.inputHorizontalValue);
		}
	}
	private void UpdaateSingleDirection(float inputValue)
    {
		if (inputValue > 0.25f)
		{
			if (this.skeletonAnimation.Skeleton.flipX)
			{
				this.skeletonAnimation.Skeleton.flipX = false;
				Vector3 localEulerAngles = this.groupTransformFlip.localEulerAngles;
				localEulerAngles.y = 0f;
				this.groupTransformFlip.localEulerAngles = localEulerAngles;
			}
		}
		else if (inputValue < -0.25f && !this.skeletonAnimation.Skeleton.flipX)
		{
			this.skeletonAnimation.Skeleton.flipX = true;
			Vector3 localEulerAngles2 = this.groupTransformFlip.localEulerAngles;
			localEulerAngles2.y = 180f;
			this.groupTransformFlip.localEulerAngles = localEulerAngles2;
		}
	}


	protected override void EffectTakeDamage()
	{
		if (!this.isBlinkingEffect)
		{
			this.isBlinkingEffect = true;
			DOTween.To(new DOSetter<float>(this.ColorSetter), 1f, 0f, 0.1f).OnComplete(new TweenCallback(this.ChangeColorToDefault));
		}
		Singleton<UIController>.Instance.takeDamageScreen.Rewind();
		Singleton<UIController>.Instance.takeDamageScreen.Play();
	}

	protected override void AvoidSlideOnInclinedPlane()
	{
		if (this.isGrounded && !this.isUsingHorizontal && !base.IsDisableAction)
		{
			this.rigid.constraints = (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation);
		}
		else
		{
			this.rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
		}
	}

	public override AttackData GetCurentAttackData()
	{
		return new AttackData(this, this.stats.Damage, 0f, false, WeaponType.NormalGun, -1, null)
		{
			weapon = ((!this.isUsingNormalGun) ? WeaponType.SpecialGun : WeaponType.NormalGun)
		};
	}

	public override void ReloadStats()
	{
		this.stats.ResetToBaseStats();
		this.currentWeapon.ApplyOptions(this);
		this.CalculateBaseStatsIncrease();
		if (this.isRenew)
		{
			this.isRenew = false;
			this.stats.SetStats(this.stats.MaxHp, StatsType.Hp);
		}
		this.ApplyModifier();
	}

	protected virtual void CalculateBaseStatsIncrease()
	{
		if (GameData.mode == GameMode.Campaign)
		{
			GameData.isAutoCollectCoin = false;
			this.isDamageBuffed = false;
			this.isCriticalBuffed = false;
			this.isSpeedBuffed = false;
			for (int i = 0; i < GameData.selectingBoosters.Count; i++)
			{
				switch (GameData.selectingBoosters[i])
				{
				case BoosterType.Damage:
					this.isDamageBuffed = true;
					this.stats.AdjustStats(this.stats.Damage * 0.1f, StatsType.Damage);
					break;
				case BoosterType.CoinMagnet:
					GameData.isAutoCollectCoin = true;
					break;
				case BoosterType.Speed:
					this.isSpeedBuffed = true;
					this.stats.AdjustStats(this.baseStats.MoveSpeed * 0.1f, StatsType.MoveSpeed);
					break;
				case BoosterType.Critical:
					this.isCriticalBuffed = true;
					this.stats.AdjustStats(this.stats.CriticalRate * 0.1f, StatsType.CriticalRate);
					break;
				}
			}
		}
	}

	public override void AddModifier(ModifierData data)
	{
		this.listModifier.Add(data);
	}

	public override void RemoveModifier(ModifierData data)
	{
		int i = 0;
		int count = this.listModifier.Count;
		while (i < count)
		{
			if (this.listModifier[i].stats == data.stats && this.listModifier[i].type == data.type && this.listModifier[i].value == data.value)
			{
				this.listModifier.RemoveAt(i);
				break;
			}
			i++;
		}
	}

	public override void ApplyModifier()
	{
		for (int i = 0; i < this.listModifier.Count; i++)
		{
			ModifierData modifierData = this.listModifier[i];
			switch (modifierData.stats)
			{
			case StatsType.Damage:
			{
				float value = (modifierData.type != ModifierType.AddPoint) ? (this.stats.Damage * modifierData.value) : modifierData.value;
				this.stats.AdjustStats(value, StatsType.Damage);
				break;
			}
			case StatsType.MaxHp:
			{
				float value = (modifierData.type != ModifierType.AddPoint) ? (this.baseStats.HP * modifierData.value) : modifierData.value;
				this.stats.AdjustStats(value, StatsType.MaxHp);
				break;
			}
			case StatsType.MoveSpeed:
			{
				float value = (modifierData.type != ModifierType.AddPoint) ? (this.baseStats.MoveSpeed * modifierData.value) : modifierData.value;
				this.stats.AdjustStats(value, StatsType.MoveSpeed);
				break;
			}
			case StatsType.AttackTimePerSecond:
			{
				float value = (modifierData.type != ModifierType.AddPoint) ? (this.stats.AttackTimePerSecond * modifierData.value) : modifierData.value;
				this.stats.AdjustStats(value, StatsType.AttackTimePerSecond);
				break;
			}
			case StatsType.CriticalRate:
			{
				float value = (modifierData.type != ModifierType.AddPoint) ? (this.stats.CriticalRate * modifierData.value) : modifierData.value;
				this.stats.AdjustStats(value, StatsType.CriticalRate);
				break;
			}
			case StatsType.CriticalDamageBonus:
			{
				float value = (modifierData.type != ModifierType.AddPoint) ? (this.stats.CriticalDamageBonus * modifierData.value) : modifierData.value;
				this.stats.AdjustStats(value, StatsType.CriticalDamageBonus);
				break;
			}
			}
		}
	}

	public override void Renew()
	{
		base.Renew();
		this.isRenew = true;
		this.listModifier.Clear();
		this.isImmortal = false;
		this.flagJump = false;
		this.flagLookDown = false;
		this.flagLookUp = false;
		this.flagSpawnCrossBullet = false;
		this.flagMeleeAttack = false;
		this.flagThrowGrenade = false;
		this.countComboKill = 0;
		this.countComboKillBySpecialGun = 0;
		this.UpdateHealthBar(false);
		this.ActiveHealthBar(true);
		this.ActiveSoundLowHp(false);
		this.skeletonAnimation.ClearState();
		this.effectStun.SetActive(false);
		this.effectImmortal.SetActive(false);
		Singleton<CameraFollow>.Instance.SetGrayScaleEffect(false);
	}

	public override void TakeDamage(AttackData attackData)
	{
		if (this.isDead || attackData.attacker.isDead)
		{
			return;
		}
		if (this.isImmortal)
		{
			EffectController arg_5D_0 = EffectController.Instance;
			Vector2 position = base.BodyCenterPoint.position;
			Color yellow = Color.yellow;
			string content = "BLOCK";
			Transform groupText = Singleton<PoolingController>.Instance.groupText;
			arg_5D_0.SpawnTextTMP(position, yellow, content, 3.5f, groupText);
			return;
		}
		this.EffectTakeDamage();
		this.ShowTextDamageTaken(attackData.damage);
		this.stats.AdjustStats(-attackData.damage, StatsType.Hp);
		this.UpdateHealthBar(false);
		if (this.HpPercent < 0.2f)
		{
			this.ActiveSoundLowHp(true);
		}
		if (this.stats.HP <= 0f)
		{
			this.Die();
			if (GameData.mode == GameMode.Campaign && ((BaseEnemy)attackData.attacker).isFinalBoss)
			{
				EventLogger.LogEvent("N_KilledByFinalBoss", new object[]
				{
					string.Format("ID={0},{1}-{2}", ((BaseEnemy)attackData.attacker).id, GameData.currentStage.id, GameData.currentStage.difficulty)
				});
			}
		}
	}

	public override void TakeDamage(float damage)
	{
		if (!this.isDead)
		{
			if (this.isImmortal)
			{
				EffectController arg_4C_0 = EffectController.Instance;
				Vector2 position = base.BodyCenterPoint.position;
				Color yellow = Color.yellow;
				string content = "BLOCK";
				Transform groupText = Singleton<PoolingController>.Instance.groupText;
				arg_4C_0.SpawnTextTMP(position, yellow, content, 3.5f, groupText);
				return;
			}
			this.EffectTakeDamage();
			this.ShowTextDamageTaken(damage);
			this.stats.AdjustStats(-damage, StatsType.Hp);
			this.UpdateHealthBar(false);
			if (this.stats.HP <= 0f)
			{
				this.Die();
			}
		}
	}

	public override void UpdateHealthBar(bool isAutoHide = false)
	{
		Singleton<UIController>.Instance.UpdatePlayerHpBar(this.HpPercent);
		if (this.healthBar != null)
		{
			Vector2 size = this.healthBar.size;
			size.x = this.healthBarSizeX * this.HpPercent;
			this.healthBar.size = size;
		}
	}

	public override void GetStun(float duration)
	{
		this.isStun = true;
		this.effectStun.SetActive(true);
		this.StopMoving();
		this.rigid.angularDrag = 0f;
		this.rigid.velocity = Vector3.zero;
		this.StartDelayAction(delegate
		{
			this.isStun = false;
			this.effectStun.SetActive(false);
			this.rigid.angularDrag = 0.05f;
		}, duration);
	}

	public override void FallBackward(float duration)
	{
		this.isKnockBack = true;
		Vector2 force;
		force.x = ((!this.IsFacingRight) ? this.fallForceValue : (-this.fallForceValue));
		force.y = 0f;
		this.rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
		this.rigid.AddForce(force, ForceMode2D.Impulse);
		this.PlayAnimationFallbackward();
		if (this.isUsingNormalGun)
		{
			this.normalGun.gameObject.SetActive(false);
		}
		else if (this.dropGun)
		{
			this.dropGun.gameObject.SetActive(false);
		}
		else if (this.specialGun)
		{
			this.specialGun.gameObject.SetActive(false);
		}
		this.StartDelayAction(delegate
		{
			this.isKnockBack = false;
			this.PlayAnimationIdle();
			this.rigid.angularDrag = 0.05f;
			if (this.isUsingNormalGun)
			{
				this.normalGun.gameObject.SetActive(true);
			}
			else if (this.dropGun)
			{
				this.dropGun.gameObject.SetActive(true);
			}
			else if (this.specialGun)
			{
				this.specialGun.gameObject.SetActive(true);
			}
		}, duration);
	}

	private void ProcessInput()
	{
		if (!this.enableMoving && !this.enableAiming)
		{
			return;
		}
		if (base.IsDisableAction)
		{
			this.inputHorizontalValue = 0f;
			this.inputVerticalValue = 0f;
			this.inputAttackHorizontalValue = 0f;
			this.inputAttackVerticalValue = 0f;
			this.isUsingHorizontal = false;
			this.isUsingAimHorizontal = false;
			this.isUsingAimVertical = false;
			this.isUsingAim = false;
			this.isUsingMove = false;
			return;
		}
		this.inputHorizontalValue = CnInputManager.GetAxis("Horizontal");
		this.isUsingHorizontal = (this.inputHorizontalValue > 0.25f || this.inputHorizontalValue < -0.25f);
		this.inputVerticalValue = CnInputManager.GetAxis("Vertical");
		float num = Mathf.Abs(this.inputVerticalValue / this.inputHorizontalValue);
		this.isUsingVerticalUp = (this.inputVerticalValue > 0.25f && num > 1f);
		this.isUsingVerticalDown = (this.inputVerticalValue < -0.25f && num > 0.5f);
		this.isUsingVerticalCross = (this.inputVerticalValue > 0.25f && num > 0.5f);
		if (this.isUsingVerticalDown)
		{
			if (this.isGrounded)
			{
				if (GameController.Instance.joystick.isTouch == true)
				{
					this.SwitchState(PlayerState.Crouch);
				}
				else
				{
					this.SwitchState(PlayerState.Idle);
				}
			}
			else
			{
				this.LookDown();
			}
		}
		else
		{
			if (this.currentWeaponType != WeaponType.MeleeWeapon)
			{
				if (this.isUsingVerticalUp)
				{
					this.LookUp();
				}
				else if (this.isUsingVerticalCross)
				{
					this.LookCross();
				}
			}
			if (this.isGrounded)
			{
				if (this.isUsingHorizontal)
				{
					if (GameController.Instance.joystick.isTouch == true)
					{
						this.SwitchState(PlayerState.Move);
					}
					else
					{
						this.SwitchState(PlayerState.Idle);
					}
				}
				else
				{
					this.SwitchState(PlayerState.Idle);
				}
			}
		}
		if (!this.isGrounded)
		{
			this.SwitchState(PlayerState.Jump);
		}
		if (!this.isUsingVerticalUp && !this.isUsingVerticalCross && (this.isGrounded || (!this.isGrounded && !this.isUsingVerticalDown)))
		{
			this.LookStraight();
		}
		//Aim horrizontal
		this.inputAttackHorizontalValue = CnInputManager.GetAxis("AttackHorizontal");
		this.inputAttackVerticalValue = CnInputManager.GetAxis("AttackVertical");
		this.isUsingAimHorizontal = (this.inputAttackHorizontalValue > 0.25f || this.inputAttackHorizontalValue < -0.25f);
		this.isUsingAimVertical = (this.inputAttackVerticalValue > 0.25f || this.inputAttackVerticalValue < -0.25f);
		this.isUsingAim = isUsingAimHorizontal || isUsingAimVertical;
	}

	private void InitSkills()
	{
		this.skillTree = UnityEngine.Object.Instantiate<BaseSkillTree>(this.skillTreePrefab, base.transform);
		this.skillTree.Init(this.id);
	}

	private void InitWeapons()
	{
		BaseGun gunPrefab = GameResourcesUtils.GetGunPrefab(ProfileManager.UserProfile.gunNormalId);
		if (gunPrefab != null)
		{
			BaseGun baseGun = UnityEngine.Object.Instantiate<BaseGun>(gunPrefab, this.groupWeapon);
			BoneFollower boneFollower = baseGun.gameObject.AddComponent<BoneFollower>();
			if (boneFollower != null)
			{
				boneFollower.skeletonRenderer = this.skeletonRenderer;
				boneFollower.boneName = this.equipGunBoneName;
			}
			this.normalGun = baseGun;
			this.normalGun.gameObject.name = this.normalGun.equipmentName;
			int gunLevel = GameData.playerGuns.GetGunLevel(ProfileManager.UserProfile.gunNormalId);
			this.normalGun.Init(gunLevel);
		}
		BaseGun gunPrefab2 = GameResourcesUtils.GetGunPrefab(ProfileManager.UserProfile.gunSpecialId);
		if (gunPrefab2 != null)
		{
			BaseGun baseGun2 = UnityEngine.Object.Instantiate<BaseGun>(gunPrefab2, this.groupWeapon);
			BoneFollower boneFollower2 = baseGun2.gameObject.AddComponent<BoneFollower>();
			if (boneFollower2 != null)
			{
				boneFollower2.skeletonRenderer = this.skeletonRenderer;
				boneFollower2.boneName = this.equipGunBoneName;
			}
			this.specialGun = baseGun2;
			this.specialGun.gameObject.name = this.specialGun.equipmentName;
			int gunLevel2 = GameData.playerGuns.GetGunLevel(ProfileManager.UserProfile.gunSpecialId);
			this.specialGun.Init(gunLevel2);
			this.specialGun.gameObject.SetActive(false);
		}
		else
		{
			Singleton<UIController>.Instance.buttonSwitchGun.Disable();
		}
		BaseMeleeWeapon meleeWeaponPrefab = GameResourcesUtils.GetMeleeWeaponPrefab(ProfileManager.UserProfile.meleeWeaponId);
		if (meleeWeaponPrefab != null)
		{
			BaseMeleeWeapon baseMeleeWeapon = UnityEngine.Object.Instantiate<BaseMeleeWeapon>(meleeWeaponPrefab, this.groupWeapon);
			BoneFollower boneFollower3 = baseMeleeWeapon.gameObject.AddComponent<BoneFollower>();
			if (boneFollower3 != null)
			{
				boneFollower3.skeletonRenderer = this.skeletonRenderer;
				boneFollower3.boneName = this.equipMeleeWeaponBoneName;
			}
			this.meleeWeapon = baseMeleeWeapon;
			this.meleeWeapon.gameObject.name = this.meleeWeapon.equipmentName;
			int meleeWeaponLevel = GameData.playerMeleeWeapons.GetMeleeWeaponLevel(ProfileManager.UserProfile.meleeWeaponId);
			this.meleeWeapon.Init(meleeWeaponLevel);
			this.meleeWeapon.InitEffect(this.skeletonAnimation, this.effectWindBoneName);
			this.meleeWeapon.gameObject.SetActive(false);
			this.meleeAttackRate = 1f / this.meleeWeapon.baseStats.AttackTimePerSecond;
		}
		int num = ProfileManager.UserProfile.grenadeId;
		this.grenadePrefab = GameResourcesUtils.GetGrenadePrefab(num);
		this.numberOfGrenade = ((!GameData.playerGrenades.ContainsKey(num)) ? 0 : GameData.playerGrenades[num].quantity);
		Singleton<UIController>.Instance.UpdateGrenadeText(this.numberOfGrenade);
		if (this.numberOfGrenade <= 0)
		{
			Singleton<UIController>.Instance.ActiveButtonGrenade(false);
		}
		this.SwitchWeapon(WeaponType.NormalGun);
		if (GameData.isAutoFire)
		{
			Singleton<UIController>.Instance.ToggleAutoFire();
		}
	}

	protected virtual void SwitchWeapon(WeaponType weaponType)
	{
		if (this.currentWeaponType == weaponType)
		{
			return;
		}
		if (this.currentWeapon != null)
		{
			this.currentWeapon.gameObject.SetActive(false);
		}
		switch (weaponType)
		{
		case WeaponType.NormalGun:
			this.currentWeapon = this.normalGun;
			this.isUsingNormalGun = true;
			break;
		case WeaponType.SpecialGun:
			this.currentWeapon = this.specialGun;
			this.isUsingNormalGun = false;
			break;
		case WeaponType.MeleeWeapon:
			this.currentWeapon = this.meleeWeapon;
			break;
		case WeaponType.DropGun:
			this.currentWeapon = this.dropGun;
			this.isUsingNormalGun = false;
			break;
		}
		this.flagMeleeAttack = false;
		this.flagThrowGrenade = false;
		this.currentWeapon.gameObject.SetActive(true);
		this.currentWeaponType = weaponType;
		if (this.currentWeapon is BaseGun)
		{
			Singleton<UIController>.Instance.UpdateGunTypeText(this.isUsingNormalGun, ((BaseGun)this.currentWeapon).ammo);
			this.idle = ((((BaseGun)this.currentWeapon).gunType != GunType.Pistol) ? this.idleRifle : this.idlePistol);
			this.shoot = ((((BaseGun)this.currentWeapon).gunType != GunType.Pistol) ? this.shootRifle : this.shootPistol);
			this.aim = ((((BaseGun)this.currentWeapon).gunType != GunType.Pistol) ? this.aimRifle : this.aimPistol);
		}
		this.ReloadStats();
	}

	private void SwitchState(PlayerState newState)
	{
		if (this.state == newState)
		{
			return;
		}
		this.state = newState;
		switch (this.state)
		{
		case PlayerState.Idle:
			this.PlayAnimationIdle();
			this.StopMoving();
			break;
		case PlayerState.Move:
			if (this.enableMoving)
			{
				this.PlayAnimationMove();
			}
			break;
		case PlayerState.Crouch:
			if (this.enableCrouching)
			{
				this.PlayAnimationCrouch();
				this.StopMoving();
			}
			break;
		case PlayerState.Jump:
			if (this.enableJumping)
			{
				this.PlayAnimationJump();
			}
			break;
		case PlayerState.Die:
			this.PlayAnimationDie();
			break;
		}
		this.ResizeColliderBody(this.state == PlayerState.Crouch);
	}

	public virtual AttackData GetGunAttackData()
	{
		bool flag = UnityEngine.Random.Range(0f, 1f) <= this.stats.CriticalRate / 100f;
		float num = this.stats.Damage;
		if (flag)
		{
			float num2 = 1f + ((BaseGun)this.currentWeapon).baseStats.CriticalDamageBonus / 100f;
			num *= num2;
		}
		float damage = num;
		bool isCritical = flag;
		return new AttackData(this, damage, 0f, isCritical, WeaponType.NormalGun, -1, null)
		{
			weapon = ((!this.isUsingNormalGun) ? WeaponType.SpecialGun : WeaponType.NormalGun),
			weaponId = ((BaseGun)this.currentWeapon).id
		};
	}

	public virtual AttackData GetMeleeWeaponAttackData()
	{
		bool flag = UnityEngine.Random.Range(0f, 1f) <= this.stats.CriticalRate / 100f;
		float num = this.stats.Damage;
		if (flag)
		{
			float num2 = 1f + this.meleeWeapon.baseStats.CriticalDamageBonus / 100f;
			num *= num2;
		}
		float damage = num;
		bool isCritical = flag;
		return new AttackData(this, damage, 0f, isCritical, WeaponType.NormalGun, -1, null)
		{
			weapon = WeaponType.MeleeWeapon
		};
	}

	public virtual AttackData GetGrenadeAttackData(BaseGrenade grenade)
	{
		float damage = (!this.isDamageBuffed) ? grenade.baseStats.Damage : (grenade.baseStats.Damage * 1.15f);
		return new AttackData(this, damage, grenade.baseStats.Radius, false, WeaponType.Grenade, -1, null);
	}

	protected virtual float GetReviveImmortalDuration()
	{
		return this.defaultReviveImmortalTime;
	}

	private void Revive(float hpPercent, Vector2 position)
	{
		this.Renew();
		this.SwitchState(PlayerState.Idle);
		this.currentWeaponType = WeaponType.None;
		this.SwitchWeapon(WeaponType.NormalGun);
		float value = this.stats.MaxHp * hpPercent;
		this.stats.SetStats(value, StatsType.Hp);
		this.UpdateHealthBar(false);
		this.ActiveSoundLowHp(this.HpPercent < 0.2f);
		base.transform.position = position;
		this.isFiring = false;
		this.colliderBody.enabled = false;
		this.colliderBody.enabled = true;
		EffectController arg_C9_0 = EffectController.Instance;
		Vector2 position2 = base.BodyCenterPoint.position;
		Color green = Color.green;
		string content = string.Format("+{0}% HP", hpPercent * 100f);
		Transform groupText = Singleton<PoolingController>.Instance.groupText;
		arg_C9_0.SpawnTextTMP(position2, green, content, 3.5f, groupText);
		Singleton<GameController>.Instance.IsPaused = false;
		SoundManager.Instance.PlaySfx(this.soundRevive, 0f);
		GameData.isUseRevive = true;
		float reviveImmortalDuration = this.GetReviveImmortalDuration();
		base.StartCoroutine(this.CoroutineImmortal(reviveImmortalDuration));
	}

	private void LookUp()
	{
		if (this.flagLookDown)
		{
			this.flagLookDown = false;
			this.ActiveLookDown(false);
		}
		this.flagLookUp = true;
		this.flagLookCross = false;
		if (!this.flagSpawnCrossBullet)
		{
			this.flagSpawnCrossBullet = true;
			if (this.isFiring && this.currentWeapon is BaseGun && this.stats.AttackRate <= 0.2f)
			{
				((BaseGun)this.currentWeapon).ReleaseCrossBullets(this.GetGunAttackData(), this.spawnCrossBulletPoint, this.IsFacingRight);
			}
		}
	}

	private void LookCross()
	{
		if (this.flagLookDown)
		{
			this.flagLookDown = false;
			this.ActiveLookDown(false);
		}
		this.flagLookUp = false;
		this.flagLookCross = true;
	}

	private void LookDown()
	{
		if (!this.enableAiming || this.isOnVehicle)
		{
			return;
		}
		if (!this.flagLookDown)
		{
			this.flagLookDown = true;
			this.PlayAnimationLookDown();
		}
		this.flagLookUp = false;
		this.flagLookCross = false;
		this.flagSpawnCrossBullet = false;
	}

	private void LookStraight()
	{
		if (this.flagLookDown)
		{
			this.flagLookDown = false;
			this.ActiveLookDown(false);
		}
		this.flagLookUp = false;
		this.flagLookCross = false;
		this.flagSpawnCrossBullet = false;
	}

	private void TrackAimPoint()
	{
		if (this.flagLookDown || this.state == PlayerState.Crouch)
		{
			// this.aimBone.transform.position = this.straightAimPoint.position;
			return;
		}

		//****New Code
		// float angle = Mathf.Atan2(xAttackDirection, yAttackDirection) * Mathf.Rad2Deg;
		if (isUsingAim)
        {

			float angle = Mathf.Atan2(inputAttackHorizontalValue, inputAttackVerticalValue) * Mathf.Rad2Deg;
			aimParent.rotation = Quaternion.Euler(0, 0, -angle);
		}

		else if(isUsingMove)

		{
			float angle = Mathf.Atan2(inputHorizontalValue >= 0 ? 1 : -1, 0) * Mathf.Rad2Deg;
			aimParent.rotation = Quaternion.Euler(0, 0, -angle);

		}
		if (aimBone)
		{
			aimBone.transform.position = aimReference.transform.position;
		}

		//Define new player direction
		if (xAttackDirection != 0 || yAttackDirection != 0)
		{
			if (xAttackDirection > 0)
			{
				this.skeletonAnimation.Skeleton.flipX = false;
				Vector3 localEulerAngles = this.groupTransformFlip.localEulerAngles;
				localEulerAngles.y = 0f;
				this.groupTransformFlip.localEulerAngles = localEulerAngles;

			}
			else if (xAttackDirection < 0)
			{
				this.skeletonAnimation.Skeleton.flipX = true;
				Vector3 localEulerAngles2 = this.groupTransformFlip.localEulerAngles;
				localEulerAngles2.y = 180f;
				this.groupTransformFlip.localEulerAngles = localEulerAngles2;
			}
		}
		//****New Code

		//****Original Code
		// if (this.flagLookUp)
		// {
		// 	this.aimBone.transform.position = this.upAimPoint.position;
		// }
		// else if (this.flagLookDown)
		// {
		// 	this.aimBone.transform.position = this.downAimPoint.position;
		// }
		// else if (this.flagLookCross)
		// {
		// 	this.aimBone.transform.position = this.crossAimPoint.position;
		// }
		// else if (this.state == PlayerState.Crouch)
		// {
		// 	this.aimBone.transform.position = this.crouchAimPoint.position;
		// }
		// else
		// {
		// 	this.aimBone.transform.position = this.straightAimPoint.position;
		// }
		//****Original Code
	}

	protected virtual void ReleaseGrenade()
	{
		BaseGrenade baseGrenade = this.grenadePrefab.Create();
		baseGrenade.Init(this.grenadeLevel);
		AttackData grenadeAttackData = this.GetGrenadeAttackData(baseGrenade);
		Vector2 throwForce = this.throwGrenadeDirection * this.throwForceValue;
		throwForce.x = ((!this.IsFacingRight) ? (-throwForce.x) : throwForce.x);
		baseGrenade.Active(grenadeAttackData, this.throwGrenadePoint.position, throwForce, Singleton<PoolingController>.Instance.groupGrenade);
		this.numberOfGrenade--;
		if (this.numberOfGrenade <= 0)
		{
			Singleton<UIController>.Instance.ActiveButtonGrenade(false);
		}
		else
		{
			base.StartCoroutine(this.CoroutineCooldownGrenade(baseGrenade.baseStats.Cooldown));
		}
		Singleton<UIController>.Instance.UpdateGrenadeText(this.numberOfGrenade);
		GameData.playerGrenades.Consume(ProfileManager.UserProfile.grenadeId, 1);
	}

	private void DealMeleeWeaponDamage()
	{
		for (int i = 0; i < this.meleeWeaponVictims.Count; i++)
		{
			BaseUnit baseUnit = this.meleeWeaponVictims[i];
			if (baseUnit.CompareTag("Enemy"))
			{
				AttackData meleeWeaponAttackData = this.GetMeleeWeaponAttackData();
				baseUnit.TakeDamage(meleeWeaponAttackData);
			}
		}
	}

	protected virtual void RestoreHP(float value, bool isFromItemDrop)
	{
		float num = value;
		if (isFromItemDrop)
		{
			num = (this.stats.MaxHp - this.stats.HP) * 0.5f;
			if (num < 500f)
			{
				num = 500f;
			}
		}
		this.stats.AdjustStats(num, StatsType.Hp);
		if (this.stats.HP > this.stats.MaxHp)
		{
			this.stats.SetStats(this.stats.MaxHp, StatsType.Hp);
		}
		this.UpdateHealthBar(false);
		this.ActiveSoundLowHp(this.HpPercent < 0.2f);
		this.effectRestoreHP.Play();
		int num2 = Mathf.RoundToInt(num * 10f);
		EffectController arg_ED_0 = EffectController.Instance;
		Vector2 position = base.BodyCenterPoint.position;
		Color green = Color.green;
		string content = string.Format("+{0} HP", num2);
		Transform groupText = Singleton<PoolingController>.Instance.groupText;
		arg_ED_0.SpawnTextTMP(position, green, content, 3.5f, groupText);
		SoundManager.Instance.PlaySfx(this.soundRevive, 0f);
	}

	private void RestoreFullHp()
	{
		float value = this.stats.MaxHp - this.stats.HP;
		this.RestoreHP(value, false);
	}

	private IEnumerator CoroutineImmortal(float duration)
	{
		Rambo._CoroutineImmortal_c__Iterator0 _CoroutineImmortal_c__Iterator = new Rambo._CoroutineImmortal_c__Iterator0();
		_CoroutineImmortal_c__Iterator.duration = duration;
		_CoroutineImmortal_c__Iterator._this = this;
		return _CoroutineImmortal_c__Iterator;
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

	private void TrackIdleTime()
	{
		if (GameData.mode != GameMode.Campaign)
		{
			return;
		}
		if (this.state != PlayerState.Idle || !this.isFiring)
		{
		}
	}

	private void ResizeColliderBody(bool isCrouch)
	{
		if (isCrouch)
		{
			Vector2 vector = this.colliderBody.offset;
			vector.y = 0.4f;
			this.colliderBody.offset = vector;
			vector = this.colliderBody.size;
			vector.y = 0.6f;
			this.colliderBody.size = vector;
		}
		else
		{
			Vector2 vector2 = this.colliderBody.offset;
			vector2.y = 0.63f;
			this.colliderBody.offset = vector2;
			vector2 = this.colliderBody.size;
			vector2.y = 1.09f;
			this.colliderBody.size = vector2;
		}
	}

	protected virtual IEnumerator CoroutineCooldownGrenade(float cooldown)
	{
		Rambo._CoroutineCooldownGrenade_c__Iterator1 _CoroutineCooldownGrenade_c__Iterator = new Rambo._CoroutineCooldownGrenade_c__Iterator1();
		_CoroutineCooldownGrenade_c__Iterator.cooldown = cooldown;
		_CoroutineCooldownGrenade_c__Iterator._this = this;
		return _CoroutineCooldownGrenade_c__Iterator;
	}

	protected void ActiveSoundLowHp(bool isActive)
	{
		if (isActive)
		{
			if (this.audioSource.clip == null)
			{
				this.audioSource.loop = true;
				this.audioSource.clip = this.soundLowHp;
				this.audioSource.Play();
			}
		}
		else
		{
			this.audioSource.Stop();
			this.audioSource.clip = null;
		}
		Singleton<UIController>.Instance.alarmRedScreen.SetActive(isActive);
	}

	private void GetGunDrop(int id)
	{
		if (GameData.staticGunData.ContainsKey(id))
		{
			if (this.dropGun == null || this.dropGun.id != id)
			{
				BaseGun gunPrefab = GameResourcesUtils.GetGunPrefab(id);
				BaseGun baseGun = UnityEngine.Object.Instantiate<BaseGun>(gunPrefab, this.groupWeapon);
				BoneFollower boneFollower = baseGun.gameObject.AddComponent<BoneFollower>();
				if (boneFollower != null)
				{
					boneFollower.skeletonRenderer = this.skeletonRenderer;
					boneFollower.boneName = this.equipGunBoneName;
				}
				if (this.dropGun)
				{
					UnityEngine.Object.Destroy(this.dropGun.gameObject);
					this.dropGun = null;
					this.currentWeaponType = WeaponType.None;
				}
				this.dropGun = baseGun;
				this.dropGun.gameObject.name = this.dropGun.equipmentName;
				this.dropGun.Init(1);
				SO_GunStats baseStats = GameData.staticGunData.GetBaseStats(id, 1);
				this.dropGun.ammo = baseStats.Ammo;
				this.dropGun.gameObject.SetActive(false);
				this.SwitchWeapon(WeaponType.DropGun);
			}
			else
			{
				this.dropGun.ammo = this.dropGun.baseStats.Ammo;
			}
			Singleton<UIController>.Instance.buttonSwitchGun.Enable();
		}
	}

	private void UnequipGunDrop()
	{
		if (this.dropGun)
		{
			this.dropGun.gameObject.SetActive(false);
			this.dropGun = null;
			if (this.specialGun == null)
			{
				Singleton<UIController>.Instance.buttonSwitchGun.Disable();
			}
		}
		this.SwitchWeapon(WeaponType.NormalGun);
	}

	public void OnEnemyEnterNearby(BaseUnit unit)
	{
		if (!this.nearbyEnemies.Contains(unit))
		{
			this.nearbyEnemies.Add(unit);
		}
	}

	public void OnEnemyExitNearby(BaseUnit unit)
	{
		if (this.nearbyEnemies.Contains(unit))
		{
			this.nearbyEnemies.Remove(unit);
		}
	}

	private void TryJump()
	{
		if (this.isGrounded && !this.isOnVehicle)
		{
			Vector2 velocity = this.rigid.velocity;
			velocity.y = 0f;
			this.rigid.velocity = velocity;
			this.flagJump = true;
		}
	}

	private void TryAttack(bool isAttack)
	{
		this.isFiring = isAttack;
		if (!isAttack && this.state == PlayerState.Idle)
		{
			this.PlayAnimationIdle();
		}
	}

	private void TryThrowGrenade()
	{
		if (!this.isCooldownGrenade && this.numberOfGrenade > 0 && !this.flagThrowGrenade)
		{
			this.flagThrowGrenade = true;
			this.flagMeleeAttack = false;
			this.PlayAnimationThrow();
			EventDispatcher.Instance.PostEvent(EventID.UseGrenade);
			SoundManager.Instance.PlaySfx("sfx_throw_grenade", 0f);
		}
	}

	private void TrySwitchGun()
	{
		if (this.isUsingNormalGun)
		{
			if (this.dropGun)
			{
				this.SwitchWeapon(WeaponType.DropGun);
			}
			else if (this.specialGun)
			{
				this.SwitchWeapon(WeaponType.SpecialGun);
			}
			this.isFiring = false;
		}
		else
		{
			this.SwitchWeapon(WeaponType.NormalGun);
		}
		SoundManager.Instance.PlaySfx(this.soundChangeWeapon, 0f);
	}

	private void OnToggleAutoFire()
	{
		this.isAutoFire = !this.isAutoFire;
		if (!this.isAutoFire)
		{
			this.isFiring = false;
		}
		GameData.isAutoFire = this.isAutoFire;
	}

	private void OnSpecialGunOutOfAmmo()
	{
		if (this.currentWeaponType == WeaponType.SpecialGun)
		{
			this.SwitchWeapon(WeaponType.NormalGun);
		}
		else if (this.currentWeaponType == WeaponType.DropGun)
		{
			this.UnequipGunDrop();
		}
	}

	private void OnKillUnit(UnitDieData data)
	{
		BaseEnemy component = data.unit.GetComponent<BaseEnemy>();
		if (component.isMiniBoss || component.isFinalBoss || data.attackData == null)
		{
			return;
		}
		this.countComboKill++;
		EventDispatcher.Instance.PostEvent(EventID.GetComboKill, this.countComboKill);
		if (data.attackData.weapon == WeaponType.SpecialGun)
		{
			this.countComboKillBySpecialGun++;
			EventDispatcher.Instance.PostEvent(EventID.GetComboKillBySpecialGun, this.countComboKillBySpecialGun);
			EventDispatcher.Instance.PostEvent(EventID.KillEnemyBySpecialGun);
		}
		else
		{
			this.countComboKillBySpecialGun = 0;
		}
		if (data.attackData.weapon == WeaponType.MeleeWeapon)
		{
			if (data.attackData.weaponId == 600)
			{
				this.PlaySound(this.soundKnifeKill);
			}
			EventDispatcher.Instance.PostEvent(EventID.KillEnemyByKnife);
			Vector2 position = component.transform.position;
			position.y += 2f;
			this.meleeWeapon.SpawnEffectText(position, null);
		}
		if (data.attackData.weapon == WeaponType.Grenade)
		{
			this.PlaySound(this.soundGrenadeKill);
			EventDispatcher.Instance.PostEvent(EventID.KillEnemyByGrenade);
		}
	}

	private void OnTimeOutComboKill()
	{
		this.countComboKill = 0;
		this.countComboKillBySpecialGun = 0;
	}

	private void OnGetItemDrop(ItemDropData data)
	{
		switch (data.type)
		{
		case ItemDropType.Health:
			this.RestoreHP(data.value, true);
			break;
		case ItemDropType.Coin:
		{
			EffectController arg_9C_0 = EffectController.Instance;
			Vector2 position = base.BodyCenterPoint.position;
			Color yellow = Color.yellow;
			string content = string.Format("+{0}", data.value);
			Transform groupText = Singleton<PoolingController>.Instance.groupText;
			arg_9C_0.SpawnTextTMP(position, yellow, content, 3.5f, groupText);
			break;
		}
		case ItemDropType.GunSpread:
			EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, 100);
			break;
		case ItemDropType.GunRocketChaser:
			EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, 101);
			break;
		case ItemDropType.GunFamas:
			EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, 102);
			break;
		case ItemDropType.GunLaser:
			EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, 103);
			break;
		case ItemDropType.GunSplit:
			EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, 104);
			break;
		case ItemDropType.GunFireBall:
			EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, 105);
			break;
		case ItemDropType.GunTesla:
			EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, 106);
			break;
		case ItemDropType.GunKamePower:
			EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, 107);
			break;
		case ItemDropType.GunFlame:
			EventDispatcher.Instance.PostEvent(EventID.GetGunDrop, 108);
			break;
		}
		SoundManager.Instance.PlaySfx(this.soundGetItemDrop, 0f);
	}

	private void OnReviveByGem()
	{
		this.countRevive++;
		this.Revive(0.5f, this.lastDiePosition);
		Singleton<CameraFollow>.Instance.ResetCameraToPlayer();
	}

	private void OnReviveByAds()
	{
		this.countRevive++;
		this.Revive(0.3f, this.lastDiePosition);
		Singleton<CameraFollow>.Instance.ResetCameraToPlayer();
	}

	protected virtual void OnFinishStage(float delayEndGame)
	{
		this.isImmortal = true;
		this.StartDelayAction(delegate
		{
			SoundManager.Instance.PlaySfx("sfx_voice_victory", 0f);
			this.PlayAnimationVictory();
		}, delayEndGame);
	}

	private void OnOutGamePlay()
	{
		if (this.specialGun != null)
		{
			GameData.playerGuns.SetGunAmmo(ProfileManager.UserProfile.gunSpecialId, this.specialGun.ammo);
		}
	}

	protected virtual void OnCompleteSurvivalWave()
	{
		if (this.isDamageBuffed)
		{
			this.isDamageBuffed = false;
			this.RemoveModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, 0.15f));
		}
		if (this.isCriticalBuffed)
		{
			this.isCriticalBuffed = false;
			this.RemoveModifier(new ModifierData(StatsType.CriticalRate, ModifierType.AddPercentBase, 0.1f));
		}
		if (this.isSpeedBuffed)
		{
			this.isSpeedBuffed = false;
			this.RemoveModifier(new ModifierData(StatsType.MoveSpeed, ModifierType.AddPercentBase, 0.2f));
		}
		this.ReloadStats();
		GameData.survivalUsingBooster = BoosterType.None;
		Singleton<UIController>.Instance.ActiveBoosters();
	}

	protected virtual void OnActiveSkill()
	{
		if (this.skillTree.activeSkill)
		{
			this.skillTree.activeSkill.Excute();
		}
	}

	protected virtual void OnUseSupportHP()
	{
		this.RestoreFullHp();
	}

	protected virtual void OnUseSupportGrenade(int quantity)
	{
		if (this.numberOfGrenade <= 0 && quantity > 0)
		{
			Singleton<UIController>.Instance.ActiveButtonGrenade(true);
		}
		this.numberOfGrenade += quantity;
		Singleton<UIController>.Instance.UpdateGrenadeText(this.numberOfGrenade);
	}

	protected virtual void OnUseSupportBomb()
	{
	}

	protected virtual void OnUseSupportBooster(BoosterType type)
	{
		GameData.survivalUsingBooster = type;
		Singleton<UIController>.Instance.ActiveBoosters();
		this.isDamageBuffed = false;
		this.isCriticalBuffed = false;
		this.isSpeedBuffed = false;
		if (GameData.mode == GameMode.Survival)
		{
			if (GameData.survivalUsingBooster == BoosterType.Damage)
			{
				this.isDamageBuffed = true;
				this.AddModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, 0.15f));
			}
			else if (GameData.survivalUsingBooster == BoosterType.Critical)
			{
				this.isCriticalBuffed = true;
				this.AddModifier(new ModifierData(StatsType.CriticalRate, ModifierType.AddPercentBase, 0.1f));
			}
			else if (GameData.survivalUsingBooster == BoosterType.Speed)
			{
				this.isSpeedBuffed = true;
				this.AddModifier(new ModifierData(StatsType.Damage, ModifierType.AddPercentBase, 0.2f));
			}
		}
		this.ReloadStats();
	}

	protected override void HandleAnimationStart(TrackEntry entry)
	{
		if (string.Compare(entry.animation.name, this.meleeAttack) == 0)
		{
			this.meleeWeaponVictims.Clear();
			for (int i = 0; i < this.nearbyEnemies.Count; i++)
			{
				this.meleeWeaponVictims.Add(this.nearbyEnemies[i]);
			}
			this.currentWeapon.PlaySoundAttack();
			this.meleeWeapon.ActiveEffect(true);
		}
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventMeleeAttack) == 0)
		{
			this.DealMeleeWeaponDamage();
		}
		if (string.Compare(e.Data.Name, this.eventThrowGrenade) == 0)
		{
			this.ReleaseGrenade();
		}
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		if (string.Compare(entry.animation.name, this.meleeAttack) == 0)
		{
			this.flagMeleeAttack = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
			this.meleeWeapon.ActiveEffect(false);
			if (this.isUsingNormalGun)
			{
				this.SwitchWeapon(WeaponType.NormalGun);
			}
			else if (this.dropGun)
			{
				this.SwitchWeapon(WeaponType.DropGun);
			}
			else
			{
				this.SwitchWeapon(WeaponType.SpecialGun);
			}
		}
		if (string.Compare(entry.animation.name, this.throwGrenade) == 0)
		{
			this.flagThrowGrenade = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
		}
		if (string.Compare(entry.animation.name, this.victory) == 0 && !this.flagAnimVictory)
		{
			this.flagAnimVictory = true;
			if (this.countRevive <= 0)
			{
				EventDispatcher.Instance.PostEvent(EventID.CompleteStageWithoutReviving);
			}
			Singleton<UIController>.Instance.alarmRedScreen.SetActive(false);
			this.StartDelayAction(delegate
			{
				EventDispatcher.Instance.PostEvent(EventID.GameEnd, true);
			}, 0.5f);
		}
		if (string.Compare(entry.animation.name, this.fallBackward) == 0)
		{
			this.StopMoving();
			base.Rigid.angularDrag = 0f;
		}
		if (this.dieAnimationNames.Contains(entry.animation.name))
		{
			if (GameData.mode == GameMode.Campaign)
			{
				if (this.countRevive > 0)
				{
					EventDispatcher.Instance.PostEvent(EventID.GameEnd, false);
				}
				else
				{
					float num = base.transform.position.x - Singleton<GameController>.Instance.CampaignMap.playerSpawnPoint.position.x;
					float num2 = Singleton<GameController>.Instance.CampaignMap.mapEndPoint.position.x - Singleton<GameController>.Instance.CampaignMap.playerSpawnPoint.position.x;
					float curProgress = Mathf.Clamp01(num / num2);
					Singleton<UIController>.Instance.hudSaveMe.Open(curProgress);
				}
			}
			else if (GameData.mode == GameMode.Survival)
			{
				EventDispatcher.Instance.PostEvent(EventID.GameEnd, false);
			}
		}
	}

	private void ActiveAim(bool isActive)
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
		}
	}

	private void ActiveLookDown(bool isActive)
	{
		if (isActive)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(3, this.lookDown, false);
		}
		else
		{
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(3, 0f);
		}
	}

	public void PlayAnimationIdle()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.idle, true);
	}

	public void PlayAnimationIdleInJet()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.idleInJet, true);
	}

	public void PlayAnimationParachute()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.parachute, true);
	}

	private void PlayAnimationCrouch()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.crouch, false);
	}

	private void PlayAnimationMove()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.move, true);
	}

	private void PlayAnimationJump()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.jump, true);
	}

	private void PlayAnimationAttack()
	{
		if (this.currentWeaponType == WeaponType.MeleeWeapon)
		{
			if (!this.flagMeleeAttack)
			{
				this.flagMeleeAttack = true;
				MeleeWeaponType type = ((BaseMeleeWeapon)this.currentWeapon).type;
				if (type != MeleeWeaponType.Knife)
				{
					if (type != MeleeWeaponType.Pan)
					{
						if (type == MeleeWeaponType.Guitar)
						{
							this.meleeAttack = this.guitar;
						}
					}
					else
					{
						this.meleeAttack = this.pan;
					}
				}
				else
				{
					this.meleeAttack = this.knife;
				}
				this.skeletonAnimation.AnimationState.SetAnimation(1, this.meleeAttack, false);
			}
		}
		else if (this.state == PlayerState.Idle)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.shoot, false);
			this.skeletonAnimation.AnimationState.AddEmptyAnimation(0, 0.5f, 0.1f);
		}
		else if (!this.flagThrowGrenade)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(1, this.shoot, false);
			this.skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
		}
	}

	private void PlayAnimationLookDown()
	{
		this.ActiveLookDown(true);
	}

	private void PlayAnimationThrow()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(1, this.throwGrenade, false).TimeScale = 1.5f;
	}

	private void PlayAnimationDie()
	{
		int index = UnityEngine.Random.Range(0, this.dieAnimationNames.Count);
		string animationName = this.dieAnimationNames[index];
		this.skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
	}

	private void PlayAnimationFallbackward()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.fallBackward, false);
	}

	protected void PlayAnimationVictory()
	{
		this.skeletonAnimation.ClearState();
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.victory, true);
		base.enabled = false;
	}
}
