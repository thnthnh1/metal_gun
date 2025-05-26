using System;
using UnityEngine;

public class QuestComboKill : BaseQuest
{
	public int comboKillRequirement;

	private int highestComboKill;

	public override void Init()
	{
		this.keyDescription = "combo_kill";
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.GetComboKill, delegate(Component sender, object param)
		{
			this.SetHighestComboKill((int)param);
		});
	}

	public override bool IsCompleted()
	{
		this.isCompleted = (this.highestComboKill >= this.comboKillRequirement);
		return this.isCompleted;
	}

	public override string GetDescription()
	{
		return string.Format(this.description, this.comboKillRequirement);
	}

	public override string GetCurrentProgress()
	{
		return string.Format("{0}/{1}", this.highestComboKill, this.comboKillRequirement);
	}

	private void SetHighestComboKill(int count)
	{
		if (count >= this.highestComboKill)
		{
			this.highestComboKill = count;
		}
	}
}
