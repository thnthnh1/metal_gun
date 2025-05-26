using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
	public UIModeSurvival modeSurvivalUI;

	public GameObject modeCampaignUI;

	public HudComboKill hudComboKill;

	public HudPause hudPause;

	public HudQuest hudQuest;

	public HudBoss hudBoss;

	public HudWin hudWin;

	public HudLose hudLose;

	public HudSelectBooster hudSelectBooster;

	public HudSaveMe hudSaveMe;

	public HudGunDrop hudGunDrop;

	public HudSurvivalResult hudSurvivalResult;

	public HudSurvivalGuide hudSurvivalGuide;

	public LabelMissionStart missionStart;

	[Header("WEAPONS")]
	public ButtonActionIngame buttonSwitchGun;

	public ButtonActionIngame buttonThrowGrenade;

	public Text textSpecialGunAmmo;

	public GameObject infinityAmmoSymbol;

	public Text textNumberOfGrenade;

	public Text textCooldownGrenade;

	public Image imageCooldownGrenade;

	public GameObject btnFire;

	public Button btnAutoFire;

	public Sprite sprAutoFireOff;

	public Sprite sprAutoFireOn;

	[Header("BOOSTER")]
	public ButtonActionIngame buttonUseBoosterHP;

	public Text textRemainingBoosterHP;

	public GameObject[] activeBoosters;

	private int totalBoosterHP;

	[Header("TUTORIAL")]
	public TutorialGamePlayController tutorialGamePlay;

	[Header("SKILL")]
	public Image imgSkillBackground;

	public Button btnActiveSkill;

	public Text textCooldownSkill;

	[Space(20f)]
	public TMP_Text _textName;
	public Text textLevelRambo;

	public Text textGameTime;

	public Text textCoinCollected;

	public Image hpPlayer;

	public Image arrowGoRight;

	public Image arrowGoLeft;

	public RectTransform iconRamboMapProgress;

	public GameObject panelIngameUI;

	public GameObject alarmRedScreen;

	public Animation takeDamageScreen;

	private void Start()
	{
		EventDispatcher.Instance.RegisterListener(EventID.SelectBoosterDone, delegate(Component sender, object param)
		{
			this.ActiveBoosters();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ToggleAutoFire, delegate(Component sender, object param)
		{
			this.OnToggleAutoFire();
		});
		EventDispatcher.Instance.RegisterListener(EventID.GameStart, delegate(Component sender, object param)
		{
			this.OnGameStart();
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReviveByGem, delegate(Component sender, object param)
		{
			this.ActiveIngameUI(true);
		});
		EventDispatcher.Instance.RegisterListener(EventID.ReviveByAds, delegate(Component sender, object param)
		{
			this.ActiveIngameUI(true);
		});
		this.hudComboKill.Init();
		this.hudBoss.Init();
		this.hudGunDrop.Init();
		this.modeCampaignUI.SetActive(GameData.mode == GameMode.Campaign);
		this.modeSurvivalUI.gameObject.SetActive(GameData.mode == GameMode.Survival);
		if (GameData.mode == GameMode.Campaign)
		{
			this.hudSelectBooster.Open();
		}
		else if (GameData.mode == GameMode.Survival)
		{
			this.modeSurvivalUI.Init();
			this.hudSurvivalGuide.Open();
		}
	}

	public void Jump()
	{
		EventDispatcher.Instance.PostEvent(EventID.ClickButtonJump);
	}

	public void Shoot(bool isHold)
	{
		EventDispatcher.Instance.PostEvent(EventID.ClickButtonShoot, isHold);
	}

	public void ThrowGrenade()
	{
		EventDispatcher.Instance.PostEvent(EventID.ClickButtonThrowGrenade);
	}

	public void SetCooldownButtonGrenade(bool isDone)
	{
		if (isDone)
		{
			this.buttonThrowGrenade.Enable();
		}
		else
		{
			this.buttonThrowGrenade.Disable();
		}
		this.imageCooldownGrenade.gameObject.SetActive(!isDone);
		this.textCooldownGrenade.gameObject.SetActive(!isDone);
	}

	public void ActiveButtonGrenade(bool isActive)
	{
		if (isActive)
		{
			this.buttonThrowGrenade.Enable();
		}
		else
		{
			this.buttonThrowGrenade.Disable();
		}
	}

	public void ToggleSwitchGun()
	{
		EventDispatcher.Instance.PostEvent(EventID.ToggleSwitchGun);
	}

	public void ToggleAutoFire()
	{
		EventDispatcher.Instance.PostEvent(EventID.ToggleAutoFire);
	}

	private void OnToggleAutoFire()
	{
		if (this.btnAutoFire.image.sprite == this.sprAutoFireOff)
		{
			this.btnAutoFire.image.sprite = this.sprAutoFireOn;
		}
		else
		{
			this.btnAutoFire.image.sprite = this.sprAutoFireOff;
			SoundManager.Instance.PlaySfx(SoundManager.Instance.GetAudioClip("sfx_cartouche"), -15f);
		}
	}

	public void UseBoosterHP()
	{
		this.totalBoosterHP--;
		this.textRemainingBoosterHP.text = ((this.totalBoosterHP <= 0) ? string.Empty : string.Format("x{0:n0}", this.totalBoosterHP));
		if (this.totalBoosterHP <= 0)
		{
			this.buttonUseBoosterHP.Disable();
		}
		EventDispatcher.Instance.PostEvent(EventID.UseBoosterHP);
	}

	public void ActiveIngameUI(bool isActive)
	{
		this.panelIngameUI.SetActive(isActive);
	}

	public void UpdateCoinCollectedText(int value)
	{
		this.textCoinCollected.text = value.ToString("n0");
	}

	public void UpdateGunTypeText(bool isUsingNormalGun, int specialAmmo)
	{
		this.textSpecialGunAmmo.gameObject.SetActive(!isUsingNormalGun);
		this.infinityAmmoSymbol.SetActive(isUsingNormalGun);
		if (!isUsingNormalGun)
		{
			this.textSpecialGunAmmo.text = string.Format("x{0:n0}", specialAmmo);
		}
	}

	public void UpdateGrenadeText(int remainingGrenade)
	{
		this.textNumberOfGrenade.text = string.Format("x{0:n0}", remainingGrenade);
	}

	public void UpdateGameTime(int min, int second)
	{
		if (second < 0)
		{
			second = 0;
		}
		this.textGameTime.text = string.Format("{0:00}:{1:00}", min, second);
	}

	public void UpdatePlayerHpBar(float percent)
	{
		this.hpPlayer.fillAmount = percent;
	}

	public void UpdateMapProgress(float percent)
	{
		float target = Mathf.Clamp(percent * 184f, 0f, 184f);
		Vector2 anchoredPosition = this.iconRamboMapProgress.anchoredPosition;
		anchoredPosition.x = Mathf.MoveTowards(anchoredPosition.x, target, 200f * Time.deltaTime);
		this.iconRamboMapProgress.anchoredPosition = anchoredPosition;
	}

	public void ShowArrowGo(bool isRight)
	{
		this.arrowGoRight.gameObject.SetActive(isRight);
		this.arrowGoLeft.gameObject.SetActive(!isRight);
		this.StartDelayAction(delegate
		{
			this.arrowGoRight.gameObject.SetActive(false);
			this.arrowGoLeft.gameObject.SetActive(false);
		}, 3f);
	}

	public void ShowMissionStart()
	{
	}

	public void BackToMainMenu()
	{
		Time.timeScale = 1f;
		Singleton<GameController>.Instance.SetActiveAllUnits(false);
		SceneFading.Instance.FadeOutAndLoadScene("Menu", true, 2f);
	}

	public void Retry()
	{
		Time.timeScale = 1f;
		SoundManager.Instance.PlaySfxClick();
		Singleton<GameController>.Instance.SetActiveAllUnits(false);
		SceneFading.Instance.FadeOutAndLoadScene("GamePlay", true, 2f);
	}

	private void OnGameStart()
	{
		this.ActiveIngameUI(true);
		this.InitRamboInfo();
	}

	public void ActiveBoosters()
	{
		if (GameData.mode == GameMode.Survival)
		{
			this.buttonUseBoosterHP.gameObject.SetActive(false);
			for (int i = 0; i < this.activeBoosters.Length; i++)
			{
				BoosterType boosterType = (BoosterType)int.Parse(this.activeBoosters[i].name);
				this.activeBoosters[i].SetActive(GameData.survivalUsingBooster == boosterType);
			}
		}
		else
		{
			for (int j = 0; j < this.activeBoosters.Length; j++)
			{
				BoosterType item = (BoosterType)int.Parse(this.activeBoosters[j].name);
				this.activeBoosters[j].SetActive(GameData.selectingBoosters.Contains(item));
			}
			this.buttonUseBoosterHP.gameObject.SetActive(true);
			if (GameData.selectingBoosters.Contains(BoosterType.Hp))
			{
				this.totalBoosterHP = 1;
				this.buttonUseBoosterHP.Enable();
				this.textRemainingBoosterHP.text = ((this.totalBoosterHP <= 1) ? string.Empty : string.Format("x{0:n0}", this.totalBoosterHP));
			}
			else
			{
				this.buttonUseBoosterHP.Disable();
			}
		}
	}

	private void InitRamboInfo()
	{
		int key = ProfileManager.UserProfile.ramboId;
		int num = (!GameData.playerRambos.ContainsKey(key)) ? 0 : GameData.playerRambos[key].level;
		this.textLevelRambo.text = string.Format("Lv: {0}", num);
		_textName.text = PlayerPrefs.GetString("playerName");
		this.UpdatePlayerHpBar(1f);
	}

	public void SetSkillIcon(int id)
	{
		this.btnActiveSkill.image.sprite = GameResourcesUtils.GetSkillUnlockImage(id);
		this.imgSkillBackground.sprite = GameResourcesUtils.GetSkillUnlockImage(id);
	}

	public void ActiveSkill()
	{
		EventDispatcher.Instance.PostEvent(EventID.RamboActiveSkill);
	}

	public void EnableSkill(bool isActive)
	{
		this.btnActiveSkill.enabled = isActive;
		this.btnActiveSkill.image.raycastTarget = isActive;
		this.textCooldownSkill.gameObject.SetActive(!isActive);
		if (isActive)
		{
			this.btnActiveSkill.image.fillAmount = 1f;
		}
	}

	public void SetCooldownSkill(float percent)
	{
		this.btnActiveSkill.image.fillAmount = percent;
	}

	public void SetTextCooldownSkill(float remaining)
	{
		this.textCooldownSkill.text = remaining.ToString();
	}

	public void InstantEndGame(bool isWin)
	{
		EventDispatcher.Instance.PostEvent(EventID.GameEnd, isWin);
	}
}
