using System;
using UnityEngine;

public class LaserByGun : BaseBullet
{
	public LineRenderer laserRender;

	public LineRenderer laserNoise;

	public Transform hitEffect;

	public LayerMask victimLayerMask;

	public LayerMask stopLayerMask;

	public float laserRange = 10f;

	public int noiseCount = 10;

	public float noiseWidth = 0.02f;

	public float noiseRandomOffset = 0.12f;

	[HideInInspector]
	public GunLaser gun;

	public AudioClip soundLaser;

	protected float timerApplyDamage;

	protected Vector3 hitPoint;

	protected RaycastHit2D[] victims = new RaycastHit2D[3];

	private AudioSource aud;

	private RaycastHit2D hit;

	protected override void Awake()
	{
		base.Awake();
		this.aud = base.GetComponent<AudioSource>();
		this.aud.loop = true;
		this.aud.clip = this.soundLaser;
	}

	private void Start()
	{
		this.laserNoise.positionCount = this.noiseCount + 1;
		this.laserNoise.startWidth = this.noiseWidth;
		this.laserNoise.endWidth = this.noiseWidth;
	}

	private void LateUpdate()
	{
		if (this.laserRender != null)
		{
			this.hit = Physics2D.Linecast(base.transform.position, base.transform.position + base.transform.right * this.laserRange, this.stopLayerMask);
			float d;
			if (this.hit)
			{
				this.laserRender.SetPosition(0, base.transform.position);
				this.hitPoint = this.hit.point;
				this.laserRender.SetPosition(1, this.hitPoint);
				d = this.hit.distance / (float)this.noiseCount;
			}
			else
			{
				this.laserRender.SetPosition(0, base.transform.position);
				this.hitPoint = base.transform.position + base.transform.right * this.laserRange;
				this.laserRender.SetPosition(1, this.hitPoint);
				d = this.laserRange / (float)this.noiseCount;
			}
			this.hitEffect.transform.position = this.hitPoint;
			this.laserNoise.SetPosition(0, base.transform.position);
			this.laserNoise.SetPosition(10, this.hitPoint);
			for (int i = 1; i < 10; i++)
			{
				Vector3 position = base.transform.position + base.transform.right * (float)i * d + base.transform.up * UnityEngine.Random.Range(-this.noiseRandomOffset, this.noiseRandomOffset);
				this.laserNoise.SetPosition(i, position);
			}
		}
		this.ApplyDamage();
	}

	public void Active(bool isActive)
	{
		this.timerApplyDamage = 0f;
		base.gameObject.SetActive(isActive);
		if (isActive)
		{
			this.aud.Play();
		}
		else
		{
			this.aud.Stop();
		}
	}

	private void ApplyDamage()
	{
		this.timerApplyDamage += Time.deltaTime;
		if (this.timerApplyDamage >= ((SO_GunLaserStats)this.gun.baseStats).TimeApplyDamage)
		{
			this.timerApplyDamage = 0f;
			int num = Physics2D.LinecastNonAlloc(base.transform.position, this.hitPoint, this.victims, this.victimLayerMask);
			for (int i = 0; i < num; i++)
			{
				BaseUnit unit = Singleton<GameController>.Instance.GetUnit(this.victims[i].transform.root.gameObject);
				if (unit != null && !unit.IsOutOfScreen())
				{
					AttackData gunAttackData = ((Rambo)this.gun.shooter).GetGunAttackData();
					unit.TakeDamage(gunAttackData);
				}
			}
			if (num > 0)
			{
				this.gun.ConsumeAmmo(1);
			}
		}
	}
}
