using System;
using UnityEngine;
using UnityEngine.UI;

public class HudSurvivalResult : MonoBehaviour
{
	public Text soldierKill;

	public Text vehicleKill;

	public Text bossKill;

	public Text comboKill;

	public Text soldierScore;

	public Text vehicleScore;

	public Text bossScore;

	public Text comboKillScore;

	public Text timeScore;

	public Text totalScore;

	public Text seasonScore;

	public Text coinReward;

	public void Open(SurvivalResultData data)
	{
		this.soldierKill.text = data.soldierKill.ToString("n0");
		this.vehicleKill.text = data.vehicleKill.ToString("n0");
		this.bossKill.text = data.bossKill.ToString("n0");
		this.comboKill.text = data.highestComboKill.ToString("n0");
		this.soldierScore.text = data.soldierScore.ToString("n0");
		this.vehicleScore.text = data.vehicleScore.ToString("n0");
		this.bossScore.text = data.bossScore.ToString("n0");
		this.comboKillScore.text = data.highestComboKill.ToString("n0");
		this.timeScore.text = data.timeScore.ToString("n0");
		this.totalScore.text = data.totalScore.ToString("n0");
		this.seasonScore.text = GameData.playerTournamentData.score.ToString("n0");
		int value = data.totalScore;
		this.coinReward.text = value.ToString("n0");
		GameData.playerResources.ReceiveCoin(value);
		base.gameObject.SetActive(true);
	}

	public void MainMenu()
	{
		SoundManager.Instance.PlaySfxClick();
		Singleton<UIController>.Instance.BackToMainMenu();
	}
}
