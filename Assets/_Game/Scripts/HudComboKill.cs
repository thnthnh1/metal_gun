using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class HudComboKill : MonoBehaviour
{
	private sealed class _CoroutineHideCombo_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float _timeCount___0;

		internal float _percent___1;

		internal HudComboKill _this;

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

		public _CoroutineHideCombo_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.textComboKill.gameObject.SetActive(false);
				this._this.textComboKill.gameObject.SetActive(true);
				this._this.SetAlphaElements(1f);
				this._timeCount___0 = 0f;
				break;
			case 1u:
				break;
			default:
				return false;
			}
			if (this._timeCount___0 <= this._this.timeOutResetCombo)
			{
				this._timeCount___0 += Time.deltaTime;
				this._percent___1 = Mathf.Clamp01((this._this.timeOutResetCombo - this._timeCount___0) / this._this.timeOutResetCombo);
				this._this.SetAlphaElements(this._percent___1);
				this._current = null;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			}
			EventDispatcher.Instance.PostEvent(EventID.TimeOutComboKill);
			this._this.coroutineHideCombo = null;
			this._this.imageCombo.gameObject.SetActive(false);
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

	public Image imageCombo;

	public Text textComboKill;

	public Outline textComboKillOutline;

	public AudioClip[] sfxComboKills;

	public AudioClip comboKillMax;

	private float timeOutResetCombo = 2f;

	private IEnumerator coroutineHideCombo;

	public void Init()
	{
		EventDispatcher.Instance.RegisterListener(EventID.GetComboKill, delegate(Component sender, object param)
		{
			this.UpdateComboKills((int)param);
		});
	}

	public void UpdateComboKills(int killCount)
	{
		if (killCount > 0)
		{
			this.imageCombo.gameObject.SetActive(true);
			this.textComboKill.text = killCount.ToString();
			AudioClip clip = (killCount <= this.sfxComboKills.Length) ? this.sfxComboKills[killCount - 1] : this.comboKillMax;
			float decibel = (killCount % 2 != 1) ? 0f : -15f;
			SoundManager.Instance.PlaySfx(clip, decibel);
			if (this.coroutineHideCombo != null)
			{
				base.StopCoroutine(this.coroutineHideCombo);
				this.coroutineHideCombo = null;
			}
			this.coroutineHideCombo = this.CoroutineHideCombo();
			base.StartCoroutine(this.coroutineHideCombo);
		}
	}

	private IEnumerator CoroutineHideCombo()
	{
		HudComboKill._CoroutineHideCombo_c__Iterator0 _CoroutineHideCombo_c__Iterator = new HudComboKill._CoroutineHideCombo_c__Iterator0();
		_CoroutineHideCombo_c__Iterator._this = this;
		return _CoroutineHideCombo_c__Iterator;
	}

	private void SetAlphaElements(float a)
	{
		Color color = this.imageCombo.color;
		color.a = a;
		this.imageCombo.color = color;
		color = this.textComboKill.color;
		color.a = a;
		this.textComboKill.color = color;
		color = this.textComboKillOutline.effectColor;
		color.a = a;
		this.textComboKillOutline.effectColor = color;
	}
}
