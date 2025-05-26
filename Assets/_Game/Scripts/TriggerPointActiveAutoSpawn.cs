using System;
using UnityEngine;

public class TriggerPointActiveAutoSpawn : MonoBehaviour
{
	public bool isActiveAutoSpawn = true;

	private BoxCollider2D sensor;

	private void Awake()
	{
		this.sensor = base.GetComponent<BoxCollider2D>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			((CampaignModeController)Singleton<GameController>.Instance.modeController).IsAllowSpawnSideEnemy = this.isActiveAutoSpawn;
			base.gameObject.SetActive(false);
		}
	}
}
