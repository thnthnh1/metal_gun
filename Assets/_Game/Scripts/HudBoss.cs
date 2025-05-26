using System;
using UnityEngine;
using UnityEngine.UI;

public class HudBoss : MonoBehaviour
{
	public WarningBoss warning;

	public Image hpBoss;

	public Image iconBoss;

	public GameObject infoBossMegatron;

	public Sprite[] icons;

	public void Init()
	{
		EventDispatcher.Instance.RegisterListener(EventID.ShowInfoBossMegatron, delegate(Component sender, object param)
		{
			this.infoBossMegatron.SetActive(true);
		});
	}

	public void HideUI()
	{
		this.hpBoss.transform.parent.gameObject.SetActive(false);
	}

	public void UpdateHP(float percent)
	{
		this.hpBoss.fillAmount = percent;
		this.hpBoss.transform.parent.gameObject.SetActive(true);
	}

	public void SetIconBoss(int bossId)
	{
		for (int i = 0; i < this.icons.Length; i++)
		{
			if (int.Parse(this.icons[i].name) == bossId)
			{
				this.iconBoss.sprite = this.icons[i];
				this.iconBoss.SetNativeSize();
				return;
			}
		}
	}

	public void WarningBoss()
	{
		this.warning.Active();
	}

	public void StartFightingBoss()
	{
		this.infoBossMegatron.SetActive(false);
		EventDispatcher.Instance.PostEvent(EventID.ShowInfoBossDone);
	}
}
