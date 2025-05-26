using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class WindBlade : MonoBehaviour
{
	public float animTimeScale = 1f;

	public SkeletonAnimation skeletonAnimation;

	[SpineAnimation("", "", true, false)]
	public string animAttack;

	public bool isDeactiveCompleteAnimation = true;

	private void Awake()
	{
		this.skeletonAnimation.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.HandleSpineEventCompleted);
	}

	public void Active(bool isActive)
	{
		if (isActive)
		{
			base.gameObject.SetActive(true);
			this.skeletonAnimation.AnimationState.SetAnimation(0, this.animAttack, false).TimeScale = this.animTimeScale;
		}
		else
		{
			base.gameObject.SetActive(false);
		}
	}

	private void HandleSpineEventCompleted(TrackEntry entry)
	{
		if (this.isDeactiveCompleteAnimation && string.Compare(entry.animation.name, this.animAttack) == 0)
		{
			this.skeletonAnimation.AnimationState.SetEmptyAnimation(1, 0f);
			base.gameObject.SetActive(false);
		}
	}
}
