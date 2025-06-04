using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewbieController : MonoBehaviour
{
	const string KEY_NEWBIE_CODE = "metalx";
	[SerializeField] GameObject _popupView;
	[SerializeField] TMP_InputField _inputCode;
	[SerializeField] Button _btnClaim;

	public GameObject View => _popupView;
	
	string _targetCode = KEY_NEWBIE_CODE;
	int _idGun = 4;
	int _coinReward = 40000;

	void Awake()
	{
		_btnClaim.interactable = false;
	}

	public void OnCodeChanged()
	{
		_btnClaim.interactable = _inputCode.text == _targetCode;
	}

	public void Open()
	{
		_popupView.SetActive(true);
		SoundManager.Instance.PlaySfxClick();
	}

	public void ClaimReward()
	{
		Close();
		SoundManager.Instance.PlaySfxClick();
		ProfileManager.UserProfile.gunNormalId.Set(_idGun);
		GameData.playerResources.ReceiveCoin(_coinReward);
		ProfileManager.UserProfile.isClaimNewbiePack.Set(true);
		GameData.playerGuns.ReceiveNewGun(_idGun);
		EventDispatcher.Instance.PostEvent(EventID.ClaimNewbiePackage);
	}

	public void Close()
	{
		_popupView.SetActive(false);
		SoundManager.Instance.PlaySfxClick();
	}
}