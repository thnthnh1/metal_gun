using Spine.Unity;
using System;
using UnityEngine;

public class BaseMeleeWeaponPreview : MonoBehaviour
{
	public int id;

	public MeleeWeaponType type;

	public SO_MeleeWeaponStats baseStats;

	public WindBlade windEffect;

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
}
