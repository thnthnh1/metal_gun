using System;
using System.Collections.Generic;
using UnityEngine;

public class BossProfessorEnergyPulse : MonoBehaviour
{
	public Transform hitEffect;

	public LayerMask collisionMask;

	public Transform castPoint;

	public Transform destinationPoint;

	public RaycastHit2D hit;

	public List<BaseUnit> pulseVictims = new List<BaseUnit>();

	private float timerApplyDamage;

	private BossProfessor boss;

	private void Awake()
	{
		this.boss = base.transform.root.gameObject.GetComponent<BossProfessor>();
	}

	private void LateUpdate()
	{
		this.hit = Physics2D.Linecast(this.castPoint.position, this.destinationPoint.position, this.collisionMask);
		Vector2 v;
		if (this.hit)
		{
			v = this.hit.point;
		}
		else
		{
			v = this.destinationPoint.position;
		}
		this.hitEffect.transform.position = v;
		this.ApplyDamage();
	}

	public void Active(bool isActive)
	{
		base.gameObject.SetActive(isActive);
	}

	private void ApplyDamage()
	{
		this.timerApplyDamage += Time.deltaTime;
		if (this.timerApplyDamage >= 0.3f)
		{
			this.timerApplyDamage = 0f;
			for (int i = 0; i < this.pulseVictims.Count; i++)
			{
				float energyPulseDamage = ((SO_BossProfessorStats)this.boss.baseStats).EnergyPulseDamage;
				AttackData attackData = new AttackData(this.boss, energyPulseDamage, 0f, false, WeaponType.NormalGun, -1, null);
				this.pulseVictims[i].TakeDamage(attackData);
			}
		}
	}
}
