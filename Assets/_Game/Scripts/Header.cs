using System;
using UnityEngine;
using UnityEngine.UI;

public class Header : MonoBehaviour
{
	public Text playerName;

	public Text rankName;

	public Image rankIcon;

	public Text level;

	public RectTransform levelBar;

	public Text coin;
	public Text gem;
	public Text ticket;
    public Text medal;

	//[Header("IAP screen")]
	//public GameObject IAPBoardGameObject;
	//private bool _isIAPScreenOn => IAPBoardGameObject.activeSelf;
    //public Text coinIAP;
    //public Text gemIAP;
    //public Text ticketIAP;

	public ResourcesChangeText changeTextPrefab;

	public static ObjectPooling<ResourcesChangeText> poolTextChange = new ObjectPooling<ResourcesChangeText>(8);

	private void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ReceiveExp, delegate (Component sender, object param)
		{
			this.UpdatePlayerInfo();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReceiveCoin, delegate (Component sender, object param)
		{
			this.ChangeValueCoin(true, (int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReceiveGem, delegate (Component sender, object param)
		{
			this.ChangeValueGem(true, (int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReceiveMedal, delegate (Component sender, object param)
		{
			this.ChangeValueMedal(true, (int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ConsumeCoin, delegate (Component sender, object param)
		{
			this.ChangeValueCoin(false, (int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ConsumeGem, delegate (Component sender, object param)
		{
			this.ChangeValueGem(false, (int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ConsumeMedal, delegate (Component sender, object param)
		{
			this.ChangeValueMedal(false, (int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReceiveTicket, delegate (Component sender, object param)
		{
			this.ChangeValueTicket(true, (int)param);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ConsumeTicket, delegate (Component sender, object param)
		{
			this.ChangeValueTicket(false, (int)param);
		});
		this.FillData();
	}

	public void FillData()
	{
		this.UpdateCoinText();
		this.UpdateGemText();
		this.UpdateMedalText();
		this.UpdatePlayerInfo();
		this.UpdateTicketText();
	}

	private void UpdatePlayerInfo()
	{
		int num = GameData.playerProfile.level;
		this.level.text = string.Format("RANK LEVEL: {0}", num);
		this.rankName.text = GameData.staticRankData.GetRankName(num).ToUpper();
		this.rankIcon.sprite = GameResourcesUtils.GetRankImage(num);
		bool flag = num >= GameData.staticRankData.Count;
		if (flag)
		{
			Vector2 sizeDelta = this.levelBar.sizeDelta;
			sizeDelta.x = 147f;
			this.levelBar.sizeDelta = sizeDelta;
		}
		else
		{
			int exp = GameData.playerProfile.exp;
			int expOfLevel = GameData.staticRankData.GetExpOfLevel(num + 1);
			float x = Mathf.Clamp((float)exp / (float)expOfLevel * 147f, 15f, 147f);
			Vector2 sizeDelta2 = this.levelBar.sizeDelta;
			sizeDelta2.x = x;
			this.levelBar.sizeDelta = sizeDelta2;
		}
	}

	private void UpdateCoinText()
	{
		this.coin.text = GameData.playerResources.coin.ToString("n0");
        //this.coinIAP.text = GameData.playerResources.coin.ToString("n0");
    }

	private void ChangeValueCoin(bool isReceive, int value)
	{
		this.UpdateCoinText();
		ResourcesChangeText resourcesChangeText = Header.poolTextChange.New();
		if (resourcesChangeText == null)
		{
			resourcesChangeText = UnityEngine.Object.Instantiate<ResourcesChangeText>(this.changeTextPrefab);
		}
        resourcesChangeText.Active(isReceive, value, this.coin.rectTransform.position, base.transform);
        //if (_isIAPScreenOn)
        //{
        //    resourcesChangeText.Active(isReceive, value, this.coinIAP.rectTransform.position, base.transform);
		//
        //}
		//else
        //{
        //    resourcesChangeText.Active(isReceive, value, this.coin.rectTransform.position, base.transform);
		//
        //}
	}

	private void UpdateTicketText()
	{
		this.ticket.text = GameData.playerResources.tournamentTicket.ToString("n0");
        //this.ticketIAP.text = GameData.playerResources.tournamentTicket.ToString("n0");
    }

	private void ChangeValueTicket(bool isReceive, int value)
	{
		this.UpdateTicketText();
		ResourcesChangeText resourcesChangeText = Header.poolTextChange.New();
		if (resourcesChangeText == null)
		{
			resourcesChangeText = UnityEngine.Object.Instantiate<ResourcesChangeText>(this.changeTextPrefab);
		}
        resourcesChangeText.Active(isReceive, value, this.ticket.rectTransform.position, base.transform);
        //if (_isIAPScreenOn)
        //{
        //    resourcesChangeText.Active(isReceive, value, this.ticketIAP.rectTransform.position, base.transform);
		//
        //}
        //else
        //{
        //    resourcesChangeText.Active(isReceive, value, this.ticket.rectTransform.position, base.transform);
		//
        //}
	}

	private void UpdateGemText()
	{
		this.gem.text = GameData.playerResources.gem.ToString("n0");
        //this.gemIAP.text = GameData.playerResources.gem.ToString("n0");
    }

	private void ChangeValueGem(bool isReceive, int value)
	{
		this.UpdateGemText();
		ResourcesChangeText resourcesChangeText = Header.poolTextChange.New();
		if (resourcesChangeText == null)
		{
			resourcesChangeText = UnityEngine.Object.Instantiate<ResourcesChangeText>(this.changeTextPrefab);
        }
        resourcesChangeText.Active(isReceive, value, this.gem.rectTransform.position, base.transform);
        //if (_isIAPScreenOn)
        //{
        //    resourcesChangeText.Active(isReceive, value, this.gemIAP.rectTransform.position, base.transform);
		//
        //}
        //else
        //{
        //    resourcesChangeText.Active(isReceive, value, this.gem.rectTransform.position, base.transform);
		//
        //}
	}

	private void UpdateMedalText()
	{
		this.medal.text = GameData.playerResources.medal.ToString("n0");
	}

	private void ChangeValueMedal(bool isReceive, int value)
	{
		this.UpdateMedalText();
		ResourcesChangeText resourcesChangeText = Header.poolTextChange.New();
		if (resourcesChangeText == null)
		{
			resourcesChangeText = UnityEngine.Object.Instantiate<ResourcesChangeText>(this.changeTextPrefab);
		}
		resourcesChangeText.Active(isReceive, value, this.medal.rectTransform.position, base.transform);
	}
}
