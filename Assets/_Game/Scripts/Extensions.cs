using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class Extensions
{
	private sealed class _Delay_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal float time;

		internal Action callBack;

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

		public _Delay_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForSeconds(this.time);
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this.callBack();
				this._PC = -1;
				break;
			}
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

	private sealed class _DelayEndOfFrame_c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal Action callBack;

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

		public _DelayEndOfFrame_c__Iterator1()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._current = new WaitForEndOfFrame();
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this.callBack();
				this._PC = -1;
				break;
			}
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

	public static void FaceTo(this Transform transform, Transform target, float speed)
	{
		Vector2 vector = target.position - transform.position;
		float angle = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		Quaternion b = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, b, Time.deltaTime * speed);
	}

	public static void StartDelayAction(this MonoBehaviour mono, Action callback, float time)
	{
		mono.StartCoroutine(Extensions.Delay(callback, time));
	}

	public static void StartActionEndOfFrame(this MonoBehaviour mono, Action callback)
	{
		mono.StartCoroutine(Extensions.DelayEndOfFrame(callback));
	}

	private static IEnumerator Delay(Action callBack, float time)
	{
		Extensions._Delay_c__Iterator0 _Delay_c__Iterator = new Extensions._Delay_c__Iterator0();
		_Delay_c__Iterator.time = time;
		_Delay_c__Iterator.callBack = callBack;
		return _Delay_c__Iterator;
	}

	private static IEnumerator DelayEndOfFrame(Action callBack)
	{
		Extensions._DelayEndOfFrame_c__Iterator1 _DelayEndOfFrame_c__Iterator = new Extensions._DelayEndOfFrame_c__Iterator1();
		_DelayEndOfFrame_c__Iterator.callBack = callBack;
		return _DelayEndOfFrame_c__Iterator;
	}
}
