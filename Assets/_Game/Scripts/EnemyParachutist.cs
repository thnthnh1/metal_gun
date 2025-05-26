using Spine;
using Spine.Unity;
using System;
using System.Linq;
using UnityEngine;

public class EnemyParachutist : BaseEnemy
{
	[Header("ENEMY PARACHUTIST PROPERTIES")]
	public Transform groundUnderCheckPoint;

	[SpineAnimation("", "", true, false)]
	public string parachute;

	public MeshRenderer[] frontWeaponParts;

	public MeshRenderer[] behindWeaponParts;

	public BaseGunEnemy[] gunPrefabs;

	[SerializeField]
	private bool isParachuting;

	private BaseGunEnemy gun;

	protected override void Update()
	{
		if (!this.isDead)
		{
			if (this.isParachuting)
			{
				this.CheckGround();
			}
			this.UpdateDirection();
			this.TrackAimPoint();
			if (!this.isParachuting)
			{
				this.Idle();
				this.Patrol();
			}
			this.Attack();
		}
	}

	protected override void LoadScriptableObject()
	{
		string path = string.Format("Scriptable Object/Enemy/Enemy Parachutist/enemy_parachutist_lv{0}", this.level);
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
			if (!this.isParachuting)
			{
				this.GetCloseToTarget();
			}
			float time = Time.time;
			if (time - this.lastTimeAttack > this.stats.AttackRate)
			{
				this.lastTimeAttack = time;
				this.PlayAnimationShoot(1);
			}
		}
	}

	protected override void ReleaseAttack()
	{
		base.ReleaseAttack();
		this.gun.Attack(this);
	}

	protected override void StartDie()
	{
		base.StartDie();
		this.rigid.bodyType = RigidbodyType2D.Dynamic;
	}

	protected override void PlayAnimationShoot(int trackIndex = 1)
	{
		TrackEntry trackEntry;
		if (this.isParachuting)
		{
			trackEntry = this.skeletonAnimation.AnimationState.SetAnimation(1, this.shoot, false);
		}
		else if (this.flagGetCloseToTarget)
		{
			trackEntry = this.skeletonAnimation.AnimationState.SetAnimation(1, this.shoot, false);
		}
		else
		{
			trackEntry = this.skeletonAnimation.AnimationState.SetAnimation(0, this.shoot, false);
		}
		trackEntry.AttachmentThreshold = 1f;
		trackEntry.MixDuration = 0f;
		TrackEntry trackEntry2 = this.skeletonAnimation.AnimationState.AddEmptyAnimation(1, 0.5f, 0.1f);
		trackEntry2.AttachmentThreshold = 1f;
	}

	protected override void CancelCombat()
	{
		this.target = null;
		if (this.isParachuting)
		{
			this.PlayAnimationParachute();
		}
		else
		{
			base.SwitchState(EnemyState.Idle);
		}
	}

	protected override void TrackAimPoint()
	{
		if (this.target)
		{
			bool isActive = Mathf.Abs(base.transform.position.x - this.target.transform.position.x) >= 0.7f;
			this.ActiveAim(isActive);
			if (this.isParachuting)
			{
				this.aimBone.transform.position = this.target.BodyCenterPoint.position;
			}
			else
			{
				this.aimBone.transform.position = this.aimPoint.position;
			}
		}
		else
		{
			this.ActiveAim(false);
		}
	}

	public override void OnUnitGetInFarSensor(BaseUnit unit)
	{
		this.SetTarget(unit);
		if (this.isParachuting)
		{
			return;
		}
		if (Vector2.Distance(this.target.transform.position, base.BodyCenterPoint.position) > this.nearSensor.col.radius && this.canMove)
		{
			this.flagGetCloseToTarget = true;
			this.PlayAnimationMoveFast();
		}
	}

	public override void OnUnitGetOutFarSensor(BaseUnit unit)
	{
		if (this.isParachuting)
		{
			this.CancelCombat();
			return;
		}
		if (this.canMove)
		{
			this.farSensor.gameObject.SetActive(false);
			base.StartCoroutine(base.DelayAction(delegate
			{
				this.farSensor.gameObject.SetActive(true);
				this.flagGetCloseToTarget = false;
				this.StartChasingTarget();
			}, StaticValue.waitHalfSec));
		}
		else
		{
			this.CancelCombat();
		}
	}

	public override void OnUnitGetInNearSensor(BaseUnit unit)
	{
		if (this.isParachuting)
		{
			return;
		}
		if (this.canMove)
		{
			this.flagGetCloseToTarget = false;
			this.PlayAnimationIdle();
			this.StopMoving();
		}
	}

	public override void OnUnitGetOutNearSensor(BaseUnit unit)
	{
		if (this.isParachuting)
		{
			return;
		}
		if (this.canMove)
		{
			this.nearSensor.gameObject.SetActive(false);
			base.StartCoroutine(base.DelayAction(delegate
			{
				this.nearSensor.gameObject.SetActive(true);
				this.flagGetCloseToTarget = true;
				this.flagMove = true;
				this.PlayAnimationMoveFast();
			}, StaticValue.waitHalfSec));
		}
	}

	public override void Renew()
	{
		base.Renew();
		this.rigid.bodyType = RigidbodyType2D.Kinematic;
		this.PlayAnimationParachute();
		this.isParachuting = true;
	}

	public override BaseEnemy GetFromPool()
	{
		EnemyParachutist enemyParachutist = Singleton<PoolingController>.Instance.poolEnemyParachutist.New();
		if (enemyParachutist == null)
		{
			enemyParachutist = UnityEngine.Object.Instantiate<EnemyParachutist>(this);
		}
		return enemyParachutist;
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolEnemyParachutist.Store(this);
	}

	private void CheckGround()
	{
		bool flag = Physics2D.Linecast(base.transform.position, this.groundUnderCheckPoint.position, this.layerMaskCheckObstacle);
		if (flag)
		{
			this.isParachuting = false;
			this.rigid.bodyType = RigidbodyType2D.Dynamic;
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.idle, false);
			this.UpdateDirection();
			this.ResetAim();
		}
		if (this.isParachuting)
		{
			base.transform.Translate(Vector3.down * 1f * Time.deltaTime, Space.World);
		}
	}

	private void PlayAnimationParachute()
	{
		this.skeletonAnimation.AnimationState.SetAnimation(0, this.parachute, true);
	}
}
