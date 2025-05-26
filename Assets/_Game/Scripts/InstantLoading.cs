using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class InstantLoading : MonoBehaviour
{
	private sealed class _Timeout_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal InstantLoading _this;

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

		public _Timeout_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			// UnityEngine.Debug.Log("Nik Log is the " + num);
			switch (num)
			{
			case 0u:
				break;
			case 1u:
				this._this.timeout--;
				this._this.labelTimeout.text = this._this.timeout.ToString();
				break;
			default:
				return false;
			}
			if (this._this.timeout > 0)
			{
				this._current = StaticValue.waitOneSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			if (MainMenu.instance.IsFBLogin)
			{
				MainMenu.instance.IsFBLogin= false;
				Mp_PlayerInfoScreen.instance.LoadData();
				Launcher.launcher.OpenMultiplayerMenu();
			}
			UnityEngine.Debug.Log("Nik Log is the Close Popup false");
			this._this.gameObject.SetActive(false);
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

	[SerializeField]
	private Text labelTimeout;

	private int timeout = 15;

	public void Show(int timeout)
	{
		base.StopAllCoroutines();
		this.timeout = timeout;
		this.labelTimeout.text = timeout.ToString();
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.Timeout());
	}

	public void Hide()
	{
		UnityEngine.Debug.Log("Nik Log is the Close Popup");
		base.StopAllCoroutines();
		base.gameObject.SetActive(false);
	}

	private IEnumerator Timeout()
	{
		InstantLoading._Timeout_c__Iterator0 _Timeout_c__Iterator = new InstantLoading._Timeout_c__Iterator0();
		_Timeout_c__Iterator._this = this;
		return _Timeout_c__Iterator;
	}
}
