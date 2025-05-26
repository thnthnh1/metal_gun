using Spine.Unity;
using System;
using UnityEngine;

public class BaseGunEnemy : MonoBehaviour
{
	public SpriteRenderer spr;

	public Transform firePoint;

	public Transform muzzlePoint;

	public BaseBullet bulletPrefab;

	public BaseMuzzle muzzlePrefab;

	private BaseMuzzle muzzle;

	private BoneFollower bone;

	private void Awake()
	{
		this.bone = base.gameObject.AddComponent<BoneFollower>();
	}

	public void Active(BaseEnemy shooter)
	{
		this.bone.skeletonRenderer = shooter.skeletonAnimation;
		this.bone.boneName = shooter.gunBone;
		this.bone.followBoneRotation = true;
		this.bone.followZPosition = true;
		this.bone.followLocalScale = false;
		this.bone.followSkeletonFlip = true;
		base.gameObject.SetActive(true);
	}

	public virtual void Attack(BaseEnemy attacker)
	{
		if (this.muzzle == null)
		{
			this.muzzle = UnityEngine.Object.Instantiate<BaseMuzzle>(this.muzzlePrefab, this.muzzlePoint.position, this.muzzlePoint.rotation, this.muzzlePoint.parent);
		}
		this.muzzle.Active();
	}
}
