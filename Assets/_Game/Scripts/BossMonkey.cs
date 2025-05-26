using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossMonkey : BaseEnemy
{
	private enum NextAction
	{
		None,
		ThrowStone,
		DropSpike,
		SpawnMinions
	}

	private sealed class _SpawnMinions_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _count___0;

		internal int _totalMinions___0;

		internal int _minionLevel___0;

		internal BossMonkeyMinion _minion___1;

		internal BossMonkey _this;

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

		public _SpawnMinions_c__Iterator0()
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
				this._totalMinions___0 = 0;
				this._minionLevel___0 = ((SO_BossMonkeyStats)this._this.baseStats).LevelMinions;
				if (this._this.HpPercent < 0.35f)
				{
					this._totalMinions___0 = ((SO_BossMonkeyStats)this._this.baseStats).Hp35_NumberMinions;
				}
				else if (this._this.HpPercent < 0.65f)
				{
					this._totalMinions___0 = ((SO_BossMonkeyStats)this._this.baseStats).Hp65_NumberMinions;
				}
				else
				{
					this._totalMinions___0 = ((SO_BossMonkeyStats)this._this.baseStats).NumberMinions;
				}
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._count___0 < this._totalMinions___0)
			{
				this._minion___1 = (this._this.minionPrefab.GetFromPool() as BossMonkeyMinion);
				this._minion___1.isInvisibleWhenActive = true;
				this._minion___1.SetPoints(this._this.minionMostLeftPoint, this._this.minionMostRightPoint);
				this._minion___1.Active(this._this.minionPrefab.id, this._minionLevel___0, this._this.minionSpawnPoint);
				this._minion___1.SetTarget(Singleton<GameController>.Instance.Player);
				Singleton<GameController>.Instance.AddUnit(this._minion___1.gameObject, this._minion___1);
				this._this.activeMinions.Add(this._minion___1.gameObject, this._minion___1);
				this._count___0++;
				this._current = StaticValue.waitHalfSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.flagChangeStateAfterSpawn = true;
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

	[Header("BOSS MONKEY PROPERTIES")]
	public BossMonkeyMinion minionPrefab;

	public BaseBullet stonePrefab;

	public Transform stoneStartPoint;

	public Vector2 stoneDirection;

	[SpineAnimation("", "", true, false)]
	public string throwStone;

	[SpineAnimation("", "", true, false)]
	public string groundSlam;

	[SpineAnimation("", "", true, false)]
	public string roar1;

	[SpineAnimation("", "", true, false)]
	public string roar2;

	[SpineAnimation("", "", true, false)]
	public string appear;

	[SpineEvent("", "", true, false)]
	public string eventThrowStone;

	[SpineEvent("", "", true, false)]
	public string eventSlam;

	public AudioClip soundAppear;

	public AudioClip soundIdle;

	public AudioClip soundSpawn;

	public AudioClip soundThrowStone;

	private BossMonkey.NextAction nextAction;

	private bool flagMovingEntrance = true;

	[SerializeField]
	private bool flagStone;

	[SerializeField]
	private bool flagSpike;

	[SerializeField]
	private bool flagSpawn;

	[SerializeField]
	private bool flagChangeStateAfterSpawn;

	private int totalStone;

	private int countStone;

	private bool isSpikeHitPlayer;

	private Vector2 minionSpawnPoint;

	private Vector2 minionMostLeftPoint;

	private Vector2 minionMostRightPoint;

	[SerializeField]
	private Transform spawnSlamSmokePoint;

	private Dictionary<GameObject, BaseUnit> activeMinions = new Dictionary<GameObject, BaseUnit>();

	private static TweenCallback __f__am_cache0;

	protected override void Start()
	{
		base.Start();
		EventDispatcher.Instance.RegisterListener(EventID.BossMonkeySpikeHitPlayer, delegate(Component sender, object param)
		{
			this.isSpikeHitPlayer = true;
		});
		EventDispatcher.Instance.RegisterListener(EventID.BossMonkeySpikeTrapEnd, delegate(Component sender, object param)
		{
			this.OnSpikeTrapEnd();
		});
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, new Action<Component, object>(this.OnUnitDie));
	}

	protected override void Update()
	{
		if (!this.isDead)
		{
			this.Entrance();
			if (this.isReadyAttack)
			{
				this.Attack();
			}
		}
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Boss/Boss Monkey/boss_monkey_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BossMonkeyStats>(path);
	}

	protected override void Attack()
	{
		if (this.flagStone)
		{
			this.flagStone = false;
			this.PlayAnimationThrow();
		}
		else if (this.flagSpike)
		{
			this.flagSpike = false;
			this.PlayAnimationSlam();
		}
		else if (this.flagSpawn)
		{
			this.flagSpawn = false;
			this.PlayAnimationRoar();
		}
	}

	protected override void StartDie()
	{
		base.StartDie();
		EventDispatcher.Instance.PostEvent(EventID.BossMonkeyDie);
	}

	public override void Renew()
	{
		this.isDead = false;
		this.LoadScriptableObject();
		this.stats.Init(this.baseStats);
		this.isFinalBoss = true;
		this.bodyCollider.enabled = false;
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

	public void SetPoints(Vector2 minionSpawnPoint, Vector2 minionMostLeftPoint, Vector2 minionMostRightPoint)
	{
		this.minionSpawnPoint = minionSpawnPoint;
		this.minionMostLeftPoint = minionMostLeftPoint;
		this.minionMostRightPoint = minionMostRightPoint;
	}

	private void Entrance()
	{
		if (this.flagMovingEntrance)
		{
			this.flagMovingEntrance = false;
			base.transform.DOMove(this.basePosition, 0.5f, false).SetEase(Ease.Linear).OnComplete(delegate
			{
			}).OnStart(delegate
			{
				this.PlaySound(this.soundAppear);
				this.skeletonAnimation.AnimationState.SetAnimation(0, this.appear, false);
				Singleton<CameraFollow>.Instance.AddShake(1f, 1.5f);
			});
		}
	}

	private void OnSpikeTrapEnd()
	{
		if (this.nextAction == BossMonkey.NextAction.SpawnMinions)
		{
			this.ActiveSpawn();
			this.nextAction = BossMonkey.NextAction.None;
		}
		else
		{
			this.nextAction = BossMonkey.NextAction.SpawnMinions;
			if (this.isSpikeHitPlayer)
			{
				this.isSpikeHitPlayer = false;
				this.ActiveStone(1);
			}
			else
			{
				this.ActiveSpike();
			}
		}
	}

	private void ActiveSoundIdle(bool isActive)
	{
		if (isActive)
		{
			if (this.audioSource.clip == null)
			{
				this.audioSource.loop = true;
				this.audioSource.clip = this.soundIdle;
				this.audioSource.Play();
			}
		}
		else
		{
			this.audioSource.Stop();
			this.audioSource.clip = null;
		}
	}

	private void ActiveStone(int numberStone)
	{
		this.isReadyAttack = false;
		this.PlayAnimationIdle();
		this.countStone = 0;
		this.totalStone = numberStone;
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagStone = true;
			this.flagSpike = false;
			this.flagSpawn = false;
			this.ActiveSoundIdle(false);
		}, StaticValue.waitHalfSec));
	}

	private void ActiveSpike()
	{
		this.isReadyAttack = false;
		this.PlayAnimationIdle();
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagStone = false;
			this.flagSpike = true;
			this.flagSpawn = false;
			this.ActiveSoundIdle(false);
		}, StaticValue.waitHalfSec));
	}

	private void ActiveSpawn()
	{
		this.isReadyAttack = false;
		this.PlayAnimationIdle();
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagStone = false;
			this.flagSpike = false;
			this.flagSpawn = true;
			this.ActiveSoundIdle(false);
		}, StaticValue.waitHalfSec));
	}

	private IEnumerator SpawnMinions()
	{
		BossMonkey._SpawnMinions_c__Iterator0 _SpawnMinions_c__Iterator = new BossMonkey._SpawnMinions_c__Iterator0();
		_SpawnMinions_c__Iterator._this = this;
		return _SpawnMinions_c__Iterator;
	}

	private void OnUnitDie(Component senser, object param)
	{
		UnitDieData unitDieData = (UnitDieData)param;
		BaseEnemy component = unitDieData.unit.GetComponent<BaseEnemy>();
		if (this.activeMinions.ContainsKey(component.gameObject))
		{
			base.CancelInvoke();
			this.activeMinions.Remove(component.gameObject);
			if (this.flagChangeStateAfterSpawn)
			{
				this.flagChangeStateAfterSpawn = false;
				this.bodyCollider.enabled = true;
				if (UnityEngine.Random.Range(0, 2) == 0)
				{
					this.nextAction = BossMonkey.NextAction.DropSpike;
					this.ActiveStone(2);
				}
				else
				{
					this.ActiveSpike();
				}
			}
		}
	}

	protected override void HandleAnimationStart(TrackEntry entry)
	{
		base.HandleAnimationStart(entry);
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
		if (string.Compare(entry.animation.name, this.appear) == 0)
		{
			this.bodyCollider.enabled = true;
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.idle, false);
			this.nextAction = BossMonkey.NextAction.DropSpike;
			this.ActiveStone(2);
		}
		if (string.Compare(entry.animation.name, this.throwStone) == 0)
		{
			this.countStone++;
			if (this.countStone < this.totalStone)
			{
				this.StartDelayAction(delegate
				{
					this.flagStone = true;
				}, 1f);
			}
			else
			{
				if (this.nextAction == BossMonkey.NextAction.DropSpike)
				{
					this.ActiveSpike();
				}
				else if (this.nextAction == BossMonkey.NextAction.SpawnMinions)
				{
					this.ActiveSpawn();
				}
				this.nextAction = BossMonkey.NextAction.None;
			}
			this.PlayAnimationIdle();
		}
		if (string.Compare(entry.animation.name, this.groundSlam) == 0)
		{
			this.PlayAnimationIdle();
			DropSpikeData dropSpikeData = new DropSpikeData();
			dropSpikeData.boss = this;
			int numberSpikes;
			float spikeDamage;
			float spikeDropSpeed;
			float spikeDelay;
			if (this.HpPercent < 0.35f)
			{
				numberSpikes = ((SO_BossMonkeyStats)this.baseStats).Hp35_NumberSpikes;
				spikeDamage = ((SO_BossMonkeyStats)this.baseStats).Hp35_SpikeDamage;
				spikeDropSpeed = ((SO_BossMonkeyStats)this.baseStats).Hp35_SpikeSpeed;
				spikeDelay = ((SO_BossMonkeyStats)this.baseStats).Hp35_SpikeDelay;
			}
			else if (this.HpPercent < 0.65f)
			{
				numberSpikes = ((SO_BossMonkeyStats)this.baseStats).Hp65_NumberSpikes;
				spikeDamage = ((SO_BossMonkeyStats)this.baseStats).Hp65_SpikeDamage;
				spikeDropSpeed = ((SO_BossMonkeyStats)this.baseStats).Hp65_SpikeSpeed;
				spikeDelay = ((SO_BossMonkeyStats)this.baseStats).Hp65_SpikeDelay;
			}
			else
			{
				numberSpikes = ((SO_BossMonkeyStats)this.baseStats).NumberSpikes;
				spikeDamage = ((SO_BossMonkeyStats)this.baseStats).SpikeDamage;
				spikeDropSpeed = ((SO_BossMonkeyStats)this.baseStats).SpikeSpeed;
				spikeDelay = ((SO_BossMonkeyStats)this.baseStats).SpikeDelay;
			}
			dropSpikeData.numberSpikes = numberSpikes;
			dropSpikeData.spikeDamage = spikeDamage;
			dropSpikeData.spikeDropSpeed = spikeDropSpeed;
			dropSpikeData.spikeDelay = spikeDelay;
			EventDispatcher.Instance.PostEvent(EventID.BossMonkeySpikeTrapStart, dropSpikeData);
		}
		if (string.Compare(entry.animation.name, this.roar1) == 0 || string.Compare(entry.animation.name, this.roar2) == 0)
		{
			this.bodyCollider.enabled = false;
			this.PlayAnimationIdle();
			base.StartCoroutine(this.SpawnMinions());
		}
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventThrowStone) == 0)
		{
			StoneBossMonkey stoneBossMonkey = Singleton<PoolingController>.Instance.poolStoneBossMonkey.New();
			if (stoneBossMonkey == null)
			{
				stoneBossMonkey = (UnityEngine.Object.Instantiate<BaseBullet>(this.stonePrefab) as StoneBossMonkey);
			}
			float damage;
			if (this.HpPercent < 0.35f)
			{
				damage = ((SO_BossMonkeyStats)this.baseStats).Hp35_StoneDamage;
			}
			else if (this.HpPercent < 0.65f)
			{
				damage = ((SO_BossMonkeyStats)this.baseStats).Hp65_StoneDamage;
			}
			else
			{
				damage = ((SO_BossMonkeyStats)this.baseStats).StoneDamage;
			}
			AttackData attackData = new AttackData(this, damage, 0f, false, WeaponType.NormalGun, -1, null);
			stoneBossMonkey.Active(attackData, this.stoneStartPoint, this.target.BodyCenterPoint, this.stoneDirection);
		}
		if (string.Compare(e.Data.Name, this.eventSlam) == 0)
		{
			EffectController.Instance.SpawnParticleEffect(EffectObjectName.GroundSmoke, this.spawnSlamSmokePoint.position);
			Singleton<CameraFollow>.Instance.AddShake(0.2f, 0.3f);
		}
	}

	protected override void PlayAnimationThrow()
	{
		this.PlaySound(this.soundThrowStone);
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.throwStone, false);
	}

	public override void PlayAnimationIdle()
	{
		base.PlayAnimationIdle();
		this.ActiveSoundIdle(true);
	}

	private void PlayAnimationSlam()
	{
		this.PlaySound(this.soundSpawn);
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.groundSlam, false);
	}

	private void PlayAnimationRoar()
	{
		this.PlaySound(this.soundSpawn);
		Singleton<CameraFollow>.Instance.AddShake(0.2f, 2f);
		string animationName = (UnityEngine.Random.Range(0, 2) != 0) ? this.roar2 : this.roar1;
		this.skeletonAnimation.AnimationState.SetAnimation(0, animationName, false);
	}
}
