using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
	public List<bool> result;

	public BaseQuest[] listQuest;

	public void Init()
	{
		this.result = new List<bool>
		{
			false,
			false,
			false
		};
		for (int i = 0; i < this.listQuest.Length; i++)
		{
			this.listQuest[i].Init();
		}
	}

	public string GetDescription(int index)
	{
		if (index >= this.listQuest.Length)
		{
			return string.Empty;
		}
		return this.listQuest[index].GetDescription();
	}

	public string GetProgress(int index)
	{
		if (index >= this.listQuest.Length)
		{
			return string.Empty;
		}
		return this.listQuest[index].GetCurrentProgress();
	}

	public bool IsAlreadyCompleted(int index)
	{
		return index < this.listQuest.Length && this.listQuest[index].IsCompleted();
	}

	public void LoadQuestProgressWhenWin()
	{
		this.result[0] = true;
		this.result[1] = this.listQuest[1].IsCompleted();
		this.result[2] = this.listQuest[2].IsCompleted();
	}

	public int GetStar()
	{
		int num = 0;
		for (int i = 0; i < this.result.Count; i++)
		{
			if (this.result[i])
			{
				num++;
			}
		}
		return num;
	}
}
