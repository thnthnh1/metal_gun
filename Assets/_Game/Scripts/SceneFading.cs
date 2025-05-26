using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFading : MonoBehaviour
{
	public enum FadeAlpha
	{
		Transparent,
		Black
	}

	private sealed class _FadePingPongBlackAlpha_c__AnonStorey1
	{
		internal float fadingTime;

		internal UnityAction toBlackCallback;

		internal UnityAction finishCallback;

		internal SceneFading _this;

		internal void __m__0()
		{
			this._this.ToBlack(this.fadingTime, this.toBlackCallback, this.finishCallback);
		}
	}

	private sealed class _FadeOutAndLoadScene_c__AnonStorey2
	{
		internal string nextSceneName;

		internal void __m__0()
		{
			SceneManager.LoadScene(this.nextSceneName);
		}
	}

	private sealed class _StartFadeTo_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal Color _c___0;

		internal SceneFading.FadeAlpha color;

		internal int _alpha___0;

		internal float fadingTime;

		internal bool resetAfterFinish;

		internal UnityAction callback;

		internal SceneFading _this;

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

		public _StartFadeTo_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._c___0 = this._this.fadeImg.color;
				this._alpha___0 = (int)this.color;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._c___0.a != (float)this._alpha___0)
			{
				this._c___0.a = Mathf.MoveTowards(this._c___0.a, (float)this._alpha___0, this.fadingTime * Time.deltaTime);
				this._this.fadeImg.color = this._c___0;
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			this._this.isFading = false;
			if (this.resetAfterFinish)
			{
				this._c___0.a = 0f;
				this._this.fadeImg.color = this._c___0;
			}
			if (this.callback != null)
			{
				this.callback();
			}
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

	private static SceneFading _Instance_k__BackingField;

	[SerializeField]
	private Image fadeImg;

	private bool isFading;

	public static SceneFading Instance
	{
		get;
		private set;
	}

	private void Awake()
	{
		if (SceneFading.Instance == null)
		{
			SceneFading.Instance = this;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void FadeAlphaTo(SceneFading.FadeAlpha alpha, float fadingTime, bool resetAfterFinish, UnityAction callback = null)
	{
		if (!this.isFading)
		{
			this.isFading = true;
			base.StartCoroutine(this.StartFadeTo(alpha, fadingTime, resetAfterFinish, callback));
		}
	}

	public void FadePingPongBlackAlpha(float fadingTime, UnityAction toBlackCallback = null, UnityAction finishCallback = null)
	{
		this.FadeAlphaTo(SceneFading.FadeAlpha.Black, fadingTime, false, delegate
		{
			this.ToBlack(fadingTime, toBlackCallback, finishCallback);
		});
	}

	public void FadeOutAndLoadScene(string nextSceneName, bool isShowLoading = true, float fadingTime = 2f)
	{
		if (isShowLoading)
		{
			Loading.nextScene = nextSceneName;
			Singleton<Popup>.Instance.loading.Show();
		}
		else
		{
			this.FadePingPongBlackAlpha(fadingTime, delegate
			{
				SceneManager.LoadScene(nextSceneName);
			}, null);
		}
	}

	public void ResetAlpha()
	{
		Color color = this.fadeImg.color;
		color.a = 0f;
		this.fadeImg.color = color;
	}

	private IEnumerator StartFadeTo(SceneFading.FadeAlpha color, float fadingTime, bool resetAfterFinish, UnityAction callback)
	{
		SceneFading._StartFadeTo_c__Iterator0 _StartFadeTo_c__Iterator = new SceneFading._StartFadeTo_c__Iterator0();
		_StartFadeTo_c__Iterator.color = color;
		_StartFadeTo_c__Iterator.fadingTime = fadingTime;
		_StartFadeTo_c__Iterator.resetAfterFinish = resetAfterFinish;
		_StartFadeTo_c__Iterator.callback = callback;
		_StartFadeTo_c__Iterator._this = this;
		return _StartFadeTo_c__Iterator;
	}

	private void ToBlack(float fadingTime, UnityAction toBlackCallback, UnityAction finishCallback)
	{
		if (toBlackCallback != null)
		{
			toBlackCallback();
		}
		this.FadeAlphaTo(SceneFading.FadeAlpha.Transparent, fadingTime, false, finishCallback);
	}
}
