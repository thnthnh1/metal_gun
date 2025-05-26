using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPreviewFireball : BaseBulletPreview
{
	public float timeApplyDamage;

	public float distance;

	private Vector2 startPoint;

	private float timerDamage;

	private List<GameObject> victims = new List<GameObject>();

	protected override void Move()
	{
		if (Vector2.Distance(base.transform.position, this.startPoint) >= this.distance)
		{
			this.Deactive();
			return;
		}
		base.Move();
		if (base.transform.localScale.x <= 2f)
		{
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, Vector3.one * 2f, 2f * Time.deltaTime);
		}
	}

	private void LateUpdate()
	{
		this.ApplyDamage();
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy") && !this.victims.Contains(other.transform.root.gameObject))
		{
			this.victims.Add(other.transform.root.gameObject);
		}
	}

	protected void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Enemy") && this.victims.Contains(other.transform.root.gameObject))
		{
			this.victims.Remove(other.transform.root.gameObject);
		}
	}

	public override void Active(Transform firePoint, float moveSpeed, Transform parent = null)
	{
		base.Active(firePoint, moveSpeed, parent);
		this.startPoint = base.transform.position;
		base.transform.localScale = Vector3.one;
		this.timerDamage = 0f;
		this.victims.Clear();
	}

	private void ApplyDamage()
	{
		if (this.victims.Count <= 0)
		{
			return;
		}
		this.timerDamage += Time.deltaTime;
		if (this.timerDamage >= this.timeApplyDamage)
		{
			this.timerDamage = 0f;
			EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
		}
	}

	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.fireball.Store(this);
	}
}
