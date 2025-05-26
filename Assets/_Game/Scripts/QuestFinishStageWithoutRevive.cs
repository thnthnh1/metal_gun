using System;
using UnityEngine;

public class QuestFinishStageWithoutRevive : BaseQuest
{
	private bool isFailed;

	public override void Init()
	{
		this.keyDescription = "finish_stage_without_revive";
		base.Init();
		this.isFailed = false;
		this.isCompleted = true;
		EventDispatcher.Instance.RegisterListener(EventID.ReviveByGem, delegate(Component sender, object param)
		{
			if (!this.isFailed)
			{
				this.isFailed = true;
				base.SetComplete(false);
			}
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReviveByAds, delegate(Component sender, object param)
		{
			if (!this.isFailed)
			{
				this.isFailed = true;
				base.SetComplete(false);
			}
		});
	}
}
