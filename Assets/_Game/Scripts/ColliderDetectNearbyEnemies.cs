using System;
using UnityEngine;

public class ColliderDetectNearbyEnemies : MonoBehaviour
{
	private Rambo rambo;

	private void Awake()
	{
		this.rambo = base.transform.root.GetComponent<Rambo>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!other.isTrigger && other.transform.root.CompareTag("Enemy"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null && ((BaseEnemy)unit).isEffectMeleeWeapon)
			{
				this.rambo.OnEnemyEnterNearby(unit);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (!other.isTrigger && other.transform.root.CompareTag("Enemy"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null && ((BaseEnemy)unit).isEffectMeleeWeapon)
			{
				this.rambo.OnEnemyExitNearby(unit);
			}
		}
	}
}
