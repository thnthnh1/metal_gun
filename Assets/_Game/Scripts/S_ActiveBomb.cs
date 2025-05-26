using System;

public class S_ActiveBomb : BaseSkill
{
	public override void Excute()
	{
		base.Excute();
		EventDispatcher.Instance.PostEvent(EventID.ActiveBomb);
	}
}
