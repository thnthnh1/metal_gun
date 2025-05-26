using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButton : MonoBehaviour
{
    public string stageNameId;

    public Button icon;

    public Sprite iconLock;

    public Sprite iconUnlock;

    public Sprite starLock;

    public Sprite starUnlock;

    public Text textStageName;

    public GameObject focus;

    public Image[] stars;

    private bool isLock;

    public void SelectStage()
    {
        SoundManager.Instance.PlaySfxClick();
        if (this.isLock)
        {
            return;
        }
        EventDispatcher.Instance.PostEvent(EventID.ClickStageOnWorldMap, this.stageNameId);
        if (GameData.isShowingTutorial && string.Compare(this.stageNameId, "1.1") == 0)
        {
            EventDispatcher.Instance.PostEvent(EventID.SubStepSelectStage);
        }
    }

    public void Load()
    {

        this.textStageName.text = this.stageNameId;
        if (MapUtils.IsStagePassed(this.stageNameId, Difficulty.Normal))
        {
            this.icon.image.sprite = this.iconUnlock;
            List<bool> list = GameData.playerCampaignStageProgress[this.stageNameId];
            int num = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i])
                {
                    num++;
                }
            }
            this.ActiveStars(num);
            this.isLock = false;
        }
        else
        {
            this.icon.image.sprite = this.iconLock;
            this.ActiveStars(0);
            this.isLock = true;
        }
        this.isLock = false; //testing to unlock levels added by hardik

        string currentProgressStageId = MapUtils.GetCurrentProgressStageId();
        MapType mapType = MapUtils.GetMapType(currentProgressStageId);
        MapType mapType2 = MapUtils.GetMapType(this.stageNameId);
        if (string.Compare(currentProgressStageId, this.stageNameId) == 0)
        {
            if (!MapUtils.IsStagePassed(currentProgressStageId, Difficulty.Normal))
            {
                this.icon.image.sprite = this.iconUnlock;
                this.ActiveStars(0);
                this.isLock = false;
            }
            this.focus.SetActive(true);
        }
        else if (mapType2 < mapType)
        {
            if (!MapUtils.IsStagePassed(this.stageNameId, Difficulty.Normal))
            {
                this.icon.image.sprite = this.iconUnlock;
                this.ActiveStars(0);
                this.isLock = false;
            }
            this.focus.SetActive(false);
        }
        else
        {
            this.focus.SetActive(false);
        }
        this.icon.image.SetNativeSize();
    }

    private void ActiveStars(int number)
    {
        for (int i = 0; i < this.stars.Length; i++)
        {
            this.stars[i].sprite = ((i >= number) ? this.starLock : this.starUnlock);
            this.stars[i].SetNativeSize();
        }
    }
}
