using System;
using UnityEngine;

namespace Water2DTool
{
	public class RippleSource : MonoBehaviour
	{
		private Vector3 prevPos;

		private Water2D_Simulation sim;

		private float precisionFactor = 0.0001f;

		private float currentPeriod = 0.1f;

		private float timeCount = 0.1f;

		public float radius = 0.3f;

		public float strength = -0.15f;

		public float frequency = 5f;

		public float minPeriod = 0.5f;

		public float maxPeriod = 1f;

		public float waterLineYAxisWorldPosition;

		public bool active = true;

		public bool newRipple;

		public float handleScale = 0.3f;

		public bool ignoreYAxisPosition;

		public RippleSourceMode sourceMode = RippleSourceMode.RandomInterval;

		public float interactionDistance = 1f;

		public new Transform transform;

		private void Start()
		{
			this.transform = base.GetComponent<Transform>();
		}

		private void Update()
		{
			if (this.active)
			{
				if (this.sim != null)
				{
					this.waterLineYAxisWorldPosition = this.sim.waterLineCurrentWorldPos.y;
				}
				RippleSourceMode rippleSourceMode = this.sourceMode;
				if (rippleSourceMode != RippleSourceMode.WhenMoving)
				{
					if (rippleSourceMode != RippleSourceMode.RandomInterval)
					{
						if (rippleSourceMode == RippleSourceMode.FixedInterval)
						{
							this.FixedInterval();
						}
					}
					else
					{
						this.RandomInterval();
					}
				}
				else
				{
					this.WhenMoving();
				}
			}
		}

		private void WhenMoving()
		{
			float sqrMagnitude = (this.prevPos - this.transform.position).sqrMagnitude;
			if (this.ignoreYAxisPosition)
			{
				this.WhenMovingRipple(sqrMagnitude);
			}
			else if (Mathf.Abs(this.transform.position.y - this.waterLineYAxisWorldPosition) <= this.interactionDistance && this.transform.position.y > this.waterLineYAxisWorldPosition - this.interactionDistance)
			{
				this.WhenMovingRipple(sqrMagnitude);
			}
		}

		private void RandomInterval()
		{
			if (this.ignoreYAxisPosition)
			{
				this.RandomIntervalRipple();
			}
			else if (Mathf.Abs(this.transform.position.y - this.waterLineYAxisWorldPosition) <= this.interactionDistance && this.transform.position.y > this.waterLineYAxisWorldPosition - this.interactionDistance)
			{
				this.RandomIntervalRipple();
			}
		}

		private void FixedInterval()
		{
			if (this.ignoreYAxisPosition)
			{
				this.FixedIntervalRipple();
			}
			else if (Mathf.Abs(this.transform.position.y - this.waterLineYAxisWorldPosition) <= this.interactionDistance && this.transform.position.y > this.waterLineYAxisWorldPosition - this.interactionDistance)
			{
				this.FixedIntervalRipple();
			}
		}

		private void WhenMovingRipple(float sqrLen)
		{
			if (sqrLen > this.precisionFactor * this.precisionFactor)
			{
				this.newRipple = true;
			}
			this.prevPos = this.transform.position;
		}

		private void RandomIntervalRipple()
		{
			this.timeCount += Time.deltaTime;
			if (this.timeCount > this.currentPeriod)
			{
				this.timeCount = 0f;
				this.newRipple = true;
				this.NewPeriod();
			}
		}

		private void FixedIntervalRipple()
		{
			float num = 1f / this.frequency;
			if (this.timeCount > num)
			{
				this.timeCount -= num;
				this.newRipple = true;
			}
			this.timeCount += Time.deltaTime;
		}

		public void NewPeriod()
		{
			this.currentPeriod = UnityEngine.Random.Range(this.minPeriod, this.maxPeriod);
		}

		private void OnDrawGizmosSelected()
		{
			if (this.transform == null)
			{
				this.transform = base.GetComponent<Transform>();
			}
			Gizmos.color = new Color(0f, 1f, 0f, 1f);
			Gizmos.DrawLine(this.transform.position - new Vector3(0f, this.interactionDistance, 0f), this.transform.position + new Vector3(0f, this.interactionDistance, 0f));
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			this.sim = collision.GetComponent<Water2D_Simulation>();
			if (this.sim != null)
			{
				this.active = true;
				this.waterLineYAxisWorldPosition = this.sim.waterLineCurrentWorldPos.y;
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (collision.GetComponent<Water2D_Simulation>() == this.sim)
			{
				this.active = false;
				this.sim = null;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			this.sim = other.GetComponent<Water2D_Simulation>();
			if (this.sim != null)
			{
				this.active = true;
				this.waterLineYAxisWorldPosition = this.sim.waterLineCurrentWorldPos.y;
			}
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.GetComponent<Water2D_Simulation>() == this.sim)
			{
				this.active = false;
				this.sim = null;
			}
		}
	}
}
