using System;
using UnityEngine;
using UnityEngine.UI;

public class HudQuest : MonoBehaviour
{
	public Text stageName;

	public Text[] descriptions;

	public Text[] progress;

	public GameObject[] completeTicks;

	private void Awake()
	{
		this.stageName.text = string.Format("Stage {0} - {1}", GameData.currentStage.id, GameData.currentStage.difficulty).ToUpper();
		this.LoadQuestDescription();
	}

	private void OnEnable()
	{
		this.LoadQuestProgress();
	}

	public void LoadQuestProgress()
	{
	}

	private void LoadQuestDescription()
	{
	}
}
