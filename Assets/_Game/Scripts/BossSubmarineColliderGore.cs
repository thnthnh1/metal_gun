using System;
using UnityEngine;

public class BossSubmarineColliderGore : MonoBehaviour
{
	private BossSubmarine boss;

	private void Awake()
	{
		this.boss = base.transform.root.GetComponent<BossSubmarine>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			BaseUnit component = other.transform.root.GetComponent<BaseUnit>();
			if (component != null)
			{
				float damage = (this.boss.HpPercent <= 0.5f) ? ((SO_BossSubmarineStats)this.boss.baseStats).RageGoreDamage : ((SO_BossSubmarineStats)this.boss.baseStats).GoreDamage;
				AttackData attackData = new AttackData(this.boss, damage, 0f, false, WeaponType.NormalGun, -1, null);
				component.TakeDamage(attackData);
				component.AddForce(component.transform.right, 6f, ForceMode2D.Impulse);
				Singleton<CameraFollow>.Instance.AddShake(0.3f, 0.5f);
				base.gameObject.SetActive(false);
			}
		}
	}
}
