using System;
using System.Collections.Generic;
using UnityEngine;

public class GameResourcesUtils
{
	private static Dictionary<int, Sprite> gunImages = new Dictionary<int, Sprite>();

	private static Dictionary<int, Sprite> grenadeImages = new Dictionary<int, Sprite>();

	private static Dictionary<int, Sprite> meleeWeaponImages = new Dictionary<int, Sprite>();

	private static Dictionary<int, Sprite> rankImages = new Dictionary<int, Sprite>();

	private static Dictionary<int, Sprite> tournamentRankImages = new Dictionary<int, Sprite>();

	private static Dictionary<string, Sprite> enemyImages = new Dictionary<string, Sprite>();

	private static Dictionary<int, Sprite> skillLockImages = new Dictionary<int, Sprite>();

	private static Dictionary<int, Sprite> skillUnlockImages = new Dictionary<int, Sprite>();

	private static Dictionary<RewardType, Sprite> rewardImages = new Dictionary<RewardType, Sprite>();

	public static Rambo GetRamboPrefab(int id)
	{
		return Resources.Load<Rambo>($"Rambo/{GetRamboName(id)}");
    }
    public static string GetRamboName(int id)
    {
        return $"rambo_{id}";
    }

    public static BaseGun GetGunPrefab(int id)
	{
		string str = string.Format("gun_{0}", id);
		return Resources.Load<BaseGun>("Gun/" + str);
	}

	public static BaseGrenade GetGrenadePrefab(int id)
	{
		string str = string.Format("grenade_{0}", id);
		return Resources.Load<BaseGrenade>("Grenade/" + str);
	}

	public static BaseMeleeWeapon GetMeleeWeaponPrefab(int id)
	{
		string str = string.Format("melee_weapon_{0}", id);
		return Resources.Load<BaseMeleeWeapon>("Melee Weapon/" + str);
	}

