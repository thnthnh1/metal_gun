using System;
using UnityEngine;

public class HouseSpawnEnemySensor : MonoBehaviour
{
	private CircleCollider2D sensor;

	private HouseSpawnEnemy house;

	private void Awake()
	{
		this.sensor = base.GetComponent<CircleCollider2D>();
		this.house = base.transform.parent.GetComponent<HouseSpawnEnemy>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			this.house.Open();
			base.gameObject.SetActive(false);
		}
	}
}
