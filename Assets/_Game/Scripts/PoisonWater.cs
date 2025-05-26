using System;
using System.Collections.Generic;
using UnityEngine;

public class PoisonWater : MonoBehaviour
{
	public float damagePerSecond = 20f;

	public float timeApplyDamage = 0.5f;

	public float slowPercent = 0.5f;

	private BoxCollider2D col;

	private List<BaseUnit> victims = new List<BaseUnit>();

	private float lastTimeDealDamage;

	private void Awake()
	{
		this.col = base.GetComponent<BoxCollider2D>();
		EventDispatcher.Instance.RegisterListener(EventID.UnitDie, new Action<Component, object>(this.OnUnitDie));
	}

	private void Update()
	{
		if (this.victims.Count <= 0)
		{
			return;
		}
		float time = Time.time;
		if (time - this.lastTimeDealDamage > this.timeApplyDamage)
		{
			this.lastTimeDealDamage = time;
			this.DealDamage();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player") || other.transform.root.CompareTag("Enemy"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null && !unit.isDead && !unit.isOnVehicle)
			{
				if (!this.victims.Contains(unit))
				{
					this.victims.Add(unit);
				}
				SoundManager.Instance.PlaySfx("sfx_trigger_water", 0f);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player") || other.transform.root.CompareTag("Enemy"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null && this.victims.Contains(unit))
			{
				this.victims.Remove(unit);
			}
		}
	}

	private void DealDamage()
	{
		for (int i = 0; i < this.victims.Count; i++)
		{
			this.victims[i].TakeDamage(this.damagePerSecond);
		}
	}

	private void OnUnitDie(Component senser, object param)
	{
		UnitDieData unitDieData = (UnitDieData)param;
		if (unitDieData.unit != null && this.victims.Contains(unitDieData.unit))
		{
			this.victims.Remove(unitDieData.unit);
		}
	}
}
