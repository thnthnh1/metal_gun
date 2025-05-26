using System;
using UnityEngine;

public class SupportGrenades : BaseSupportItem
{
	public override void Init()
	{
		base.Init();
		this.groupFree.SetActive(false);
		this.groupPrice.SetActive(true);
		this.priceUse = 5000;
		this.textPrice.text = this.priceUse.ToString();
		this.textPrice.color = ((GameData.playerResources.coin < this.priceUse) ? StaticValue.colorNotEnoughMoney : Color.yellow);
	}

	protected override void Consume()
	{
		if (GameData.playerResources.coin >= this.priceUse)
		{
			GameData.playerResources.ConsumeCoin(this.priceUse);
			GameData.playerGrenades.Receive(500, 10);
			base.Consume();
			if (this.countUsed >= 2)
			{
				this.Active(false);
			}
			else
			{
				this.textPrice.color = ((GameData.playerResources.coin < this.priceUse) ? StaticValue.colorNotEnoughMoney : Color.yellow);
			}
			EventDispatcher.Instance.PostEvent(EventID.UseSupportItemGrenade, 10);
			EventLogger.LogEvent("N_UseSurvivalSupportItem", new object[]
			{
				"Grenades"
			});
		}
	}
}
