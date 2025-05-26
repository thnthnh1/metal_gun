using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropController : MonoBehaviour
{
	public ItemDropHealth itemHealthPrefab;

	public ItemDropCoin itemCoinPrefab;

	public ItemDropGun itemDropGunPrefab;

	public void Spawn(List<ItemDropData> items, Vector2 position, BaseEnemy unitDrop = null)
	{
		if (items.Count > 0)
		{
			for (int i = 0; i < items.Count; i++)
			{
				ItemDropData itemDropData = items[i];
				if (itemDropData.type == ItemDropType.Health)
				{
					this.SpawnHealth(itemDropData, position);
				}
				else if (itemDropData.type == ItemDropType.Coin)
				{
					this.SpawnCoin(itemDropData, position);
				}
				else if (itemDropData.type != ItemDropType.Ammo)
				{
					bool flag = GameData.mode == GameMode.Survival;
					bool flag2 = GameData.mode == GameMode.Campaign && GameData.currentStage.difficulty == Difficulty.Normal;
					if (flag || flag2)
					{
						this.SpawnGun(itemDropData, position);
					}
				}
			}
		}
	}

	private void SpawnHealth(ItemDropData data, Vector2 position)
	{
		ItemDropHealth itemDropHealth = Singleton<PoolingController>.Instance.poolItemDropHealth.New();
		if (itemDropHealth == null)
		{
			itemDropHealth = UnityEngine.Object.Instantiate<ItemDropHealth>(this.itemHealthPrefab);
		}
		itemDropHealth.Active(data, position);
	}

	private void SpawnCoin(ItemDropData data, Vector2 position)
	{
		int num = UnityEngine.Random.Range(2, 5);
		if ((float)num > data.value)
		{
			num = (int)data.value;
		}
		int num2 = Mathf.RoundToInt(data.value / (float)num);
		for (int i = 0; i < num; i++)
		{
			ItemDropCoin itemDropCoin = Singleton<PoolingController>.Instance.poolItemDropCoin.New();
			if (itemDropCoin == null)
			{
				itemDropCoin = UnityEngine.Object.Instantiate<ItemDropCoin>(this.itemCoinPrefab);
			}
			ItemDropData data2 = new ItemDropData(data.type, (float)num2, 100f);
			itemDropCoin.Active(data2, position);
		}
	}

	private void SpawnGun(ItemDropData data, Vector2 position)
	{
		ItemDropGun itemDropGun = Singleton<PoolingController>.Instance.poolItemDropGun.New();
		if (itemDropGun == null)
		{
			itemDropGun = UnityEngine.Object.Instantiate<ItemDropGun>(this.itemDropGunPrefab);
		}
		itemDropGun.Active(data, position);
	}
}
