using System;
using UnityEngine;

public class QuestUseGrenade : BaseQuest
{
	public int requirement;

	private int useTimes;

	public override void Init()
	{
		this.keyDescription = "use_grenades";
		base.Init();
		this.useTimes = 0;
		EventDispatcher.Instance.RegisterListener(EventID.UseGrenade, delegate(Component sender, object param)
		{
			this.useTimes++;
		});
	}

	public override bool IsCompleted()
	{
		this.isCompleted = (this.useTimes >= this.requirement);
		return this.isCompleted;
	}

	public override string GetDescription()
	{
		return string.Format(this.description, this.requirement);
	}

	public override string GetCurrentProgress()
	{
		return string.Format("{0}/{1}", this.useTimes, this.requirement);
	}
}
