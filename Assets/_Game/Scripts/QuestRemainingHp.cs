using System;

public class QuestRemainingHp : BaseQuest
{
	public float hpPercentRequirement;

	public override void Init()
	{
		this.keyDescription = "remaining_hp";
		base.Init();
	}

	public override bool IsCompleted()
	{
		this.isCompleted = (Singleton<GameController>.Instance.Player.HpPercent >= this.hpPercentRequirement / 100f);
		return this.isCompleted;
	}

	public override string GetDescription()
	{
		return string.Format(this.description, this.hpPercentRequirement);
	}
}
