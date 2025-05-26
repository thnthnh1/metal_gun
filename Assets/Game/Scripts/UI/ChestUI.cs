using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestUI : MonoBehaviour, ISimpleUI
{
    bool clickable = false;
    bool isTouchChestDone = false;
    bool isTouchChestEnd = false;
    public Animator chestOpenAnimator;
    RewardType _rewardType;
    public GameObject[] rewardObjects;
    public Text[] rewardTexts;
    public Image gunIcon;
    private int curGunChanceBonus = 0;
    private int ticketChance;  // DO NOT TOUCH THIS
    public Button openBox1Time;
    public Button openBox5Time;
    int rewardTimes = 0;
    private void Start()
    {
        openBox1Time.onClick.AddListener(()=>TouchButton(ChestOpenType.OneTime));
        openBox5Time.onClick.AddListener(() => TouchButton(ChestOpenType.FiveTime));
    }
    private void OnEnable()
    {
        clickable = true;
        isTouchChestDone = false;
        isTouchChestEnd = false;
        chestOpenAnimator.Play("OpenChestBegin");
        curGunChanceBonus = PlayerPrefs.GetInt("curGunChanceBonus", 0);
        HideAllRewardObject(); ShowTicketButton(true);
        DisableChestButtonNotEnoughTicket(openBox1Time,20);
        DisableChestButtonNotEnoughTicket(openBox5Time, 80);
    }
    void DisableChestButtonNotEnoughTicket(Button but, int ticketsRequire)
    {
        if (GameData.playerResources.tournamentTicket < ticketsRequire)
        {
            but.interactable = false;
        }
        else
        {

            but.interactable = true;
        }
    }
    void HideAllRewardObject()
    {
        foreach (var item in rewardObjects)
        {
            item.gameObject.SetActive(false);
        }
    }
    void ShowTicketButton(bool show)
    {
        openBox1Time.gameObject.SetActive(show);
        openBox5Time.gameObject.SetActive(show);
    }
    public void TouchButton(ChestOpenType openTime)
    {
        if (clickable)
        {
            clickable = false;
            chestOpenAnimator.Play("OpenChestOpen");
            ShowTicketButton(false);
            switch (openTime)
            {
                case ChestOpenType.OneTime:
                    rewardTimes = 1;
                    GameData.playerResources.ConsumeTournamentTicket(20);
                    break;
                case ChestOpenType.FiveTime:
                    rewardTimes = 5;
                    GameData.playerResources.ConsumeTournamentTicket(80);
                    break;
                default:
                    break;
            }
        }
    }
    public void TouchChest()
    {
        if (isTouchChestDone)
        {
            if (rewardTimes > 0)
            {
                isTouchChestDone = false;
                chestOpenAnimator.Play("OpenChestOpen",0,0);

            }
            else if (!clickable && isTouchChestEnd)
            {
                Close();
            }
        }
    }
   
    public void TouchChestDone()
    {
        RewardSingleProgress();
        //StartCoroutine(RewardProgress());
    }
    public IEnumerator RewardProgress()
    {
        for (int i = 0; i < rewardTimes; i++)
        {
            GetRewardType(1, 35); 
            Reward();
            yield return new WaitForSeconds(1);
        }
        isTouchChestDone = true;
    }
    public void RewardSingleProgress()
    {
        GetRewardType(1, 35);
        Reward();
        rewardTimes -= 1;
        isTouchChestDone = true;
        if (rewardTimes <= 0) isTouchChestEnd = true;
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gold"></param>
    /// <param name="ticket"></param>
    public void GetRewardType(int gun, int ticket)
    {
        ticketChance = ticket;
        if (gun >= ticket)
        {
            Debug.LogError("gun chance have to be < ticket chance");
            return;
        }
        int randomType = Random.Range(0, 100);
        if (randomType < gun + curGunChanceBonus)
        {
            _rewardType = RewardType.Gun;
            ResetGunChance();
        }
        else if (randomType < ticket)
        {
            _rewardType = RewardType.Ticket;
            IncreaseGunChance();
        }
        else
        {
            _rewardType = RewardType.Gold;
            IncreaseGunChance();
        }

    }
    public void Reward()
    {
        switch (_rewardType)
        {
            case RewardType.Gold:
                int goldReward = Random.Range(500, 5001);
                ShowRewardInfo(RewardType.Gold, goldReward.ToString());
                GameData.playerResources.ReceiveCoin(goldReward);
                break;
            case RewardType.Ticket:
                int ticketReward = Random.Range(1, 9);
                ShowRewardInfo(RewardType.Ticket, ticketReward.ToString());
                GameData.playerResources.ReceiveTournamentTicket(ticketReward);
                break;
            case RewardType.Gun:
                int gunReward = 0;
                bool gunAvailable = false;
                for (int i = 0; i < 300; i++)
                {
                    gunReward = Random.Range(0, 300);
                    if (GameData.staticGunData.ContainsKey(gunReward) && !GameData.playerGuns.ContainsKey(gunReward))
                    {
                        gunAvailable = true;
                        break;
                    }
                }
                if (gunAvailable)
                {
                    ShowRewardInfo(RewardType.Gun, GameData.staticGunData[gunReward].gunName, gunReward);
                    GameData.playerGuns.ReceiveNewGun(gunReward);
                }
                else
                {
                    _rewardType = RewardType.Gold;
                    Reward();
                }
                break;
        }
    }
    public void ShowRewardInfo(RewardType rewardType, string textx, int gunID = 0)
    {
        HideAllRewardObject();
        switch (rewardType)
        {
            case RewardType.Gold:
                rewardTexts[0].text = textx;
                rewardObjects[0].gameObject.SetActive(true);
                break;
            case RewardType.Ticket:
                rewardTexts[1].text = textx;
                rewardObjects[1].gameObject.SetActive(true);
                break;
            case RewardType.Gun:
                rewardTexts[2].text = textx;
                rewardObjects[2].gameObject.SetActive(true);
                gunIcon.sprite = GameResourcesUtils.GetGunRewardImage(gunID);
                break;
        }
    }
    public enum RewardType
    {
        Gold,
        Ticket,
        Gun,
    }
    public enum ChestOpenType
    {
        OneTime,
        FiveTime
    }
    public void IncreaseGunChance()
    {
        curGunChanceBonus += 1;
        if (curGunChanceBonus >= ticketChance) curGunChanceBonus = ticketChance - 1;
        PlayerPrefs.SetInt("curGunChanceBonus", curGunChanceBonus);
    }
    public void ResetGunChance()
    {
        curGunChanceBonus =0;
        PlayerPrefs.SetInt("curGunChanceBonus", curGunChanceBonus);
    }
}
