using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class BossVenom : BaseEnemy
{
    private sealed class _CoroutineReleasePoisonBullet_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal int _count___0;

        internal int _shootTimes___0;

        internal BossVenom _this;

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

        public _CoroutineReleasePoisonBullet_c__Iterator0()
        {
        }

        public bool MoveNext()
        {
            uint num = (uint)this._PC;
            this._PC = -1;
            switch (num)
            {
                case 0u:
                    this._count___0 = 0;
                    this._shootTimes___0 = ((this._this.HpPercent <= 0.5f) ? ((SO_BossVenomStats)this._this.baseStats).RageShootTimes : ((SO_BossVenomStats)this._this.baseStats).ShootTimes);
                    break;
                case 1u:
                    break;
                default:
                    return false;
            }
            if (this._count___0 < this._shootTimes___0)
            {
                SoundManager.Instance.PlaySfx(this._this.soundShootPoison, 0f);
                this._this.aimPoint.position = this._this.target.transform.position;
                this._this.skeletonAnimation.AnimationState.SetAnimation(0, this._this.shootPoison, false);
                this._count___0++;
                this._current = this._this.delayShoot;
                if (!this._disposing)
                {
                    this._PC = 1;
                }
                return true;
            }
            this._this.skeletonAnimation.AnimationState.SetAnimation(0, this._this.shootToIdle, false);
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

    [Header("BOSS VENOM PROPERTIES")]
    public BulletPoisonBossVenom bulletPrefab;

    public PoisonTrap poisonTrapPrefab;

    public Collider2D head;

    public LaserBossVenom laser;

    [SpineAnimation("", "", true, false)]
    public string idleToLaser;

    [SpineAnimation("", "", true, false)]
    public string idleToShoot;

    [SpineAnimation("", "", true, false)]
    public string shootToIdle;

    [SpineAnimation("", "", true, false)]
    public string shootLaser;

    [SpineAnimation("", "", true, false)]
    public string shootPoison;

    [SpineEvent("", "", true, false)]
    public string eventShootLaser;

    [SpineEvent("", "", true, false)]
    public string eventShootPoison1;

    [SpineEvent("", "", true, false)]
    public string eventShootPoison2;

    [SpineEvent("", "", true, false)]
    public string eventShootPoison3;

    public AudioClip soundChangeState1;

    public AudioClip soundChangeState2;

    public AudioClip soundShootPoison;

    public AudioClip soundLaser;

    public Transform[] poisonFirePoints;

    private bool flagLaser;

    private bool flagPoison;

    private bool isScanningLaser;

    private bool isPingPongLaser;

    private Vector2 defaultAimPointPosition;

    private float timerScanLaser;

    private WaitForSeconds delayChangeAction;

    private WaitForSeconds delayShoot;

    public Transform pingPointLaser;

    public Transform pongPointLaser;

    public float maxDistanceDelta = 15;

    private Transform _FurthestLaserPoint_k__BackingField;

    private Transform _NearestLaserPoint_k__BackingField;

    public Transform FurthestLaserPoint
    {
        get;
        set;
    }

    public Transform NearestLaserPoint
    {
        get;
        set;
    }

    protected override void Awake()
    {
        base.Awake();
        this.defaultAimPointPosition = this.aimPoint.position;
    }

    protected override void Start()
    {
        base.Start();
        base.StartCoroutine(base.DelayAction(new UnityAction(this.AppearDone), StaticValue.waitOneSec));
        this.delayChangeAction = new WaitForSeconds(0.3f);
        this.delayShoot = new WaitForSeconds(((SO_BossVenomStats)this.baseStats).DelayShootTime);
        EventDispatcher.Instance.RegisterListener(EventID.LaserPoisonHitGround, delegate (Component sender, object param)
        {
            this.SpawnPoisonTrap((Vector2)param);
        });
    }

    protected override void Update()
    {
        this.aimBone.transform.position = this.aimPoint.position;
        if (this.isReadyAttack)
        {
            this.Attack();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == StaticValue.LAYER_GROUND && this.isDead)
        {
            EffectController.Instance.SpawnParticleEffect(EffectObjectName.ExplosionBomb, base.transform.position);
            base.StartCoroutine(base.DelayAction(new UnityAction(this.Deactive), StaticValue.waitOneSec));
        }
    }

    protected override void LoadScriptableObject()
    {
        string path = string.Format("Scriptable Object/Boss/Boss Venom/boss_venom_lv{0}", this.level);
        this.baseStats = Resources.Load<SO_BossVenomStats>(path);
    }

    protected override void Attack()
    {
        this.ScanLaser();
        if (this.flagLaser)
        {
            this.flagLaser = false;
            this.skeletonAnimation.AnimationState.SetAnimation(0, this.idleToLaser, false);
            SoundManager.Instance.PlaySfx(this.soundLaser, 0f);
        }
        else if (this.flagPoison)
        {
            this.flagPoison = false;
            this.skeletonAnimation.AnimationState.SetAnimation(0, this.idleToShoot, false);
        }
    }

    protected override void StartDie()
    {
        base.StartDie();
        this.laser.gameObject.SetActive(false);
    }

    public override void Renew()
    {
        this.isDead = false;
        this.LoadScriptableObject();
        this.stats.Init(this.baseStats);
        this.isFinalBoss = true;
        this.head.enabled = false;
        this.rigid.bodyType = RigidbodyType2D.Kinematic;
        this.laser.gameObject.SetActive(false);
        this.isEffectMeleeWeapon = false;
        this.isReadyAttack = false;
        base.transform.parent = null;
        this.UpdateTransformPoints();
        this.UpdateHealthBar(false);
    }

    protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
    {
        if (string.Compare(e.Data.Name, this.eventShootLaser) == 0)
        {
            this.laser.gameObject.SetActive(true);

            base.StartCoroutine(base.DelayAction(delegate
            {
                this.isScanningLaser = true;
                // this.pingPointLaser = ((this.aimPoint.position.x <= this.target.transform.position.x) ? this.NearestLaserPoint : this.FurthestLaserPoint);
                // this.pongPointLaser = ((this.aimPoint.position.x <= this.target.transform.position.x) ? this.FurthestLaserPoint : this.NearestLaserPoint);
            }, StaticValue.waitHalfSec));
        }
        if (string.Compare(e.Data.Name, this.eventShootPoison1) == 0)
        {
            this.ReleasePoisonBullet(this.poisonFirePoints[0]);
        }
        if (string.Compare(e.Data.Name, this.eventShootPoison2) == 0)
        {
            this.ReleasePoisonBullet(this.poisonFirePoints[1]);
        }
        if (string.Compare(e.Data.Name, this.eventShootPoison3) == 0)
        {
            this.ReleasePoisonBullet(this.poisonFirePoints[2]);
        }
    }

    protected override void HandleAnimationCompleted(TrackEntry entry)
    {
        if (string.Compare(entry.animation.name, this.idleToLaser) == 0)
        {
            this.skeletonAnimation.AnimationState.SetAnimation(0, this.shootLaser, false);
        }
        if (string.Compare(entry.animation.name, this.idleToShoot) == 0)
        {
            base.StartCoroutine(this.CoroutineReleasePoisonBullet());
        }
        if (string.Compare(entry.animation.name, this.shootToIdle) == 0)
        {
            this.PlayAnimationIdle();
            this.ResetAim();
            base.StartCoroutine(base.DelayAction(delegate
            {
                this.ActiveLaser();
            }, this.delayChangeAction));
        }
        if (this.dieAnimationNames.Contains(entry.animation.name))
        {
            this.rigid.bodyType = RigidbodyType2D.Dynamic;
        }
    }

    public override void UpdateHealthBar(bool isAutoHide = false)
    {
        Singleton<UIController>.Instance.hudBoss.SetIconBoss(this.id);
        Singleton<UIController>.Instance.hudBoss.UpdateHP(this.HpPercent);
    }

    protected override void ResetAim()
    {
        this.aimPoint.parent.rotation = Quaternion.identity;
        this.aimPoint.position = this.defaultAimPointPosition;
    }

    private IEnumerator CoroutineReleasePoisonBullet()
    {
        BossVenom._CoroutineReleasePoisonBullet_c__Iterator0 _CoroutineReleasePoisonBullet_c__Iterator = new BossVenom._CoroutineReleasePoisonBullet_c__Iterator0();
        _CoroutineReleasePoisonBullet_c__Iterator._this = this;
        return _CoroutineReleasePoisonBullet_c__Iterator;
    }

    private void ReleasePoisonBullet(Transform point)
    {
        BulletPoisonBossVenom bulletPoisonBossVenom = Singleton<PoolingController>.Instance.poolBulletPoisonBossVenom.New();
        if (bulletPoisonBossVenom == null)
        {
            bulletPoisonBossVenom = UnityEngine.Object.Instantiate<BulletPoisonBossVenom>(this.bulletPrefab);
        }
        float damage = (this.HpPercent <= 0.5f) ? ((SO_BossVenomStats)this.baseStats).RageDamage : this.baseStats.Damage;
        float moveSpeed = (this.HpPercent <= 0.5f) ? ((SO_BossVenomStats)this.baseStats).RageBulletSpeed : this.baseStats.BulletSpeed;
        AttackData attackData = new AttackData(this, damage, 0f, false, WeaponType.NormalGun, -1, null);
        bulletPoisonBossVenom.Active(attackData, point, moveSpeed, Singleton<PoolingController>.Instance.groupBullet);
    }

    private void AppearDone()
    {
        this.ReadyToAttack();
        this.head.enabled = true;
        this.ActiveLaser();
        EventDispatcher.Instance.PostEvent(EventID.FinalBossStart);
    }

    private void ScanLaser()
    {
        if (!this.isScanningLaser)
        {
            return;
        }
        if (this.timerScanLaser < 1.5f)
        {
            this.timerScanLaser += Time.deltaTime;
            // this.aimPoint.position = Vector2.MoveTowards(this.aimPoint.position, this.pingPointLaser.position, 10f * Time.deltaTime); // original commented by hardik
            // this.aimPoint.position = Vector2.MoveTowards(this.aimPoint.position, this.target.transform.position, maxDistanceDelta * Time.deltaTime);

            UnityEngine.Debug.Log("roate 1");
            //added by hardik
            Vector3 vectorToTarget = this.target.transform.position - this.laser.transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            this.laser.transform.rotation = Quaternion.Slerp(this.laser.transform.rotation, q, Time.deltaTime * maxDistanceDelta);
        }
        else if (this.isPingPongLaser)
        {
            if (this.timerScanLaser < 3f)
            {
                UnityEngine.Debug.Log("roate 2");
                this.timerScanLaser += Time.deltaTime;
                // this.aimPoint.position = Vector2.MoveTowards(this.aimPoint.position, this.pongPointLaser.position, 15f * Time.deltaTime); // commented by hardik
                // this.aimPoint.position = Vector2.MoveTowards(this.aimPoint.position, this.target.transform.position, maxDistanceDelta * Time.deltaTime);
                //added by hardik
                Vector3 vectorToTarget = this.target.transform.position - this.laser.transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                this.laser.transform.rotation = Quaternion.Slerp(this.laser.transform.rotation, q, Time.deltaTime * maxDistanceDelta);
            }
            else
            {
                this.StopLaser();
            }
        }
        else
        {
            this.StopLaser();
        }
    }

    private void StopLaser()
    {
        this.timerScanLaser = 0f;
        this.isScanningLaser = false;
        if (this.laser.hit.collider != null && this.laser.hit.collider.gameObject.layer == StaticValue.LAYER_GROUND)
        {
            this.SpawnPoisonTrap(this.laser.hit.point);
        }
        this.laser.gameObject.SetActive(false);
        this.ResetAim();
        this.ActiveShootPoison();
    }

    private void SpawnPoisonTrap(Vector2 position)
    {
        PoisonTrap poisonTrap = Singleton<PoolingController>.Instance.poolPoisonTrap.New();
        if (poisonTrap == null)
        {
            poisonTrap = UnityEngine.Object.Instantiate<PoisonTrap>(this.poisonTrapPrefab);
        }
        poisonTrap.Active(position);
    }

    private void ActiveLaser()
    {
        if (this.isDead)
        {
            return;
        }
        this.isReadyAttack = false;
        this.ResetAim();
        this.aimPoint.position = this.target.transform.position;
        //3lines commented by hardik
        // Vector2 v = this.target.transform.position;
        // v.x = Mathf.Clamp(v.x, this.FurthestLaserPoint.position.x, this.NearestLaserPoint.position.x);
        // this.aimPoint.position = v;
        this.timerScanLaser = 0f;
        this.isPingPongLaser = (this.HpPercent < 0.5f);
        base.StartCoroutine(base.DelayAction(delegate
        {
            this.isReadyAttack = true;
            this.flagLaser = true;
            this.flagPoison = false;
            SoundManager.Instance.PlaySfx(this.soundChangeState1, 0f);
        }, this.delayChangeAction));
    }

    private void ActiveShootPoison()
    {
        if (this.isDead)
        {
            return;
        }
        this.isReadyAttack = false;
        this.ResetAim();
        this.aimPoint.position = this.target.transform.position;
        base.StartCoroutine(base.DelayAction(delegate
        {
            this.isReadyAttack = true;
            this.flagLaser = false;
            this.flagPoison = true;
            SoundManager.Instance.PlaySfx(this.soundChangeState2, 0f);
        }, this.delayChangeAction));
    }
}
