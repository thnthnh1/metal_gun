using DG.Tweening;
using System;
using UnityEngine;

public class MoveByWaypoints : MonoBehaviour
{
	[SerializeField]
	private bool startOnAwake = true;

	[SerializeField]
	private bool loop = true;

	[SerializeField]
	private float speed;

	[SerializeField]
	private Transform[] waypoints;

	private int nextPointIndex = 1;

	private void Start()
	{
		if (this.waypoints.Length <= 1)
		{
			base.enabled = false;
			return;
		}
		base.transform.position = this.waypoints[0].position;
		if (this.startOnAwake)
		{
			this.StartMove();
		}
	}

	private void OnStepComplete()
	{
		this.nextPointIndex++;
		this.nextPointIndex %= this.waypoints.Length;
		if (this.nextPointIndex == 0 && !this.loop)
		{
			this.StopMove();
		}
		else
		{
			this.StartMove();
		}
	}

	public void StartMove()
	{
		base.transform.DOMove(this.waypoints[this.nextPointIndex].position, this.speed, false).SetEase(Ease.Linear).SetSpeedBased(true).OnComplete(new TweenCallback(this.OnStepComplete));
	}

	public void StopMove()
	{
		base.transform.DOKill(false);
	}
}
