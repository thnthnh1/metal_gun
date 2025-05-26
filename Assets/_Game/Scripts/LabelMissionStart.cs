using Spine;
using Spine.Unity;
using System;
using UnityEngine;

public class LabelMissionStart : MonoBehaviour
{
	public SkeletonGraphic skeletonGraphic;

	[SpineAnimation("", "", true, false)]
	public string anim;

	private void Awake()
	{
		base.gameObject.transform.localScale = Vector3.zero;
		this.skeletonGraphic.AnimationState.Complete += new Spine.AnimationState.TrackEntryDelegate(this.HandleAnimationCompleted);
	}

	public void Show()
	{
		base.gameObject.transform.localScale = Vector3.one;
		this.skeletonGraphic.AnimationState.SetAnimation(0, this.anim, false);
	}

	private void HandleAnimationCompleted(TrackEntry entry)
	{
		if (string.Compare(entry.animation.name, this.anim) == 0)
		{
			base.gameObject.transform.localScale = Vector3.zero;
			base.gameObject.SetActive(false);
		}
	}
}
