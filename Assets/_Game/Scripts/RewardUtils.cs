using System;
using System.Collections.Generic;

public class RewardUtils
{
	public static void Receive(List<RewardData> rewards)
	{
		for (int i = 0; i < rewards.Count; i++)
		{
			RewardData rewardData = rewards[i];
			RewardType type = rewardData.type;
			switch (type)
			{
			case RewardType.GunM4:
				if (GameData.playerGuns.ContainsKey(1))
				{
					rewardData.type = RewardType.Gem;
					rewardData.value = GameData.gunValueGem[1];
					GameData.playerResources.ReceiveGem(rewardData.value);
				}
				else
				{
					GameData.playerGuns.ReceiveNewGun(1);
				}
				break;
			case RewardType.GunSpread:
				if (GameData.playerGuns.ContainsKey(100))
				{
					rewardData.type = RewardType.Gem;
					rewardData.value = GameData.gunValueGem[100];
					GameData.playerResources.ReceiveGem(rewardData.value);
				}
				else
				{
					GameData.playerGuns.ReceiveNewGun(100);
				}
				break;
			case RewardType.GunScarH:
				if (GameData.playerGuns.ContainsKey(2))
				{
					rewardData.type = RewardType.Gem;
					rewardData.value = GameData.gunValueGem[2];
					GameData.playerResources.ReceiveGem(rewardData.value);
				}
				else
				{
					GameData.playerGuns.ReceiveNewGun(2);
				}
				break;
			case RewardType.GunBullpup:
				if (GameData.playerGuns.ContainsKey(6))
				{
					rewardData.type = RewardType.Gem;
					rewardData.value = GameData.gunValueGem[6];
					GameData.playerResources.ReceiveGem(rewardData.value);
				}
				else
				{
					GameData.playerGuns.ReceiveNewGun(6);
				}
				break;
			case RewardType.GunKamePower:
				if (GameData.playerGuns.ContainsKey(107))
				{
					rewardData.type = RewardType.Gem;
					rewardData.value = GameData.gunValueGem[107];
					GameData.playerResources.ReceiveGem(rewardData.value);
				}
				else
				{
					GameData.playerGuns.ReceiveNewGun(107);
				}
				break;
			case RewardType.GunSniperRifle:
				if (GameData.playerGuns.ContainsKey(7))
				{
					rewardData.type = RewardType.Gem;
					rewardData.value = GameData.gunValueGem[7];
					GameData.playerResources.ReceiveGem(rewardData.value);
				}
				else
				{
					GameData.playerGuns.ReceiveNewGun(7);
				}
				break;
			case RewardType.GunTeslaMini:
				if (GameData.playerGuns.ContainsKey(8))
				{
					rewardData.type = RewardType.Gem;
					rewardData.value = GameData.gunValueGem[8];
					GameData.playerResources.ReceiveGem(rewardData.value);
				}
				else
				{
					GameData.playerGuns.ReceiveNewGun(8);
				}
				break;
			case RewardType.GunLaser:
				if (GameData.playerGuns.ContainsKey(103))
				{
					rewardData.type = RewardType.Gem;
					rewardData.value = GameData.gunValueGem[103];
					GameData.playerResources.ReceiveGem(rewardData.value);
				}
				else
				{
					GameData.playerGuns.ReceiveNewGun(103);
				}
				break;
			case RewardType.GunFlame:
				if (GameData.playerGuns.ContainsKey(108))
				{
					rewardData.type = RewardType.Gem;
					rewardData.value = GameData.gunValueGem[108];
					GameData.playerResources.ReceiveGem(rewardData.value);
				}
				else
				{
					GameData.playerGuns.ReceiveNewGun(108);
				}
				break;
			default:
				switch (type)
				{
				case RewardType.Coin:
					GameData.playerResources.ReceiveCoin(rewardData.value);
					break;
				case RewardType.Gem:
					GameData.playerResources.ReceiveGem(rewardData.value);
					break;
				case RewardType.Exp:
					GameData.playerProfile.ReceiveExp(rewardData.value);
					break;
				case RewardType.Stamina:
					GameData.playerResources.ReceiveStamina(rewardData.value);
					break;
				case RewardType.Medal:
					GameData.playerResources.ReceiveMedal(rewardData.value);
					break;
				case RewardType.TournamentTicket:
					GameData.playerResources.ReceiveTournamentTicket(rewardData.value);
					break;
				default:
					switch (type)
					{
					case RewardType.BoosterHp:
						GameData.playerBoosters.Receive(BoosterType.Hp, rewardData.value);
						break;
					case RewardType.BoosterDamage:
						GameData.playerBoosters.Receive(BoosterType.Damage, rewardData.value);
						break;
					case RewardType.BoosterCoinMagnet:
						GameData.playerBoosters.Receive(BoosterType.CoinMagnet, rewardData.value);
						break;
					case RewardType.BoosterSpeed:
						GameData.playerBoosters.Receive(BoosterType.Speed, rewardData.value);
						break;
					case RewardType.BoosterCritical:
						GameData.playerBoosters.Receive(BoosterType.Critical, rewardData.value);
						break;
					default:
						if (type != RewardType.GrenadeF1)
						{
							if (type != RewardType.GrenadeTet)
							{
								if (type != RewardType.MeleeWeaponPan)
								{
									if (type == RewardType.MeleeWeaponGuitar)
									{
										GameData.playerMeleeWeapons.ReceiveNewMeleeWeapon(602);
									}
								}
								else
								{
									GameData.playerMeleeWeapons.ReceiveNewMeleeWeapon(601);
								}
							}
							else
							{
								GameData.playerGrenades.Receive(501, rewardData.value);
							}
						}
						else
						{
							GameData.playerGrenades.Receive(500, rewardData.value);
						}
						break;
					}
					break;
				}
				break;
			}
		}
	}
}
