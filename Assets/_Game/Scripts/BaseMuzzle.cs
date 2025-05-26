using System;
using UnityEngine;

public class BaseMuzzle : MonoBehaviour
{
	public bool isDeactiveByTime = true;

	private float timeDeactive = 0.1f;

	private SpriteRenderer sprite;

	private float timer;

	private bool isActive;

	protected virtual void Awake()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
	}

	protected virtual void Update()
	{
		if (this.isActive && this.isDeactiveByTime)
		{
			this.timer += Time.deltaTime;
			if (this.timer >= this.timeDeactive)
			{
				this.timer = 0f;
				this.Deactive();
			}
		}
	}

	public virtual void Active()
	{
		this.timer = 0f;
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<SpriteRenderer>();
		}
		if (this.sprite)
		{
			this.sprite.enabled = true;
		}
		this.isActive = true;
		base.gameObject.SetActive(true);
	}

	public virtual void Deactive()
	{
		this.timer = 0f;
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<SpriteRenderer>();
		}
		if (this.sprite)
		{
			this.sprite.enabled = false;
		}
		this.isActive = false;
		base.gameObject.SetActive(false);
	}
}
