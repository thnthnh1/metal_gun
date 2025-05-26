using System;
using UnityEngine;
using UnityEngine.UI;

public class HorizontalScrollText : MonoBehaviour
{
	public Text textContent;

	public Vector2 mostRightPoint;

	public float viewSize;

	private Vector2 initialPoint;

	private Vector2 mostLeftPoint;

	[SerializeField]
	private bool isScrollable;

	[SerializeField]
	private bool flagScroll;

	private void Awake()
	{
		this.textContent = base.GetComponent<Text>();
		this.initialPoint = this.textContent.rectTransform.anchoredPosition;
	}

	private void Update()
	{
		if (this.isScrollable && this.flagScroll)
		{
			this.textContent.rectTransform.anchoredPosition = Vector2.MoveTowards(this.textContent.rectTransform.anchoredPosition, this.mostLeftPoint, Time.deltaTime * 60f);
			if (this.textContent.rectTransform.anchoredPosition == this.mostLeftPoint)
			{
				this.textContent.rectTransform.anchoredPosition = this.mostRightPoint;
			}
		}
	}

	public void Active(string content)
	{
		this.flagScroll = false;
		this.textContent.text = content;
		this.isScrollable = (this.textContent.preferredWidth > this.viewSize);
		if (this.isScrollable)
		{
			this.mostLeftPoint.x = this.mostRightPoint.x - this.viewSize - this.textContent.preferredWidth;
			this.mostLeftPoint.y = this.textContent.rectTransform.anchoredPosition.y;
			base.enabled = true;
			base.Invoke("ActiveFlagScroll", 1.5f);
		}
	}

	public void Deactive()
	{
		this.flagScroll = false;
		this.textContent.text = string.Empty;
		if (this.isScrollable)
		{
			this.textContent.rectTransform.anchoredPosition = this.initialPoint;
			this.isScrollable = false;
			base.enabled = false;
		}
	}

	private void ActiveFlagScroll()
	{
		this.flagScroll = true;
	}
}
