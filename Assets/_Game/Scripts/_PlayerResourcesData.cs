using Newtonsoft.Json;
using System;

public class _PlayerResourcesData
{
	public int coin;

	public int gem;

	public int stamina;

	public int medal;

	public int tournamentTicket;

	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerResourcesData.Set(value);
		ProfileManager.SaveAll();
	}

	public void ReceiveCoin(int value)
	{
		if (value > 0)
		{
			this.coin += value;
			this.Save();
			EventDispatcher.Instance.PostEvent(EventID.ReceiveCoin, value);
		}
	}

	public void ConsumeCoin(int value)
	{
		if (value > 0)
		{
			this.coin -= value;
			if (this.coin < 0)
			{
				this.coin = 0;
			}
			this.Save();
			EventDispatcher.Instance.PostEvent(EventID.ConsumeCoin, value);
		}
	}

	public void ReceiveGem(int value)
	{
		if (value > 0)
		{
			this.gem += value;
			this.Save();
			EventDispatcher.Instance.PostEvent(EventID.ReceiveGem, value);
		}
	}

	public void ConsumeGem(int value)
	{
		if (value > 0)
		{
			this.gem -= value;
			if (this.gem < 0)
			{
				this.gem = 0;
			}
			this.Save();
			EventDispatcher.Instance.PostEvent(EventID.ConsumeGem, value);
		}
	}

	public void ReceiveStamina(int value)
	{
		if (value > 0)
		{
			this.stamina += value;
			this.Save();
			EventDispatcher.Instance.PostEvent(EventID.ReceiveStamina, value);
		}
	}

	public void ConsumeStamina(int value)
	{
		if (value > 0)
		{
			this.stamina -= value;
			if (this.stamina < 0)
			{
				this.stamina = 0;
			}
			this.Save();
			EventDispatcher.Instance.PostEvent(EventID.ConsumeStamina, value);
		}
	}

	public void ReceiveMedal(int value)
	{
		if (value > 0)
		{
			this.medal += value;
			this.Save();
			EventDispatcher.Instance.PostEvent(EventID.ReceiveMedal, value);
		}
	}

	public void ConsumeMedal(int value)
	{
		if (value > 0)
		{
			this.medal -= value;
			if (this.medal < 0)
			{
				this.medal = 0;
			}
			this.Save();
			EventDispatcher.Instance.PostEvent(EventID.ConsumeMedal, value);
		}
	}

	public void ReceiveTournamentTicket(int value)
	{
		if (value > 0)
		{
			this.tournamentTicket += value;
			/*if (Mp_Armory.instance)
			{
				if (Mp_Armory.instance.tourTicket)
				{
					Mp_Armory.instance.tourTicket.text = GameData.playerResources.tournamentTicket + " / " + 5;
				}
			}*/
			EventDispatcher.Instance.PostEvent(EventID.ReceiveTicket, value);
			this.Save();
		}
	}

	public void ConsumeTournamentTicket(int value)
	{
		if (value > 0)
		{
			this.tournamentTicket -= value;
			if (this.tournamentTicket < 0)
			{
				this.tournamentTicket = 0;
			}
			/*if (Mp_Armory.instance)
			{
				if (Mp_Armory.instance.tourTicket)
				{
					Mp_Armory.instance.tourTicket.text = GameData.playerResources.tournamentTicket + " / " + 5;
				}
			}*/
			EventDispatcher.Instance.PostEvent(EventID.ConsumeTicket, value);
			this.Save();
		}
	}

	public void ResetTicketNewday()
	{
		if (this.tournamentTicket < 5)
		{
			this.tournamentTicket = 5;
		}
		this.Save();
	}
}
