using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Text textVersion;

    private static Func<KeyValuePair<int, StaticGunData>, bool> __f__am_cache0;

    private static Func<KeyValuePair<int, StaticGunData>, int> __f__am_cache1;

    private static Func<KeyValuePair<int, StaticGunData>, int> __f__am_cache2;

    private static Func<KeyValuePair<int, StaticGunData>, StaticGunData> __f__am_cache3;

    private void Awake()
    {
        this.LoadStaticData();
        this.LoadPlayerData();
    }

    private void Start()
    {
        // this.textVersion.text = "v" + Singleton<MasterInfo>.Instance.Version;
        this.textVersion.text = "v" + 1.02;

    }

    public void PlayAsGuest()
    {
        SoundManager.Instance.PlaySfx("sfx_explosive", 0f);
        SceneFading.Instance.FadeOutAndLoadScene("Menu", true, 2f);
    }

    private void LoadStaticData()
    {
        this.LoadStaticGunData();
        this.LoadStaticGrenadeData();
        this.LoadStaticMeleeWeaponData();
        this.LoadStaticCampaignStageData();
        this.LoadStaticCampaignBoxRewardData();
        this.LoadStaticRecommendGunData();
        this.LoadStaticRamboData();
        this.LoadStaticRamboSkillData();
        this.LoadStaticBoosterData();
        this.LoadStaticDailyQuestData();
        this.LoadStaticAchievementData();
        this.LoadStaticRankData();
        this.LoadStaticFreeGiftData();
        this.LoadStaticTournamentRankData();
        this.LoadQuestDescription();
        this.LoadRankName();
        this.LoadGunValueGem();
        this.LoadCampaignStageLevelData();
    }

    private void LoadStaticGunData()
    {
        if (GameData.staticGunData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_gun_data");
            Dictionary<int, StaticGunData> dictionary = JsonConvert.DeserializeObject<Dictionary<int, StaticGunData>>(textAsset.text);
            List<KeyValuePair<int, StaticGunData>> source = dictionary.ToList<KeyValuePair<int, StaticGunData>>();
            source = (from x in source
                      orderby x.Value.isSpecialGun, x.Value.index
                      select x).ToList<KeyValuePair<int, StaticGunData>>();
            dictionary = source.ToDictionary((KeyValuePair<int, StaticGunData> x) => x.Key, (KeyValuePair<int, StaticGunData> x) => x.Value);
            string value = JsonConvert.SerializeObject(dictionary);
            GameData.staticGunData = JsonConvert.DeserializeObject<_StaticGunData>(value);
        }
    }

    private void LoadStaticGrenadeData()
    {
        if (GameData.staticGrenadeData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_grenade_data");
            GameData.staticGrenadeData = JsonConvert.DeserializeObject<_StaticGrenadeData>(textAsset.text);
        }
    }

    private void LoadStaticMeleeWeaponData()
    {
        if (GameData.staticMeleeWeaponData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_melee_weapon_data");
            GameData.staticMeleeWeaponData = JsonConvert.DeserializeObject<_StaticMeleeWeaponData>(textAsset.text);
        }
    }

    private void LoadStaticCampaignStageData()
    {
        if (GameData.staticCampaignStageData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_campaign_stage_data");
            GameData.staticCampaignStageData = JsonConvert.DeserializeObject<_StaticCampaignStageData>(textAsset.text);
        }
    }

    private void LoadStaticCampaignBoxRewardData()
    {
        if (GameData.staticCampaignBoxRewardData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_campaign_box_reward_data");
            GameData.staticCampaignBoxRewardData = JsonConvert.DeserializeObject<_StaticCampaignBoxRewardData>(textAsset.text);
        }
    }

    private void LoadStaticRamboData()
    {
        if (GameData.staticRamboData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_rambo_data");
            GameData.staticRamboData = JsonConvert.DeserializeObject<_StaticRamboData>(textAsset.text);
        }
    }

    private void LoadStaticRamboSkillData()
    {
        if (GameData.staticRamboSkillData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_rambo_skill_data");
            GameData.staticRamboSkillData = JsonConvert.DeserializeObject<_StaticRamboSkillData>(textAsset.text);
        }
    }

    private void LoadStaticBoosterData()
    {
        if (GameData.staticBoosterData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_booster_data");
            GameData.staticBoosterData = JsonConvert.DeserializeObject<_StaticBoosterData>(textAsset.text);
        }
    }

    private void LoadStaticDailyQuestData()
    {
        if (GameData.staticDailyQuestData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_daily_quest_data");
            GameData.staticDailyQuestData = JsonConvert.DeserializeObject<_StaticDailyQuestData>(textAsset.text);
        }
    }

    private void LoadStaticAchievementData()
    {
        if (GameData.staticAchievementData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_achievement_data");
            GameData.staticAchievementData = JsonConvert.DeserializeObject<_StaticAchievementData>(textAsset.text);
        }
    }

    private void LoadStaticRankData()
    {
        if (GameData.staticRankData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_rank_data");
            GameData.staticRankData = JsonConvert.DeserializeObject<_StaticRankData>(textAsset.text);
        }
    }

    private void LoadStaticFreeGiftData()
    {
        if (GameData.staticFreeGiftData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_free_gift_data");
            GameData.staticFreeGiftData = JsonConvert.DeserializeObject<_StaticFreeGiftData>(textAsset.text);
        }
    }

    private void LoadStaticTournamentRankData()
    {
        if (GameData.staticTournamentRankData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Tournament Data/tournament_rank_data");
            GameData.staticTournamentRankData = JsonConvert.DeserializeObject<_StaticTournamentRankData>(textAsset.text);
        }
        if (GameData.tournamentTopRankRewards == null)
        {
            TextAsset textAsset2 = Resources.Load<TextAsset>("JSON/Tournament Data/tournament_top_rank_reward_data");
            GameData.tournamentTopRankRewards = JsonConvert.DeserializeObject<Dictionary<int, List<RewardData>>>(textAsset2.text);
        }
    }

    private void LoadStaticRecommendGunData()
    {
        if (GameData.staticRecommendGunData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Static Data/static_recommend_gun_data");
            GameData.staticRecommendGunData = JsonConvert.DeserializeObject<_StaticRecommendGunData>(textAsset.text);
        }
    }

    private void LoadQuestDescription()
    {
        if (GameData.questDescriptions == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Mix/stage_quest_description");
            GameData.questDescriptions = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
        }
    }

    private void LoadRankName()
    {
        if (GameData.rankNames == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Mix/rank_name");
            GameData.rankNames = JsonConvert.DeserializeObject<Dictionary<int, string>>(textAsset.text);
        }
    }

    private void LoadGunValueGem()
    {
        if (GameData.gunValueGem == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Mix/weapon_exchange_to_gem");
            GameData.gunValueGem = JsonConvert.DeserializeObject<Dictionary<int, int>>(textAsset.text);
        }
    }

    private void LoadCampaignStageLevelData()
    {
        if (GameData.campaignStageLevelData == null)
        {
            TextAsset textAsset = Resources.Load<TextAsset>("JSON/Mix/campaign_stage_level_data");
            GameData.campaignStageLevelData = JsonConvert.DeserializeObject<Dictionary<string, int>>(textAsset.text);
        }
    }

    private void LoadPlayerData()
    {
        this.LoadPlayerProfile();
        this.LoadPlayerResourcesData();
        this.LoadPlayerRamboData();
        this.LoadPlayerRamboSkillData();
        this.LoadPlayerGunData();
        this.LoadPlayerGrenadeData();
        this.LoadPlayerMeleeWeaponData();
        this.LoadPlayerCampaignProgressData();
        this.LoadPlayerCampaignRewardProgressData();
        this.LoadPlayerBoosterData();
        this.LoadPlayerSelectingBooster();
        this.LoadPlayerDailyQuestData();
        this.LoadPlayerAchievementData();
        this.LoadPlayerTutorialData();
        this.LoadPlayerPurchased();
        ProfileManager.SaveAll();
    }

    private void LoadPlayerProfile()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerProfile))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_profile").text;
            ProfileManager.UserProfile.playerProfile.Set(value);
        }
        else
        {
            value = ProfileManager.UserProfile.playerProfile;
        }
        GameData.playerProfile = JsonConvert.DeserializeObject<_PlayerProfile>(value);
    }

    private void LoadPlayerResourcesData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerResourcesData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_resources_data").text;
            ProfileManager.UserProfile.playerResourcesData.Set(value);
        }
        else
        {
            value = ProfileManager.UserProfile.playerResourcesData;
        }
        GameData.playerResources = JsonConvert.DeserializeObject<_PlayerResourcesData>(value);
    }

    private void LoadPlayerRamboData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerRamboData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_rambo_data").text;
            ProfileManager.UserProfile.playerRamboData.Set(value);
            ProfileManager.UserProfile.ramboId.Set(0);
        }
        else
        {
            value = ProfileManager.UserProfile.playerRamboData;
        }
        GameData.playerRambos = JsonConvert.DeserializeObject<_PlayerRamboData>(value);

        var defaultData = JsonConvert.DeserializeObject<_PlayerRamboData>(Resources.Load<TextAsset>("JSON/New Player Data/new_player_rambo_data").text);
        foreach (var player in defaultData)
        {
            if (!GameData.playerRambos.ContainsKey(player.Key))
            {
                GameData.playerRambos.Add(player.Key, player.Value);
            }
        }
    }

    private void LoadPlayerRamboSkillData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerRamboSkillData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_rambo_skill_data").text;
            ProfileManager.UserProfile.playerRamboSkillData.Set(value);
        }
        else
        {
            value = ProfileManager.UserProfile.playerRamboSkillData;
        }
        GameData.playerRamboSkills = JsonConvert.DeserializeObject<_PlayerRamboSkillData>(value);

        var defaultData = JsonConvert.DeserializeObject<_PlayerRamboSkillData>(Resources.Load<TextAsset>("JSON/New Player Data/new_player_rambo_skill_data").text);
        foreach (var player in GameData.playerRambos)
        {
            if (!GameData.playerRamboSkills.ContainsKey(player.Key))
            {
                GameData.playerRamboSkills.Add(player.Key, defaultData[player.Value.id]);
            }
        }
    }

    private void LoadPlayerGunData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerGunData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_gun_data").text;
            ProfileManager.UserProfile.playerGunData.Set(value);
            ProfileManager.UserProfile.gunNormalId.Set(0);
            ProfileManager.UserProfile.gunSpecialId.Set(-1);
        }
        else
        {
            value = ProfileManager.UserProfile.playerGunData;
        }
        GameData.playerGuns = JsonConvert.DeserializeObject<_PlayerGunData>(value);
    }

    private void LoadPlayerGrenadeData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerGrenadeData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_grenades_data").text;
            ProfileManager.UserProfile.playerGrenadeData.Set(value);
            ProfileManager.UserProfile.grenadeId.Set(500);
        }
        else
        {
            value = ProfileManager.UserProfile.playerGrenadeData;
        }
        GameData.playerGrenades = JsonConvert.DeserializeObject<_PlayerGrenadeData>(value);
        int num = ProfileManager.UserProfile.grenadeId;
        if (!GameData.staticGrenadeData.ContainsKey(num))
        {
            GameData.playerGrenades.RemoveGrenade(num);
            ProfileManager.UserProfile.grenadeId.Set(500);
        }
    }

    private void LoadPlayerMeleeWeaponData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerMeleeWeaponData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_melee_weapon_data").text;
            ProfileManager.UserProfile.playerMeleeWeaponData.Set(value);
            ProfileManager.UserProfile.meleeWeaponId.Set(600);
        }
        else
        {
            value = ProfileManager.UserProfile.playerMeleeWeaponData;
        }
        GameData.playerMeleeWeapons = JsonConvert.DeserializeObject<_PlayerMeleeWeaponData>(value);
    }

    private void LoadPlayerCampaignProgressData()
    {
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerCampaignProgessData))
        {
            string value;
            if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerCampaignStageProgessData))
            {
                value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_campaign_stage_progress_data").text;
                ProfileManager.UserProfile.playerCampaignStageProgessData.Set(value);
            }
            else
            {
                value = ProfileManager.UserProfile.playerCampaignStageProgessData;
            }
            GameData.playerCampaignStageProgress = JsonConvert.DeserializeObject<_PlayerCampaignStageProgressData>(value);
        }
        else
        {
            GameData.playerCampaignProgress = JsonConvert.DeserializeObject<_PlayerCampaignProgressData>(ProfileManager.UserProfile.playerCampaignProgessData);
            GameData.playerCampaignStageProgress = new _PlayerCampaignStageProgressData();
            foreach (KeyValuePair<Difficulty, PlayerCampaignProgressData> current in GameData.playerCampaignProgress)
            {
                foreach (KeyValuePair<string, List<bool>> current2 in current.Value.stageProgress)
                {
                    if (GameData.playerCampaignStageProgress.ContainsKey(current2.Key))
                    {
                        GameData.playerCampaignStageProgress[current2.Key][(int)current.Key] = true;
                    }
                    else
                    {
                        List<bool> list = new List<bool>();
                        for (int i = 0; i < 3; i++)
                        {
                            list.Add(i == (int)current.Key);
                        }
                        GameData.playerCampaignStageProgress.Add(current2.Key, list);
                    }
                }
            }
            ProfileManager.UserProfile.playerCampaignProgessData.Set(string.Empty);
            ProfileManager.UserProfile.playerCampaignStageProgessData.Set(JsonConvert.SerializeObject(GameData.playerCampaignStageProgress));
        }
    }

    private void LoadPlayerCampaignRewardProgressData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerCampaignRewardProgessData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_campaign_reward_progress_data").text;
            ProfileManager.UserProfile.playerCampaignRewardProgessData.Set(value);
        }
        else
        {
            value = ProfileManager.UserProfile.playerCampaignRewardProgessData;
        }
        GameData.playerCampaignRewardProgress = JsonConvert.DeserializeObject<_PlayerCampaignRewardProgressData>(value);
    }

    private void LoadPlayerBoosterData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerBoosterData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_booster_data").text;
            ProfileManager.UserProfile.playerBoosterData.Set(value);
        }
        else
        {
            value = ProfileManager.UserProfile.playerBoosterData;
        }
        GameData.playerBoosters = JsonConvert.DeserializeObject<_PlayerBoosterData>(value);
    }

    private void LoadPlayerSelectingBooster()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerSelectingBooster))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_selecting_booster").text;
            ProfileManager.UserProfile.playerSelectingBooster.Set(value);
        }
        else
        {
            value = ProfileManager.UserProfile.playerSelectingBooster;
        }
        GameData.selectingBoosters = JsonConvert.DeserializeObject<_PlayerSelectingBooster>(value);
    }

    private void LoadPlayerDailyQuestData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerDailyQuestData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_daily_quest_data").text;
            ProfileManager.UserProfile.playerDailyQuestData.Set(value);
        }
        else
        {
            value = ProfileManager.UserProfile.playerDailyQuestData;
        }
        GameData.playerDailyQuests = JsonConvert.DeserializeObject<_PlayerDailyQuestData>(value);
    }

    private void LoadPlayerAchievementData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerAchievementData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_achievement_data").text;
            ProfileManager.UserProfile.playerAchievementData.Set(value);
        }
        else
        {
            value = ProfileManager.UserProfile.playerAchievementData;
        }
        GameData.playerAchievements = JsonConvert.DeserializeObject<_PlayerAchievementData>(value);
        List<AchievementType> list = new List<AchievementType>();
        foreach (KeyValuePair<AchievementType, PlayerAchievementData> current in GameData.playerAchievements)
        {
            bool flag = true;
            for (int i = 0; i < GameData.staticAchievementData.Count; i++)
            {
                StaticAchievementData staticAchievementData = GameData.staticAchievementData[i];
                if (staticAchievementData.type == current.Key)
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                list.Add(current.Key);
            }
        }
        for (int j = 0; j < list.Count; j++)
        {
            GameData.playerAchievements.Remove(list[j]);
        }
    }

    private void LoadPlayerTutorialData()
    {
        string value;
        if (string.IsNullOrEmpty(ProfileManager.UserProfile.playerTutorialData))
        {
            value = Resources.Load<TextAsset>("JSON/New Player Data/new_player_tutorial_data").text;
            ProfileManager.UserProfile.playerTutorialData.Set(value);
        }
        else
        {
            value = ProfileManager.UserProfile.playerTutorialData;
        }
        GameData.playerTutorials = JsonConvert.DeserializeObject<_PlayerTutorialData>(value);
        if (GameData.playerCampaignStageProgress.Count > 0)
        {
            GameData.playerTutorials.SetComplete(TutorialType.WorldMap);
            GameData.playerTutorials.SetComplete(TutorialType.Booster);
            GameData.playerTutorials.SetComplete(TutorialType.ActionInGame);
        }
        if (GameData.playerRambos.GetRamboLevel(ProfileManager.UserProfile.ramboId) > 1)
        {
            GameData.playerTutorials.SetComplete(TutorialType.Character);
        }
        bool flag = GameData.playerGuns.GetGunLevel(0) == 1;
        bool flag2 = !GameData.playerGuns.ContainsKey(107);
        if (!flag || !flag2)
        {
            GameData.playerTutorials.SetComplete(TutorialType.Weapon);
        }
    }

    private void LoadPlayerPurchased()
    {
        if (!ProfileManager.UserProfile.isPurchasedStarterPack && GameData.playerGuns.ContainsKey(1))
        {
            ProfileManager.UserProfile.isPurchasedStarterPack.Set(true);
        }
        if (!ProfileManager.UserProfile.isPurchasedPackLetThereBeFire && GameData.playerGuns.ContainsKey(108))
        {
            ProfileManager.UserProfile.isPurchasedPackLetThereBeFire.Set(true);
        }
        if (!ProfileManager.UserProfile.isPurchasedPackEverybodyFavorite && GameData.playerGuns.ContainsKey(6))
        {
            ProfileManager.UserProfile.isPurchasedPackEverybodyFavorite.Set(true);
        }
        if (!ProfileManager.UserProfile.isPurchasedPackDragonBreath && GameData.playerGuns.ContainsKey(2))
        {
            ProfileManager.UserProfile.isPurchasedPackDragonBreath.Set(true);
        }
        if (!ProfileManager.UserProfile.isPurchasedPackSnippingForDummies && GameData.playerGuns.ContainsKey(7))
        {
            ProfileManager.UserProfile.isPurchasedPackSnippingForDummies.Set(true);
        }
        if (!ProfileManager.UserProfile.isPurchasedPackTaserLaser && GameData.playerGuns.ContainsKey(103))
        {
            ProfileManager.UserProfile.isPurchasedPackTaserLaser.Set(true);
        }
    }
}
