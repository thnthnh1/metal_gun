using System;
using UnityEngine;

public class QuestKillFinalBossInTime : BaseQuest
{
	public float timeRequirement;

	private float timeStartBoss;

	private float timeBossDie;

	private int bossId;

	public override void Init()
	{
		this.keyDescription = "kill_final_boss_in_time";
		base.Init();
		EventDispatcher.Instance.RegisterListener(EventID.FinalBossStart, delegate(Component sender, object param)
		{
			this.timeStartBoss = Time.time;
		});
		EventDispatcher.Instance.RegisterListener(EventID.FinalBossDie, delegate(Component sender, object param)
		{
			this.timeBossDie = Time.time;
			this.bossId = (int)param;
		});
	}

	public override bool IsCompleted()
	{
		float num = this.timeBossDie - this.timeStartBoss;
		this.isCompleted = (num <= this.timeRequirement);
		if (this.isCompleted)
		{
			EventLogger.LogEvent("N_KillBossTime", new object[]
			{
				"BossID=" + this.bossId,
				"Time=" + num
			});
		}
		return this.isCompleted;
	}

	public override string GetDescription()
	{
		return string.Format(this.description, this.timeRequirement);
	}
}
