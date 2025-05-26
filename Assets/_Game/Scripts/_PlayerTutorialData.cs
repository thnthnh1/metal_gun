using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class _PlayerTutorialData : Dictionary<TutorialType, bool>
{
	public void Save()
	{
		string value = JsonConvert.SerializeObject(this);
		ProfileManager.UserProfile.playerTutorialData.Set(value);
		ProfileManager.SaveAll();
	}

	public bool IsCompletedStep(TutorialType type)
	{
		if (base.ContainsKey(type))
		{
			return base[type];
		}
		base.Add(type, false);
		return false;
	}

	public void SetComplete(TutorialType type)
	{
		if (base.ContainsKey(type))
		{
			base[type] = true;
		}
		else
		{
			base.Add(type, true);
		}
		this.Save();
	}

	public void SkipTutorial(TutorialType type)
	{
		this.SetComplete(type);
		this.Save();
	}
}
