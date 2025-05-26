using System;
using UnityEngine;

public class FarSensor : MonoBehaviour
{
	public CircleCollider2D col;

	private BaseEnemy owner;

	private void Awake()
	{
		this.owner = base.transform.root.GetComponent<BaseEnemy>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (this.owner.isDead)
		{
			return;
		}
		if (other.transform.root.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null)
			{
				this.owner.OnUnitGetInFarSensor(unit);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (this.owner.isDead)
		{
			return;
		}
		if (other.transform.root.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null)
			{
				this.owner.OnUnitGetOutFarSensor(unit);
			}
		}
	}
}
