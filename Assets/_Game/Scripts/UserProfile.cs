using G2.Sdk.PlayerPrefsHelper;
using G2.Sdk.SecurityHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public class UserProfile
{
	public IntProfileData ramboId;

	public IntProfileData gunNormalId;

	public IntProfileData gunSpecialId;

	public IntProfileData grenadeId;

	public IntProfileData meleeWeaponId;

	public StringProfileData playerProfile;

	public StringProfileData playerRamboData;

	public StringProfileData playerGunData;

	public StringProfileData playerGrenadeData;

	public StringProfileData playerMeleeWeaponData;

	public StringProfileData playerCampaignProgessData;

	public StringProfileData playerCampaignStageProgessData;

	public StringProfileData playerCampaignRewardProgessData;

	public StringProfileData playerResourcesData;

	public StringProfileData playerBoosterData;

	public StringProfileData playerSelectingBooster;

	public StringProfileData playerDailyQuestData;

	public StringProfileData playerAchievementData;

	public StringProfileData playerRamboSkillData;

	public StringProfileData playerTutorialData;

	public DateTimeProfileData dateLastLogin;

	public DateTimeProfileData dateGetGift;

	public IntProfileData getDailyGiftDay;

	public BoolProfileData isReceivedDailyGiftToday;

	public BoolProfileData isPassFirstWeek;

	public IntProfileData countViewAdsFreeCoin;

	public IntProfileData countShareFacebook;

	public IntProfileData countRewardInterstitialAds;

	public FloatProfileData soundVolume;

	public FloatProfileData musicVolume;

	public BoolProfileData isRemoveAds;

	public StringProfileData weekLastLogin;

	public IntProfileData countPlayTournament;

	public BoolProfileData isClaimedRank1;

	public BoolProfileData isClaimedRank2;

	public BoolProfileData isClaimedRank3;

	public BoolProfileData isClaimedRank4;

	public BoolProfileData isClaimedRank5;

	public BoolProfileData isClaimedRank6;

	public BoolProfileData isShowUnlockTournament;

	public StringProfileData tournamentGunProfile;

	public BoolProfileData isNoLongerRate;

	public BoolProfileData isShowRateMap1;

	public BoolProfileData isShowRateMap2;

	public BoolProfileData isShowRateMap3;

	public BoolProfileData isFirstBuyGem100;

	public BoolProfileData isFirstBuyGem300;

	public BoolProfileData isFirstBuyGem500;

	public BoolProfileData isFirstBuyGem1000;

	public BoolProfileData isFirstBuyGem2500;

	public BoolProfileData isFirstBuyGem5000;

	public BoolProfileData isPurchasedStarterPack;

	public BoolProfileData isClaimNewbiePack;
	public BoolProfileData isClaimHeroPack;

	public BoolProfileData isPurchasedPackEverybodyFavorite;

	public BoolProfileData isPurchasedPackDragonBreath;

	public BoolProfileData isPurchasedPackLetThereBeFire;

	public BoolProfileData isPurchasedPackSnippingForDummies;

	public BoolProfileData isPurchasedPackTaserLaser;

	public BoolProfileData isPurchasedPackShockingSale;

	public BoolProfileData isPurchasedPackUpgradeEnthusiast;

	public List<LongProfileData> timestampNextFreeGifts;

	public UserProfile()
	{
	}

	public UserProfile(DataEncryption dataEncryption)
	{
		this.ramboId = new IntProfileData("current_rambo_id", 0, dataEncryption, true);
		this.gunSpecialId = new IntProfileData("gun_special_id", -1, dataEncryption, true);
		this.gunNormalId = new IntProfileData("gun_normal_id", 0, dataEncryption, true);
		this.grenadeId = new IntProfileData("grenade_id", 500, dataEncryption, true);
		this.meleeWeaponId = new IntProfileData("melee_weapon_id", 600, dataEncryption, true);
		this.playerProfile = new StringProfileData("player_profile", string.Empty, dataEncryption, true);
		this.playerRamboData = new StringProfileData("player_rambo_data", string.Empty, dataEncryption, true);
		this.playerGunData = new StringProfileData("player_gun_data", string.Empty, dataEncryption, true);
		this.playerGrenadeData = new StringProfileData("player_grenades_data", string.Empty, dataEncryption, true);
		this.playerMeleeWeaponData = new StringProfileData("player_melee_weapon_data", string.Empty, dataEncryption, true);
		this.playerCampaignProgessData = new StringProfileData("player_campaign_progress_data", string.Empty, dataEncryption, true);
		this.playerCampaignStageProgessData = new StringProfileData("player_campaign_stage_progress_data", string.Empty, dataEncryption, true);
		this.playerCampaignRewardProgessData = new StringProfileData("player_campaign_reward_progress_data", string.Empty, dataEncryption, true);
		this.playerResourcesData = new StringProfileData("player_resources_data", string.Empty, dataEncryption, true);
		this.playerBoosterData = new StringProfileData("player_booster_data", string.Empty, dataEncryption, true);
		this.playerSelectingBooster = new StringProfileData("player_selecting_booster", string.Empty, dataEncryption, true);
		this.playerDailyQuestData = new StringProfileData("player_daily_quest_data", string.Empty, dataEncryption, true);
		this.playerAchievementData = new StringProfileData("player_achievement_data", string.Empty, dataEncryption, true);
		this.playerRamboSkillData = new StringProfileData("player_rambo_skill_data", string.Empty, dataEncryption, true);
		this.playerTutorialData = new StringProfileData("player_tutorial_data", string.Empty, dataEncryption, true);
		this.dateLastLogin = new DateTimeProfileData("data_last_login", StaticValue.defaultDate, dataEncryption, true);
		this.dateGetGift = new DateTimeProfileData("date_get_gift", StaticValue.defaultDate, dataEncryption, true);
		this.getDailyGiftDay = new IntProfileData("get_daily_gift_day", 1, dataEncryption, true);
		this.isReceivedDailyGiftToday = new BoolProfileData("is_received_daily_gift_today", false, dataEncryption, true);
		this.isPassFirstWeek = new BoolProfileData("is_pass_first_week", false, dataEncryption, true);
		this.countViewAdsFreeCoin = new IntProfileData("count_view_ads_free_coin", 0, dataEncryption, true);
		this.countShareFacebook = new IntProfileData("count_share_facebook", 0, dataEncryption, true);
		this.countRewardInterstitialAds = new IntProfileData("count_reward_interstitial_ads", 0, dataEncryption, true);
		this.soundVolume = new FloatProfileData("sound_volume", 1f, dataEncryption, true);
		this.musicVolume = new FloatProfileData("music_volume", 1f, dataEncryption, true);
		this.isRemoveAds = new BoolProfileData("remove_ads", false, dataEncryption, true);
		this.weekLastLogin = new StringProfileData("week_last_login", string.Empty, dataEncryption, true);
		this.countPlayTournament = new IntProfileData("count_play_tournament", 0, dataEncryption, true);
		this.isClaimedRank1 = new BoolProfileData("is_claimed_rank_1", false, dataEncryption, true);
		this.isClaimedRank2 = new BoolProfileData("is_claimed_rank_2", false, dataEncryption, true);
		this.isClaimedRank3 = new BoolProfileData("is_claimed_rank_3", false, dataEncryption, true);
		this.isClaimedRank4 = new BoolProfileData("is_claimed_rank_4", false, dataEncryption, true);
		this.isClaimedRank5 = new BoolProfileData("is_claimed_rank_5", false, dataEncryption, true);
		this.isClaimedRank6 = new BoolProfileData("is_claimed_rank_6", false, dataEncryption, true);
		this.isShowUnlockTournament = new BoolProfileData("is_show_unlock_tournament", false, dataEncryption, true);
		this.tournamentGunProfile = new StringProfileData("tournament_gun_profile", string.Empty, dataEncryption, true);
		this.isNoLongerRate = new BoolProfileData("is_no_longer_rate", false, dataEncryption, true);
		this.isShowRateMap1 = new BoolProfileData("is_show_rate_map_1", false, dataEncryption, true);
		this.isShowRateMap2 = new BoolProfileData("is_show_rate_map_2", false, dataEncryption, true);
		this.isShowRateMap3 = new BoolProfileData("is_show_rate_map_3", false, dataEncryption, true);
		this.isFirstBuyGem100 = new BoolProfileData("is_first_buy_gem_100", false, dataEncryption, true);
		this.isFirstBuyGem300 = new BoolProfileData("is_first_buy_gem_300", false, dataEncryption, true);
		this.isFirstBuyGem500 = new BoolProfileData("is_first_buy_gem_500", false, dataEncryption, true);
		this.isFirstBuyGem1000 = new BoolProfileData("is_first_buy_gem_1000", false, dataEncryption, true);
		this.isFirstBuyGem2500 = new BoolProfileData("is_first_buy_gem_2500", false, dataEncryption, true);
		this.isFirstBuyGem5000 = new BoolProfileData("is_first_buy_gem_5000", false, dataEncryption, true);
		this.isPurchasedStarterPack = new BoolProfileData("is_purchased_starter_pack", false, dataEncryption, true);
		this.isClaimNewbiePack = new BoolProfileData("is_claim_newbie_pack", false, dataEncryption, true);
		this.isClaimHeroPack = new BoolProfileData("is_claim_hero_pack", false, dataEncryption, true);
		this.isPurchasedPackEverybodyFavorite = new BoolProfileData("is_purchased_pack_everybody_favorite", false, dataEncryption, true);
		this.isPurchasedPackDragonBreath = new BoolProfileData("is_purchased_pack_dragon_breath", false, dataEncryption, true);
		this.isPurchasedPackLetThereBeFire = new BoolProfileData("is_purchased_pack_let_there_be_fire", false, dataEncryption, true);
		this.isPurchasedPackSnippingForDummies = new BoolProfileData("is_purchased_pack_snipping_for_dummies", false, dataEncryption, true);
		this.isPurchasedPackTaserLaser = new BoolProfileData("is_purchased_pack_taser_laser", false, dataEncryption, true);
		this.isPurchasedPackShockingSale = new BoolProfileData("is_purchased_pack_shocking_sale", false, dataEncryption, true);
		this.isPurchasedPackUpgradeEnthusiast = new BoolProfileData("is_purchased_pack_upgrade_enthusiast", false, dataEncryption, true);

		timestampNextFreeGifts = new List<LongProfileData>();
		for (int i = 0; i < 5; i++)
		{
			var freeGift = new LongProfileData($"timestamp_next_gift_{i}", 0, dataEncryption, true);
			timestampNextFreeGifts.Add(freeGift);
		}
	}

	public void ResetTo(RawUserProfile newData)
	{
		if (newData == null)
		{
			return;
		}
		this.ramboId.Set(newData.ramboId);
		this.gunSpecialId.Set(newData.gunSpecialId);
		this.gunNormalId.Set(newData.gunNormalId);
		this.grenadeId.Set(newData.grenadeId);
		this.meleeWeaponId.Set(newData.meleeWeaponId);
		this.playerProfile.Set(newData.playerProfile);
		this.playerRamboData.Set(newData.playerRamboData);
		this.playerGunData.Set(newData.playerGunData);
		this.playerGrenadeData.Set(newData.playerGrenadeData);
		this.playerMeleeWeaponData.Set(newData.playerMeleeWeaponData);
		this.playerCampaignProgessData.Set(newData.playerCampaignProgessData);
		this.playerCampaignStageProgessData.Set(newData.playerCampaignStageProgessData);
		this.playerCampaignRewardProgessData.Set(newData.playerCampaignRewardProgessData);
		this.playerResourcesData.Set(newData.playerResourcesData);
		this.playerBoosterData.Set(newData.playerBoosterData);
		this.playerSelectingBooster.Set(newData.playerSelectingBooster);
		this.playerDailyQuestData.Set(newData.playerDailyQuestData);
		this.playerAchievementData.Set(newData.playerAchievementData);
		this.playerRamboSkillData.Set(newData.playerRamboSkillData);
		this.playerTutorialData.Set(newData.playerTutorialData);
		this.dateLastLogin.Set(newData.dateLastLogin);
		this.dateGetGift.Set(newData.dateGetGift);
		this.getDailyGiftDay.Set(newData.getDailyGiftDay);
		this.isReceivedDailyGiftToday.Set(newData.isReceivedDailyGiftToday);
		this.isPassFirstWeek.Set(newData.isPassFirstWeek);
		this.countViewAdsFreeCoin.Set(newData.countViewAdsFreeCoin);
		this.countShareFacebook.Set(newData.countShareFacebook);
		this.countRewardInterstitialAds.Set(newData.countRewardInterstitialAds);
		this.soundVolume.Set(newData.soundVolume);
		this.musicVolume.Set(newData.musicVolume);
		this.isRemoveAds.Set(newData.isRemoveAds);
		this.weekLastLogin.Set(newData.weekLastLogin);
		this.countPlayTournament.Set(newData.countPlayTournament);
		this.isClaimedRank1.Set(newData.isClaimedRank1);
		this.isClaimedRank2.Set(newData.isClaimedRank2);
		this.isClaimedRank3.Set(newData.isClaimedRank3);
		this.isClaimedRank4.Set(newData.isClaimedRank4);
		this.isClaimedRank5.Set(newData.isClaimedRank5);
		this.isClaimedRank6.Set(newData.isClaimedRank6);
		this.isShowUnlockTournament.Set(newData.isShowUnlockTournament);
		this.tournamentGunProfile.Set(newData.tournamentGunProfile);
		this.isNoLongerRate.Set(newData.isNoLongerRate);
		this.isShowRateMap1.Set(newData.isShowRateMap1);
		this.isShowRateMap2.Set(newData.isShowRateMap2);
		this.isShowRateMap3.Set(newData.isShowRateMap3);
		this.isFirstBuyGem100.Set(newData.isFirstBuyGem100);
		this.isFirstBuyGem300.Set(newData.isFirstBuyGem300);
		this.isFirstBuyGem500.Set(newData.isFirstBuyGem500);
		this.isFirstBuyGem1000.Set(newData.isFirstBuyGem1000);
		this.isFirstBuyGem2500.Set(newData.isFirstBuyGem2500);
		this.isFirstBuyGem5000.Set(newData.isFirstBuyGem5000);
		this.isPurchasedStarterPack.Set(newData.isPurchasedStarterPack);
		this.isClaimNewbiePack.Set(newData.isClaimNewbiePack);
		this.isClaimHeroPack.Set(newData.isClaimHeroPack);
		this.isPurchasedPackEverybodyFavorite.Set(newData.isPurchasedPackEverybodyFavorite);
		this.isPurchasedPackDragonBreath.Set(newData.isPurchasedPackDragonBreath);
		this.isPurchasedPackLetThereBeFire.Set(newData.isPurchasedPackLetThereBeFire);
		this.isPurchasedPackSnippingForDummies.Set(newData.isPurchasedPackSnippingForDummies);
		this.isPurchasedPackTaserLaser.Set(newData.isPurchasedPackTaserLaser);
		this.isPurchasedPackShockingSale.Set(newData.isPurchasedPackShockingSale);
		this.isPurchasedPackUpgradeEnthusiast.Set(newData.isPurchasedPackUpgradeEnthusiast);

		var timestampNextGifts = newData.strTimestampNextFreeGifts.Split(";");
		for (int i = 0; i < timestampNextGifts.Length; i++)
		{
			this.timestampNextFreeGifts[i].Set(Convert.ToInt64(timestampNextGifts[i]));
		}
	}

	public RawUserProfile GetRawUserProfile()
	{
		return new RawUserProfile
		{
			ramboId = this.ramboId.data,
			gunSpecialId = this.gunSpecialId.data,
			gunNormalId = this.gunNormalId.data,
			grenadeId = this.grenadeId.data,
			meleeWeaponId = this.meleeWeaponId.data,
			playerProfile = this.playerProfile.data,
			playerRamboData = this.playerRamboData.data,
			playerGunData = this.playerGunData.data,
			playerGrenadeData = this.playerGrenadeData.data,
			playerMeleeWeaponData = this.playerMeleeWeaponData.data,
			playerCampaignProgessData = this.playerCampaignProgessData.data,
			playerCampaignStageProgessData = this.playerCampaignStageProgessData.data,
			playerCampaignRewardProgessData = this.playerCampaignRewardProgessData.data,
			playerResourcesData = this.playerResourcesData.data,
			playerBoosterData = this.playerBoosterData.data,
			playerSelectingBooster = this.playerSelectingBooster.data,
			playerDailyQuestData = this.playerDailyQuestData.data,
			playerAchievementData = this.playerAchievementData.data,
			playerRamboSkillData = this.playerRamboSkillData.data,
			playerTutorialData = this.playerTutorialData,
			dateLastLogin = this.dateLastLogin.data,
			dateGetGift = this.dateGetGift.data,
			getDailyGiftDay = this.getDailyGiftDay.data,
			isReceivedDailyGiftToday = this.isReceivedDailyGiftToday.data,
			isPassFirstWeek = this.isPassFirstWeek.data,
			countViewAdsFreeCoin = this.countViewAdsFreeCoin.data,
			countShareFacebook = this.countShareFacebook.data,
			countRewardInterstitialAds = this.countRewardInterstitialAds,
			soundVolume = this.soundVolume.data,
			musicVolume = this.musicVolume.data,
			isRemoveAds = this.isRemoveAds.data,
			weekLastLogin = this.weekLastLogin.data,
			countPlayTournament = this.countPlayTournament.data,
			isClaimedRank1 = this.isClaimedRank1.data,
			isClaimedRank2 = this.isClaimedRank2.data,
			isClaimedRank3 = this.isClaimedRank3.data,
			isClaimedRank4 = this.isClaimedRank4.data,
			isClaimedRank5 = this.isClaimedRank5.data,
			isClaimedRank6 = this.isClaimedRank6.data,
			isShowUnlockTournament = this.isShowUnlockTournament,
			tournamentGunProfile = this.tournamentGunProfile.data,
			isNoLongerRate = this.isNoLongerRate,
			isShowRateMap1 = this.isShowRateMap1,
			isShowRateMap2 = this.isShowRateMap2,
			isShowRateMap3 = this.isShowRateMap3,
			isFirstBuyGem100 = this.isFirstBuyGem100,
			isFirstBuyGem300 = this.isFirstBuyGem300,
			isFirstBuyGem500 = this.isFirstBuyGem500,
			isFirstBuyGem1000 = this.isFirstBuyGem1000,
			isFirstBuyGem2500 = this.isFirstBuyGem2500,
			isFirstBuyGem5000 = this.isFirstBuyGem5000,
			isPurchasedStarterPack = this.isPurchasedStarterPack,
			isClaimNewbiePack = this.isClaimNewbiePack,
			isClaimHeroPack = this.isClaimHeroPack,
			isPurchasedPackEverybodyFavorite = this.isPurchasedPackEverybodyFavorite,
			isPurchasedPackDragonBreath = this.isPurchasedPackDragonBreath,
			isPurchasedPackLetThereBeFire = this.isPurchasedPackLetThereBeFire,
			isPurchasedPackSnippingForDummies = this.isPurchasedPackSnippingForDummies,
			isPurchasedPackTaserLaser = this.isPurchasedPackTaserLaser,
			isPurchasedPackShockingSale = this.isPurchasedPackShockingSale,
			isPurchasedPackUpgradeEnthusiast = this.isPurchasedPackUpgradeEnthusiast,
			strTimestampNextFreeGifts = string.Join(";", this.timestampNextFreeGifts)
		};
	}

	public string GetRawUserProfileJsonString()
	{
		return JsonConvert.SerializeObject(this.GetRawUserProfile());
	}
}
