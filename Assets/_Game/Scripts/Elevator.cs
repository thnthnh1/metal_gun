using System;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	public Transform destination;

	public Collider2D top;

	public GameObject baseElevator;

	public float moveSpeed = 1f;

	public Collider2D[] sideColliders;

	private bool isMoving;

	private Collider2D trigger;

	private void Awake()
	{
		this.trigger = base.GetComponent<BoxCollider2D>();
		this.top.enabled = true;
		this.trigger.enabled = true;
		this.ActiveSideColliders(false);
	}

	private void Update()
	{
		if (this.isMoving)
		{
			if (Mathf.Abs(this.destination.position.y - this.baseElevator.transform.position.y) >= 0.1f)
			{
				this.baseElevator.transform.position = Vector3.MoveTowards(this.baseElevator.transform.position, this.destination.position, this.moveSpeed * Time.deltaTime);
			}
			else
			{
				this.baseElevator.transform.position = this.destination.position;
				this.isMoving = false;
				this.ActiveSideColliders(false);
				base.enabled = false;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			this.trigger.enabled = false;
			this.top.enabled = false;
			this.ActiveSideColliders(true);
			this.StartDelayAction(delegate
			{
				this.isMoving = true;
			}, 1f);
		}
	}

	private void ActiveSideColliders(bool isActive)
	{
		for (int i = 0; i < this.sideColliders.Length; i++)
		{
			this.sideColliders[i].enabled = isActive;
		}
	}
}
