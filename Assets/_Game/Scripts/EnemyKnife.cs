using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyKnife : BaseEnemy
{
	[Header("ENEMY KNIFE PROPERTIES")]
	public MeshRenderer[] frontWeaponParts;

	public MeshRenderer[] behindWeaponParts;

	public WindBlade windEffect;

	[SpineAnimation("", "", true, false)]
	public string jumpForward;

	[SpineAnimation("", "", true, false)]
	public string idleShield;

	[SpineAnimation("", "", true, false)]
	public string meleeAttackShield;

	[SpineAnimation("", "", true, false)]
	public string walkShield;

	[SpineAnimation("", "", true, false)]
	public string runShield;

	[SpineAnimation("", "", true, false, startsWith = "die")]
	public List<string> dieShieldAnimationNames;

	public BaseMeleeWeaponEnemy[] knifePrefabs;

	private BaseMeleeWeaponEnemy knife;

	private bool flagKnife;

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
		string path = string.Format("Scriptable Object/Enemy/Enemy Knife/enemy_knife_lv{0}", this.level);
		this.baseStats = Resources.Load<SO_BaseUnitStats>(path);
	}

	protected override void InitSkin()
	{
		string skin = this.defaultSkin;
		if (GameData.mode == GameMode.Campaign)
		{
			int num = int.Parse(Singleton<GameController>.Instance.CampaignMap.stageNameId.Split(new char[]
			{
				'.'
			}).First<string>());
			MapType mapType = (MapType)num;
			if (mapType != MapType.Map_1_Desert)
			{
				if (mapType != MapType.Map_2_Lab)
				{
					if (mapType == MapType.Map_3_Jungle)
					{
						if (!string.IsNullOrEmpty(this.skinMap3))
						{
							skin = this.skinMap3;
						}
					}
				}
				else if (!string.IsNullOrEmpty(this.skinMap2))
				{
					skin = this.skinMap2;
					this.idle = this.idleShield;
					this.meleeAttack = this.meleeAttackShield;
					this.move = this.walkShield;
					this.moveFast = this.runShield;
					this.dieAnimationNames = this.dieShieldAnimationNames;
				}
			}
			else if (!string.IsNullOrEmpty(this.skinMap1))
			{
				skin = this.skinMap1;
			}
		}
		else if (GameData.mode == GameMode.Survival)
		{
			int num2 = UnityEngine.Random.Range(0, 3);
			if (num2 == 0)
			{
				if (!string.IsNullOrEmpty(this.skinMap1))
				{
					skin = this.skinMap1;
				}
			}
			else if (num2 == 1)
			{
				if (!string.IsNullOrEmpty(this.skinMap2))
				{
					skin = this.skinMap2;
					this.idle = this.idleShield;
					this.meleeAttack = this.meleeAttackShield;
					this.move = this.walkShield;
					this.moveFast = this.runShield;
					this.dieAnimationNames = this.dieShieldAnimationNames;
				}
			}
			else if (num2 == 2 && !string.IsNullOrEmpty(this.skinMap3))
			{
				skin = this.skinMap3;
			}
		}
		this.skeletonAnimation.Skeleton.SetSkin(skin);
	}

	protected override void InitWeapon()
	{
		if (this.knifePrefabs.Length > 0)
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
				num = UnityEngine.Random.Range(0, this.knifePrefabs.Length);
			}
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

	protected override void Attack()
	{
		if (this.state == EnemyState.Attack)
		{
			if (this.target == null || this.target.isDead)
			{
				this.CancelCombat();
				return;
			}
			if (this.flagKnife)
			{
				return;
			}
			this.GetCloseToTarget();
			if (!this.flagGetCloseToTarget)
			{
				float time = Time.time;
				if (time - this.lastTimeAttack > this.stats.AttackRate)
				{
					this.lastTimeAttack = time;
					this.flagKnife = true;
					this.PlayAnimationMeleeAttack();
					this.windEffect.Active(true);
				}
			}
		}
	}

	protected override void SetCloseRange()
	{
		if (this.nearSensor != null && this.nearSensor != null)
		{
			this.closeUpRange = UnityEngine.Random.Range(1f, 1.6f);
			this.nearSensor.col.radius = this.closeUpRange;
		}
	}

	protected override void HandleAnimationEvent(TrackEntry trackEntry, Spine.Event e)
	{
		if (string.Compare(e.Data.Name, this.eventMeleeAttack) == 0 && !this.isDead)
		{
			this.PlaySound(this.soundAttack);
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

	public override void Renew()
	{
		base.Renew();
		this.flagKnife = false;
		this.canJump = true;
	}

	public override void Active(EnemySpawnData spawnData)
	{
		base.Active(spawnData);
		this.canMove = true;
		this.canJump = true;
	}

	protected override void Die()
	{
		base.Die();
		EventDispatcher.Instance.PostEvent(EventID.KillEnemyKnife);
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyKnife enemyKnife = Singleton<PoolingController>.Instance.poolEnemyKnife.New();
		if (enemyKnife == null)
		{
			enemyKnife = UnityEngine.Object.Instantiate<EnemyKnife>(this);
		}
		return enemyKnife;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyKnife.Store(this);
	}
}
