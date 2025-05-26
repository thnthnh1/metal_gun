using System;
using UnityEngine;

public class QuestRubberBoatNotHitObstacles : BaseQuest
{
	private bool isFailed;

	public override void Init()
	{
		this.keyDescription = "boat_not_hit_obstacle";
		base.Init();
		this.isFailed = false;
		this.isCompleted = true;
		EventDispatcher.Instance.RegisterListener(EventID.BoatTriggerObstacle, delegate(Component sender, object param)
		{
			if (!this.isFailed)
			{
				this.isFailed = true;
				base.SetComplete(false);
			}
		});
	}
}
