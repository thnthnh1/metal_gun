using System;

public class ItemDropHealth : BaseItemDrop
{
	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolItemDropHealth.Store(this);
	}
}
