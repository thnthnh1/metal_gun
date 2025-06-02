/*using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
// FacebookSDK Remove
// using Facebook.Unity;

public class HudTournamentRanking : MonoBehaviour, IEnhancedScrollerDelegate, ISimpleUI
{
	private sealed class _CoroutineTimerSeason_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal double _timeleft___0;

		internal TimeSpan _t___1;

		internal int _days___1;

		internal int _hours___1;

		internal int _minutes___1;

		internal int _seconds___1;

		internal HudTournamentRanking _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;


		object IEnumerator<object>.Current
		{
			get
			{
				return this._current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return this._current;
			}
		}

		public _CoroutineTimerSeason_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._timeleft___0 = Singleton<MasterInfo>.Instance.GetTournamentTimeleftInSecond();
				break;
			case 1u:
				this._timeleft___0 -= 1.0;
				this._t___1 = TimeSpan.FromSeconds(this._timeleft___0);
				this._days___1 = 0;
				this._hours___1 = 0;
				this._minutes___1 = 0;
				this._seconds___1 = 0;
				Singleton<MasterInfo>.Instance.CountDownTimer(this._t___1, out this._days___1, out this._hours___1, out this._minutes___1, out this._seconds___1);
				this._this.textRemainingTime.text = string.Format("{0}D {1}H {2}M {3}S", new object[]
				{
					this._days___1,
					this._hours___1,
					this._minutes___1,
					this._seconds___1
				});
				this._this.textRemainingTime2.text = string.Format("{0}D {1}H {2}M {3}S", new object[]
				{
					this._days___1,
					this._hours___1,
					this._minutes___1,
					this._seconds___1
				});
					break;
			default:
				return false;
			}
			if (this._timeleft___0 > 0.0)
			{
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.textRemainingTime.text = string.Empty;
			this._this.textRemainingTime2.text = string.Empty;
			this._PC = -1;
			return false;
		}

		public void Dispose()
		{
			this._disposing = true;
			this._PC = -1;
		}

		public void Reset()
		{
			throw new NotSupportedException();
		}
	}
	public static HudTournamentRanking instance;

	public EnhancedScroller scroller;

	public EnhancedScrollerCellView cellViewTournamentRank;

	public GameObject popupRank;

	public GameObject groupRank;

	public GameObject groupRewards;

	[Header("TOURNAMENT INFO")]
	public Text textRemainingTime;
	public Text textRemainingTime2;

	public Text textStartDate;

	public Text textEndDate;

	public GameObject groupTicket;

	public GameObject groupGem;

	public Text textGemChallenge;

	public GameObject tipRefreshScoreBoard;

	[Header("PLAYER INFO")]
	public Text textPlayerScore;

	public Text textPlayerFbName;

	public Image imgPlayerAvatar;

	public Sprite sprLoadingAvatar;

	public Text textRankProgress;

	public Image imgRankProgress;

	public Image imgRankIcon;

	public Text textFreeEntrance;

	[Header("TABS")]
	public Image imgTabRank;

	public Sprite sprTabRankSelect;

	public Sprite sprTabRankUnselect;

	public Image imgTabReward;

	public Sprite sprTabRewardSelect;

	public Sprite sprTabRewardUnselect;

	public GameObject notiRankReward;

	[Header("REWARDS")]
	public CellViewRankReward[] cellViewRewards;

	private int priceChallenge;

	private SmallList<CellViewTournamentRankData> rankData = new SmallList<CellViewTournamentRankData>();

	private IEnumerator coroutineTimerSeason;

	private static UnityAction<List<TournamentData>> __f__am_cache0;

	public Mp_RewardsMenu rewardManager;

	private void Awake()
	{
		instance = this;
		this.scroller.CreateContainer();
		this.scroller.Delegate = this;
		EventDispatcher.Instance.RegisterListener(EventID.GetFacebookAvatarDone, delegate(Component sender, object param)
		{
			this.OnGetFacebookAvatarDone();
		});
		EventDispatcher.Instance.RegisterListener(EventID.GetFacebookNameDone, delegate(Component sender, object param)
		{
			this.OnGetFacebookNameDone();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ClaimTournamentRankReward, delegate(Component sender, object param)
		{
			this.CheckNotification();
		});
		this.CheckWeek();
		this.textPlayerFbName.text = GameData.playerTournamentData.fbName;
		this.imgPlayerAvatar.sprite = ((!(GameData.playerTournamentData.sprAvatar != null)) ? this.sprLoadingAvatar : GameData.playerTournamentData.sprAvatar);
	}

	private void OnEnable()
	{
		this.SwitchTabRank();
		this.CheckTimeRemaining();
		this.CheckNotification();
	}

	private void OnDisable()
	{
		if (this.coroutineTimerSeason != null)
		{
			base.StopCoroutine(this.coroutineTimerSeason);
			this.coroutineTimerSeason = null;
		}
	}

	public void SwitchTabRank()
	{
		this.imgTabRank.sprite = this.sprTabRankSelect;
		this.imgTabReward.sprite = this.sprTabRewardUnselect;
		this.groupRank.SetActive(true);
		this.groupRewards.SetActive(false);
	}

	public void SwitchTabReward()
	{
		this.imgTabRank.sprite = this.sprTabRankUnselect;
		this.imgTabReward.sprite = this.sprTabRewardSelect;
		this.groupRank.SetActive(false);
		this.groupRewards.SetActive(true);
		for (int i = 0; i < this.cellViewRewards.Length; i++)
		{
			this.cellViewRewards[i].Load();
		}
	}

	public void TestWeekReward() 
	{
		ProfileManager.UserProfile.weekLastLogin.Set("0");

		CheckWeek();
	}

	public void ResetWeekLastLogin() 
	{
		ProfileManager.UserProfile.weekLastLogin.Set("");
	}

	public void CheckWeek()
	{
		// FacebookSDK Remove
		UnityEngine.Debug.Log("Check Facebook Or Google SignIn");
		if (*//*!FB.IsLoggedIn &&*//* PlayerPrefs.GetInt("GoogleSignIn") == 0)
		{
			// UnityEngine.Debug.Log("<color>LB - Log in CheckWeek</color>" + " FB : " + FB.IsLoggedIn);
		}
		else
		{
			string currentWeekRangeString = Singleton<MasterInfo>.Instance.GetCurrentWeekRangeString();

			UnityEngine.Debug.Log("**************** Week Range *****************");
			UnityEngine.Debug.Log(currentWeekRangeString);
			UnityEngine.Debug.Log("**************** Week Range *****************");

			UnityEngine.Debug.Log("Checking Tournament");
			if (string.Compare(currentWeekRangeString, ProfileManager.UserProfile.weekLastLogin) != 0)
			{
				UnityEngine.Debug.Log("Tournament is Ended!");
				ProfileManager.UserProfile.weekLastLogin.Set(currentWeekRangeString);
				ProfileManager.UserProfile.isClaimedRank1.Set(false);
				ProfileManager.UserProfile.isClaimedRank2.Set(false);
				ProfileManager.UserProfile.isClaimedRank3.Set(false);
				ProfileManager.UserProfile.isClaimedRank4.Set(false);
				ProfileManager.UserProfile.isClaimedRank5.Set(false);
				ProfileManager.UserProfile.isClaimedRank6.Set(false);
				ProfileManager.UserProfile.tournamentGunProfile.Set(string.Empty);

				rewardManager.ClaimRewards();

				//DeleteHighscore();
			}
			if (!GameData.playerTournamentData.isReceivedTopRankReward)
			{
				//GameData.playerTournamentData.isReceivedTopRankReward = true;
				*//*
				Singleton<FireBaseDatabase>.Instance.GetTopTournamentForRewarded(delegate(List<TournamentData> data)
				{
					bool flag = false;
					int num = 0;
					for (int i = data.Count - 1; i >= 0; i--)
					{
						num++;
						if (string.Compare(data[i].id, AccessToken.CurrentAccessToken.UserId) == 0 && !data[i].received)
						{
							flag = true;
							break;
						}
					}
					if (flag && GameData.tournamentTopRankRewards.ContainsKey(num - 1))
					{
						List<RewardData> rewards = GameData.tournamentTopRankRewards[num - 1];
						RewardUtils.Receive(rewards);
						Singleton<Popup>.Instance.ShowReward(rewards, string.Format("Top {0} rank tournament rewards", num), null);
						Singleton<FireBaseDatabase>.Instance.SaveTournamentReceivedReward(AccessToken.CurrentAccessToken.UserId, Singleton<MasterInfo>.Instance.GetPreviousWeekRangeString(), null);
					}
				});
				*//*
			}
		}
	}

	private void CheckTimeRemaining()
	{
		if (this.coroutineTimerSeason != null)
		{
			base.StopCoroutine(this.coroutineTimerSeason);
		}
		this.coroutineTimerSeason = this.CoroutineTimerSeason();
		base.StartCoroutine(this.coroutineTimerSeason);
		DateTime currentDateTime = Singleton<MasterInfo>.Instance.GetCurrentDateTime();
		int num = (int)(DayOfWeek.Monday - currentDateTime.DayOfWeek);
		DateTime dateTime = currentDateTime.AddDays((double)num);
		DateTime dateTime2 = dateTime.AddDays(6.0);
		this.textStartDate.text = string.Format("START: {0:00}/{1:00}", dateTime.Day, dateTime.Month);
		this.textEndDate.text = string.Format("END: {0:00}/{1:00}", dateTime2.Day, dateTime2.Month);
	}

	private IEnumerator CoroutineTimerSeason()
	{
		HudTournamentRanking._CoroutineTimerSeason_c__Iterator0 _CoroutineTimerSeason_c__Iterator = new HudTournamentRanking._CoroutineTimerSeason_c__Iterator0();
		_CoroutineTimerSeason_c__Iterator._this = this;
		return _CoroutineTimerSeason_c__Iterator;
	}

	private void CheckNotification()
	{
		int unclaimRankRewards = this.GetUnclaimRankRewards();
		this.notiRankReward.SetActive(unclaimRankRewards > 0);
		if (ProfileManager.UserProfile.countPlayTournament < 5 && GameData.playerResources.tournamentTicket > 0)
		{
			this.textFreeEntrance.text = GameData.playerResources.tournamentTicket.ToString();
			this.textFreeEntrance.transform.parent.gameObject.SetActive(true);
		}
		else
		{
			this.textFreeEntrance.transform.parent.gameObject.SetActive(false);
		}
	}

	private void CreateRankData(List<TournamentData> listData)
	{
		this.rankData.Clear();
		int num = 0;
		for (int i = listData.Count - 1; i >= 0; i--)
		{
			TournamentData tournamentData = listData[i];
			TournamentRank currentRank = GameData.staticTournamentRankData.GetCurrentRank(tournamentData.score);
			StaticTournamentRankData data = GameData.staticTournamentRankData.GetData((int)currentRank);
			CellViewTournamentRankData cellViewTournamentRankData = new CellViewTournamentRankData();
			cellViewTournamentRankData.indexRank = num;
			if (num == 0)
			{
				cellViewTournamentRankData.rankName = "GRAND MASTER";
			}
			else if (num == 1)
			{
				cellViewTournamentRankData.rankName = "MASTER";
			}
			else if (num == 2)
			{
				cellViewTournamentRankData.rankName = "CHALLENGER";
			}
			else
			{
				cellViewTournamentRankData.rankName = string.Empty;
			}
			cellViewTournamentRankData.rewards = data.rewards;
			cellViewTournamentRankData.score = tournamentData.score;
			//Singleton<FireBaseDatabase>.Instance.GetUserInfo(tournamentData.id, new UnityAction<UserInfo>(cellViewTournamentRankData.SetUserInfo));
			//FbController.Instance.GetProfilePictureById(tournamentData.id, new UnityAction<Sprite>(cellViewTournamentRankData.SetAvatar));
			cellViewTournamentRankData.sprGunId = GameResourcesUtils.GetGunImage(tournamentData.primaryGunId);
			cellViewTournamentRankData.sprRankIcon = GameResourcesUtils.GetTournamentRankImage(data.rankIndex);
			if (num < 3 && GameData.tournamentTopRankRewards.ContainsKey(num))
			{
				cellViewTournamentRankData.rewards = GameData.tournamentTopRankRewards[num];
			}
			this.rankData.Add(cellViewTournamentRankData);
			num++;
		}
	}

	public int GetUnclaimRankRewards()
	{
		int num = 0;
		for (int i = 0; i < this.cellViewRewards.Length; i++)
		{
			if (this.cellViewRewards[i].IsAvailableClaim())
			{
				num++;
			}
		}
		
		return num;
	}
	
	public void DeleteHighscore()
	{
		StartCoroutine(DeletingHighscore());
	}
	IEnumerator DeletingHighscore() 
	{
		yield return new WaitForSeconds(1);

		if (Mp_playerSettings.instance)
		{
			string username = Mp_playerSettings.instance.facebookID;
			Highscores.instance.DeleteUsername(username);
		}
		else
		{
			StartCoroutine(DeletingHighscore());
		}
	}

	public void Challenge()
	{
		SoundManager.Instance.PlaySfxClick();
		if (ProfileManager.UserProfile.countPlayTournament >= 5)
		{
			Singleton<Popup>.Instance.Show("you have exceeded the number of times you can play tournament today.", "NOTICE", PopupType.Ok, null, null);
		}
		else if (GameData.playerResources.gem < this.priceChallenge)
		{
			Singleton<Popup>.Instance.ShowToastMessage("not enough gems", ToastLength.Normal);
		}
		else
		{
			EventDispatcher.Instance.PostEvent(EventID.ClickStartTournament, this.priceChallenge);
		}
	}

	public void Open(List<TournamentData> data)
	{
		this.CreateRankData(data);
		this.scroller.ReloadData(0f);
		this.FillInformation();
		this.tipRefreshScoreBoard.SetActive(data.Count > 0);
		base.gameObject.SetActive(true);
	}

	public void Close()
	{
		this.popupRank.SetActive(false);
	}

	public void SetPlayerFbAvatar(Sprite spr)
	{
		this.imgPlayerAvatar.sprite = spr;
	}

	private void OnGetFacebookAvatarDone()
	{
		this.scroller.RefreshActiveCellViews();
	}

	private void OnGetFacebookNameDone()
	{
		this.scroller.RefreshActiveCellViews();
	}

	private void FillInformation()
	{
		TournamentRank currentRank = GameData.staticTournamentRankData.GetCurrentRank(GameData.playerTournamentData.score);
		this.textPlayerScore.text = GameData.playerTournamentData.score.ToString();
		this.imgRankIcon.sprite = GameResourcesUtils.GetTournamentRankImage((int)currentRank);
		this.imgRankIcon.SetNativeSize();
		if (currentRank >= TournamentRank.Legend)
		{
			this.imgRankProgress.fillAmount = 1f;
			this.textRankProgress.text = GameData.playerTournamentData.score.ToString();
		}
		else
		{
			int rankIndex = (int)(currentRank + 1);
			StaticTournamentRankData data = GameData.staticTournamentRankData.GetData((int)currentRank);
			StaticTournamentRankData data2 = GameData.staticTournamentRankData.GetData(rankIndex);
			float fillAmount = Mathf.Clamp01((float)(GameData.playerTournamentData.score - data.score) / (float)(data2.score - data.score));
			this.imgRankProgress.fillAmount = fillAmount;
			this.textRankProgress.text = string.Format("{0}/{1}", GameData.playerTournamentData.score, data2.score);
		}
		if (ProfileManager.UserProfile.countPlayTournament >= 5)
		{
			this.groupGem.SetActive(false);
			this.groupTicket.SetActive(false);
			this.textFreeEntrance.transform.parent.gameObject.SetActive(false);
		}
		else if (GameData.playerResources.tournamentTicket > 0)
		{
			this.textFreeEntrance.transform.parent.gameObject.SetActive(true);
			this.textFreeEntrance.text = GameData.playerResources.tournamentTicket.ToString();
			this.groupGem.SetActive(false);
			this.groupTicket.SetActive(true);
			this.priceChallenge = 0;
		}
		else
		{
			this.textFreeEntrance.transform.parent.gameObject.SetActive(false);
			if (ProfileManager.UserProfile.countPlayTournament >= 2)
			{
				this.groupGem.SetActive(true);
				this.groupTicket.SetActive(false);
				if (ProfileManager.UserProfile.countPlayTournament == 2)
				{
					this.priceChallenge = 75;
				}
				else if (ProfileManager.UserProfile.countPlayTournament == 3)
				{
					this.priceChallenge = 150;
				}
				else if (ProfileManager.UserProfile.countPlayTournament >= 4)
				{
					this.priceChallenge = 300;
				}
				this.textGemChallenge.color = ((GameData.playerResources.gem < this.priceChallenge) ? StaticValue.colorNotEnoughMoney : Color.white);
				this.textGemChallenge.text = this.priceChallenge.ToString();
			}
			else
			{
				GameData.playerResources.ReceiveTournamentTicket(2);
				this.groupGem.SetActive(false);
				this.groupTicket.SetActive(true);
				this.priceChallenge = 0;
				this.FillInformation();
			}
		}
	}

	public int GetNumberOfCells(EnhancedScroller scroller)
	{
		return this.rankData.Count;
	}

	public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
	{
		if (dataIndex < 4)
		{
			return 100f;
		}
		return 61f;
	}

	public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
	{
		CellViewTournamentRank cellViewTournamentRank = scroller.GetCellView(this.cellViewTournamentRank) as CellViewTournamentRank;
		cellViewTournamentRank.SetData(this.rankData[dataIndex]);
		return cellViewTournamentRank;
	}
}
*/