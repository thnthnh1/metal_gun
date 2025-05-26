using System;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTrap : MonoBehaviour
{
	[HideInInspector]
	public bool isActive;

	private BoxCollider2D col;

	private List<BaseUnit> victims = new List<BaseUnit>();

	public float lifeTime = 5f;

	public float damage = 5f;

	public float timeApplyDamage = 1f;

	public float slowPercent = 0.5f;

	private float timeOut;

	private float lastTimeDealDamage;

	private void Awake()
	{
		this.col = base.GetComponent<BoxCollider2D>();
	}

	private void Update()
	{
		if (this.isActive)
		{
			this.timeOut += Time.deltaTime;
			if (this.timeOut >= this.lifeTime)
			{
				this.Deactive();
				return;
			}
			float time = Time.time;
			if (time - this.lastTimeDealDamage > this.timeApplyDamage)
			{
				this.lastTimeDealDamage = time;
				this.DealDamage();
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
		if (unit != null && unit.CompareTag("Player"))
		{
			if (!this.victims.Contains(unit))
			{
				this.victims.Add(unit);
			}
			this.AdjustSlow(true);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
		if (unit != null && unit.CompareTag("Player") && this.victims.Contains(unit))
		{
			this.AdjustSlow(false);
			this.victims.Remove(unit);
		}
	}

	public void Active(Vector2 position)
	{
		base.transform.position = position;
		this.timeOut = 0f;
		this.isActive = true;
		base.gameObject.SetActive(true);
	}

	public void Deactive()
	{
		this.AdjustSlow(false);
		this.victims.Clear();
		this.isActive = false;
		base.gameObject.SetActive(false);
		Singleton<PoolingController>.Instance.poolPoisonTrap.Store(this);
	}

	private void DealDamage()
	{
		for (int i = 0; i < this.victims.Count; i++)
		{
			this.victims[i].TakeDamage(this.damage);
		}
	}

	private void AdjustSlow(bool isSlow)
	{
		for (int i = 0; i < this.victims.Count; i++)
		{
			if (isSlow)
			{
				this.victims[i].AddModifier(new ModifierData(StatsType.MoveSpeed, ModifierType.AddPercentBase, -this.slowPercent));
			}
			else
			{
				this.victims[i].RemoveModifier(new ModifierData(StatsType.MoveSpeed, ModifierType.AddPercentBase, -this.slowPercent));
			}
			this.victims[i].ReloadStats();
		}
	}
}
