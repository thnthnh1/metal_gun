using System;
using UnityEngine;
using UnityEngine.UI;

public class SupportBooster : BaseSupportItem
{
	public Button boosterImage;

	private BoosterType type = BoosterType.Damage;

	private bool isUsedCurrentWave;

	public override void Init()
	{
		base.Init();
		this.groupFree.SetActive(true);
		this.groupPrice.SetActive(false);
		this.priceUse = 0;
		this.isUsedCurrentWave = false;
		this.RandomBooster();
	}

	protected override void Consume()
	{
		if (this.isUsedCurrentWave)
		{
			return;
		}
		if (GameData.playerResources.gem >= this.priceUse)
		{
			this.isUsedCurrentWave = true;
			GameData.playerResources.ConsumeGem(this.priceUse);
			base.Consume();
			if (this.countUsed >= 5)
			{
				this.Active(false);
			}
			else if (this.countUsed >= 1)
			{
				this.groupFree.SetActive(false);
				this.groupPrice.SetActive(true);
				this.priceUse = 20;
				this.textPrice.text = this.priceUse.ToString();
				this.textPrice.color = ((GameData.playerResources.gem < this.priceUse) ? StaticValue.colorNotEnoughMoney : Color.yellow);
			}
			EventDispatcher.Instance.PostEvent(EventID.UseSupportItemBooster, this.type);
			EventLogger.LogEvent("N_UseSurvivalSupportItem", new object[]
			{
				"Booster"
			});
		}
	}

	protected override void Active(bool isActive)
	{
		base.Active(isActive);
		this.boosterImage.image.raycastTarget = false;
		this.boosterImage.interactable = isActive;
	}

	protected override void OnCompleteWave()
	{
		this.isUsedCurrentWave = false;
		if (this.countUsed < 5)
		{
			this.RandomBooster();
		}
	}

	private void RandomBooster()
	{
		int num = UnityEngine.Random.Range(0, 3);
		if (num == 0)
		{
			this.type = BoosterType.Damage;
			this.boosterImage.image.sprite = GameResourcesUtils.GetRewardImage(RewardType.BoosterDamage);
		}
		else if (num == 1)
		{
			this.type = BoosterType.Critical;
			this.boosterImage.image.sprite = GameResourcesUtils.GetRewardImage(RewardType.BoosterCritical);
		}
		else if (num == 2)
		{
			this.type = BoosterType.Speed;
			this.boosterImage.image.sprite = GameResourcesUtils.GetRewardImage(RewardType.BoosterSpeed);
		}
		this.boosterImage.image.SetNativeSize();
	}
}
