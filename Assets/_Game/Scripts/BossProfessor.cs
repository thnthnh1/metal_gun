using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BossProfessor : BaseEnemy
{
	private sealed class _HandleAnimationCompleted_c__AnonStorey1
	{
		internal Vector2 tmp;

		internal float s;

		internal BossProfessor _this;

		internal void __m__0()
		{
			this.s = Vector2.Distance(this._this.transform.position, this.tmp);
			this._this.transform.DOMove(this.tmp, this.s / this._this.baseStats.MoveSpeed, false).SetDelay(1f).SetEase(Ease.Linear).OnComplete(delegate
			{
				this._this.energyPulse.Active(false);
				this._this.ActiveSoundPulse(false);
				this._this.PlayAnimationIdle();
				this.s = Vector2.Distance(this._this.transform.position, this._this.basePosition);
				this._this.transform.DOMove(this._this.basePosition, this.s / this._this.baseStats.MoveSpeed, false).SetDelay(1f).SetEase(Ease.Linear).OnComplete(delegate
				{
					this._this.isAtBase = true;
					if (this._this.numberSatelliteActive > 0)
					{
						this._this.ActiveShoot(true);
					}
					else
					{
						this._this.ActiveSpawn();
					}
				});
			});
		}

		internal void __m__1()
		{
			this._this.energyPulse.Active(false);
			this._this.ActiveSoundPulse(false);
			this._this.PlayAnimationIdle();
			this.s = Vector2.Distance(this._this.transform.position, this._this.basePosition);
			this._this.transform.DOMove(this._this.basePosition, this.s / this._this.baseStats.MoveSpeed, false).SetDelay(1f).SetEase(Ease.Linear).OnComplete(delegate
			{
				this._this.isAtBase = true;
				if (this._this.numberSatelliteActive > 0)
				{
					this._this.ActiveShoot(true);
				}
				else
				{
					this._this.ActiveSpawn();
				}
			});
		}

		internal void __m__2()
		{
			this._this.isAtBase = true;
			if (this._this.numberSatelliteActive > 0)
			{
				this._this.ActiveShoot(true);
			}
			else
			{
				this._this.ActiveSpawn();
			}
		}
	}

	private sealed class _CoroutineSpawnMinions_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _enemySpawned___0;

		internal BossProfessor _this;

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

		public _CoroutineSpawnMinions_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._enemySpawned___0 = 0;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._enemySpawned___0 < 2)
			{
				this._this.SpawnMinionsFromSide(this._this.pointSpawnLeft);
				this._this.SpawnMinionsFromSide(this._this.pointSpawnRight);
				this._enemySpawned___0++;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
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

	[Header("BOSS PROFESSOR PROPERTIES")]
	public BaseBullet bulletSatellitePrefab;

	[SpineAnimation("", "", true, false)]
	public string pulse;

	[SpineAnimation("", "", true, false)]
	public string extendSub;

	[SpineAnimation("", "", true, false)]
	public string narrowSub;

	[SpineAnimation("", "", true, false)]
	public string idleToImmortal;

	[SpineAnimation("", "", true, false)]
	public string immortal;

	[SpineAnimation("", "", true, false)]
	public string laugh;

	public Transform groupSatellite;

	public AudioClip soundShoot;

	public AudioClip soundPulse;

	public AudioClip soundAppear;

	public AudioClip soundLaugh;

	public GameObject effectImmortal;

	public BossProfessorEnergyPulse energyPulse;

	public BossProfessorSatellite[] satellites;

	public BaseEnemy[] minionPrefabs;

	[SerializeField]
	private bool flagShoot;

	[SerializeField]
	private bool flagPulse;

	[SerializeField]
	private bool flagSpawn;

	private bool flagMovingEntrance = true;

	private bool flagRotate;

	private bool isRotateRight;

	private bool isAtBase;

	private float timerShoot;

	private int numberSatelliteActive;

	private float mostLeftPointX;

	private float mostRightPointX;

	private Vector2 pointSpawnLeft;

	private Vector2 pointSpawnRight;

	private WaitForSeconds delayImmortalSpawn = new WaitForSeconds(3f);

	private Dictionary<GameObject, BaseUnit> minions = new Dictionary<GameObject, BaseUnit>();

	protected override void Start()
	{
		base.Start();
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, new Action<Component, object>(this.OnUnitDie));
		EventDispatcher.Instance.RegisterListener(EventID.BossProfessorSatelliteDie, delegate(Component sender, object param)
		{
			this.OnSatelliteDie();
		});
		this.numberSatelliteActive = this.satellites.Length;
		this.mostLeftPointX = this.basePosition.x - 5.4f;
		this.mostRightPointX = this.basePosition.x + 5.4f;
	}

	protected override void Update()
	{
		if (!this.isDead)
		{
			this.Entrance();
			this.RotateSatellite();
			if (this.isReadyAttack)
			{
				this.Attack();
			}
		}
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Boss/Boss Professor/boss_professor_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BossProfessorStats>(path);
	}

	protected override void Attack()
	{
		if (this.flagShoot)
		{
			if (this.timerShoot < ((SO_BossProfessorStats)this.baseStats).ShootDuration)
			{
				this.timerShoot += Time.deltaTime;
				float time = Time.time;
				if (time - this.lastTimeAttack > this.stats.AttackRate)
				{
					this.lastTimeAttack = time;
					this.Shoot();
				}
			}
			else
			{
				this.flagShoot = false;
				this.DeactiveRotate();
				this.skeletonAnimation.AnimationState.SetAnimation(2, this.narrowSub, false);
				if (this.isAtBase && this.numberSatelliteActive > 0)
				{
					Vector2 vector = base.transform.position;
					float num = UnityEngine.Random.Range(3f, 5.4f);
					vector.x = ((UnityEngine.Random.Range(0, 2) != 0) ? (vector.x - num) : (vector.x + num));
					vector.y += UnityEngine.Random.Range(0.5f, 1f);
					float num2 = Vector2.Distance(base.transform.position, vector);
					base.transform.DOMove(vector, num2 / this.baseStats.MoveSpeed, false).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(delegate
					{
						this.isAtBase = false;
						this.ActiveShoot(false);
					});
				}
				else
				{
					Vector2 vector2 = base.transform.position;
					vector2.x = Mathf.Clamp(this.target.transform.position.x, this.mostLeftPointX, this.mostRightPointX);
					float num3 = Vector2.Distance(base.transform.position, vector2);
					base.transform.DOMove(vector2, num3 / this.baseStats.MoveSpeed, false).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(delegate
					{
						this.ActivePulse();
					});
				}
			}
		}
		else if (this.flagPulse)
		{
			this.flagPulse = false;
			this.skeletonAnimation.AnimationState.SetAnimation(1, this.pulse, false);
		}
		else if (this.flagSpawn)
		{
			this.flagSpawn = false;
			base.StartCoroutine(this.CoroutineSpawnMinions());
		}
	}

	protected override void StartDie()
	{
		base.StartDie();
		this.DeactiveAllSatellites();
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		base.HandleAnimationCompleted(entry);
		if (this.isDead)
		{
			return;
		}
		if (string.Compare(entry.animation.name, this.idleToImmortal) == 0)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(3, this.immortal, false);
		}
		if (string.Compare(entry.animation.name, this.pulse) == 0)
		{
			this.energyPulse.Active(true);
			this.ActiveSoundPulse(true);
			Vector2 tmp = base.transform.position;
			Vector2 vector = base.transform.position;
			vector.x = ((base.transform.position.x > this.basePosition.x) ? Mathf.Clamp(vector.x - 6f, this.mostLeftPointX, this.mostRightPointX) : Mathf.Clamp(vector.x + 6f, this.mostLeftPointX, this.mostRightPointX));
			float s = Vector2.Distance(base.transform.position, vector);
			base.transform.DOMove(vector, s / this.baseStats.MoveSpeed, false).SetDelay(1f).SetEase(Ease.Linear).OnComplete(delegate
			{
				s = Vector2.Distance(this.transform.position, tmp);
				this.transform.DOMove(tmp, s / this.baseStats.MoveSpeed, false).SetDelay(1f).SetEase(Ease.Linear).OnComplete(delegate
				{
					this.energyPulse.Active(false);
					this.ActiveSoundPulse(false);
					this.PlayAnimationIdle();
					s = Vector2.Distance(this.transform.position, this.basePosition);
					this.transform.DOMove(this.basePosition, s / this.baseStats.MoveSpeed, false).SetDelay(1f).SetEase(Ease.Linear).OnComplete(delegate
					{
						this.isAtBase = true;
						if (this.numberSatelliteActive > 0)
						{
							this.ActiveShoot(true);
						}
						else
						{
							this.ActiveSpawn();
						}
					});
				});
			});
		}
	}

	public override void Renew()
	{
		this.isDead = false;
		this.LoadScriptableObject();
		this.stats.Init(this.baseStats);
		this.isFinalBoss = true;
		this.bodyCollider.enabled = false;
		this.DeactiveRotate();
		this.isEffectMeleeWeapon = false;
		this.isReadyAttack = false;
		this.target = null;
		base.transform.parent = null;
		this.UpdateHealthBar(false);
	}

	public override void UpdateHealthBar(bool isAutoHide = false)
	{
		Singleton<UIController>.Instance.hudBoss.SetIconBoss(this.id);
		Singleton<UIController>.Instance.hudBoss.UpdateHP(this.HpPercent);
	}

	public void SetPointSpawnMinion(Vector2 leftPoint, Vector2 rightPoint)
	{
		this.pointSpawnLeft = leftPoint;
		this.pointSpawnRight = rightPoint;
	}

	private void RotateSatellite()
	{
		if (this.flagRotate)
		{
			float rotateSpeed = ((SO_BossProfessorStats)this.baseStats).RotateSpeed;
			if (this.isRotateRight)
			{
				this.groupSatellite.Rotate(0f, 0f, -rotateSpeed * Time.deltaTime);
			}
			else
			{
				this.groupSatellite.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
			}
		}
	}

	private void Entrance()
	{
		if (this.flagMovingEntrance)
		{
			this.flagMovingEntrance = false;
			base.transform.DOMove(this.basePosition, 3f, false).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(delegate
			{
				this.Init();
				this.isAtBase = true;
				this.ActiveImmortal(false);
				this.ActiveShoot(true);
			}).OnStart(delegate
			{
				this.PlaySound(this.soundAppear);
				Singleton<CameraFollow>.Instance.AddShake(0.5f, 3f);
				this.PlayAnimationIdle();
			});
		}
	}

	private void Init()
	{
		for (int i = 0; i < this.satellites.Length; i++)
		{
			this.satellites[i].Init();
		}
	}

	private void Shoot()
	{
		this.PlaySound(this.soundShoot);
		for (int i = 0; i < this.satellites.Length; i++)
		{
			this.satellites[i].Shoot();
		}
	}

	private void ActiveImmortal(bool isActive)
	{
		if (!isActive)
		{
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(3, 0f);
		}
		else
		{
			this.skeletonAnimation.AnimationState.SetAnimation(3, this.idleToImmortal, false);
		}
		this.effectImmortal.SetActive(isActive);
		this.isImmortal = isActive;
		this.bodyCollider.enabled = !isActive;
	}

	private void ActiveRotate(bool isRotateRight)
	{
		this.flagRotate = true;
		this.isRotateRight = isRotateRight;
	}

	private void DeactiveRotate()
	{
		this.flagRotate = false;
	}

	private void DeactiveAllSatellites()
	{
		for (int i = 0; i < this.satellites.Length; i++)
		{
			if (!this.satellites[i].isDead)
			{
				this.satellites[i].Deactive();
			}
		}
	}

	private void ActiveSoundPulse(bool isActive)
	{
		if (isActive)
		{
			if (this.audioSource.clip == null)
			{
				this.audioSource.loop = true;
				this.audioSource.clip = this.soundPulse;
				this.audioSource.Play();
			}
		}
		else
		{
			this.audioSource.Stop();
			this.audioSource.clip = null;
		}
	}

	private void ActiveShoot(bool isRotateRight)
	{
		this.isReadyAttack = false;
		this.timerShoot = 0f;
		this.PlayAnimationIdle();
		this.skeletonAnimation.AnimationState.SetAnimation(2, this.extendSub, false);
		this.ActiveRotate(isRotateRight);
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagShoot = true;
			this.flagPulse = false;
			this.flagSpawn = false;
		}, StaticValue.waitOneSec));
	}

	private void ActivePulse()
	{
		this.isReadyAttack = false;
		this.timerShoot = 0f;
		this.PlayAnimationIdle();
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.isReadyAttack = true;
			this.flagShoot = false;
			this.flagPulse = true;
			this.flagSpawn = false;
		}, StaticValue.waitOneSec));
	}

	private void ActiveSpawn()
	{
		this.isReadyAttack = false;
		this.timerShoot = 0f;
		this.PlayAnimationIdle();
		this.PlaySound(this.soundLaugh);
		this.skeletonAnimation.AnimationState.SetAnimation(1, this.laugh, true);
		base.StartCoroutine(base.DelayAction(delegate
		{
			this.ActiveImmortal(true);
			this.isReadyAttack = true;
			this.flagShoot = false;
			this.flagPulse = false;
			this.flagSpawn = true;
		}, this.delayImmortalSpawn));
	}

	private void OnSatelliteDie()
	{
		this.numberSatelliteActive--;
		if (this.numberSatelliteActive <= 0)
		{
			this.ActiveImmortal(false);
		}
		else if (this.numberSatelliteActive <= 4)
		{
			this.ActiveImmortal(true);
		}
	}

	private IEnumerator CoroutineSpawnMinions()
	{
		BossProfessor._CoroutineSpawnMinions_c__Iterator0 _CoroutineSpawnMinions_c__Iterator = new BossProfessor._CoroutineSpawnMinions_c__Iterator0();
		_CoroutineSpawnMinions_c__Iterator._this = this;
		return _CoroutineSpawnMinions_c__Iterator;
	}

	private void SpawnMinionsFromSide(Vector2 position)
	{
		int num = UnityEngine.Random.Range(0, this.minionPrefabs.Length);
		int id = this.minionPrefabs[num].id;
		int enemyMinLevel = ((SO_BossProfessorStats)this.baseStats).EnemyMinLevel;
		int enemyMaxLevel = ((SO_BossProfessorStats)this.baseStats).EnemyMaxLevel;
		int level = UnityEngine.Random.Range(enemyMinLevel, enemyMaxLevel + 1);
		BaseEnemy enemyPrefab = Singleton<GameController>.Instance.GetEnemyPrefab(id);
		BaseEnemy fromPool = enemyPrefab.GetFromPool();
		fromPool.Active(id, level, position);
		fromPool.zoneId = -1;
		fromPool.isRunPassArea = true;
		fromPool.DelayTargetPlayer();
		Singleton<GameController>.Instance.AddUnit(fromPool.gameObject, fromPool);
		this.minions.Add(fromPool.gameObject, fromPool);
	}

	private void OnUnitDie(Component senser, object param)
	{
		UnitDieData unitDieData = (UnitDieData)param;
		BaseEnemy component = unitDieData.unit.GetComponent<BaseEnemy>();
		if (this.minions.ContainsKey(component.gameObject))
		{
			this.minions.Remove(component.gameObject);
			if (this.minions.Count <= 0)
			{
				if (this.numberSatelliteActive > 0)
				{
					this.ActiveShoot(true);
				}
				else
				{
					this.ActiveImmortal(false);
					Vector2 vector = base.transform.position;
					vector.x = Mathf.Clamp(this.target.transform.position.x, this.mostLeftPointX, this.mostRightPointX);
					float num = Vector2.Distance(base.transform.position, vector);
					base.transform.DOMove(vector, num / this.baseStats.MoveSpeed, false).SetDelay(0.5f).SetEase(Ease.Linear).OnComplete(delegate
					{
						this.ActivePulse();
					});
				}
			}
		}
	}
}
