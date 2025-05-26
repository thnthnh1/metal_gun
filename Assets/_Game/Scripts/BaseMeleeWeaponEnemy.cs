using Spine.Unity;
using System;
using UnityEngine;

public class BaseMeleeWeaponEnemy : MonoBehaviour
{
	public SkeletonAnimation skeletonAnimation;

	[SpineBone("", "", true, false)]
	public string windBone;

	private BoneFollower bone;

	private void Awake()
	{
		this.bone = base.gameObject.AddComponent<BoneFollower>();
	}

	public void Active(BaseEnemy shooter)
	{
		this.bone.skeletonRenderer = shooter.skeletonAnimation;
		this.bone.boneName = shooter.knifeBone;
		this.bone.followBoneRotation = true;
		this.bone.followZPosition = true;
		this.bone.followLocalScale = false;
		this.bone.followSkeletonFlip = true;
		base.gameObject.SetActive(true);
	}
}
