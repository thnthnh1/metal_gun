using Spine;
using Spine.Unity;
using System;
using System.Linq;
using UnityEngine;

public class EnemyGrenade : BaseEnemy
{
	[Header("ENEMY GRENADE PROPERTIES")]
	public MeshRenderer[] frontWeaponParts;

	public MeshRenderer[] behindWeaponParts;

	public BaseGunEnemy[] gunPrefabs;

	public BaseGrenadeEnemy grenadePrefab;

	public Transform throwStartPoint;

	public Vector2 throwDirection;

	public GameObject grenade;

	[SpineAnimation("", "", true, false)]
	public string idleNade;

	[SpineAnimation("", "", true, false)]
	public string idleGun;

	private BaseGunEnemy gun;

	private bool isUsingGun;

	[SerializeField]
	private bool flagThrow;

	private int bulletShot;

	private int grenadeThrew;

	private float lastTimeThrowGrenade;

	private Vector2 destinationThrow;

	protected override void Update()
	{
		if (!this.isDead)
		{
			this.UpdateDirection();
			this.Idle();
			this.Patrol();
			this.Attack();
		}
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Enemy/Enemy Grenade/enemy_grenade_lv{0}", this.level);
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
		this.grenade.GetComponent<SpriteRenderer>().sortingOrder = num + 1;
		for (int i = 0; i < this.frontWeaponParts.Length; i++)
		{
			this.frontWeaponParts[i].sortingOrder = num + 1;
		}
		for (int j = 0; j < this.behindWeaponParts.Length; j++)
		{
			this.behindWeaponParts[j].sortingOrder = num - 1;
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
			this.GetCloseToTarget();
			if (this.isReadyAttack && !this.flagThrow)
			{
				float time = Time.time;
				if (this.isUsingGun)
				{
					if (time - this.lastTimeAttack > this.stats.AttackRate)
					{
						this.lastTimeAttack = time;
						this.PlayAnimationShoot(1);
					}
				}
				else if (time - this.lastTimeThrowGrenade > 3f)
				{
					this.lastTimeThrowGrenade = time;
					this.flagThrow = true;
					this.destinationThrow = this.target.transform.position;
					this.PlayAnimationThrow();
				}
			}
		}
	}

	protected override void Die()
	{
		base.Die();
		EventDispatcher.Instance.PostEvent(EventID.KillEnemyGrenade);
	}

	protected override void ReleaseAttack()
	{
		base.ReleaseAttack();
		this.gun.Attack(this);
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
		if (string.Compare(entry.animation.name, this.shoot) == 0)
		{
			this.bulletShot++;
			if (this.bulletShot >= 3)
			{
				this.isReadyAttack = false;
				base.StartCoroutine(base.DelayAction(delegate
				{
					this.SwitchWeapon(false);
				}, StaticValue.waitHalfSec));
			}
		}
		if (string.Compare(entry.animation.name, this.throwGrenade) == 0)
		{
			this.flagThrow = false;
			this.grenadeThrew++;
			if (this.grenadeThrew >= 2)
			{
				this.isReadyAttack = false;
				base.StartCoroutine(base.DelayAction(delegate
				{
					this.SwitchWeapon(true);
				}, StaticValue.waitHalfSec));
			}
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(2, 0f);
		}
		if (string.Compare(entry.animation.name, this.aim) == 0)
		{
			base.Invoke("ReadyToAttack", 0.5f);
		}
	}

	protected override void TrackAimPoint()
	{
		if (this.target)
		{
			bool isActive = Mathf.Abs(base.transform.position.x - this.target.transform.position.x) >= 0.7f && this.isUsingGun;
			this.ActiveAim(isActive);
		}
		else
		{
			this.ActiveAim(false);
		}
	}

	protected override void ActiveAim(bool isActive)
	{
		if (isActive)
		{
			if (this.skeletonAnimation.AnimationState.GetCurrent(1) != null && string.Compare(this.skeletonAnimation.AnimationState.GetCurrent(1).animation.name, this.aim) == 0)
			{
				return;
			}
			this.skeletonAnimation.AnimationState.SetAnimation(1, this.aim, false);
		}
		else
		{
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
		}
	}

	protected override void PlayAnimationShoot(int trackIndex = 1)
	{
		this.skeletonAnimation.AnimationState.SetAnimation(2, this.shoot, false);
	}

	protected override void PlayAnimationThrow()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(2, this.throwGrenade, false);
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyGrenade enemyGrenade = Singleton<PoolingController>.Instance.poolEnemyGrenade.New();
		if (enemyGrenade == null)
		{
			enemyGrenade = UnityEngine.Object.Instantiate<EnemyGrenade>(this);
		}
		return enemyGrenade;
	}

	public override void Renew()
	{
		base.Renew();
		this.SwitchWeapon(false);
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyGrenade.Store(this);
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

	private void SwitchWeapon(bool isUsingGun)
	{
		this.isUsingGun = isUsingGun;
		this.gun.gameObject.SetActive(isUsingGun);
		this.grenade.SetActive(!isUsingGun);
		this.bulletShot = 0;
		this.grenadeThrew = 0;
		this.flagThrow = false;
		this.ActiveAim(isUsingGun);
		base.Invoke("ReadyToAttack", 1f);
	}

	public override void PlayAnimationIdle()
	{
		TrackEntry current = this.skeletonAnimation.AnimationState.GetCurrent(0);
		this.idle = ((!this.isUsingGun) ? this.idleNade : this.idleGun);
		if (current == null || string.Compare(current.animation.name, this.idle) != 0)
		{
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.idle, true);
		}
	}
}
