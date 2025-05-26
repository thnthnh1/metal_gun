using System;
using UnityEngine;
using UnityEngine.UI;

public class QuestTab : MonoBehaviour
{
	public Image icon;

	public Image label;

	public Image bgButton;

	public Text notiCount;

	public Sprite sprIconEnable;

	public Sprite sprIconDisable;

	public Sprite sprLabelEnable;

	public Sprite sprLabelDisable;

	public Sprite sprBgButtonEnable;

	public Sprite sprBgButtonDisable;

	public void Highlight(bool isHighlight)
	{
		this.icon.sprite = ((!isHighlight) ? this.sprIconDisable : this.sprIconEnable);
		this.label.sprite = ((!isHighlight) ? this.sprLabelDisable : this.sprLabelEnable);
		this.bgButton.sprite = ((!isHighlight) ? this.sprBgButtonDisable : this.sprBgButtonEnable);
	}
}
