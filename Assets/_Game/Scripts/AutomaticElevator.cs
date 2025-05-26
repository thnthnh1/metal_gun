using System;
using UnityEngine;

public class AutomaticElevator : MonoBehaviour
{
	public GameObject elevator;

	public Transform botPoint;

	public Transform topPoint;

	public bool isMovingDown;

	public float speed = 2.5f;

	public float waitTime = 1.5f;

	private bool isMoving = true;

	private void Update()
	{
		if (this.isMoving)
		{
			if (this.isMovingDown)
			{
				if (this.elevator.transform.position.y > this.botPoint.position.y)
				{
					this.elevator.transform.position = Vector2.MoveTowards(this.elevator.transform.position, this.botPoint.position, this.speed * Time.deltaTime);
				}
				else
				{
					this.isMoving = false;
					this.isMovingDown = false;
					this.elevator.transform.position = this.botPoint.position;
					this.StartDelayAction(delegate
					{
						this.isMoving = true;
					}, this.waitTime);
				}
			}
			else if (this.elevator.transform.position.y < this.topPoint.position.y)
			{
				this.elevator.transform.position = Vector2.MoveTowards(this.elevator.transform.position, this.topPoint.position, this.speed * Time.deltaTime);
			}
			else
			{
				this.isMoving = false;
				this.isMovingDown = true;
				this.elevator.transform.position = this.topPoint.position;
				this.StartDelayAction(delegate
				{
					this.isMoving = true;
				}, this.waitTime);
			}
		}
	}
}
