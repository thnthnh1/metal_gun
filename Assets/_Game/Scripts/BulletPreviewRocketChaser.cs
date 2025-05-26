using DG.Tweening;
using System;
using UnityEngine;

public class BulletPreviewRocketChaser : BaseBulletPreview
{
	public float turnSpeed = 10f;

	private Transform target;

	private Vector2 readyPosition;

	private bool isReady;

	private bool isPreparing;

	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.rocketChaser.Store(this);
	}

	protected override void Move()
	{
		if (this.isReady)
		{
			if (this.target)
			{
				Vector3 vector = this.target.position - base.transform.position;
				base.transform.right = Vector3.MoveTowards(base.transform.right, vector.normalized, this.turnSpeed * Time.deltaTime);
			}
			base.transform.Translate(base.transform.right * this.moveSpeed * Time.deltaTime, Space.World);
		}
		else if (this.isPreparing)
		{
			this.isPreparing = false;
			base.transform.DOMove(this.readyPosition, 0.2f, false).OnComplete(delegate
			{
				this.isReady = true;
			});
		}
	}

	public void Active(Transform firePoint, Vector2 readyPosition, float moveSpeed, Transform parent = null)
	{
		this.target = null;
		this.moveSpeed = moveSpeed;
		this.readyPosition = readyPosition;
		base.transform.position = firePoint.position;
		base.transform.rotation = firePoint.rotation;
		base.transform.parent = parent;
		this.isReady = false;
		this.isPreparing = true;
		base.gameObject.SetActive(true);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactExplodeMedium, base.transform.position);
	}

	public void Focus(Transform target)
	{
		this.target = target;
	}
}
