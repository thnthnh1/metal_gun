using System;

public class S_Rage : BaseSkill
{
	public override void Excute()
	{
		base.Excute();
		EventDispatcher.Instance.PostEvent(EventID.ActiveRage);
	}
}
