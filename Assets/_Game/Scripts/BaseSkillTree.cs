using System;
using UnityEngine;

public class BaseSkillTree : MonoBehaviour
{
	public BaseSkill activeSkill;

	public BaseSkill[] skills;

	public void Init(int ramboId)
	{
		this.activeSkill = null;
		this.SetSkillInfo(ramboId);
	}

	private void SetSkillInfo(int ramboId)
	{
		PlayerRamboSkillData ramboSkillProgress = GameData.playerRamboSkills.GetRamboSkillProgress(ramboId);
		if (ramboSkillProgress != null)
		{
			for (int i = 0; i < this.skills.Length; i++)
			{
				BaseSkill baseSkill = this.skills[i];
				StaticRamboSkillData data = GameData.staticRamboSkillData.GetData(baseSkill.id);
				baseSkill.level = ramboSkillProgress.GetSkillLevel(baseSkill.id);
				if (baseSkill.level > 0 && baseSkill.level <= data.values.Length)
				{
					baseSkill.value = data.values[baseSkill.level - 1];
				}
				else
				{
					baseSkill.value = -1f;
				}
				if (baseSkill.type == SkillType.Active && baseSkill.level > 0 && this.activeSkill == null)
				{
					this.activeSkill = baseSkill;
				}
			}
			if (this.activeSkill)
			{
				Singleton<UIController>.Instance.SetSkillIcon(this.activeSkill.id);
				Singleton<UIController>.Instance.imgSkillBackground.gameObject.SetActive(true);
				Singleton<UIController>.Instance.EnableSkill(true);
			}
			else
			{
				Singleton<UIController>.Instance.imgSkillBackground.gameObject.SetActive(false);
			}
		}
		else
		{
			Singleton<UIController>.Instance.imgSkillBackground.gameObject.SetActive(false);
		}
	}
}
