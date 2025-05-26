using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParalaxUV : MonoBehaviour
{
	private sealed class _Start_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ParalaxUV _this;

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

		public _Start_c__Iterator0()
		{
		}

		public bool MoveNext()
		{
			uint num = (uint)this._PC;
			this._PC = -1;
			switch (num)
			{
			case 0u:
				this._this.mat = this._this.render.sharedMaterial;
				this._current = StaticValue.waitHalfSec;
				if (!this._disposing)
				{
					this._PC = 1;
				}
				return true;
			case 1u:
				this._this.lastCameraX = Camera.main.transform.position.x;
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

	public float speed;

	public MeshRenderer render;

	private float lastCameraX;

	private Material mat;

	private IEnumerator Start()
	{
		ParalaxUV._Start_c__Iterator0 _Start_c__Iterator = new ParalaxUV._Start_c__Iterator0();
		_Start_c__Iterator._this = this;
		return _Start_c__Iterator;
	}

	private void Update()
	{
		if (Singleton<GameController>.Instance.Player == null)
		{
			return;
		}
		float num = -Mathf.Sign(Camera.main.transform.position.x - this.lastCameraX);
		if (Mathf.Abs(this.lastCameraX - Camera.main.transform.position.x) > 0.02f && Singleton<GameController>.Instance.Player.IsMoving)
		{
			this.lastCameraX = Camera.main.transform.position.x;
			Vector2 mainTextureOffset = this.mat.mainTextureOffset;
			mainTextureOffset.x += num * this.speed * Time.deltaTime;
			this.mat.mainTextureOffset = mainTextureOffset;
		}
	}
}
