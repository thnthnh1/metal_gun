using System;
using UnityEngine;

public class BossProfessorColliderPulse : MonoBehaviour
{
	public BossProfessorEnergyPulse pulse;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit && !this.pulse.pulseVictims.Contains(unit))
			{
				this.pulse.pulseVictims.Add(unit);
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit && this.pulse.pulseVictims.Contains(unit))
			{
				this.pulse.pulseVictims.Remove(unit);
			}
		}
	}
}
