using System;
using UnityEngine;

public class BulletPreviewSplit : BaseBulletPreview
{
	public Transform splitPoint;

	public SpriteRenderer sprRenderer;

	public Sprite sprBulletSub;

	public Animator animator;

	private bool isSplit;

	protected override void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Enemy"))
		{
			EventDispatcher.Instance.PostEvent(EventID.PreviewDummyTakeDamage);
			if (this.isSplit)
			{
				for (int i = -2; i < 3; i++)
				{
					BulletPreviewSplit bulletPreviewSplit = Singleton<PoolingPreviewController>.Instance.split.New();
					if (bulletPreviewSplit == null)
					{
						bulletPreviewSplit = UnityEngine.Object.Instantiate<BulletPreviewSplit>(this);
					}
					bulletPreviewSplit.Active(this.splitPoint, this.moveSpeed, false, Singleton<PoolingPreviewController>.Instance.group);
					bulletPreviewSplit.transform.Rotate(0f, 0f, (float)i * 45f);
				}
			}
		}
		this.SpawnHitEffect();
		this.Deactive();
	}

	public void Active(Transform firePoint, float moveSpeed, bool isSplit, Transform parent = null)
	{
		base.Active(firePoint, moveSpeed, parent);
		this.isSplit = isSplit;
		if (isSplit)
		{
			this.animator.enabled = true;
			this.sprRenderer.transform.localScale = Vector3.one * 0.75f;
		}
		else
		{
			this.animator.enabled = false;
			this.sprRenderer.sprite = this.sprBulletSub;
			this.sprRenderer.transform.localScale = Vector3.one;
		}
	}

	protected override void Deactive()
	{
		base.Deactive();
		Singleton<PoolingPreviewController>.Instance.split.Store(this);
	}

	protected override void SpawnHitEffect()
	{
		EffectController.Instance.SpawnParticleEffect(EffectObjectName.BulletImpactSplitGun, base.transform.position);
	}
}
