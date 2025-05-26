using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectDifficulty : MonoBehaviour
{
	public Difficulty difficulty;

	public Image icon;

	public Image label;

	public Sprite sprIconNormal;

	public Sprite sprIconHighlight;

	public Sprite sprIconLock;

	public Sprite sprLabelNormal;

	public Sprite sprLabelHighlight;

	public Sprite sprLabelLock;

	public bool isLock;

	public void Highlight(bool isHighlight)
	{
		this.icon.sprite = ((!isHighlight) ? this.sprIconNormal : this.sprIconHighlight);
		this.icon.SetNativeSize();
		this.label.sprite = ((!isHighlight) ? this.sprLabelNormal : this.sprLabelHighlight);
		this.label.SetNativeSize();
	}

	public void Lock()
	{
		this.isLock = true;
		this.icon.sprite = this.sprIconLock;
		this.icon.SetNativeSize();
		this.label.sprite = this.sprLabelLock;
	}

	public void Select()
	{
		SoundManager.Instance.PlaySfxClick();
		if (this.isLock)
		{
			Singleton<Popup>.Instance.ShowToastMessage("Complete previous difficulty to unlock", ToastLength.Normal);
			return;
		}
		EventDispatcher.Instance.PostEvent(EventID.SelectDifficulty, this.difficulty);
	}
}
