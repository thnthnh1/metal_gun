using System;
using UnityEngine;

public class BaseItemDrop : MonoBehaviour
{
	protected Rigidbody2D rigid;

	protected ItemDropData data;

	protected Collider2D col;

	protected SpriteRenderer spr;

	protected virtual void Awake()
	{
		this.spr = base.GetComponent<SpriteRenderer>();
		this.rigid = base.GetComponent<Rigidbody2D>();
		this.col = base.GetComponent<Collider2D>();
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			EventDispatcher.Instance.PostEvent(EventID.GetItemDrop, this.data);
			this.Deactive();
		}
	}

	public virtual void Active(ItemDropData data, Vector2 position)
	{
		this.data = data;
		base.transform.position = position;
		this.col.enabled = true;
		base.gameObject.SetActive(true);
	}

	public virtual void Deactive()
	{
		this.col.enabled = false;
		base.gameObject.SetActive(false);
	}
}
