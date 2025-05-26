using System;
using UnityEngine;

public class BossPointData
{
	public Vector2 position;

	public int bossId;

	public BossPointData(Vector2 position, int bossId)
	{
		this.position = position;
		this.bossId = bossId;
	}
}
