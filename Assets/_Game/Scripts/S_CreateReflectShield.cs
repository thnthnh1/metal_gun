using System;

public class S_CreateReflectShield : BaseSkill
{
	public override void Excute()
	{
		base.Excute();
		EventDispatcher.Instance.PostEvent(EventID.ActiveReflectShield);
	}
}
