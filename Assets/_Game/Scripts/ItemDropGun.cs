using System;
using UnityEngine;

public class ItemDropGun : BaseItemDrop
{
	public SpriteRenderer gunImage;

	protected override void Awake()
	{
		base.Awake();
		EventDispatcher.Instance.RegisterListener(EventID.CompleteWave, delegate(Component sender, object param)
		{
			if (base.gameObject.activeInHierarchy)
			{
				this.Deactive();
			}
		});
	}

	public override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingController>.Instance.poolItemDropGun.Store(this);
	}

	public override void Active(ItemDropData data, Vector2 position)
	{
		base.Active(data, position);
		switch (data.type)
		{
		case ItemDropType.GunSpread:
			this.gunImage.sprite = GameResourcesUtils.GetGunImage(100);
			break;
		case ItemDropType.GunRocketChaser:
			this.gunImage.sprite = GameResourcesUtils.GetGunImage(101);
			break;
		case ItemDropType.GunFamas:
			this.gunImage.sprite = GameResourcesUtils.GetGunImage(102);
			break;
		case ItemDropType.GunLaser:
			this.gunImage.sprite = GameResourcesUtils.GetGunImage(103);
			break;
		case ItemDropType.GunSplit:
			this.gunImage.sprite = GameResourcesUtils.GetGunImage(104);
			break;
		case ItemDropType.GunFireBall:
			this.gunImage.sprite = GameResourcesUtils.GetGunImage(105);
			break;
		case ItemDropType.GunTesla:
			this.gunImage.sprite = GameResourcesUtils.GetGunImage(106);
			break;
		case ItemDropType.GunKamePower:
			this.gunImage.sprite = GameResourcesUtils.GetGunImage(107);
			break;
		case ItemDropType.GunFlame:
			this.gunImage.sprite = GameResourcesUtils.GetGunImage(108);
			break;
		}
	}
}
