using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HudSaveMe : MonoBehaviour
{
	private sealed class _CountDown_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal int _remaining___0;

		internal HudSaveMe _this;

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

		public _CountDown_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._remaining___0 = this._this.timeOut;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._remaining___0 > 0)
			{
				this._this.textCountDown.text = this._remaining___0.ToString();
				this._this.textCountDown.gameObject.SetActive(false);
				this._this.textCountDown.gameObject.SetActive(true);
				this._remaining___0--;
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.Close();
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

	public Button btnReviveByGem;

	public Button btnWatchAds;

	public Text textPrice;

	public Text textCountDown;

	public Image progress;

	public RectTransform head;

	public Color32 colorEnoughCoin;

	private int timeOut = 10;

	public void Open(float curProgress)
	{
		Singleton<UIController>.Instance.ActiveIngameUI(false);
		this.textPrice.text = 30.ToString("n0");
		bool flag = GameData.playerResources.gem >= 30;
		this.textPrice.color = ((!flag) ? StaticValue.color32NotEnoughMoney : this.colorEnoughCoin);
		this.btnReviveByGem.enabled = flag;
		this.progress.fillAmount = curProgress;
		Vector2 anchoredPosition = this.head.anchoredPosition;
		anchoredPosition.x = curProgress * 500f;
		this.head.anchoredPosition = anchoredPosition;
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.CountDown());
	}

	public void Close()
	{
		Singleton<UIController>.Instance.ActiveIngameUI(true);
		SoundManager.Instance.PlaySfxClick();
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
		EventDispatcher.Instance.PostEvent(EventID.GameEnd, false);
	}

	public void WatchAdsToRevive()
	{
		SoundManager.Instance.PlaySfxClick();
		this.btnWatchAds.interactable = false;
		// AdMob Remove
		Singleton<AdmobController>.Instance.ShowRewardedVideoAd(delegate(ShowResult showResult)
		{
			Time.timeScale = 1f;
			if (showResult == ShowResult.Finished)
			{
				UnityEngine.Debug.Log("NIk Log is the Reward Complete");
				// this.ReviveByAds();
				// StartCoroutine(ReviveByAds());
				Time.timeScale = 1f;
				Time.fixedDeltaTime = 0.02f;
				Invoke("DelayReward", 0.1f);
			}
			else
			{
				this.btnWatchAds.interactable = true;
			}
			base.StopAllCoroutines();
		});
	}

	public void DelayReward()
	{
		int num = ProfileManager.UserProfile.countViewAdsFreeCoin;
		base.gameObject.SetActive(false);
		Singleton<GameController>.Instance.SetActiveAllUnits(true);
		EventDispatcher.Instance.PostEvent(EventID.ReviveByAds);
		EventLogger.LogEvent("N_ReviveByAds", new object[]
		{
			string.Format("{0}-{1}", GameData.currentStage.id, GameData.currentStage.difficulty)
		});
	}

	
	public void ReviveByGem()
	{
		SoundManager.Instance.PlaySfxClick();
		if (GameData.playerResources.gem >= 30)
		{
			base.gameObject.SetActive(false);
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f;
			Singleton<GameController>.Instance.SetActiveAllUnits(true);
			GameData.playerResources.ConsumeGem(30);
			EventDispatcher.Instance.PostEvent(EventID.ReviveByGem);
			EventLogger.LogEvent("N_ReviveByGem", new object[]
			{
				string.Format("{0}-{1}", GameData.currentStage.id, GameData.currentStage.difficulty)
			});
		}
	}

	private IEnumerator CountDown()
	{
		HudSaveMe._CountDown_c__Iterator0 _CountDown_c__Iterator = new HudSaveMe._CountDown_c__Iterator0();
		_CountDown_c__Iterator._this = this;
		return _CountDown_c__Iterator;
	}
}
