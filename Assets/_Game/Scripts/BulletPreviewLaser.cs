using System;
using UnityEngine;

public class BulletPreviewLaser : BaseBulletPreview
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
	public GunPreviewLaser gun;

	protected float timerApplyDamage;

	protected Vector3 hitPoint;

	protected RaycastHit2D[] victims = new RaycastHit2D[20];

	private RaycastHit2D hit;

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
		base.gameObject.SetActive(isActive);
	}

	private void ApplyDamage()
	{
		this.timerApplyDamage += Time.deltaTime;
		if (this.timerApplyDamage >= ((SO_GunLaserStats)this.gun.baseStats).TimeApplyDamage)
		{
			this.timerApplyDamage = 0f;
			RaycastHit2D raycastHit2D = Physics2D.Linecast(base.transform.position, this.hitPoint, this.victimLayerMask);
			if (raycastHit2D)
			{
				EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
			}
		}
	}
}
