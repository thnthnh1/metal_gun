using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ParalaxObject : MonoBehaviour
{
	private sealed class _Start_c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		internal ParalaxObject _this;

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
				if (this._this.nearObjects.Length <= 0 && this._this.farObjects.Length <= 0)
				{
					this._this.enabled = false;
				}
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

	public float nearSpeed;

	public float middleSpeed;

	public float farSpeed;

	public Transform endPoint;

	public Transform startPoint;

	public Transform[] nearObjects;

	public Transform[] middleObjects;

	public Transform[] farObjects;

	private float lastCameraX;

	private IEnumerator Start()
	{
		ParalaxObject._Start_c__Iterator0 _Start_c__Iterator = new ParalaxObject._Start_c__Iterator0();
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
			float num2 = num * this.nearSpeed * Time.deltaTime;
			for (int i = 0; i < this.nearObjects.Length; i++)
			{
				Vector3 position = this.nearObjects[i].position;
				position.x += num2;
				if (this.nearObjects[i].position.x < this.endPoint.position.x)
				{
					position.x = this.startPoint.position.x;
				}
				else if (this.nearObjects[i].position.x > this.startPoint.position.x)
				{
					position.x = this.endPoint.position.x;
				}
				this.nearObjects[i].position = position;
			}
			num2 = num * this.middleSpeed * Time.deltaTime;
			for (int j = 0; j < this.middleObjects.Length; j++)
			{
				Vector3 position2 = this.middleObjects[j].position;
				position2.x += num2;
				if (this.middleObjects[j].position.x < this.endPoint.position.x)
				{
					position2.x = this.startPoint.position.x;
				}
				else if (this.middleObjects[j].position.x > this.startPoint.position.x)
				{
					position2.x = this.endPoint.position.x;
				}
				this.middleObjects[j].position = position2;
			}
			num2 = num * this.farSpeed * Time.deltaTime;
			for (int k = 0; k < this.farObjects.Length; k++)
			{
				Vector3 position3 = this.farObjects[k].position;
				position3.x += num2;
				if (this.farObjects[k].position.x < this.endPoint.position.x)
				{
					position3.x = this.startPoint.position.x;
				}
				else if (this.farObjects[k].position.x > this.startPoint.position.x)
				{
					position3.x = this.endPoint.position.x;
				}
				this.farObjects[k].position = position3;
			}
		}
	}
}
