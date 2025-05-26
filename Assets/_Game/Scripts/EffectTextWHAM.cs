using System;

public class EffectTextWHAM : BaseEffect
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolTextWHAM.Store(this);
	}
}
