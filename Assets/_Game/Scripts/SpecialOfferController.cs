using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SpecialOfferController : MonoBehaviour
{
	private sealed class _CoroutineTimerEnd_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal DateTime _current___0;

		internal DateTime _endDay___0;

		internal double _timeLeft___0;

		internal TimeSpan _t___1;

		internal int _hours___1;

		internal int _minutes___1;

		internal int _seconds___1;

		internal SpecialOfferController _this;

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

		public _CoroutineTimerEnd_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current___0 = DateTime.Now;
				this._endDay___0 = new DateTime(this._current___0.Year, this._current___0.Month, this._current___0.Day, 23, 59, 59);
				this._timeLeft___0 = TimeSpan.FromTicks(this._endDay___0.Ticks - this._current___0.Ticks).TotalSeconds;
				break;
			case 1u:
				this._t___1 = TimeSpan.FromSeconds(this._timeLeft___0);
				this._hours___1 = 0;
				this._minutes___1 = 0;
				this._seconds___1 = 0;
				Singleton<MasterInfo>.Instance.CountDownTimer(this._t___1, out this._hours___1, out this._minutes___1, out this._seconds___1);
				this._this.textCountDown.text = string.Format("{0:00}:{1:00}:{2:00}", this._hours___1, this._minutes___1, this._seconds___1);
				this._timeLeft___0 -= 1.0;
				break;
			default:
				return false;
			}
			if (this._timeLeft___0 > 0.0)
			{
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.textCountDown.text = string.Empty;
			this._this.Hide();
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

	public GameObject popupSpecialOffer;

	public Text textCountDown;

	public SpecialOfferSpine packGun;

	public SpecialOfferSpine packMoney;

	public GameObject btnBuyPackEverybodyFavorite;

	public GameObject btnBuyPackDragonBreath;

	public GameObject btnBuyPackLetThereBeFire;

	public GameObject btnBuyPackSnippingForDummies;

	public GameObject btnBuyPackTaserLaser;

	public GameObject btnBuyPackShockingSale;

	public GameObject btnBuyPackEnthusiast;

	public bool isShowPack = true;

	private void Awake()
	{
		EventDispatcher.Instance.RegisterListener(EventID.BuySpecialOffer, delegate(Component sender, object param)
		{
			this.Hide();
		});
		// this.Check(); // no need to show special offer commented by hardik
	}

	public void Show(bool isShow)
	{
		//commented by hardik
		// this.popupSpecialOffer.transform.localScale = ((!isShow) ? Vector3.zero : Vector3.one);
		// this.popupSpecialOffer.SetActive(isShow);
		// if (this.packGun.gameObject.activeSelf)
		// {
		// 	this.packGun.Show();
		// }
		// else
		// {
		// 	this.packMoney.Show();
		// }
		// if (isShow)
		// {
		// 	SoundManager.Instance.PlaySfx("sfx_show_dialog", 0f);
		// }
	}

	public void Hide()
	{
		this.Show(false);
		base.gameObject.SetActive(false);
	}

	private void Check()
	{
		if (!ProfileManager.UserProfile.isPurchasedStarterPack)
		{
			this.isShowPack = false;
			base.gameObject.SetActive(false);
		}
		else
		{
			this.packGun.gameObject.SetActive(true);
			this.packMoney.gameObject.SetActive(false);
			this.isShowPack = true;
			base.gameObject.SetActive(true);
			if (!ProfileManager.UserProfile.isPurchasedPackDragonBreath && !GameData.playerGuns.ContainsKey(2))
			{
				this.packGun.type = SpecialOffer.DragonBreath;
				this.btnBuyPackDragonBreath.SetActive(true);
			}
			else if (!ProfileManager.UserProfile.isPurchasedPackSnippingForDummies && !GameData.playerGuns.ContainsKey(7))
			{
				this.packGun.type = SpecialOffer.SnippingForDummies;
				this.btnBuyPackSnippingForDummies.SetActive(true);
			}
			else if (!ProfileManager.UserProfile.isPurchasedPackTaserLaser && !GameData.playerGuns.ContainsKey(103))
			{
				this.packGun.type = SpecialOffer.TaserLaser;
				this.btnBuyPackTaserLaser.SetActive(true);
			}
			else
			{
				this.isShowPack = false;
				base.gameObject.SetActive(false);
			}
		}
	}

	private IEnumerator CoroutineTimerEnd()
	{
		SpecialOfferController._CoroutineTimerEnd_c__Iterator0 _CoroutineTimerEnd_c__Iterator = new SpecialOfferController._CoroutineTimerEnd_c__Iterator0();
		_CoroutineTimerEnd_c__Iterator._this = this;
		return _CoroutineTimerEnd_c__Iterator;
	}
}
