using System;

public class EffectTextCRIT : BaseEffect
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolTextCRIT.Store(this);
	}
}
