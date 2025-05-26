using Spine;
using System;
using System.Linq;
using UnityEngine;

public class EnemyGeneral : BaseEnemy
{
	[Header("ENEMY GENERAL PROPERTIES")]
	public MeshRenderer[] frontWeaponParts;

	public MeshRenderer[] behindWeaponParts;

	public BaseGunEnemy[] gunPrefabs;

	public BaseGrenadeEnemy grenadePrefab;

	public Transform throwStartPoint;

	public Vector2 throwDirection;

	public float timeSwitchGrenade = 3f;

	private BaseGunEnemy gun;

	[SerializeField]
	private bool flagThrow;

	private float timeCheckThrowGrenade;

	private Vector2 destinationThrow;

	protected override void Awake()
	{
		base.Awake();
		EventDispatcher.Instance.RegisterListener(EventID.EnemyShootHitPlayer, new Action<Component, object>(this.OnShootHitPlayer));
	}

	protected override void Update()
	{
		if (!this.isDead)
		{
			this.UpdateDirection();
			this.TrackAimPoint();
			this.Idle();
			this.Patrol();
			this.Attack();
		}
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Enemy/Enemy General/enemy_general_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_EnemyHasProjectileStats>(path);
	}

	protected override void InitWeapon()
	{
		if (this.gunPrefabs.Length > 0)
		{
			int num = 0;
			if (GameData.mode == GameMode.Campaign)
			{
				int num2 = int.Parse(Singleton<GameController>.Instance.CampaignMap.stageNameId.Split(new char[]
				{
					'.'
				}).First<string>());
				num = num2 - 1;
			}
			else if (GameData.mode == GameMode.Survival)
			{
				num = UnityEngine.Random.Range(0, this.gunPrefabs.Length);
			}
			if (num > this.gunPrefabs.Length - 1)
			{
				num = 0;
			}
			this.gun = UnityEngine.Object.Instantiate<BaseGunEnemy>(this.gunPrefabs[num], base.transform);
			this.gun.Active(this);
		}
	}

	protected override void InitSortingLayerSpine()
	{
		int num = UnityEngine.Random.Range(200, 700);
		this.gun.spr.sortingOrder = num;
		for (int i = 0; i < this.frontWeaponParts.Length; i++)
		{
			this.frontWeaponParts[i].sortingOrder = num + 1;
		}
		for (int j = 0; j < this.behindWeaponParts.Length; j++)
		{
			this.behindWeaponParts[j].sortingOrder = num - 1;
		}
	}

	protected override void TrackAimPoint()
	{
		if (this.target)
		{
			bool isActive = Mathf.Abs(base.transform.position.x - this.target.transform.position.x) >= 0.7f && !this.flagThrow;
			this.ActiveAim(isActive);
			if (this.aimBone != null)
			{
				this.aimBone.transform.position = this.target.BodyCenterPoint.position;
			}
		}
		else
		{
			this.ActiveAim(false);
		}
	}

	protected override void Attack()
	{
		if (this.state == EnemyState.Attack)
		{
			if (this.target == null || this.target.isDead)
			{
				this.CancelCombat();
				return;
			}
			if (this.flagThrow)
			{
				return;
			}
			this.GetCloseToTarget();
			float time = Time.time;
			if (time - this.timeCheckThrowGrenade > this.timeSwitchGrenade)
			{
				this.timeCheckThrowGrenade = Time.time;
				this.flagThrow = true;
				this.destinationThrow = this.target.transform.position;
				this.PlayAnimationThrow();
				return;
			}
			if (time - this.lastTimeAttack > this.stats.AttackRate)
			{
				this.lastTimeAttack = time;
				this.PlayAnimationShoot(1);
			}
		}
	}

	protected override void Die()
	{
		base.Die();
		EventDispatcher.Instance.PostEvent(EventID.KillEnemyGeneral);
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		base.HandleAnimationEvent(trackEntry, e);
		if (string.Compare(e.Data.Name, this.eventThrowGrenade) == 0 && !this.isDead)
		{
			this.ThrowGrenade(this.throwStartPoint.position, this.destinationThrow);
		}
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		base.HandleAnimationCompleted(entry);
		if (this.isDead)
		{
			return;
		}
		if (string.Compare(entry.animation.name, this.throwGrenade) == 0)
		{
			this.flagThrow = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
		}
	}

	protected override void ReleaseAttack()
	{
		base.ReleaseAttack();
		this.gun.Attack(this);
	}

	public override void Renew()
	{
		base.Renew();
		this.flagThrow = false;
	}

	public override void SetTarget(BaseUnit unit)
	{
		base.SetTarget(unit);
		this.timeCheckThrowGrenade = Time.time;
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyGeneral enemyGeneral = Singleton<PoolingController>.Instance.poolEnemyGeneral.New();
		if (enemyGeneral == null)
		{
			enemyGeneral = UnityEngine.Object.Instantiate<EnemyGeneral>(this);
		}
		return enemyGeneral;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyGeneral.Store(this);
	}

	private void ThrowGrenade(Vector3 startPoint, Vector3 endPoint)
	{
		BaseGrenadeEnemy baseGrenadeEnemy = Singleton<PoolingController>.Instance.poolBaseGrenadeEnemy.New();
		if (baseGrenadeEnemy == null)
		{
			baseGrenadeEnemy = UnityEngine.Object.Instantiate<BaseGrenadeEnemy>(this.grenadePrefab);
		}
		float projectileDamage = ((SO_EnemyHasProjectileStats)this.baseStats).ProjectileDamage;
		float projectileDamageRadius = ((SO_EnemyHasProjectileStats)this.baseStats).ProjectileDamageRadius;
		AttackData attackData = new AttackData(this, projectileDamage, projectileDamageRadius, false, WeaponType.NormalGun, -1, null);
		Vector2 vector = this.throwDirection;
		vector.x = ((!this.IsFacingRight) ? (-vector.x) : vector.x);
		baseGrenadeEnemy.Active(attackData, startPoint, endPoint, vector, Singleton<PoolingController>.Instance.groupGrenade);
		SoundManager.Instance.PlaySfx("sfx_throw_grenade", 0f);
	}

	private void OnShootHitPlayer(Component sender, object param)
	{
		AttackData attackData = (AttackData)param;
		if (object.ReferenceEquals(attackData.attacker, this))
		{
			this.timeCheckThrowGrenade = Time.time;
		}
	}
}
