using System;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : BaseBullet
{
	private float distance;

	private Vector2 startPoint;

	private float timeApplyDamage;

	private float timerDamage;

	private List<BaseUnit> victims = new List<BaseUnit>();

	protected override void Update()
	{
		base.Update();
		if (base.transform.localScale.x <= 2f)
		{
			base.transform.localScale = Vector3.MoveTowards(base.transform.localScale, Vector3.one * 2f, 2f * Time.deltaTime);
		}
	}

	private void LateUpdate()
	{
		this.ApplyDamage();
	}

	protected override void TrackingDeactive()
	{
		if (Vector2.Distance(base.transform.position, this.startPoint) >= this.distance)
		{
			this.Deactive();
			return;
		}
		if (this.isDeactiveWhenOutScreen)
		{
			bool flag = base.transform.position.x < Singleton<CameraFollow>.Instance.left.position.x - 1f || base.transform.position.x > Singleton<CameraFollow>.Instance.right.position.x + 1f;
			bool flag2 = base.transform.position.y > Singleton<CameraFollow>.Instance.top.position.y + 1f;
			if (flag || flag2)
			{
				this.Deactive();
			}
		}
	}

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		BaseUnit baseUnit = null;
		if (other.CompareTag("Enemy Body Part") || other.CompareTag("Destructible Obstacle"))
		{
			baseUnit = Singleton<GameController>.Instance.GetUnit(other.gameObject);
		}
		else if (other.transform.root.CompareTag("Enemy"))
		{
			baseUnit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
		}
		if (baseUnit != null && !this.victims.Contains(baseUnit))
		{
			this.victims.Add(baseUnit);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		BaseUnit baseUnit = null;
		if (other.CompareTag("Enemy Body Part") || other.CompareTag("Destructible Obstacle"))
		{
			baseUnit = Singleton<GameController>.Instance.GetUnit(other.gameObject);
		}
		else if (other.transform.root.CompareTag("Enemy"))
		{
			baseUnit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
		}
		if (baseUnit != null && this.victims.Contains(baseUnit))
		{
			this.victims.Remove(baseUnit);
		}
	}

	public void Active(AttackData attackData, Transform releasePoint, float moveSpeed, float timeApplyDamage, float distance, Transform parent = null)
	{
		this.attackData = attackData;
		this.moveSpeed = moveSpeed;
		this.timeApplyDamage = timeApplyDamage;
		this.distance = distance;
		this.SetTagAndLayer();
		base.transform.position = releasePoint.position;
		base.transform.rotation = releasePoint.rotation;
		base.transform.parent = parent;
		this.startPoint = base.transform.position;
		base.transform.localScale = Vector3.one;
		this.timerDamage = 0f;
		distance = 0f;
		this.victims.Clear();
		base.gameObject.SetActive(true);
	}

	private void ApplyDamage()
	{
		this.timerDamage += Time.deltaTime;
		if (this.timerDamage >= this.timeApplyDamage)
		{
			this.timerDamage = 0f;
			for (int i = 0; i < this.victims.Count; i++)
			{
				this.victims[i].TakeDamage(this.attackData);
			}
		}
	}
}
