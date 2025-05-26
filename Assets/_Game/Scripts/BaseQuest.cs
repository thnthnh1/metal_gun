using System;
using UnityEngine;

public class BaseQuest : MonoBehaviour
{
	protected bool isCompleted;

	protected string keyDescription;

	protected string description;

	public virtual void Init()
	{
		if (GameData.questDescriptions.ContainsKey(this.keyDescription))
		{
			this.description = GameData.questDescriptions[this.keyDescription];
		}
		else
		{
			this.description = string.Empty;
		}
	}

	public void SetComplete(bool isCompleted)
	{
		this.isCompleted = isCompleted;
	}

	public virtual string GetDescription()
	{
		return this.description;
	}

	public virtual string GetCurrentProgress()
	{
		return string.Empty;
	}

	public virtual bool IsCompleted()
	{
		return this.isCompleted;
	}
}
