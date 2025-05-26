using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossSubmarine : BaseEnemy
{
	private sealed class _ReleaseTorpedo_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _count___0;

		internal int _totalTorpedo___0;

		internal BossSubmarine _this;

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

		public _ReleaseTorpedo_c__Iterator0()
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
				this._totalTorpedo___0 = ((SO_BossSubmarineStats)this._this.baseStats).NumberOfBullet;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._count___0 < this._totalTorpedo___0)
			{
				this._this.skeletonAnimation.AnimationState.SetAnimation(0, this._this.shootTorpedo, false);
				this._count___0++;
				this._current = this._this.delayRocket;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.PlayAnimationIdle();
			if (this._this.HpPercent > 0.5f)
			{
				this._this.nextAction = BossMarineAction.SpawnMarine;
				this._this.CheckEnableGore();
			}
			else
			{
				this._this.ActiveMarine();
			}
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

	private sealed class _ReleaseRocket_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _count___0;

		internal int _totalRocket___0;

		internal BossSubmarine _this;

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

		public _ReleaseRocket_c__Iterator1()
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
				this._totalRocket___0 = ((this._this.HpPercent <= 0.5f) ? ((SO_BossSubmarineStats)this._this.baseStats).RageNumberOfRocket : ((SO_BossSubmarineStats)this._this.baseStats).NumberOfRocket);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._count___0 < this._totalRocket___0)
			{
				this._this.destinationRocket = this._this.target.transform.position;
				this._this.skeletonAnimation.AnimationState.SetAnimation(0, this._this.shootRocket, false);
				this._count___0++;
				this._current = this._this.delayRocket;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.PlayAnimationIdle();
			if (this._this.HpPercent > 0.5f)
			{
				this._this.nextAction = BossMarineAction.SpawnMarine;
				this._this.CheckEnableGore();
			}
			else
			{
				this._this.ActiveTorpedo();
			}
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

	private sealed class _SpawnMarine_c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _count___0;

		internal int _totalMarine___0;

		internal EnemyMarine _marine___1;

		internal int _marineLevel___1;

		internal BossSubmarine _this;

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

		public _SpawnMarine_c__Iterator2()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.isSpawningMarines = true;
				this._count___0 = 0;
				this._totalMarine___0 = ((this._this.HpPercent <= 0.5f) ? ((SO_BossSubmarineStats)this._this.baseStats).RageNumberOfMarine : ((SO_BossSubmarineStats)this._this.baseStats).NumberOfMarine);
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._count___0 < this._totalMarine___0)
			{
				if (this._this.soundMarineAppear.Length > 0)
				{
					SoundManager.Instance.PlaySfx(this._this.soundMarineAppear[UnityEngine.Random.Range(0, this._this.soundMarineAppear.Length)], 0f);
				}
				this._marine___1 = Singleton<PoolingController>.Instance.poolEnemyMarine.New();
				if (this._marine___1 == null)
				{
					this._marine___1 = UnityEngine.Object.Instantiate<EnemyMarine>(this._this.marinePrefab);
				}
				this._marine___1.isInvisibleWhenActive = true;
				this._marineLevel___1 = ((this._this.HpPercent <= 0.5f) ? ((SO_BossSubmarineStats)this._this.baseStats).RageMarineLevel : ((SO_BossSubmarineStats)this._this.baseStats).MarineLevel);
				this._marine___1.Active(this._marineLevel___1, this._this.marineSpawnPoint.position, this._this.underWaterPosY);
				this._marine___1.SetTarget(Singleton<GameController>.Instance.Player);
				Singleton<GameController>.Instance.AddUnit(this._marine___1.gameObject, this._marine___1);
				this._this.activeMarines.Add(this._marine___1.gameObject, this._marine___1);
				this._count___0++;
				this._current = this._this.delaySpawnMarine;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.isSpawningMarines = false;
			this._this.PlayAnimationIdle();
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

	[Header("BOSS SUBMARINE PROPERTIES")]
	public RocketBossSubmarine rocketPrefab;

	public BaseMuzzle dustMuzzlePrefab;

	public Torpedo torpedoPrefab;

	public EnemyMarine marinePrefab;

	public Transform rocketFirePoint;

	public Transform torpedoFirePoint;

	public Transform marineSpawnPoint;

	public BossSubmarineColliderGore colliderGore;

	[SpineAnimation("", "", true, false)]
	public string shootRocket;

	[SpineAnimation("", "", true, false)]
	public string idleToRocket;

	[SpineAnimation("", "", true, false)]
	public string spawn;

	[SpineAnimation("", "", true, false)]
	public string shootTorpedo;

	[SpineEvent("", "", true, false)]
	public string eventShootRocket;

	[SpineEvent("", "", true, false)]
	public string eventShootTorpedo;

	[SpineEvent("", "", true, false)]
	public string eventSink;

	public AudioClip soundOpenDoor;

	public AudioClip soundShootRocket;

	public AudioClip soundShootTorpedo;

	public AudioClip[] soundMarineAppear;

	private BaseMuzzle dustMuzzle;

	private bool isMovingToBase = true;

	private bool flagRocket;

	private bool flagTorpedo;

	private bool flagMarine;

	private bool isSpawningMarines;

	private float underWaterPosY;

	private Vector3 destinationRocket;

	private WaitForSeconds delayRocket;

	private WaitForSeconds delaySpawnMarine;

	private BossMarineAction nextAction;

	private Dictionary<GameObject, EnemyMarine> activeMarines = new Dictionary<GameObject, EnemyMarine>();

	private static TweenCallback __f__am_cache0;

	protected override void Start()
	{
		base.Start();
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, new Action<Component, object>(this.OnUnitDie));
		EventDispatcher.Instance.RegisterListener(EventID.BoatTriggerWater, new Action<Component, object>(this.OnRubberBoatTriggerWater));
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
		string path = string.Format("Scriptable Object/Boss/Boss Submarine/boss_submarine_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BossSubmarineStats>(path);
	}

	public override void Renew()
	{
		base.Renew();
		this.isFinalBoss = true;
		this.isEffectMeleeWeapon = false;
		this.bodyCollider.gameObject.SetActive(false);
		this.delayRocket = new WaitForSeconds(((SO_BossSubmarineStats)this.baseStats).TimeDelayRocket);
		this.delaySpawnMarine = new WaitForSeconds(((SO_BossSubmarineStats)this.baseStats).TimeDelaySpawnMarine);
	}

	public override void UpdateHealthBar(bool isAutoHide = false)
	{
		Singleton<UIController>.Instance.hudBoss.SetIconBoss(this.id);
		Singleton<UIController>.Instance.hudBoss.UpdateHP(this.HpPercent);
	}

	protected override void Attack()
	{
		if (this.flagRocket)
		{
			this.flagRocket = false;
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.idleToRocket, false);
		}
		else if (this.flagTorpedo)
		{
			this.flagTorpedo = false;
			base.StartCoroutine(this.ReleaseTorpedo());
		}
		else if (this.flagMarine)
		{
			this.flagMarine = false;
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.spawn, false);
			SoundManager.Instance.PlaySfx(this.soundOpenDoor, 0f);
		}
	}

	protected override void StartDie()
	{
		base.StartDie();
		EventDispatcher.Instance.PostEvent(EventID.BossSubmarineDie);
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventShootRocket) == 0)
		{
			RocketBossSubmarine rocketBossSubmarine = Singleton<PoolingController>.Instance.poolRocketBossSubmarine.New();
			if (rocketBossSubmarine == null)
			{
				rocketBossSubmarine = UnityEngine.Object.Instantiate<RocketBossSubmarine>(this.rocketPrefab);
			}
			float damage = (this.HpPercent <= 0.5f) ? ((SO_BossSubmarineStats)this.baseStats).RageRocketDamage : ((SO_BossSubmarineStats)this.baseStats).RocketDamage;
			float moveSpeed = (this.HpPercent <= 0.5f) ? ((SO_BossSubmarineStats)this.baseStats).RageRocketSpeed : ((SO_BossSubmarineStats)this.baseStats).RocketSpeed;
			AttackData attackData = new AttackData(this, damage, 0f, false, WeaponType.NormalGun, -1, null);
			rocketBossSubmarine.Active(attackData, this.rocketFirePoint, this.destinationRocket, moveSpeed, Singleton<PoolingController>.Instance.groupBullet);
			this.ActiveDustMuzzle(this.rocketFirePoint);
			SoundManager.Instance.PlaySfx(this.soundShootRocket, 0f);
		}
		if (string.Compare(e.Data.Name, this.eventShootTorpedo) == 0)
		{
			Torpedo torpedo = Singleton<PoolingController>.Instance.poolTorpedo.New();
			if (torpedo == null)
			{
				torpedo = UnityEngine.Object.Instantiate<Torpedo>(this.torpedoPrefab);
			}
			float damage2 = ((SO_BossSubmarineStats)this.baseStats).Damage;
			float bulletSpeed = ((SO_BossSubmarineStats)this.baseStats).BulletSpeed;
			AttackData attackData2 = new AttackData(this, damage2, 0f, false, WeaponType.NormalGun, -1, null);
			torpedo.Active(attackData2, this.torpedoFirePoint, bulletSpeed, Singleton<PoolingController>.Instance.groupBullet);
			this.ActiveDustMuzzle(this.torpedoFirePoint);
			SoundManager.Instance.PlaySfx(this.soundShootTorpedo, 0f);
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
		if (string.Compare(entry.animation.name, this.idleToRocket) == 0)
		{
			base.StartCoroutine(this.ReleaseRocket());
		}
		if (string.Compare(entry.animation.name, this.spawn) == 0)
		{
			base.StartCoroutine(this.SpawnMarine());
		}
	}

	private void ActiveDustMuzzle(Transform point)
	{
		if (this.dustMuzzle == null)
		{
			this.dustMuzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.dustMuzzlePrefab, point.position, point.rotation, point);
		}
		this.dustMuzzle.Active();
	}

	private void MoveToBase()
	{
		if (this.isMovingToBase)
		{
			this.isMovingToBase = false;
			this.PlayAnimationIdle();
			base.transform.DOMove(this.basePosition, 4f, false).OnComplete(delegate
			{
				EventDispatcher.Instance.PostEvent(EventID.FinalBossStart);
				this.bodyCollider.gameObject.SetActive(true);
				this.ActiveRocket();
				float num = base.transform.position.y;
				num -= 1f;
				this.underWaterPosY = num;
			}).OnStart(delegate
			{
				Singleton<CameraFollow>.Instance.AddShake(0.3f, 3.5f);
			});
		}
	}

	private IEnumerator ReleaseTorpedo()
	{
		BossSubmarine._ReleaseTorpedo_c__Iterator0 _ReleaseTorpedo_c__Iterator = new BossSubmarine._ReleaseTorpedo_c__Iterator0();
		_ReleaseTorpedo_c__Iterator._this = this;
		return _ReleaseTorpedo_c__Iterator;
	}

	private IEnumerator ReleaseRocket()
	{
		BossSubmarine._ReleaseRocket_c__Iterator1 _ReleaseRocket_c__Iterator = new BossSubmarine._ReleaseRocket_c__Iterator1();
		_ReleaseRocket_c__Iterator._this = this;
		return _ReleaseRocket_c__Iterator;
	}

	private IEnumerator SpawnMarine()
	{
		BossSubmarine._SpawnMarine_c__Iterator2 _SpawnMarine_c__Iterator = new BossSubmarine._SpawnMarine_c__Iterator2();
		_SpawnMarine_c__Iterator._this = this;
		return _SpawnMarine_c__Iterator;
	}

	private void CheckEnableGore()
	{
		float num = base.transform.position.x - this.target.transform.position.x;
		if (num > 0f && num < 5f)
		{
			this.ActiveGore();
		}
		else
		{
			this.ActiveNextAction();
		}
	}

	private void ActiveNextAction()
	{
		BossMarineAction bossMarineAction = this.nextAction;
		if (bossMarineAction != BossMarineAction.Rocket)
		{
			if (bossMarineAction != BossMarineAction.Torpedo)
			{
				if (bossMarineAction == BossMarineAction.SpawnMarine)
				{
					this.ActiveMarine();
				}
			}
			else
			{
				this.ActiveTorpedo();
			}
		}
		else
		{
			this.ActiveRocket();
		}
	}

	private void ActiveRocket()
	{
		this.isReadyAttack = false;
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagRocket = true;
			this.flagTorpedo = false;
			this.flagMarine = false;
		}, StaticValue.waitHalfSec));
	}

	private void ActiveTorpedo()
	{
		this.isReadyAttack = false;
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagRocket = false;
			this.flagTorpedo = true;
			this.flagMarine = false;
		}, StaticValue.waitHalfSec));
	}

	private void ActiveMarine()
	{
		this.isReadyAttack = false;
		this.isSpawningMarines = true;
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagRocket = false;
			this.flagTorpedo = false;
			this.flagMarine = true;
		}, StaticValue.waitHalfSec));
	}

	private void ActiveGore()
	{
		this.isReadyAttack = false;
		this.flagRocket = false;
		this.flagTorpedo = false;
		this.flagMarine = false;
		base.StartCoroutine(base.DelayAction(delegate
		{
			float num = base.transform.position.x;
			num -= 3.5f;
			this.colliderGore.gameObject.SetActive(true);
			this.PlayAnimationIdle();
			base.transform.DOMoveX(num, 1f, false).OnComplete(delegate
			{
				base.transform.DOMoveX(this.basePosition.x, 1.5f, false).OnComplete(delegate
				{
					this.CheckEnableGore();
				});
			});
		}, StaticValue.waitOneSec));
	}

	private void OnRubberBoatTriggerWater(Component senser, object param)
	{
		List<EnemyMarine> list = new List<EnemyMarine>();
		Vector3 vector = (Vector3)param;
		foreach (EnemyMarine current in this.activeMarines.Values)
		{
			float num = Mathf.Abs(current.transform.position.x - vector.x);
			float num2 = vector.y - current.transform.position.y;
			if (num <= 1.2f && num2 <= 1.3f)
			{
				list.Add(current);
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			int num3 = UnityEngine.Random.Range(100, 150);
			list[i].TakeDamage((float)num3);
		}
	}

	private void OnUnitDie(Component senser, object param)
	{
		UnitDieData unitDieData = (UnitDieData)param;
		BaseEnemy component = unitDieData.unit.GetComponent<BaseEnemy>();
		if (this.activeMarines.ContainsKey(component.gameObject))
		{
			this.activeMarines.Remove(component.gameObject);
			if (this.activeMarines.Count == 1 && !this.isSpawningMarines)
			{
				this.nextAction = BossMarineAction.Rocket;
				this.CheckEnableGore();
			}
		}
	}
}
