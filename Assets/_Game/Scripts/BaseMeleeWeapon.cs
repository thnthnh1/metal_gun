using Spine.Unity;
using System;
using UnityEngine;

public class BaseMeleeWeapon : BaseWeapon
{
	[Header("BASE STATS")]
	public SO_MeleeWeaponStats baseStats;

	public WindBlade windEffect;

	[Header("MELEE WEAPON PROPERTIES")]
	public MeleeWeaponType type;

	public BaseEffect effectTextPrefab;

	public override void LoadScriptableObject()
	{
	}

	public override void Init(int level)
	{
		base.Init(level);
		this.damage = this.baseStats.Damage;
		this.attackTimePerSecond = this.baseStats.AttackTimePerSecond;
		this.criticalRate = this.baseStats.CriticalRate;
		this.criticalDamageBonus = this.baseStats.CriticalDamageBonus;
	}

	public override void Attack(AttackData attackData)
	{
	}

	public virtual void ActiveEffect(bool isActive)
	{
		if (this.windEffect)
		{
			this.windEffect.Active(isActive);
		}
	}

	public virtual void InitEffect(SkeletonAnimation skeleton, string boneName)
	{
		if (this.windEffect)
		{
			BoneFollower boneFollower = this.windEffect.gameObject.AddComponent<BoneFollower>();
			boneFollower.skeletonRenderer = skeleton;
			boneFollower.boneName = boneName;
			this.windEffect.transform.parent = null;
		}
	}

	public virtual void SpawnEffectText(Vector2 position, Transform parent = null)
	{
		EffectTextWHAM effectTextWHAM = Singleton<PoolingController>.Instance.poolTextWHAM.New();
		if (effectTextWHAM == null)
		{
			effectTextWHAM = (UnityEngine.Object.Instantiate<BaseEffect>(this.effectTextPrefab) as EffectTextWHAM);
		}
		effectTextWHAM.Active(position, parent);
	}
}
