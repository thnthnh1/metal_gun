using System;
using UnityEngine;

public class MapTriggerPoint : MonoBehaviour
{
	public MapTriggerPointType triggerType;

	private Zone mainZone;

	private int nextZoneId;

	private void Start()
	{
		MapTriggerPointType mapTriggerPointType = this.triggerType;
		if (mapTriggerPointType != MapTriggerPointType.LoadNextEnemyPack)
		{
			if (mapTriggerPointType == MapTriggerPointType.EnterZone)
			{
				this.mainZone = base.transform.parent.GetComponent<Zone>();
			}
		}
		else
		{
			this.nextZoneId = int.Parse(base.name);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			MapTriggerPointType mapTriggerPointType = this.triggerType;
			if (mapTriggerPointType != MapTriggerPointType.EnterZone)
			{
				if (mapTriggerPointType != MapTriggerPointType.LockZone)
				{
					if (mapTriggerPointType == MapTriggerPointType.LoadNextEnemyPack)
					{
						this.LoadNextZoneEnemy();
					}
				}
				else
				{
					this.LockCurrentZone();
				}
			}
			else
			{
				this.EnterNewZone();
			}
		}
		base.gameObject.SetActive(false);
	}

	private void EnterNewZone()
	{
		if (this.mainZone != null)
		{
			EventDispatcher.Instance.PostEvent(EventID.EnterZone, this.mainZone.id);
			base.gameObject.SetActive(false);
		}
	}

	private void LockCurrentZone()
	{
		Singleton<GameController>.Instance.CampaignMap.LockCurrentZone();
	}

	private void LoadNextZoneEnemy()
	{
		((CampaignModeController)Singleton<GameController>.Instance.modeController).CreateEnemyInZone(this.nextZoneId);
	}
}
