using System;

public class QuestFinishStageInTime : BaseQuest
{
	public int timeRequirement;

	public override void Init()
	{
		this.keyDescription = "finish_stage_in_time";
		base.Init();
	}

	public override bool IsCompleted()
	{
		this.isCompleted = (Singleton<GameController>.Instance.PlayTime <= this.timeRequirement);
		return this.isCompleted;
	}

	public override string GetDescription()
	{
		return string.Format(this.description, this.timeRequirement);
	}
}
