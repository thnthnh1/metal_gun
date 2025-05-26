using System;
using UnityEngine;
using UnityEngine.UI;

public class UIModeSurvival : MonoBehaviour
{
	[Header("LAYOUT")]
	public Text textWave;

	public Text textScore;

	public Text textNotice;

	public GameObject labelWaveComplete;

	[Header("SUPPORTS")]
	public GameObject groupSupportItems;

	public BaseSupportItem[] supports;

	public void Init()
	{
		this.textWave.text = string.Empty;
		this.textScore.text = string.Empty;
		this.HideNotice();
		this.groupSupportItems.SetActive(false);
		for (int i = 0; i < this.supports.Length; i++)
		{
			this.supports[i].Init();
		}
	}

	public void SetTextWave(int number)
	{
		this.textWave.text = number.ToString();
	}

	public void SetScoreText(int score)
	{
		this.textScore.text = score.ToString("n0");
	}

	public void ShowNotice(string content)
	{
		this.textNotice.text = content;
		this.textNotice.gameObject.SetActive(true);
	}

	public void HideNotice()
	{
		this.textNotice.text = string.Empty;
		this.textNotice.gameObject.SetActive(false);
	}

	public void ShowComplete()
	{
		this.labelWaveComplete.SetActive(false);
		this.labelWaveComplete.SetActive(true);
	}

	public void ToggleShowGroupSupport()
	{
		if (this.groupSupportItems.activeInHierarchy)
		{
			this.groupSupportItems.SetActive(false);
		}
		else
		{
			this.groupSupportItems.SetActive(true);
		}
	}

	public void NextWave()
	{
		((SurvivalModeController)Singleton<GameController>.Instance.modeController).NextWave();
	}
}
