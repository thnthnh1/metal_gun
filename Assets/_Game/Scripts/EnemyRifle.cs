using Spine;
using System;
using System.Linq;
using UnityEngine;

public class EnemyRifle : BaseEnemy
{
	[Header("ENEMY RIFLE PROPERTIES")]
	public bool isHideOnBush;

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
		string path = string.Format("Scriptable Object/Enemy/Enemy Rifle/enemy_rifle_lv{0}", this.level);
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
			this.CheckAllowAttackTarget();
			if (this.isAllowAttackTarget && this.isReadyAttack)
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

	protected override void Die()
	{
		base.Die();
		EventDispatcher.Instance.PostEvent(EventID.KillEnemyRifle);
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
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyRifle enemyRifle = Singleton<PoolingController>.Instance.poolEnemyRifle.New();
		if (enemyRifle == null)
		{
			enemyRifle = UnityEngine.Object.Instantiate<EnemyRifle>(this);
		}
		return enemyRifle;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyRifle.Store(this);
	}
}
