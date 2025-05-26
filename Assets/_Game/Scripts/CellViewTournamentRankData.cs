using System;
using System.Collections.Generic;
using UnityEngine;

public class CellViewTournamentRankData
{
	public int indexRank;

	public Sprite sprRankIcon;

	public string rankName;

	public string playerName;

	public Sprite sprAvatar;

	public Sprite sprGunId;

	public int score;

	public List<RewardData> rewards;

	public void SetAvatar(Sprite spr)
	{
		this.sprAvatar = spr;
		EventDispatcher.Instance.PostEvent(EventID.GetFacebookAvatarDone);
	}

	public void SetUserInfo(UserInfo info)
	{
		if (info != null)
		{
			this.playerName = info.name;
			EventDispatcher.Instance.PostEvent(EventID.GetFacebookNameDone);
		}
	}
}
