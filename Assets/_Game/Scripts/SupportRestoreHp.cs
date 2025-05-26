using System;
using UnityEngine;

public class SupportRestoreHp : BaseSupportItem
{
	public override void Init()
	{
		base.Init();
		this.groupFree.SetActive(true);
		this.groupPrice.SetActive(false);
		this.priceUse = 0;
	}

	protected override void Consume()
	{
		if (GameData.playerResources.gem >= this.priceUse)
		{
			GameData.playerResources.ConsumeGem(this.priceUse);
			base.Consume();
			if (this.countUsed >= 2)
			{
				this.Active(false);
			}
			else if (this.countUsed >= 1)
			{
				this.groupFree.SetActive(false);
				this.groupPrice.SetActive(true);
				this.priceUse = 40;
				this.textPrice.text = this.priceUse.ToString();
				this.textPrice.color = ((GameData.playerResources.gem < this.priceUse) ? StaticValue.colorNotEnoughMoney : Color.yellow);
			}
			EventDispatcher.Instance.PostEvent(EventID.UseSupportItemHP);
			EventLogger.LogEvent("N_UseSurvivalSupportItem", new object[]
			{
				"FullHP"
			});
		}
	}
}
