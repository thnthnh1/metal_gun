using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossMegatank : BaseEnemy
{
    [Header("BOSS MEGATANK PROPERTIES")]
    public BulletPlasma bulletPlasmaPrefab;

    public RocketBossMegatank rocketPrefab;

    public BossMegatankColliderWheel colWheel;

    public BaseMuzzle muzzlePlasmaPrefab;

    public BaseMuzzle dustMuzzlePrefab;

    public Transform aimPlasma;

    public Transform firePointPlasma;

    public Transform muzzlePointPlasma;

    public Transform defaultAimGunPosition;

    public Transform aimCannon;

    public Transform firePointCannon;

    public Transform muzzlePointCannon;

    public Vector2 cannonFireDirection;

    [SpineAnimation("", "", true, false)]
    public string cannonShoot;

    [SpineAnimation("", "", true, false)]
    public string gunShoot;

    [SpineAnimation("", "", true, false)]
    public string preGore;

    [SpineAnimation("", "", true, false)]
    public string gore1;

    [SpineAnimation("", "", true, false)]
    public string gore2;

    [SpineAnimation("", "", true, false)]
    public string goreToIdle;

    public AudioClip soundPregore;

    public AudioClip soundGore;

    public AudioClip soundPlasma;

    public AudioClip soundCannon;

    public float Angle = 50f;
    private BaseMuzzle muzzlePlasma;

    private BaseMuzzle dustMuzzle;

    [SerializeField]
    private bool isMovingToBase = true;

    [SerializeField]
    private bool flagCannon = true;

    [SerializeField]
    private bool flagPlasma;

    private Vector2 destinationGore;

    private float countGunPlasma;

    private int countGoreTime;

    private static TweenCallback __f__am_cache0;

    protected override void Awake()
    {
        base.Awake();
        EventDispatcher.Instance.RegisterListener(EventID.RocketMegatankHitPlayer, delegate (Component sender, object param)
        {
            this.OnCannonHitPlayer();
        });
        EventDispatcher.Instance.RegisterListener(EventID.RocketMegatankMissPlayer, delegate (Component sender, object param)
        {
            this.OnCannonMissPlayer();
        });
    }

    protected override void Update()
    {
        if (!this.isDead)
        {
            this.MoveToBase();
            if (this.isReadyAttack)
            {
                this.Attack();
            }
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format("Scriptable Object/Boss/Boss Megatank/boss_megatank_lv{0}", this.level);
        this.baseStats = Resources.Load<SO_BossMegatankStats>(path);
    }

    protected override void Attack()
    {
        if (this.flagCannon)
        {
            this.UpdateDirection();
            this.flagCannon = false;
            this.skeletonAnimation.AnimationState.SetAnimation(1, this.cannonShoot, false);
        }
        else if (this.flagPlasma)
        {
            this.UpdateDirection();
            this.UpdateAimPlasma();
            if (this.countGunPlasma < ((SO_BossMegatankStats)this.baseStats).PlasmaDuration)
            {
                this.countGunPlasma += Time.deltaTime;
                float time = Time.time;
                float num = (this.HpPercent <= 0.5f) ? (1f / ((SO_BossMegatankStats)this.baseStats).RageAttackTimeSecond) : this.stats.AttackRate;
                if (time - this.lastTimeAttack > num)
                {
                    this.lastTimeAttack = time;
                    this.PlayAnimationGunPlasma();
                    this.ShootPlasma();
                }

            }
            else
            {
                this.ActiveCannon();
            }
        }
    }

    protected override void UpdateDirection()
    {
        if (this.isMovingToBase)
        {
            this.skeletonAnimation.Skeleton.flipX = (this.basePosition.x < base.transform.position.x);
        }
        else if (this.target != null)
        {
            this.skeletonAnimation.Skeleton.flipX = (this.target.transform.position.x < base.transform.position.x);
        }
        this.UpdateTransformPoints();
        //4 lines added by hardik
        // UnityEngine.Debug.Log("updating direction");
        Vector3 vectorToTarget = (this.target.transform.position) - this.firePointPlasma.position;
        if (!this.IsFacingRight)
        {
            Angle = Angle * 1;
        }
        else
        {
            Angle = Angle * -1;
        }
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        Quaternion q = Quaternion.AngleAxis(angle + Angle, Vector3.forward);
        this.firePointPlasma.rotation = q;
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
        if (string.Compare(entry.animation.name, this.cannonShoot) == 0)
        {
            this.ShootCannon(this.firePointCannon, this.target.transform);
            this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
        }
        if (string.Compare(entry.animation.name, this.preGore) == 0)
        {
            SoundManager.Instance.PlaySfx(this.soundGore, 0f);
            this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
            this.skeletonAnimation.AnimationState.SetAnimation(1, this.gore1, true);
            this.skeletonAnimation.AnimationState.SetAnimation(2, this.gore2, true);
            this.colWheel.gameObject.SetActive(true);
            this.destinationGore = this.target.transform.position;
            this.destinationGore.y = base.transform.position.y;
            Singleton<CameraFollow>.Instance.AddShake(0.15f, 1f);
            base.transform.DOMoveX(this.destinationGore.x, 1f, false).OnComplete(delegate
            {
                this.StopMoving();
                this.UpdateDirection();
                base.transform.position = this.destinationGore;
                this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
                this.skeletonAnimation.AnimationState.SetEmptyAnimation(2, 0f);
                this.colWheel.gameObject.SetActive(false);
                this.PlayAnimationIdle();
                if (this.HpPercent < 0.5f && this.countGoreTime < 2)
                {
                    this.countGoreTime++;
                    this.ActiveGore();
                }
                else
                {
                    this.countGoreTime = 0;
                    this.ActivePlasma();
                }
            });
        }
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
    }

    public override void Renew()
    {
        this.isDead = false;
        this.LoadScriptableObject();
        this.stats.Init(this.baseStats);
        this.isFinalBoss = true;
        this.canMove = true;
        this.isEffectMeleeWeapon = false;
        this.isReadyAttack = false;
        this.target = null;
        base.transform.parent = null;
        this.UpdateTransformPoints();
        this.UpdateHealthBar(false);
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        Singleton<UIController>.Instance.hudBoss.SetIconBoss(this.id);
        Singleton<UIController>.Instance.hudBoss.UpdateHP(this.HpPercent);
    }

    private void MoveToBase()
    {
        if (this.isMovingToBase)
        {
            this.isMovingToBase = false;
            this.PlayAnimationMove();
            base.transform.DOMove(this.basePosition, 3f, false).OnComplete(delegate
            {
                EventDispatcher.Instance.PostEvent(EventID.FinalBossStart);
                this.PrepareAttack();
            }).OnStart(delegate
            {
                Singleton<CameraFollow>.Instance.AddShake(0.3f, 3f);
            });
        }
    }

    private void UpdateAimPlasma()
    {
        if (this.target)
        {
            this.aimPlasma.position = this.target.BodyCenterPoint.position;
        }
    }

    private void PrepareAttack()
    {
        this.PlayAnimationIdle();
        this.StopMoving();
        this.ActiveCannon();
    }

    private void ShootPlasma()
    {
        BulletPlasma bulletPlasma = Singleton<PoolingController>.Instance.poolBulletPlasma.New();

        if (bulletPlasma == null)
        {
            bulletPlasma = UnityEngine.Object.Instantiate<BulletPlasma>(this.bulletPlasmaPrefab);
        }
        float damage = (this.HpPercent <= 0.5f) ? ((SO_BossMegatankStats)this.baseStats).RageGunDamage : this.baseStats.Damage;
        float moveSpeed = (this.HpPercent <= 0.5f) ? ((SO_BossMegatankStats)this.baseStats).RageBulletSpeed : this.baseStats.BulletSpeed;
        AttackData attackData = new AttackData(this, damage, 0f, false, WeaponType.NormalGun, -1, null);
        bulletPlasma.Active(attackData, this.firePointPlasma, moveSpeed, Singleton<PoolingController>.Instance.groupBullet);
        if (this.muzzlePlasma == null)
        {
            this.muzzlePlasma = UnityEngine.Object.Instantiate<BaseMuzzle>(this.muzzlePlasmaPrefab, this.muzzlePointPlasma.position, this.muzzlePointPlasma.rotation, this.muzzlePointPlasma.parent);
        }
        this.muzzlePlasma.Active();
        SoundManager.Instance.PlaySfx(this.soundPlasma, 0f);
    }

    private void ShootCannon(Transform startPoint, Transform endPoint)
    {
        RocketBossMegatank rocketBossMegatank = Singleton<PoolingController>.Instance.poolRocketBossMegatank.New();
        if (rocketBossMegatank == null)
        {
            rocketBossMegatank = UnityEngine.Object.Instantiate<RocketBossMegatank>(this.rocketPrefab);
        }
        float damage = (this.HpPercent <= 0.5f) ? ((SO_BossMegatankStats)this.baseStats).RageRocketDamage : ((SO_BossMegatankStats)this.baseStats).RocketDamage;
        float rocketRadius = ((SO_BossMegatankStats)this.baseStats).RocketRadius;
        AttackData attackData = new AttackData(this, damage, rocketRadius, false, WeaponType.NormalGun, -1, null);
        Vector2 throwDirection = this.cannonFireDirection;
        throwDirection.x = ((!this.IsFacingRight) ? (-throwDirection.x) : throwDirection.x);
        rocketBossMegatank.Active(attackData, startPoint, endPoint, throwDirection);
        if (this.dustMuzzle == null)
        {
            this.dustMuzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.dustMuzzlePrefab, this.muzzlePointCannon.position, this.muzzlePointCannon.rotation, this.muzzlePointCannon.parent);
        }
        this.dustMuzzle.Active();
        SoundManager.Instance.PlaySfx(this.soundCannon, 0f);
    }

    private void PlayAnimationGunPlasma()
    {
        TrackEntry trackEntry = this.skeletonAnimation.AnimationState.SetAnimation(1, this.gunShoot, false);
        trackEntry.AttachmentThreshold = 1f;
        trackEntry.MixDuration = 0f;
        TrackEntry trackEntry2 = this.skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
        trackEntry2.AttachmentThreshold = 1f;
    }

    private void TriggerAnimationPreGore()
    {
        if (string.Compare(this.skeletonAnimation.AnimationState.GetCurrent(0).animation.name, this.preGore) != 0)
        {
            this.skeletonAnimation.AnimationState.SetAnimation(0, this.preGore, false);
            this.skeletonAnimation.AnimationState.SetAnimation(1, this.gore2, true);
            this.UpdateDirection();
        }
    }

    private void OnCannonHitPlayer()
    {
        if (!this.isDead)
        {
            this.ActivePlasma();
        }
    }

    private void OnCannonMissPlayer()
    {
        if (!this.isDead)
        {
            if (Mathf.Abs(base.transform.position.x - this.target.transform.position.x) <= 2f)
            {
                this.ActivePlasma();
            }
            else
            {
                this.ActiveGore();
            }
        }
    }

    private void ActiveCannon()
    {
        this.isReadyAttack = false;
        this.ResetAim();
        base.StartCoroutine(base.DelayAction(delegate
        {
            this.isReadyAttack = true;
            this.flagCannon = true;
            this.flagPlasma = false;
            this.countGunPlasma = 0f;
        }, StaticValue.waitOneSec));
    }

    private void ActivePlasma()
    {
        this.isReadyAttack = false;
        base.StartCoroutine(base.DelayAction(delegate
        {
            this.isReadyAttack = true;
            this.flagCannon = false;
            this.flagPlasma = true;
            this.countGunPlasma = 0f;
        }, StaticValue.waitOneSec));
    }

    private void ActiveGore()
    {
        this.isReadyAttack = false;
        this.ResetAim();
        base.StartCoroutine(base.DelayAction(delegate
        {
            this.isReadyAttack = true;
            this.flagCannon = false;
            this.flagPlasma = false;
            this.skeletonAnimation.AnimationState.SetAnimation(0, this.preGore, false);
            this.skeletonAnimation.AnimationState.SetAnimation(1, this.gore2, true);
            this.UpdateDirection();
            SoundManager.Instance.PlaySfx(this.soundPregore, 0f);
        }, StaticValue.waitOneSec));
    }
}
