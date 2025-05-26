using System;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
	public bool isActive;

	public AudioClip soundFire;

	public Transform fireEffect;

	private AudioSource aud;

	private EnemyFire owner;

	private BoxCollider2D col;

	private List<BaseUnit> victims = new List<BaseUnit>();

	private float timeApplyDamage = 0.3f;

	private float slowPercent = 0.1f;

	private float timeOut;

	private float lastTimeDealDamage;

	private void Awake()
	{
		this.aud = base.GetComponent<AudioSource>();
		this.aud.loop = true;
		this.aud.clip = this.soundFire;
		this.owner = base.transform.root.GetComponent<EnemyFire>();
		this.col = base.GetComponent<BoxCollider2D>();
		this.timeApplyDamage = ((SO_EnemyFireStats)this.owner.baseStats).TimeApplyDamage;
		this.slowPercent = Mathf.Clamp01(((SO_EnemyFireStats)this.owner.baseStats).SlowPercent / 100f);
	}

	private void Update()
	{
		if (this.isActive)
		{
			float time = Time.time;
			float num = time - this.lastTimeDealDamage;
			if (num > this.timeApplyDamage)
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

	public void Active()
	{
		base.gameObject.SetActive(true);
		this.timeOut = 0f;
		this.isActive = true;
		this.aud.Play();
	}

	public void Deactive()
	{
		this.AdjustSlow(false);
		this.victims.Clear();
		this.isActive = false;
		this.aud.Stop();
		base.gameObject.SetActive(false);
	}

	private void DealDamage()
	{
		for (int i = 0; i < this.victims.Count; i++)
		{
			AttackData attackData = new AttackData(this.owner, this.owner.baseStats.Damage, 0f, false, WeaponType.NormalGun, -1, null);
			this.victims[i].TakeDamage(attackData);
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
