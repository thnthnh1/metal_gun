using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CampaignBoxReward : MonoBehaviour
{
	public int index;

	public SkeletonGraphic skeletonGraphic;

	[SpineAnimation("", "", true, false)]
	public string normal;

	[SpineAnimation("", "", true, false)]
	public string ready;

	[SpineAnimation("", "", true, false)]
	public string opened;

	[SpineSkin("", "", true, false)]
	public string skinNormal;

	[SpineSkin("", "", true, false)]
	public string skinReady;

	[SpineSkin("", "", true, false)]
	public string skinOpened;

	public Button button;

	private int requireStar;

	public void LoadState(int currentStar, List<bool> progress)
	{
		if (progress[this.index])
		{
			this.skeletonGraphic.AnimationState.SetAnimation(0, this.opened, false).TimeScale = 50f;
			this.button.enabled = false;
		}
		else
		{
			this.requireStar = (this.index + 1) * 8;
			if (currentStar >= this.requireStar)
			{
				this.skeletonGraphic.AnimationState.SetAnimation(0, this.ready, true);
				this.button.enabled = true;
			}
			else
			{
				this.skeletonGraphic.AnimationState.SetAnimation(0, this.normal, false).TimeScale = 50f;
				this.button.enabled = false;
			}
		}
	}

	public void Claim()
	{
		EventDispatcher.Instance.PostEvent(EventID.ClaimCampaignBox, this.index);
	}
}
