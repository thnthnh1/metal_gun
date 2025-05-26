using EnhancedUI.EnhancedScroller;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CellViewTournamentRank : EnhancedScrollerCellView
{
	public Image imgRankIconTop;

	public Image imgRankIconNormal;

	public Image imgAvatar;

	public Text textRankName;

	public Image imgGun;

	public Text textScore;

	public Text textRankIndex;

	public Text textPlayerName;

	public Image header;

	public Sprite sprHeaderTop1;

	public Sprite sprHeaderTop2;

	public Sprite sprHeaderTop3;

	public Image bgInfo;

	public Sprite bgInfoTop1;

	public Sprite bgInfoTop2;

	public Sprite bgInfoTop3;

	public Sprite bgInfoNormal;

	public Sprite loadingSpr;

	public TournamentRankRewardDisplay[] rewardCells;

	private CellViewTournamentRankData _data;

	public void SetData(CellViewTournamentRankData data)
	{
		this._data = data;
		this.UpdateInformation();
	}

	private void UpdateInformation()
	{
		this.imgRankIconTop.sprite = this._data.sprRankIcon;
		this.imgRankIconTop.SetNativeSize();
		this.textRankName.text = this._data.rankName;
		this.textRankIndex.text = (this._data.indexRank + 1).ToString();
		this.textPlayerName.text = ((!string.IsNullOrEmpty(this._data.playerName)) ? this._data.playerName : "Player Unknown");
		if (this._data.indexRank < 3)
		{
			this.header.gameObject.SetActive(true);
			if (this._data.indexRank == 0)
			{
				this.header.sprite = this.sprHeaderTop1;
				this.bgInfo.sprite = this.bgInfoTop1;
			}
			else if (this._data.indexRank == 1)
			{
				this.header.sprite = this.sprHeaderTop2;
				this.bgInfo.sprite = this.bgInfoTop2;
			}
			else if (this._data.indexRank == 2)
			{
				this.header.sprite = this.sprHeaderTop3;
				this.bgInfo.sprite = this.bgInfoTop3;
			}
			this.SetRewards();
			this.imgRankIconNormal.gameObject.SetActive(false);
		}
		else
		{
			this.header.gameObject.SetActive(false);
			this.bgInfo.sprite = this.bgInfoNormal;
			this.imgRankIconNormal.gameObject.SetActive(true);
			this.imgRankIconNormal.sprite = this._data.sprRankIcon;
			this.imgRankIconNormal.SetNativeSize();
		}
		this.imgGun.sprite = this._data.sprGunId;
		this.imgGun.SetNativeSize();
		this.imgAvatar.sprite = ((!(this._data.sprAvatar == null)) ? this._data.sprAvatar : this.loadingSpr);
		this.textScore.text = this._data.score.ToString();
	}

	public override void RefreshCellView()
	{
		base.RefreshCellView();
		this.UpdateInformation();
	}

	private void SetRewards()
	{
		if (this._data.indexRank < 3)
		{
			for (int i = 0; i < this.rewardCells.Length; i++)
			{
				TournamentRankRewardDisplay tournamentRankRewardDisplay = this.rewardCells[i];
				if (i < this._data.rewards.Count)
				{
					tournamentRankRewardDisplay.gameObject.SetActive(true);
					tournamentRankRewardDisplay.SetInformation(this._data.rewards[i]);
				}
				else
				{
					tournamentRankRewardDisplay.gameObject.SetActive(false);
				}
			}
		}
	}
}
