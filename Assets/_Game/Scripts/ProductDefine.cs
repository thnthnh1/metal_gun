using System;
using UnityEngine.Purchasing;

public class ProductDefine
{
	public static readonly ProductIAP GEM_100 = new ProductIAP("com.metal.gem100", ProductType.Consumable);

	public static readonly ProductIAP GEM_300 = new ProductIAP("com.metal.gem300", ProductType.Consumable);

	public static readonly ProductIAP GEM_500 = new ProductIAP("com.metal.gem500", ProductType.Consumable);

	public static readonly ProductIAP GEM_1000 = new ProductIAP("com.metal.gem1000", ProductType.Consumable);

	public static readonly ProductIAP GEM_2500 = new ProductIAP("com.metal.gem2500", ProductType.Consumable);

	public static readonly ProductIAP GEM_5000 = new ProductIAP("com.metal.gem5000", ProductType.Consumable);



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

	public static readonly ProductIAP STARTER_PACK = new ProductIAP("com.metaltitan.starterpack", ProductType.Consumable);

	public static readonly ProductIAP REMOVE_ADS = new ProductIAP("com.metaltitan.removeads", ProductType.Consumable);

	public static readonly ProductIAP Ticket0 = new ProductIAP("com.metal.ticket5", ProductType.Consumable);

	public static readonly ProductIAP Ticket1 = new ProductIAP("com.metal.ticket17", ProductType.Consumable);

	public static readonly ProductIAP Ticket2 = new ProductIAP("com.metal.ticket35", ProductType.Consumable);

	public static readonly ProductIAP Ticket3 = new ProductIAP("com.metal.ticket80", ProductType.Consumable);



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
		ProductDefine.GEM_100,
		ProductDefine.GEM_300,
		ProductDefine.GEM_500,
		ProductDefine.GEM_1000,
		ProductDefine.GEM_2500,
		ProductDefine.GEM_5000,
		// ProductDefine.BATTLE_ESSENTIALS_1,
		// ProductDefine.BATTLE_ESSENTIALS_2,
		// ProductDefine.BATTLE_ESSENTIALS_3,
		ProductDefine.STARTER_PACK,
		// ProductDefine.REMOVE_ADS,
		ProductDefine.Ticket0,
		ProductDefine.Ticket1,
		ProductDefine.Ticket2,
		ProductDefine.Ticket3,
	};

	public static ProductIAP[] GetListProducts()
	{
		return ProductDefine.arr;
	}
}
