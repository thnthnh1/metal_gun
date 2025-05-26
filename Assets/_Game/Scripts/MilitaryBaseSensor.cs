using System;
using UnityEngine;

public class MilitaryBaseSensor : MonoBehaviour
{
	private CircleCollider2D sensor;

	private MilitaryBase office;

	private void Awake()
	{
		this.sensor = base.GetComponent<CircleCollider2D>();
		this.office = base.transform.parent.GetComponent<MilitaryBase>();
		if (this.sensor == null)
		{
		}
		if (this.office == null)
		{
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			this.office.OnAlarm();
		}
	}
}
