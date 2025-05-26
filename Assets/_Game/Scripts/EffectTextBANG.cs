using System;

public class EffectTextBANG : BaseEffect
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolTextBANG.Store(this);
	}
}