	public static Sprite GetGunImage(int id)
	{
		if (GameResourcesUtils.gunImages.ContainsKey(id))
		{
			return GameResourcesUtils.gunImages[id];
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/Gun/" + id);
		if (sprite != null)
		{
			GameResourcesUtils.gunImages.Add(id, sprite);
			Debug.LogWarning("Sprites/Gun/ dont have " + id);
			return sprite;
		}
		return null;
	}

	public static Sprite GetGrenadeImage(int id)
	{
		if (GameResourcesUtils.grenadeImages.ContainsKey(id))
		{
			return GameResourcesUtils.grenadeImages[id];
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/Grenade/" + id);
		if (sprite != null)
		{
			GameResourcesUtils.grenadeImages.Add(id, sprite);
			return sprite;
		}
		return null;
	}

	public static Sprite GetMeleeWeaponImage(int id)
	{
		if (GameResourcesUtils.meleeWeaponImages.ContainsKey(id))
		{
			return GameResourcesUtils.meleeWeaponImages[id];
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/Melee Weapon/" + id);
		if (sprite != null)
		{
			GameResourcesUtils.meleeWeaponImages.Add(id, sprite);
			return sprite;
		}
		return null;
	}

	public static Sprite GetEnemyImage(int id, MapType map)
	{
		string str = id.ToString();
		string text = string.Format("{0}-{1}", id, (int)map);
		if (GameResourcesUtils.enemyImages.ContainsKey(text))
		{
			return GameResourcesUtils.enemyImages[text];
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/Enemy Icon/" + text);
		if (sprite != null)
		{
			GameResourcesUtils.enemyImages.Add(text, sprite);
			return sprite;
		}
		sprite = Resources.Load<Sprite>("Sprites/Enemy Icon/" + str);
		if (sprite != null)
		{
			GameResourcesUtils.enemyImages.Add(text, sprite);
			return sprite;
		}
		return null;
	}

	public static Sprite GetRewardImage(RewardType type)
	{
		if (GameResourcesUtils.rewardImages.ContainsKey(type))
		{
			return GameResourcesUtils.rewardImages[type];
		}
		Sprite sprite = null;
		switch (type)
		{
		case RewardType.GunM4:
			sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_1");
			break;
		case RewardType.GunSpread:
			sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_100");
			break;
		case RewardType.GunScarH:
			sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_2");
			break;
		case RewardType.GunBullpup:
			sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_6");
			break;
		case RewardType.GunKamePower:
			sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_107");
			break;
		case RewardType.GunSniperRifle:
			sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_7");
			break;
		case RewardType.GunTeslaMini:
			sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_8");
			break;
		case RewardType.GunLaser:
			sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_103");
			break;
		case RewardType.GunFlame:
			sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_108");
			break;


            case RewardType.Coin:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_coin");
                break;
            case RewardType.Gem:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gem");
                break;
            case RewardType.Exp:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_exp");
                break;
            case RewardType.BoosterHp:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_hp");
                break;
            case RewardType.BoosterDamage:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_damage");
                break;
            case RewardType.BoosterCoinMagnet:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_coin_magnet");
                break;
            case RewardType.BoosterSpeed:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_speed");
                break;
            case RewardType.BoosterCritical:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_critical");
                break;

            case RewardType.GrenadeF1:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_grenade_f1");
                break;

            case RewardType.GrenadeTet:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_grenade_tet");
                break;

            case RewardType.MeleeWeaponPan:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_melee_weapon_pan");
                break;

            case RewardType.MeleeWeaponGuitar:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_melee_weapon_guitar");
                break;

            case RewardType.Medal:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_medal");
                break;

            case RewardType.TournamentTicket:
                sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_tournament_ticket");
                break;

                /*
            default:
			switch (type)
			{
			case RewardType.Coin:
				sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_coin");
				goto IL_227;
			case RewardType.Gem:
				sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_gem");
				goto IL_227;
			case RewardType.Exp:
				sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_exp");
				goto IL_227;
			case RewardType.Stamina:
				IL_69:
				switch (type)
				{
				case RewardType.BoosterHp:
					sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_hp");
					goto IL_227;
				case RewardType.BoosterDamage:
					sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_damage");
					goto IL_227;
				case RewardType.BoosterCoinMagnet:
					sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_coin_magnet");
					goto IL_227;
				case RewardType.BoosterSpeed:
					sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_speed");
					goto IL_227;
				case RewardType.BoosterCritical:
					sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_booster_critical");
					goto IL_227;
				default:
					if (type == RewardType.GrenadeF1)
					{
						sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_grenade_f1");
						goto IL_227;
					}
					if (type == RewardType.GrenadeTet)
					{
						sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_grenade_tet");
						goto IL_227;
					}
					if (type == RewardType.MeleeWeaponPan)
					{
						sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_melee_weapon_pan");
						goto IL_227;
					}
					if (type != RewardType.MeleeWeaponGuitar)
					{
						goto IL_227;
					}
					sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_melee_weapon_guitar");
					goto IL_227;
				}
				break;
			case RewardType.Medal:
				sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_medal");
				goto IL_227;
			case RewardType.TournamentTicket:
				sprite = Resources.Load<Sprite>("Sprites/Reward Icon/reward_tournament_ticket");
				goto IL_227;
			}
			goto IL_69;
			*/
        }
		//IL_227:      
		GameResourcesUtils.rewardImages.Add(type, sprite);
		return sprite;
	}
	public static Sprite GetGunRewardImage(int ID)
	{
		Sprite sprite = null;
		try
        {
			sprite = Resources.Load<Sprite>($"Sprites/Reward Icon/reward_{ID}");
		}
		catch(Exception e)
        {
			Debug.Log("no gun in location");
        }
		if (sprite == null) sprite = Resources.Load<Sprite>($"Sprites/Reward Icon/reward_200");
		return sprite;
	}
	public static Sprite GetRankImage(int level)
	{
		if (GameResourcesUtils.rankImages.ContainsKey(level))
		{
			return GameResourcesUtils.rankImages[level];
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/Rank Icon/" + level);
		if (sprite != null)
		{
			GameResourcesUtils.rankImages.Add(level, sprite);
			return sprite;
		}
		return null;
	}

	public static Sprite GetTournamentRankImage(int index)
	{
		if (GameResourcesUtils.tournamentRankImages.ContainsKey(index))
		{
			return GameResourcesUtils.tournamentRankImages[index];
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/Tournament Rank Icon/" + index);
		if (sprite != null)
		{
			GameResourcesUtils.tournamentRankImages.Add(index, sprite);
			return sprite;
		}
		return null;
	}

	public static Sprite GetSkillLockImage(int id)
	{
		if (GameResourcesUtils.skillLockImages.ContainsKey(id))
		{
			return GameResourcesUtils.skillLockImages[id];
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/Skill Lock/" + id);
		if (sprite != null)
		{
			GameResourcesUtils.skillLockImages.Add(id, sprite);
			return sprite;
		}
		return null;
	}

	public static Sprite GetSkillUnlockImage(int id)
	{
		if (GameResourcesUtils.skillUnlockImages.ContainsKey(id))
		{
			return GameResourcesUtils.skillUnlockImages[id];
		}
		Sprite sprite = Resources.Load<Sprite>("Sprites/Skill Unlock/" + id);
		if (sprite != null)
		{
			GameResourcesUtils.skillUnlockImages.Add(id, sprite);
			return sprite;
		}
		return null;
	}
}
