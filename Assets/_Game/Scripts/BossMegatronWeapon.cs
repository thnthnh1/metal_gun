using System;
using UnityEngine;

public class BossMegatronWeapon : MonoBehaviour
{
	private BossMegatron boss;

	private void Awake()
	{
		this.boss = base.transform.root.GetComponent<BossMegatron>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player") && this.boss.IsSmashing)
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null)
			{
				AttackData attackData = new AttackData(this.boss, ((SO_BossMegatronStats)this.boss.baseStats).SmashDamage, 0f, false, WeaponType.NormalGun, -1, null);
				unit.TakeDamage(attackData);
			}
		}
	}
}
