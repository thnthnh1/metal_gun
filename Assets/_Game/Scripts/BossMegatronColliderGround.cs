using System;
using UnityEngine;

public class BossMegatronColliderGround : MonoBehaviour
{
	private BossMegatron boss;

	private void Awake()
	{
		this.boss = base.transform.root.GetComponent<BossMegatron>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null)
			{
				AttackData attackData = new AttackData(this.boss, ((SO_BossMegatronStats)this.boss.baseStats).JumpDamage, 0f, false, WeaponType.NormalGun, -1, null);
				unit.TakeDamage(attackData);
				if (!unit.isDead)
				{
					unit.FallBackward(1.5f);
				}
			}
			base.gameObject.SetActive(false);
		}
	}
}
