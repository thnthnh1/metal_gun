using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActionIngame : MonoBehaviour
{
	public Button button;

	public Sprite sprDisable;

	public Sprite sprNormal;

	public float sizeNormal;

	public float sizePress;

	private bool isDisabled;

	public void Normal()
	{
		this.SetSizeToNormal();
		if (this.isDisabled)
		{
			this.SetAlpha(0.4f);
		}
		else
		{
			this.button.image.sprite = this.sprNormal;
			this.SetAlpha(0.6f);
		}
	}

	public void Press()
	{
		this.SetSizeToPress();
		this.SetAlpha(1f);
	}

	public void Enable()
	{
		this.isDisabled = false;
		this.button.enabled = true;
		this.button.image.raycastTarget = true;
		this.Normal();
	}

	public void Disable()
	{
		this.isDisabled = true;
		this.button.enabled = false;
		this.button.image.raycastTarget = false;
		if (this.sprDisable)
		{
			this.button.image.sprite = this.sprDisable;
		}
		this.SetSizeToNormal();
		this.SetAlpha(0.4f);
	}

	private void SetSizeToNormal()
	{
		Vector2 sizeDelta = this.button.image.rectTransform.sizeDelta;
		sizeDelta.x = this.sizeNormal;
		sizeDelta.y = this.sizeNormal;
		this.button.image.rectTransform.sizeDelta = sizeDelta;
	}

	private void SetSizeToPress()
	{
		Vector2 sizeDelta = this.button.image.rectTransform.sizeDelta;
		sizeDelta.x = this.sizePress;
		sizeDelta.y = this.sizePress;
		this.button.image.rectTransform.sizeDelta = sizeDelta;
	}

	private void SetAlpha(float a)
	{
		Color color = this.button.image.color;
		color.a = a;
		this.button.image.color = color;
	}
}
