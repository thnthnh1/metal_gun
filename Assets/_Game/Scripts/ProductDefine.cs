using System;
using UnityEngine.Purchasing;

public class ProductDefine
{
	public static readonly ProductIAP GEM_40 = new ProductIAP("metal.x.gem40", ProductType.Consumable);

	public static readonly ProductIAP GEM_80 = new ProductIAP("metal.x.gem80", ProductType.Consumable);

	public static readonly ProductIAP GEM_160 = new ProductIAP("metal.x.gem160", ProductType.Consumable);

	public static readonly ProductIAP GEM_350 = new ProductIAP("metal.x.gem350", ProductType.Consumable);

	public static readonly ProductIAP GEM_700 = new ProductIAP("metal.x.gem700", ProductType.Consumable);

	public static readonly ProductIAP GEM_1250 = new ProductIAP("metal.x.gem1250", ProductType.Consumable);

	// Over

	// public static readonly ProductIAP EVERY_FAVORITE = new ProductIAP("com.metaltitan.everybodyfavorite", ProductType.Consumable);

	// public static readonly ProductIAP DRAGON_BREATH = new ProductIAP("com.metaltitan.dragonbreath", ProductType.Consumable);

	// public static readonly ProductIAP LET_THERE_BE_FIRE = new ProductIAP("com.metaltitan.lettherebefire", ProductType.Consumable);

	// public static readonly ProductIAP SNIPPING_FOR_DUMMIES = new ProductIAP("com.metaltitan.snippingdummies", ProductType.Consumable);

	// public static readonly ProductIAP TASER_LASER = new ProductIAP("com.metaltitan.taserlaser", ProductType.Consumable);

	// public static readonly ProductIAP SHOCKING_SALE = new ProductIAP("com.metaltitan.shockingsale", ProductType.Consumable);

	// public static readonly ProductIAP UPGRADE_ENTHUSIAST = new ProductIAP("com.metaltitan.upgradeenthusiast", ProductType.Consumable);

	// public static readonly ProductIAP BATTLE_ESSENTIALS_1 = new ProductIAP("com.metaltitan.battleessentials1", ProductType.Consumable);

	// public static readonly ProductIAP BATTLE_ESSENTIALS_2 = new ProductIAP("com.metaltitan.battleessentials2", ProductType.Consumable);

	// public static readonly ProductIAP BATTLE_ESSENTIALS_3 = new ProductIAP("com.metaltitan.battleessentials3", ProductType.Consumable);

	public static readonly ProductIAP STARTER_PACK = new ProductIAP("metal.x.starterpack", ProductType.Consumable);

	public static readonly ProductIAP SPECIAL_OFFER = new ProductIAP("metal.x.special.offer", ProductType.Consumable);

	public static readonly ProductIAP REMOVE_ADS = new ProductIAP("metal.x.removeads", ProductType.Consumable);

	public static readonly ProductIAP TICKET_5 = new ProductIAP("metal.x.ticket5", ProductType.Consumable);

	public static readonly ProductIAP TICKET_17 = new ProductIAP("metal.x.ticket17", ProductType.Consumable);

	public static readonly ProductIAP TICKET_35 = new ProductIAP("metal.x.ticket35", ProductType.Consumable);

	public static readonly ProductIAP TICKET_80 = new ProductIAP("metal.x.ticket80", ProductType.Consumable);



	// Over

	// private static ProductIAP[] arr = new ProductIAP[]
	// {
	// 	ProductDefine.GEM_100,
	// 	ProductDefine.GEM_300,
	// 	ProductDefine.GEM_500,
	// 	ProductDefine.GEM_1000,
	// 	ProductDefine.GEM_2500,
	// 	ProductDefine.GEM_5000,
	// 	ProductDefine.EVERY_FAVORITE,
	// 	ProductDefine.DRAGON_BREATH,
	// 	ProductDefine.LET_THERE_BE_FIRE,
	// 	ProductDefine.SNIPPING_FOR_DUMMIES,
	// 	ProductDefine.TASER_LASER,
	// 	ProductDefine.SHOCKING_SALE,
	// 	ProductDefine.UPGRADE_ENTHUSIAST,
	// 	ProductDefine.BATTLE_ESSENTIALS_1,
	// 	ProductDefine.BATTLE_ESSENTIALS_2,
	// 	ProductDefine.BATTLE_ESSENTIALS_3,
	// 	ProductDefine.STARTER_PACK,
	// 	ProductDefine.REMOVE_ADS
	// };

	private static ProductIAP[] arr = new ProductIAP[]
	{
		ProductDefine.GEM_40,
		ProductDefine.GEM_80,
		ProductDefine.GEM_160,
		ProductDefine.GEM_350,
		ProductDefine.GEM_700,
		ProductDefine.GEM_1250,
		// ProductDefine.BATTLE_ESSENTIALS_1,
		// ProductDefine.BATTLE_ESSENTIALS_2,
		// ProductDefine.BATTLE_ESSENTIALS_3,
		ProductDefine.STARTER_PACK,
		// ProductDefine.REMOVE_ADS,
		ProductDefine.TICKET_5,
		ProductDefine.TICKET_17,
		ProductDefine.TICKET_35,
		ProductDefine.TICKET_80,
	};

	public static ProductIAP[] GetListProducts()
	{
		return ProductDefine.arr;
	}
}
