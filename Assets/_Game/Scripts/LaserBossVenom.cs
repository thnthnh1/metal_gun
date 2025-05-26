using System;
using UnityEngine;

public class LaserBossVenom : MonoBehaviour
{
	public LineRenderer laserRender;

	public LineRenderer laserNoise;

	public Transform hitEffect;

	public LayerMask collisionMask;

	public float laserRange = 10f;

	public int noiseCount = 10;

	public float noiseWidth = 0.02f;

	public float noiseRandomOffset = 0.12f;

	public RaycastHit2D hit;

	private bool flagFirstHitGround;

	private BossVenom boss;

	private float timerApplyDamage;

	private void OnDisable()
	{
		this.flagFirstHitGround = false;
	}

	private void Start()
	{
		this.laserNoise.positionCount = this.noiseCount + 1;
		this.laserNoise.startWidth = this.noiseWidth;
		this.laserNoise.endWidth = this.noiseWidth;
		this.boss = base.transform.root.GetComponent<BossVenom>();
	}

	private void LateUpdate()
	{
		if (this.laserRender != null)
		{
			this.hit = Physics2D.Linecast(base.transform.position, base.transform.position + base.transform.right * this.laserRange, this.collisionMask);
			Vector3 position;
			float d;
			if (this.hit)
			{
				this.laserRender.SetPosition(0, base.transform.position);
				position = this.hit.point;
				this.laserRender.SetPosition(1, position);
				d = this.hit.distance / (float)this.noiseCount;
				if (!this.flagFirstHitGround && this.hit.collider.gameObject.layer == StaticValue.LAYER_GROUND)
				{
					this.flagFirstHitGround = true;
					EventDispatcher.Instance.PostEvent(EventID.LaserPoisonHitGround, this.hit.point);
				}
			}
			else
			{
				this.laserRender.SetPosition(0, base.transform.position);
				position = base.transform.position + base.transform.right * this.laserRange;
				this.laserRender.SetPosition(1, position);
				d = this.laserRange / (float)this.noiseCount;
			}
			this.hitEffect.transform.position = position;
			this.laserNoise.SetPosition(0, base.transform.position);
			this.laserNoise.SetPosition(10, position);
			for (int i = 1; i < 10; i++)
			{
				Vector3 position2 = base.transform.position + base.transform.right * (float)i * d + base.transform.up * UnityEngine.Random.Range(-this.noiseRandomOffset, this.noiseRandomOffset);
				this.laserNoise.SetPosition(i, position2);
			}
			this.ApplyDamage();
		}
	}

	private void ApplyDamage()
	{
		this.timerApplyDamage += Time.deltaTime;
		if (this.timerApplyDamage >= 0.3f)
		{
			this.timerApplyDamage = 0f;
			if (this.hit && this.hit.collider.transform.root.CompareTag("Player"))
			{
				float damage = (this.boss.HpPercent <= 0.5f) ? ((SO_BossVenomStats)this.boss.baseStats).RageLaserDamage : ((SO_BossVenomStats)this.boss.baseStats).LaserDamage;
				AttackData attackData = new AttackData(this.boss, damage, 0f, false, WeaponType.NormalGun, -1, null);
				Singleton<GameController>.Instance.Player.TakeDamage(attackData);
			}
		}
	}
}
