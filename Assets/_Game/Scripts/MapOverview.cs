using System;
using UnityEngine;

public class MapOverview : MonoBehaviour
{
	public StageButton[] stageButtons;

	public void Init()
	{
		this.Load();
	}

	public void Active(bool isActive)
	{
		base.gameObject.SetActive(isActive);
	}

	private void Load()
	{
		for (int i = 0; i < this.stageButtons.Length; i++)
		{
			StageButton stageButton = this.stageButtons[i];
			stageButton.Load();
		}
	}
}
