using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextDamage : BaseEffect
{
	public TMP_Text textDamage;

	public float sizeNormalDamage = 3.5f;

	public Color32 colorNormalDamage;

	public float sizeCritDamage = 7f;

	public Color32 colorCriticalDamage;

	public float sizeGrenadeDamage = 7f;

	public Color32 colorGrenadeDamage;

	private bool isInGame = true;

	public override void Deactive()
	{
		base.Deactive(); 
        this.textDamage.text = string.Empty;
		if (this.isInGame)
		{
			Singleton<PoolingController>.Instance.poolTextDamageTMP.Store(this);
			base.transform.SetParent(Singleton<PoolingController>.Instance.groupEffect, true);
		}
		else
		{
			Singleton<PoolingPreviewController>.Instance.textDamage.Store(this);
			base.transform.SetParent(Singleton<PoolingPreviewController>.Instance.group, true);
		}
	}

	public void Active(Vector2 position, AttackData attackData, Transform parent = null, bool isInGame = true)
	{
		this.isInGame = isInGame;
		base.transform.SetParent(parent, true);
		base.transform.position = position;
		if (attackData.weapon == WeaponType.Grenade)
		{
		    this.textDamage.color = this.colorGrenadeDamage;
			this.textDamage.fontSize = this.sizeGrenadeDamage;
			// this.textDamage.fontSize = (int)this.sizeGrenadeDamage;
		}
		else if (attackData.isCritical)
		{
			this.textDamage.color = this.colorCriticalDamage;
			this.textDamage.fontSize = this.sizeCritDamage;
			// this.textDamage.fontSize = (int)this.sizeCritDamage;
		}
		else
		{
			this.textDamage.color = this.colorNormalDamage;
			this.textDamage.fontSize = this.sizeNormalDamage;
			// this.textDamage.fontSize = (int)this.sizeNormalDamage;
		}
		float num = UnityEngine.Random.Range(0.85f, 1.15f);
		int num2 = Mathf.RoundToInt(attackData.damage * 10f * num);
		this.textDamage.text = num2.ToString();
		base.gameObject.SetActive(true);
	}

	// public void Active(Vector2 position, string content, Color32 color, float fontSize = 3.5f, Transform parent = null, bool isInGame = true)
	public void Active(Vector2 position, string content, Color32 color, float fontSize = 0.5f, Transform parent = null, bool isInGame = true)
	{
		fontSize = .3f;
		// Debug.Log("Enter Red " + content);
		this.isInGame = isInGame;
		base.transform.SetParent(parent, true);
		base.transform.position = position;
		this.textDamage.fontSize = fontSize;
		// this.textDamage.fontSize = (int)fontSize;
		this.textDamage.text = content;
		this.textDamage.color = color;
		base.gameObject.SetActive(true);
		this.textDamage.rectTransform.localScale = Vector3.one;
	}
}
