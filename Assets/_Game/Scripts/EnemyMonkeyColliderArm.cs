using System;
using UnityEngine;

public class EnemyMonkeyColliderArm : MonoBehaviour
{
	private EnemyMonkey monkey;

	private void Awake()
	{
		this.monkey = base.transform.root.GetComponent<EnemyMonkey>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player") && this.monkey.IsAttacking)
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null)
			{
				AttackData curentAttackData = this.monkey.GetCurentAttackData();
				unit.TakeDamage(curentAttackData);
			}
		}
	}
}
