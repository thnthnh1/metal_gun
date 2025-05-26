using System;
using UnityEngine;

public class BossMegatankColliderWheel : MonoBehaviour
{
	public AudioClip soundHit;

	private BossMegatank boss;

	private void Awake()
	{
		this.boss = base.transform.root.GetComponent<BossMegatank>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root.CompareTag("Player"))
		{
			BaseUnit unit = Singleton<GameController>.Instance.GetUnit(other.transform.root.gameObject);
			if (unit != null)
			{
				float damage = (this.boss.HpPercent <= 0.5f) ? ((SO_BossMegatankStats)this.boss.baseStats).RageGoreDamage : ((SO_BossMegatankStats)this.boss.baseStats).GoreDamage;
				AttackData attackData = new AttackData(this.boss, damage, 0f, false, WeaponType.NormalGun, -1, null);
				unit.TakeDamage(attackData);
				if (!unit.isDead)
				{
					unit.FallBackward(1.5f);
				}
			}
			SoundManager.Instance.PlaySfx(this.soundHit, 0f);
			Singleton<CameraFollow>.Instance.AddShake(0.3f, 0.5f);
			base.gameObject.SetActive(false);
		}
	}
}
