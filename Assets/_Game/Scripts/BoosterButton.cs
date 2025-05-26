using System;
using UnityEngine;
using UnityEngine.UI;

public class BoosterButton : MonoBehaviour
{
	public BoosterType type;

	public Button selectButton;

	public Text textPrice;

	public Text textRemaining;

	public GameObject highlight;

	public Image labelEquip;

	public Sprite sprEquip;

	public Sprite sprUnequip;

	private StaticBoosterData data;

	private void Awake()
	{
		this.data = GameData.staticBoosterData.GetData(this.type);
		EventDispatcher.Instance.RegisterListener(EventID.ConsumeCoin, delegate(Component sender, object param)
		{
			this.SetPriceTextColor();
		});
		this.Load();
	}

	private void Load()
	{
		int quantityHave = GameData.playerBoosters.GetQuantityHave(this.type);
		if (this.type != BoosterType.Grenade)
		{
			this.textRemaining.text = quantityHave.ToString();
			this.textRemaining.color = ((quantityHave <= 0) ? StaticValue.colorNotEnoughMoney : Color.white);
			this.labelEquip.transform.parent.gameObject.SetActive(quantityHave > 0);
			if (quantityHave <= 0 && GameData.selectingBoosters.Contains(this.type))
			{
				GameData.selectingBoosters.Remove(this.type);
			}
		}
		else
		{
			this.textRemaining.text = string.Empty;
		}
		this.textPrice.text = this.data.price.ToString("n0");
		this.SetPriceTextColor();
		this.Highlight();
	}

	public void Select()
	{
		SoundManager.Instance.PlaySfxClick();
		if (this.type != BoosterType.Grenade)
		{
			int quantityHave = GameData.playerBoosters.GetQuantityHave(this.type);
			if (quantityHave > 0)
			{
				if (GameData.selectingBoosters.Contains(this.type))
				{
					GameData.selectingBoosters.Remove(this.type);
				}
				else
				{
					GameData.selectingBoosters.Add(this.type);
				}
				this.Highlight();
			}
		}
		EventDispatcher.Instance.PostEvent(EventID.SelectBooster, this.data);
		if (GameData.isShowingTutorial && this.type == BoosterType.CoinMagnet)
		{
			EventDispatcher.Instance.PostEvent(EventID.SubStepSelectBooster);
		}
	}

	public void Buy()
	{
		if (GameData.playerResources.coin < this.data.price)
		{
			SoundManager.Instance.PlaySfxClick();
			return;
		}
		if (this.type == BoosterType.Grenade)
		{
			GameData.playerGrenades.Receive(500, 1);
			EventDispatcher.Instance.PostEvent(EventID.BuyGrenade);
		}
		else
		{
			GameData.playerBoosters.Receive(this.type, 1);
			if (!GameData.selectingBoosters.Contains(this.type))
			{
				GameData.selectingBoosters.Add(this.type);
			}
			EventLogger.LogEvent("N_BuyBooster", new object[]
			{
				this.type.ToString()
			});
		}
		this.Load();
		GameData.playerResources.ConsumeCoin(this.data.price);
		SoundManager.Instance.PlaySfx("sfx_purchase_success", 0f);
		EventDispatcher.Instance.PostEvent(EventID.BuyBooster, this.type);
		if (GameData.isShowingTutorial && this.type == BoosterType.CoinMagnet)
		{
			EventDispatcher.Instance.PostEvent(EventID.SubStepBuyBooster);
		}
	}

	private void Highlight()
	{
		this.highlight.SetActive(GameData.selectingBoosters.Contains(this.type));
		this.labelEquip.sprite = ((!GameData.selectingBoosters.Contains(this.type)) ? this.sprEquip : this.sprUnequip);
		this.labelEquip.SetNativeSize();
	}

	private void SetPriceTextColor()
	{
		this.textPrice.color = ((GameData.playerResources.coin < this.data.price) ? StaticValue.colorNotEnoughMoney : Color.white);
	}
}
