using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Core.Extension;

public class CellViewFreeGift : MonoBehaviour
{
	private sealed class _StartCountDown_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _min___1;

		internal int _sec___1;

		internal Action callback;

		internal CellViewFreeGift _this;

		internal object _current;

		internal bool _disposing;

		internal int _PC;

		internal int _duration;

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

		public _StartCountDown_c__Iterator0(int duration)
		{
			_duration = duration;
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
				case 0u:
					break;
				case 1u:
					_duration--;
					if (_duration == 0)
					{
						this.callback();
						goto IL_C2;
					}
					break;
				default:
					return false;
			}
			if (_duration > 0)
			{
				this._min___1 = _duration / 60;
				this._sec___1 = _duration % 60;
				this._this.countDown.text = string.Format("{0:D2}:{1:D2}", this._min___1, this._sec___1);
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
		IL_C2:
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

	public int idGift;

	public RewardElement[] rewards;

	public GameObject labelAchieved;

	public Button btnWatch;

	public Text countDown;

	private List<RewardData> _rewardData;

	private int durationNextGift;

	private void OnEnable()
	{
		var nextDate = ProfileManager.UserProfile.timestampNextFreeGifts[idGift].data.Value.FromUnixTime();
		durationNextGift = (int)(nextDate - System.DateTime.Now).TotalSeconds;
		this.UpdateState();
	}

	private void OnDisable()
	{
		GameData.timeCloseFreeGift = Time.realtimeSinceStartup;
	}

	public void Init()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ViewAdsGetFreeCoin, delegate (Component sender, object param)
		{
			this.UpdateState();
		});
		List<RewardData> list = GameData.staticFreeGiftData.GetRewards(this.idGift + 1);
		this._rewardData = list;
		for (int i = 0; i < this.rewards.Length; i++)
		{
			RewardElement rewardElement = this.rewards[i];
			if (i < list.Count)
			{
				rewardElement.gameObject.SetActive(true);
				rewardElement.SetInformation(list[i], false);
			}
			else
			{
				rewardElement.gameObject.SetActive(false);
			}
		}
		this.UpdateState();
	}

	public void UpdateState()
	{
		int num = ProfileManager.UserProfile.countViewAdsFreeCoin;
		// if (num >= this.times)
		// {
		// 	this.labelAchieved.SetActive(true);
		// 	this.btnWatch.gameObject.SetActive(false);
		// }
		// else
		{
			this.labelAchieved.SetActive(false);
			// if (num == this.times - 1)
			{
				if (durationNextGift > 0)
				{
					if (base.gameObject.activeInHierarchy)
					{
						this.btnWatch.gameObject.SetActive(false);
						this.countDown.transform.parent.gameObject.SetActive(true);
						base.StopAllCoroutines();
						base.StartCoroutine(this.StartCountDown(delegate
						{
							this.countDown.transform.parent.gameObject.SetActive(false);
							this.btnWatch.gameObject.SetActive(true);
							this.btnWatch.interactable = true;
						}));
					}
				}
				else
				{
					this.countDown.transform.parent.gameObject.SetActive(false);
					this.btnWatch.gameObject.SetActive(true);
					this.btnWatch.interactable = true;
				}
			}
			// else
			// {
			// 	this.btnWatch.gameObject.SetActive(true);
			// 	this.btnWatch.interactable = false;
			// }
		}
	}

	public void ViewAds()
	{
		SoundManager.Instance.PlaySfxClick();
		this.btnWatch.interactable = false;
		// if (ProfileManager.UserProfile.countViewAdsFreeCoin >= this.idGift)
		// {
		// 	return;
		// }
		// AdMob Remove

		if (Singleton<AdmobController>.Instance.IsRewardedAdLoaded())
		{
			Singleton<AdmobController>.Instance.ShowRewardedVideoAd(delegate (ShowResult showResult)
			{
				Time.timeScale = 1;
				if (showResult == ShowResult.Finished)
				{
					UnityEngine.Debug.Log("NIk Log is the Reward Complete");
					// StartCoroutine(DelayReward());
					Time.timeScale = 1;
					Invoke("DelayReward", 0.1f);
				}
				else
				{
					this.btnWatch.interactable = true;
				}
			});
		}
		else
		{
			Singleton<AdmobController>.Instance.RequestRewardBasedVideo();
			Singleton<Popup>.Instance.Show("Video Ad is not ready!\nPlease retry later.", "Video Ad", PopupType.Ok, delegate
			{
			}, null);
		}
	}

	public void DelayReward()
	{
		int num = ProfileManager.UserProfile.countViewAdsFreeCoin;
		num++;
		ProfileManager.UserProfile.countViewAdsFreeCoin.Set(num);
		RewardUtils.Receive(this._rewardData);
		Singleton<Popup>.Instance.ShowReward(this._rewardData, null, null);
		// GameData.durationNextGift = 10; // origina it was 10 sec
		durationNextGift = (idGift + 1) * 3 * 60; // now new one is added by hardik 1 min

		var nextDate = System.DateTime.Now.AddSeconds(durationNextGift);
		ProfileManager.UserProfile.timestampNextFreeGifts[idGift].Set(nextDate.ToUnixTime());
		EventDispatcher.Instance.PostEvent(EventID.ViewAdsGetFreeCoin);
		SoundManager.Instance.PlaySfx("sfx_get_reward", 0f);
		EventLogger.LogEvent("N_GetFreeGift", new object[]
		{
			this.idGift + 1
		});
	}

	private IEnumerator StartCountDown(Action callback)
	{
		CellViewFreeGift._StartCountDown_c__Iterator0 _StartCountDown_c__Iterator = new CellViewFreeGift._StartCountDown_c__Iterator0(this.durationNextGift);
		_StartCountDown_c__Iterator.callback = callback;
		_StartCountDown_c__Iterator._this = this;
		return _StartCountDown_c__Iterator;
	}
}
