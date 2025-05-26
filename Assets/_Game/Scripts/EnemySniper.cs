using Spine;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySniper : BaseEnemy
{
	[Header("ENEMY SNIPER PROPERTIES")]
	public MeshRenderer[] frontWeaponParts;

	public MeshRenderer[] behindWeaponParts;

	public BaseGunEnemy[] gunPrefabs;

	public BaseMeleeWeaponEnemy[] knifePrefabs;

	public WindBlade windEffect;

	private GunEnemyAWP gun;

	private BaseMeleeWeaponEnemy knife;

	private bool isUsingGun;

	[SerializeField]
	private bool flagKnife;

	private float lastTimeKnife;

	protected override void Update()
	{
		if (!this.isDead)
		{
			this.UpdateDirection();
			this.TrackAimPoint();
			this.Idle();
			this.Attack();
			if (this.gun)
			{
				this.gun.ActiveLaserAim(this.target);
			}
		}
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Enemy/Enemy Sniper/enemy_sniper_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_EnemySniperStats>(path);
	}

	protected override void InitWeapon()
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
		if (this.gunPrefabs.Length > 0)
		{
			if (num > this.gunPrefabs.Length - 1)
			{
				num = 0;
			}
			this.gun = (UnityEngine.Object.Instantiate<BaseGunEnemy>(this.gunPrefabs[num], base.transform) as GunEnemyAWP);
			this.gun.Active(this);
		}
		if (this.knifePrefabs.Length > 0)
		{
			if (num > this.knifePrefabs.Length - 1)
			{
				num = 0;
			}
			this.knife = UnityEngine.Object.Instantiate<BaseMeleeWeaponEnemy>(this.knifePrefabs[num], base.transform);
			this.knife.Active(this);
		}
	}

	protected override void InitSortingLayerSpine()
	{
		int num = UnityEngine.Random.Range(200, 700);
		this.gun.spr.sortingOrder = num;
		this.knife.GetComponent<MeshRenderer>().sortingOrder = num;
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
			bool isActive = Mathf.Abs(base.transform.position.x - this.target.transform.position.x) >= 0.7f && !this.flagKnife;
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
			float time = Time.time;
			if (this.isUsingGun)
			{
				if (this.flagKnife)
				{
					return;
				}
				if (time - this.lastTimeAttack > this.stats.AttackRate)
				{
					this.lastTimeAttack = time;
					this.PlayAnimationShoot(1);
				}
			}
			else
			{
				if (this.flagKnife)
				{
					return;
				}
				if (!this.flagGetCloseToTarget)
				{
					float num = 1f / ((SO_EnemySniperStats)this.baseStats).KnifeAttackTimePerSecond;
					if (time - this.lastTimeKnife > num)
					{
						this.lastTimeKnife = time;
						this.flagKnife = true;
						this.skeletonAnimation.AnimationState.SetAnimation(1, this.meleeAttack, false);
						this.windEffect.Active(true);
					}
				}
			}
		}
	}

	protected override void Die()
	{
		base.Die();
		EventDispatcher.Instance.PostEvent(EventID.KillEnemySniper);
	}

	protected override void ReleaseAttack()
	{
		base.ReleaseAttack();
		this.gun.Attack(this);
	}

	protected override void SetCloseRange()
	{
		if (this.nearSensor != null && this.nearSensor != null)
		{
			this.closeUpRange = UnityEngine.Random.Range(0.9f, 1.25f);
			this.nearSensor.col.radius = this.closeUpRange;
		}
	}

	public override void TakeDamage(AttackData attackData)
	{
		base.TakeDamage(attackData);
		if (this.isDead && attackData.weaponId != -1)
		{
			StaticGunData data = GameData.staticGunData.GetData(attackData.weaponId);
			if (data != null && data.id == 3)
			{
				EventDispatcher.Instance.PostEvent(EventID.KillEnemySniperByGunAWP);
			}
		}
	}

	public override BaseEnemy GetFromPool()
	{
		EnemySniper enemySniper = Singleton<PoolingController>.Instance.poolEnemySniper.New();
		if (enemySniper == null)
		{
			enemySniper = UnityEngine.Object.Instantiate<EnemySniper>(this);
		}
		return enemySniper;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemySniper.Store(this);
	}

	public override void Renew()
	{
		base.Renew();
		this.flagKnife = false;
		this.SwitchWeapon(true);
		this.gun.ActiveLaserAim(false);
	}

	public override void OnUnitGetInNearSensor(BaseUnit unit)
	{
		this.flagGetCloseToTarget = false;
		this.PlayAnimationIdle();
		this.StopMoving();
		if (!this.nearbyVictims.Contains(unit))
		{
			this.nearbyVictims.Add(unit);
		}
		this.SwitchWeapon(false);
	}

	public override void OnUnitGetOutNearSensor(BaseUnit unit)
	{
		this.isReadyAttack = false;
		this.SwitchWeapon(true);
		base.StartCoroutine(base.DelayAction(new UnityAction(this.ReadyToAttack), StaticValue.waitOneSec));
	}

	public override void OnUnitGetInFarSensor(BaseUnit unit)
	{
		this.SetTarget(unit);
		this.flagGetCloseToTarget = false;
	}

	public override void OnUnitGetOutFarSensor(BaseUnit unit)
	{
		this.CancelCombat();
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		base.HandleAnimationEvent(trackEntry, e);
		if (string.Compare(e.Data.Name, this.eventMeleeAttack) == 0 && !this.isDead)
		{
			for (int i = 0; i < this.nearbyVictims.Count; i++)
			{
				AttackData curentAttackData = this.GetCurentAttackData();
				this.nearbyVictims[i].TakeDamage(curentAttackData);
			}
		}
	}

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		base.HandleAnimationCompleted(entry);
		if (this.isDead)
		{
			return;
		}
		if (string.Compare(entry.animation.name, this.meleeAttack) == 0)
		{
			this.flagKnife = false;
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
		}
	}

	private void SwitchWeapon(bool isUsingGun)
	{
		this.isUsingGun = isUsingGun;
		this.gun.gameObject.SetActive(isUsingGun);
		this.knife.gameObject.SetActive(!isUsingGun);
		this.ResetAim();
	}
}
