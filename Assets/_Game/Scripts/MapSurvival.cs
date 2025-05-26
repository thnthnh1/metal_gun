using System;
using System.Collections.Generic;
using UnityEngine;

public class MapSurvival : MonoBehaviour
{
	[Header("MARGIN")]
	public Transform marginLeft;

	public Transform marginTop;

	public Transform marginRight;

	public Transform marginBottom;

	[Header("")]
	public Transform playerSpawnPoint;

	public Transform cameraInitialPoint;

	public BaseSpawnLocation[] locations;

	public void Init()
	{
		this.SetLocationId();
		this.SetDefaultMapMargin();
	}

	public void SetDefaultMapMargin()
	{
		Singleton<CameraFollow>.Instance.SetMarginTop(this.marginTop.position.y);
		Singleton<CameraFollow>.Instance.SetMarginLeft(this.marginLeft.position.x);
		Singleton<CameraFollow>.Instance.SetMarginRight(this.marginRight.position.x);
		Singleton<CameraFollow>.Instance.SetMarginBottom(this.marginBottom.position.y);
	}

	private void SetLocationId()
	{
		for (int i = 0; i < this.locations.Length; i++)
		{
			this.locations[i].id = i;
		}
	}

	public List<int> GetLocationCanSpawnUnit(SurvivalEnemy unit)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.locations.Length; i++)
		{
			if (!this.locations[i].noSpawnTypes.Contains(unit) && !this.locations[i].isSpawning)
			{
				list.Add(this.locations[i].id);
			}
		}
		return list;
	}

	public void AddUnitToSpawnLocation(SurvivalEnemy enemy, int locationId, int minLevelUnit, int maxLevelUnit)
	{
		for (int i = 0; i < this.locations.Length; i++)
		{
			if (this.locations[i].id == locationId)
			{
				this.locations[i].AddUnit(enemy, minLevelUnit, maxLevelUnit);
			}
		}
	}

	public void Spawn()
	{
		for (int i = 0; i < this.locations.Length; i++)
		{
			BaseSpawnLocation baseSpawnLocation = this.locations[i];
			if (baseSpawnLocation.CanSpawn())
			{
				baseSpawnLocation.Spawn();
			}
		}
	}
}
