using System;
using UnityEngine;

public class LabAutoDoorSensor : MonoBehaviour
{
	public GameObject[] objectShowWhenOpen;

	private CircleCollider2D sensor;

	private bool isOpeningDoor;

	private Transform door;

	private Vector2 doorDestination;

	private void Awake()
	{
		this.sensor = base.GetComponent<CircleCollider2D>();
		this.door = base.transform.parent;
		Vector2 vector = this.door.position;
		vector.y += 2.35f;
		this.doorDestination = vector;
		this.ShowObject(false);
	}

	private void Update()
	{
		if (this.isOpeningDoor)
		{
			if (Mathf.Abs(this.doorDestination.y - this.door.position.y) > 0.1f)
			{
				this.door.position = Vector2.MoveTowards(this.door.position, this.doorDestination, 2f * Time.deltaTime);
			}
			else
			{
				this.door.position = this.doorDestination;
				this.isOpeningDoor = false;
				this.ShowObject(true);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.root.CompareTag("Player"))
		{
			this.isOpeningDoor = true;
			this.sensor.enabled = false;
			SoundManager.Instance.PlaySfx("sfx_door_open", 0f);
		}
	}

	private void ShowObject(bool isShow)
	{
		for (int i = 0; i < this.objectShowWhenOpen.Length; i++)
		{
			this.objectShowWhenOpen[i].SetActive(isShow);
		}
	}
}
