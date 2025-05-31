using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PopupTitleID
{
    Confirmation = 0
}

public class Popup : Singleton<Popup>
{
    public Setting setting;
    public Rate rate;
    public Loading loading;
    public InstantLoading instantLoading;

    [Header("NOTICE POPUP")]
    public GameObject noticePopup;
    public GameObject btnYes;
    public GameObject btnNo;
    public GameObject btnOk;
    public Text textTitle;
    public Text textContent;
    public GameObject[] imageTitles;

    [Header("TOAST MESSAGE")]
    public Animation toastAnim;
    public Text textToastContent;

    [Header("REWARD")]
    public Text textRewardContent;
    public GameObject rewardPopup;
    public RewardElement[] rewardCells;

    [Header("RATING")]
    public GameObject popupRate;

    [Header("PRIVACY")]
    public GameObject privacy;
    private UnityAction yesCallback;
    private UnityAction noCallback;
    private UnityAction privacyCallback;

    public bool IsShowing
    {
        get
        {
            return this.noticePopup.activeSelf || this.setting.gameObject.activeSelf;
        }
    }

    public bool IsInstantLoading
    {
        get
        {
            return this.instantLoading.gameObject.activeSelf;
        }
    }

    private void Awake()
    {
        UnityEngine.Object.DontDestroyOnLoad(this);
    }
    private bool isPopupShowing = false;

    private void ShowNoConnectionPopup()
    {
        isPopupShowing = true;
        this.textContent.text = "Please turn on internet to continue playing".ToUpper();
        this.textTitle.text = "No internet connection".ToUpper();
        this.btnOk.gameObject.SetActive(true);
        this.btnNo.gameObject.SetActive(false);
        this.btnYes.gameObject.SetActive(false);
        this.textContent.gameObject.SetActive(true);
        this.rewardPopup.SetActive(false);
        Debug.Log("Nik log is the notice popup on 1 ");
        this.noticePopup.SetActive(true);
        SoundManager.Instance.PlaySfx("sfx_show_dialog", -20f);
    }

    private bool isNetworkAvailable()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            // Debug.Log("Error. Check internet connection!");
            return false;
        }
        else
        {
            // Debug.Log("Network. Availalbe!");
            return true;
        }
    }

    public void Show(string content, string title = "NOTICE", PopupType type = PopupType.Ok, UnityAction yesCallback = null, UnityAction noCallback = null)
    {
        foreach (var image in this.imageTitles)
        {
            image.gameObject.SetActive(false);
        }

        this.textContent.text = content.ToUpper();
        this.textTitle.gameObject.SetActive(true);
        this.textTitle.text = title.ToUpper();
        this.yesCallback = yesCallback;
        this.noCallback = noCallback;
        this.btnNo.gameObject.SetActive(type == PopupType.YesNo);
        this.btnYes.gameObject.SetActive(type == PopupType.YesNo);
        this.btnOk.gameObject.SetActive(type != PopupType.YesNo);
        this.textContent.gameObject.SetActive(true);
        this.rewardPopup.SetActive(false);
        Debug.Log("Nik log is the notice popup on 2 ");
        this.noticePopup.SetActive(true);
        SoundManager.Instance.PlaySfx("sfx_show_dialog", -20f);
    }

    public void Show(string content, PopupTitleID titleId, PopupType type = PopupType.Ok, UnityAction yesCallback = null, UnityAction noCallback = null)
    {
        foreach (var image in this.imageTitles)
        {
            image.gameObject.SetActive(false);
        }

        this.textContent.text = content.ToUpper();
        this.textTitle.gameObject.SetActive(false);
        this.imageTitles[(int)titleId].gameObject.SetActive(true);
        this.yesCallback = yesCallback;
        this.noCallback = noCallback;
        this.btnNo.gameObject.SetActive(type == PopupType.YesNo);
        this.btnYes.gameObject.SetActive(type == PopupType.YesNo);
        this.btnOk.gameObject.SetActive(type != PopupType.YesNo);
        this.textContent.gameObject.SetActive(true);
        this.rewardPopup.SetActive(false);
        Debug.Log("Nik log is the notice popup on 2 ");
        this.noticePopup.SetActive(true);
        SoundManager.Instance.PlaySfx("sfx_show_dialog", -20f);
    }

    public void ShowToastMessage(string content, ToastLength length = ToastLength.Normal)
    {
        this.textToastContent.text = content.ToUpper();
        if (length == ToastLength.Normal)
        {
            this.toastAnim.Play("toast_message");
        }
        else
        {
            this.toastAnim.Play("toast_message_long");
        }
    }

    public void ShowReward(List<RewardData> rewards, string content = null, UnityAction yesCallback = null)
    {
        this.textTitle.text = "CONGRATULATIONS";
        this.textContent.gameObject.SetActive(false);
        this.textRewardContent.text = ((content != null) ? content : "YOU'VE GOT REWARDS!");
        this.btnNo.gameObject.SetActive(false);
        this.btnYes.gameObject.SetActive(false);
        this.btnOk.gameObject.SetActive(true);
        this.yesCallback = yesCallback;
        for (int i = 0; i < this.rewardCells.Length; i++)
        {
            RewardElement rewardElement = this.rewardCells[i];
            rewardElement.gameObject.SetActive(false);
            rewardElement.gameObject.SetActive(i < rewards.Count);
            if (i < rewards.Count)
            {
                RewardData data = rewards[i];
                rewardElement.SetInformation(data, false);
            }
        }
        this.rewardPopup.SetActive(true);
        Debug.Log("Nik log is the notice popup on 3 ");
        this.noticePopup.SetActive(true);
        SoundManager.Instance.PlaySfx("sfx_get_reward", 0f);
    }

    public void ShowRateUs()
    {
        this.rate.Show();
    }

    public void ShowInstantLoading(int timeout = 15)
    {
        this.instantLoading.Show(timeout);
    }

    public void HideInstantLoading()
    {
        this.instantLoading.Hide();
    }

    public void Yes()
    {
        if (this.yesCallback != null)
        {
            this.yesCallback();
        }
        this.Hide();
        SoundManager.Instance.PlaySfxClick();

        if (isPopupShowing)
        {
            isPopupShowing = false;
        }
    }

    public void No()
    {
        if (this.noCallback != null)
        {
            this.noCallback();
        }
        this.Hide();
        SoundManager.Instance.PlaySfxClick();
    }

    public void Hide()
    {
        this.yesCallback = null;
        this.noCallback = null;
        this.noticePopup.SetActive(false);
        this.setting.gameObject.SetActive(false);
    }

    public void ShowPrivacy(UnityAction privacyCallback)
    {
        this.privacyCallback = privacyCallback;
        this.privacy.SetActive(true);
    }

    public void PrivacyAgree()
    {
        PlayerPrefs.SetString("user_consent", "PERSONALIZED");
        PlayerPrefs.Save();
        this.privacy.SetActive(false);
        if (this.privacyCallback != null)
        {
            this.privacyCallback();
        }
    }

    public void PrivacyDisagree()
    {
        PlayerPrefs.SetString("user_consent", "NON_PERSONALIZED");
        PlayerPrefs.Save();
        this.privacy.SetActive(false);
        if (this.privacyCallback != null)
        {
            this.privacyCallback();
        }
    }
}
