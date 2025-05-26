using Spine;
using System;
using System.Linq;
using UnityEngine;

public class EnemyBazooka : BaseEnemy
{
	[Header("ENEMY BAZOOKA PROPERTIES")]
	public BaseGunEnemy[] gunPrefabs;

	public MeshRenderer[] frontWeaponParts;

	public MeshRenderer[] behindWeaponParts;

	private BaseGunEnemy gun;

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
		string path = string.Format("Scriptable Object/Enemy/Enemy Bazooka/enemy_bazooka_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BaseUnitStats>(path);
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
			this.behindWeaponParts[j].sortingOrder = num;
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
			if (this.isReadyAttack)
			{
				float time = Time.time;
				if (time - this.lastTimeAttack > this.stats.AttackRate)
				{
					this.lastTimeAttack = time;
					this.PlayAnimationShoot(1);
				}
			}
		}
	}

	protected override void ReleaseAttack()
	{
		base.ReleaseAttack();
		this.gun.Attack(this);
	}

	protected override void ActiveAim(bool isActive)
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

	protected override void HandleAnimationCompleted(TrackEntry entry)
	{
		base.HandleAnimationCompleted(entry);
		if (this.isDead)
		{
			return;
		}
		if (string.Compare(entry.animation.name, this.aim) == 0)
		{
			base.Invoke("ReadyToAttack", 0.5f);
		}
		if (string.Compare(entry.animation.name, this.shoot) == 0)
		{
			this.PlayAnimationIdle();
		}
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyBazooka enemyBazooka = Singleton<PoolingController>.Instance.poolEnemyBazooka.New();
		if (enemyBazooka == null)
		{
			enemyBazooka = UnityEngine.Object.Instantiate<EnemyBazooka>(this);
		}
		return enemyBazooka;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyBazooka.Store(this);
	}

	public override void OnUnitGetInFarSensor(BaseUnit unit)
	{
		this.SetTarget(unit);
		this.PlayAnimationIdle();
	}
}
